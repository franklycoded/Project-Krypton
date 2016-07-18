require 'open3'
require 'sqlite3'
require 'bunny'
require './kryptonapi.acc.t.helpers.rb'

#Arguments
dest = ARGV[0];

queueEngineHostname = "localhost"
queueEnginePort = 8070
taskQueueName = "task_queue"
apiHostname = "localhost"
apiPort = 5000
dbPath = "#{dest}/kryptonapi.db"

testHelper = Helper.new(dest, dbPath, queueEngineHostname, queueEnginePort)

puts "Loading tests..."

require '../test/KryptonAPI.AcceptanceTests/tests.jobscheduler.rb'

jobSchedulerTests = JobSchedulerTests.new(apiHostname, apiPort, queueEngineHostname, queueEnginePort, taskQueueName, dbPath)

tests = [jobSchedulerTests.method(:test_getNext_emptyQueue_return404),
         jobSchedulerTests.method(:test_getNext_itemInQueue_notInDatabase_return500)]

numTests = tests.length
numPassed = 0
numFailed = 0
finalResult = true

puts "Running tests..."

tests.each do |test|
    testResult = testHelper.runTest(dest, test)
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