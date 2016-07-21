class Helper
    def initialize(binPath, dbPath, queueEngineHostname, queueEnginePort)
        @binPath = binPath
        @dbPath = dbPath
        @queueEngineHostname = queueEngineHostname
        @queueEnginePort = queueEnginePort
    end

    def cleanDb()
        puts "Cleaning database"
        
        db = SQLite3::Database.new( @dbPath )
        db.execute("delete from JobItems")
        db.execute("delete from Jobs")
        db.close
        # rows = db.execute( "select * from JobItems" ) do |row|
        #     puts row
        # end
    end

    def cleanQueueEngine()
        puts "Removing krypton-test-mq docker container"

        system("docker stop krypton-test-mq")
        system("docker rm krypton-test-mq")

        puts "Removed krypton-test-mq docker container" 
    end

    def createQueueEngine()
        puts "Creating new instance of krypton-test-mq docker container"

        system("docker run --hostname krypton-test-host-mq --name krypton-test-mq -d -p #{@queueEnginePort}:5672 rabbitmq:3")

        puts "Created new instance of krypton-test-mq docker container"
        puts "Connecting to queue engine"

        engineIsUp = false

        while(!engineIsUp) do
            begin
                conn = Bunny.new(:hostname => @queueEngineHostname, :port => @queueEnginePort)
                conn.start
                engineIsUp = true
                return conn
            rescue
                puts 'waiting for queue engine to start...'
                sleep(1)
            end
        end
    end

    def createKryptonApiService()
        puts "Starting KryptonAPI"

        kryptonApiPid = -1

        Dir.chdir("#{@binPath}") do
            ENV['ASPNETCORE_ENVIRONMENT']='AcceptanceTesting'
            stdin, stdout, stderr, wait_thr = Open3.popen3('dotnet KryptonAPI.dll');
            
            kryptonApiPid = wait_thr.pid
            
            output = "";

            while !output.match(/^Application started/) do
                output = stdout.gets
                puts output
            end
        end

        puts "KryptonAPI is running"

        return kryptonApiPid
    end

    def killKryptonApiService(pid)
        puts "Stopping KryptonAPI"

        system("kill #{pid}")

        puts "KryptonAPI stopped"
    end

    def runTest(testMethod)
        cleanDb()
        cleanQueueEngine()
        queueConnection = createQueueEngine()
        kryptonApiPid = createKryptonApiService()

        puts "Running: #{testMethod.name}"
        testResult = testMethod.call

        if testResult
            puts "Pass: #{testMethod.name}"
        else
            puts "Fail: #{testMethod.name}"
        end

        killKryptonApiService(kryptonApiPid)

        queueConnection.close

        cleanQueueEngine()
        cleanDb()

        return testResult
    end
end