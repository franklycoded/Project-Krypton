fs = require('fs');
fs.readFile('/Users/nilur/Documents/Projects/src/github.com/Frankly Coded/Project-Krypton/KryptonAPI/outputimage.bmp', function (err,data) {
  if (err) {
    return console.log(err);
  }
  
  var bmp = require("bmp-js");
  var bmpData={data:data,width:640,height:480} 
  var rawData = bmp.encode(bmpData);//default no compression 
  console.log(rawData);
  //console.log(data);
  
  var fs = require('fs');
    fs.writeFile("/Users/nilur/Documents/Projects/src/github.com/Frankly Coded/Project-Krypton/KryptonAPI/myimage.bmp", rawData.data, function(err) {
        if(err) {
            return console.log(err);
        }

        console.log("The file was saved!");
    }); 
  
});