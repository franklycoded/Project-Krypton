# script to cover manual UI testing scenarios
require 'open3'
require 'sqlite3'
require 'bunny'
require './kryptonapi.acc.t.helpers.rb'

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

system("ruby kryptonapi.publish.rb #{dest}")
testHelper.cleanDb()
testHelper.cleanQueueEngine()
testHelper.createQueueEngine()
testHelper.createKryptonApiService()




