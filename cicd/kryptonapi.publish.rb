# Libs
require 'fileutils'

#Arguments
dest = ARGV[0];

puts "Starting publish..."
Dir.chdir("../src/KryptonAPI") do
    system "dotnet publish -c Release --no-build"
end

puts "Finished publish"
puts "Copying artifacts to #{dest}"
FileUtils.cp_r("../src/KryptonAPI/bin/Release/netcoreapp1.0/publish/.", "#{dest}")
FileUtils.cp_r("../src/KryptonAPI/bin/Release/netcoreapp1.0/kryptonapi.db", "#{dest}")
puts "Finished copying artifacts to #{dest}"