require 'open3'

#Arguments
dest = ARGV[0];

puts "Removing krypton-test-mq docker container"

system("docker stop krypton-test-mq")
system("docker rm krypton-test-mq")

puts "Removed krypton-test-mq docker container"
puts "Creating new instance of krypton-test-mq docker container"

system("docker run --hostname krypton-test-host-mq --name krypton-test-mq -d -p 8070:5672 rabbitmq:3")

puts "Created new instance of krypton-test-mq docker container"
puts "Starting KryptonAPI"

Dir.chdir("#{dest}") do
    stdin, stdout, stderr, wait_thr = Open3.popen3('dotnet KryptonAPI.dll');
    
    puts stdout.gets
end


puts "KryptonAPI is running"
