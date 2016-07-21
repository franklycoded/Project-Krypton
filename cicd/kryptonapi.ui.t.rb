# script to cover manual UI testing scenarios
require 'open3'
require 'sqlite3'
require 'bunny'
require './kryptonapi.acc.t.helpers.rb'
require './kryptonapi.ui.t.cases.rb'

#Arguments
scenario = ARGV[0];
dest = ARGV[1];

queueEngineHostname = "localhost"
queueEnginePort = 8070
taskQueueName = "task_queue"
apiHostname = "localhost"
apiPort = 5000
dbPath = "#{dest}/kryptonapi.db"

testHelper = Helper.new(dest, dbPath, queueEngineHostname, queueEnginePort)
testCases = KryptonApiUiTestCases.new(queueEngineHostname, queueEnginePort, taskQueueName, apiHostname, apiPort, dbPath)

system("ruby kryptonapi.publish.rb #{dest}")
testHelper.cleanDb()
testHelper.killKryptonApiService(nil)
testHelper.cleanQueueEngine()
testHelper.createQueueEngine()
kryptonApiPid = testHelper.createKryptonApiService()

puts "scenario to run: #{scenario}"

begin
    if(scenario == 'calcexamples')
        testCases.fillTaskQueueCalcExamples()
    end
rescue
    if kryptonApiPid != nil
        testHelper.killKryptonApiService(kryptonApiPid)
    end

    testHelper.cleanDb()
    testHelper.cleanQueueEngine()
end


