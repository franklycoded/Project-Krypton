require 'open3'
require 'sqlite3'
require 'bunny'


#Arguments
dest = ARGV[0];

def cleanDb(dest)
    puts "Cleaning database"
    
    db = SQLite3::Database.new( "#{dest}/kryptonapi.db" )
    db.execute("delete from JobItems")
    db.execute("delete from Jobs")
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

    system("docker run --hostname krypton-test-host-mq --name krypton-test-mq -d -p 8070:5672 rabbitmq:3")

    puts "Created new instance of krypton-test-mq docker container"
    puts "Connecting to queue engine"

    engineIsUp = false

    while(!engineIsUp) do
        begin
            conn = Bunny.new(:hostname => "localhost", :port => 8070)
            conn.start
            engineIsUp = true
            return conn
        rescue
            puts 'waiting for queue engine to start...'
            sleep(1)
        end
    end
end

def createKryptonApiService(dest)
    puts "Starting KryptonAPI"

    kryptonApiPid = -1

    Dir.chdir("#{dest}") do
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

def runTest(dest, testMethod)
    cleanDb(dest)
    cleanQueueEngine()
    queueConnection = createQueueEngine()
    kryptonApiPid = createKryptonApiService(dest)

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
    cleanDb(dest)

    return testResult
end

puts "Loading tests..."

require '../test/KryptonAPI.AcceptanceTests/tests.jobscheduler.rb'

tests = [method(:test_getNext_emptyQueue_return404)]

numTests = tests.length
numPassed = 0
numFailed = 0
finalResult = true

puts "Running tests..."

tests.each do |test|
    testResult = runTest(dest, test)
    finalResult = finalResult && testResult

    if testResult
        numPassed = numPassed + 1
    else
        numFailed = numFailed + 1
    end
end

if finalResult
    finalResult = "Pass"
else    
    finalResult = "Fail"
end

puts "Tests outcome: #{finalResult}"
puts "Tests ran: #{numTests}, Passed: #{numPassed}, Failed: #{numFailed}"