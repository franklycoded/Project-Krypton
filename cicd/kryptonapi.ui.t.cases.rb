class KryptonApiUiTestCases
    def initialize(queueEngineHostname, queueEnginePort, taskQueueName, apiHostname, apiPort, dbPath)
        @queueEngineHostname = queueEngineHostname
        @queueEnginePort = queueEnginePort
        @taskQueueName = taskQueueName
        @apiHostname = apiHostname
        @apiPort = apiPort
        @dbPath = dbPath
    end

    def fillTaskQueueCalcExamples()
        code = "function executeTask(taskData){ return taskData.left + taskData.right; } 
                this.onmessage = function(e) {
                    var taskResult = executeTask(e.data);
                    postMessage({taskResult: taskResult});
                };";
        data = '{"left": 5, "right": 7}';

        db = nil
        queueConn = nil

        begin
            # Adding data to database
            puts "Adding data to database"
            
            db = SQLite3::Database.new(@dbPath)
            db.execute("insert into Jobs (CreatedUTC, FinalResult, ModifiedUTC, StatusId, UserId) values('2016-01-01', '', '2016-01-01', 2, 1)")

            lastRowId = getLastRowId(db)

            db.execute("insert into JobItems (CreatedUTC, JobId, JsonResult, ModifiedUTC, StatusId, Code, JsonData) values('2016-01-01', #{lastRowId}, '', '2016-01-01', 2, '#{code}', '#{data}')")

            lastRowId = getLastRowId(db)
            puts lastRowId
            puts "Database ready"

            #Adding data to queue
            puts "Adding data to queue"
            queueConn = Bunny.new(:hostname => @queueEngineHostname, :port => @queueEnginePort)
            queueConn.start
            ch = queueConn.create_channel
            taskQueue = ch.queue(@taskQueueName, :durable => true);
            ch.default_exchange.publish("{Id:#{lastRowId}}", :routing_key => taskQueue.name)
        rescue Exception => ex
            puts "Error while running fillTaskQueueCalcExamples:"
            puts ex
            raise ex
        ensure
            if db != nil
                db.close
            end

            if queueConn != nil
                queueConn.close
            end
        end
    end

    def getLastRowId(db)
        db.execute("select last_insert_rowid()") do |row|
            return row[0]
        end
    end
end