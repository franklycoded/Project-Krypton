require 'rest-client'
require 'json'

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
                    # Checking properties of response object
                    responseObject = JSON.parse(response.body, object_class: OpenStruct)

                    if !(responseObject.JobItemId == lastRowId && responseObject.Code == "code" && responseObject.JsonData == "jsondata")
                        puts "Unexpected response object: #{response.body}"
                        return false
                    end

                    # Checking job item status id
                    db.results_as_hash = true
                    db.execute("select * from JobItems where Id = #{lastRowId}") do |row|
                        statusId = row["StatusId"].to_i
                    end

                    if statusId != 3
                        puts "Unexpected status id: The status of the JobItem is expected to be 'Running' with StatusId 3"
                        return false
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
                    return true
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

    def test_submitResult_submitEmpty_return500()
        RestClient.post("http://#{@apiHostname}:#{@apiPort}/api/jobitems/result", nil, {:content_type => :json}) { |response, request, result, &block|
        case response.code
            when 500
                return true
            else
                puts "Unexpected response code: #{response.code}"
                return false
            end
        } 
    end

    def test_submitResult_jobItemNotInDatabase_return500()
        db = nil

        begin
            #Submitting successful task result
            jdata = JSON.generate(["JobItemId" => 1, "TaskResult" => "successful result", "IsSuccessful" => true, "ErrorMessage" => nil])

            RestClient.post("http://#{@apiHostname}:#{@apiPort}/api/jobitems/result", jdata, {:content_type => :json}) { |response, request, result, &block|
            case response.code
                when 500
                    return true
                else
                    puts "Unexpected response code: #{response.code}"
                    return false
                end
            } 

        ensure
            if db != nil
                db.close
            end
        end
    end

    def test_submitResult_successfulResult_stateChangedToSuccess_return200()
        db = nil

        begin
            #Adding running Job and JobItem to database
            db = SQLite3::Database.new( @dbPath )
            db.execute("insert into Jobs (CreatedUTC, FinalResult, ModifiedUTC, StatusId, UserId) values('2016-01-01', '', '2016-01-01', 2, 1)")

            lastRowId = getLastRowId(db)

            db.execute("insert into JobItems (CreatedUTC, JobId, JsonResult, ModifiedUTC, StatusId, Code, JsonData) values('2016-01-01', #{lastRowId}, 'noresult', '2016-01-01', 3, 'code', 'jsondata')")

            lastRowId = getLastRowId(db)

            #Submitting successful task result
            jdata = JSON.generate(["JobItemId" => lastRowId, "TaskResult" => "successful result", "IsSuccessful" => true, "ErrorMessage" => nil])

            RestClient.post("http://#{@apiHostname}:#{@apiPort}/api/jobitems/result", jdata, {:content_type => :json}) { |response, request, result, &block|
            case response.code
                when 200
                    # Check if JobItem status id is Success (4)
                    db.results_as_hash = true
                    statusId = 0
                    jsonResult = ""

                    db.execute("select * from JobItems where Id = #{lastRowId}") do |row|
                        statusId = row["StatusId"].to_i
                        jsonResult = row["JsonResult"]
                    end

                    if statusId != 4
                        puts "Unexpected status id: The status of the JobItem is expected to be 'Success' with StatusId 4"
                        return false
                    end

                    if jsonResult != "successful result"
                        puts "Unexpected jsonResult: #{jsonResult}"
                        return false
                    end

                    return true
                else
                    puts "Unexpected response code: #{response.code}"
                    return false
                end
            } 

        ensure
            if db != nil
                db.close
            end
        end
    end

    def test_submitResult_failedResult_stateChangedToFailed_errorFieldFilled_return200()
        db = nil

        begin
            #Adding running Job and JobItem to database
            db = SQLite3::Database.new( @dbPath )
            db.execute("insert into Jobs (CreatedUTC, FinalResult, ModifiedUTC, StatusId, UserId) values('2016-01-01', '', '2016-01-01', 2, 1)")

            lastRowId = getLastRowId(db)

            db.execute("insert into JobItems (CreatedUTC, JobId, JsonResult, ModifiedUTC, StatusId, Code, JsonData) values('2016-01-01', #{lastRowId}, 'noresult', '2016-01-01', 3, 'code', 'jsondata')")

            lastRowId = getLastRowId(db)

            #Submitting successful task result
            jdata = JSON.generate(["JobItemId" => lastRowId, "TaskResult" => "error result", "IsSuccessful" => false, "ErrorMessage" => "error message"])

            RestClient.post("http://#{@apiHostname}:#{@apiPort}/api/jobitems/result", jdata, {:content_type => :json}) { |response, request, result, &block|
            case response.code
                when 200
                    # Check if JobItem status id is Fail (5)
                    db.results_as_hash = true
                    errorMessage = ""
                    statusId = 0

                    db.execute("select * from JobItems where Id = #{lastRowId}") do |row|
                        statusId = row["StatusId"].to_i
                        errorMessage = row["ErrorMessage"]
                    end

                    if statusId != 5
                        puts "Unexpected status id: The status of the JobItem is expected to be 'Fail' with StatusId 5"
                        return false
                    end

                    if errorMessage != "error message"
                        puts "Unexpected error message: #{errorMessage}"
                        return false
                    end

                    return true
                else
                    puts "Unexpected response code: #{response.code}"
                    return false
                end
            } 

        ensure
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