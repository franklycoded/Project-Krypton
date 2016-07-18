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
            taskQueue = ch.queue(@taskQueueName);
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
end