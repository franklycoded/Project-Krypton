# Libs
require 'fileutils'

#Arguments
dest = ARGV[0];

puts "Starting publish..."
Dir.chdir("../src/KryptonAPI") do
    system "dotnet publish --no-build"
end

puts "Finished publish"
puts "Copying artifacts to #{dest}"
FileUtils.cp_r("../src/KryptonAPI/bin/Debug/netcoreapp1.0/publish/.", "#{dest}")
puts "Finished copying artifacts to #{dest}"