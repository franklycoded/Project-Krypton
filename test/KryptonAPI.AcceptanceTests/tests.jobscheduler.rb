require 'rest-client'

class JobSchedulerTests
    def initialize(apiHostname, apiPort, queueEngineHostname, queueEnginePort, taskQueueName, dbPath)
        @apiHostname = apiHostname
        @apiPort = apiPort
        @queueEngineHostname = queueEngineHostname
        @queueEnginePort = queueEnginePort
        @taskQueueName = taskQueueName
        @dbPath = dbPath
    end

    def test_getNext_emptyQueue_return404()
        RestClient.get("http://#{@apiHostname}:#{@apiPort}/api/jobitems/next") { |response, request, result, &block|
        case response.code
            when 404
                return true
            else
                puts "Unexpected response code: #{response.code}"
                return false
            end
        }
    end

    def test_getNext_itemInQueue_notInDatabase_return500()
        queueConn = nil
        
        begin
            # Adding item to queue
            queueConn = Bunny.new(:hostname => @queueEngineHostname, :port => @queueEnginePort)
            queueConn.start
            ch = queueConn.create_channel
            taskQueue = ch.queue(@taskQueueName, :durable => true);
            ch.default_exchange.publish("{Id:5}", :routing_key => taskQueue.name)

            RestClient.get("http://#{@apiHostname}:#{@apiPort}/api/jobitems/next") { |response, request, result, &block|
            case response.code
                when 500
                    return true
                else
                    puts "Unexpected response code: #{response.code}"
                    return false
                end
            }
        ensure
            if queueConn != nil
                queueConn.close
            end
        end
    end

    def test_getNext_itemInQueue_inDatabase_return200_statusRunning_itemRemovedFromQueue()
        queueConn = nil
        db = nil

        begin
            #Adding item to database
            db = SQLite3::Database.new( @dbPath )
            db.execute("insert into Jobs (CreatedUTC, FinalResult, ModifiedUTC, StatusId, UserId) values('2016-01-01', 'result', '2016-01-01', 2, 1)")

            lastRowId = getLastRowId(db)

            db.execute("insert into JobItems (CreatedUTC, JobId, JsonResult, ModifiedUTC, StatusId, Code, JsonData) values('2016-01-01', #{lastRowId}, 'testresult', '2016-01-01', 2, 'code', 'jsondata')")

            lastRowId = getLastRowId(db)

            # Adding item to queue
            queueConn = Bunny.new(:hostname => @queueEngineHostname, :port => @queueEnginePort)
            queueConn.start
            ch = queueConn.create_channel
            taskQueue = ch.queue(@taskQueueName, :durable => true);
            ch.default_exchange.publish("{Id:#{lastRowId}}", :routing_key => taskQueue.name)

            statusId = 0

            # Query item
            RestClient.get("http://#{@apiHostname}:#{@apiPort}/api/jobitems/next") { |response, request, result, &block|
            case response.code
                when 200
                    db.results_as_hash = true
                    db.execute("select * from JobItems where Id = #{lastRowId}") do |row|
                        statusId = row["StatusId"].to_i
                    end
                else
                    puts "Unexpected response code: #{response.code}"
                    return false
                end
            }

            #Making sure the queue is empty
            RestClient.get("http://#{@apiHostname}:#{@apiPort}/api/jobitems/next") { |response, request, result, &block|
            case response.code
                when 404
                    return statusId == 3
                else
                    puts "The queue is not empty"
                    return false
                end
            }
        ensure
            if queueConn != nil
                queueConn.close
            end

            if db != nil
                db.close
            end
        end
    end

    def getLastRowId(db)
        db.execute("select last_insert_rowid()") do |row|
            return row[0]
        end
    end
end