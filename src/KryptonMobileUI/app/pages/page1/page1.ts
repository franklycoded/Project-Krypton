import {Page} from 'ionic-angular';
import {Component, Pipe, ChangeDetectorRef} from 'angular2/core';
import {AuthHttp} from 'angular2-jwt';
import {AuthService} from '../../services/auth/auth';
import 'rxjs/add/operator/map';

@Page({
  templateUrl: 'build/pages/page1/page1.html',
})
export class Page1 {
   webWorkerMessage = 'initializing...';
   workerCounter = "";
  constructor(private cd: ChangeDetectorRef, private authHttp: AuthHttp, private auth: AuthService) {
    if(typeof(Worker) !== "undefined") {
        console.log("web workers DO work!");
        this.webWorkerMessage = "web workers DO work!";
    } else {
        console.log("web workers DON'T work!");
        this.webWorkerMessage = "web workers DON'T work!";
    }  
    
    // Prime finding algo
    var primeFinder = "var checkedNumber = 10000000;" +
    "while(true){" +
        "var hasDivider = false;" +
        
        "for(var divider = 2; divider < Math.sqrt(checkedNumber); divider++){"+
            "if(checkedNumber % divider == 0){"+
                "hasDivider = true;"+
                "break;"+
            "}"+
        "}"+
        
        "if(!hasDivider) postMessage(checkedNumber);"+
        "checkedNumber++;"+
    "}";
    
    //var blob = new Blob(["var i = 0;function timedCount() {i = i + 1;console.log('timing');postMessage(i);setTimeout('timedCount()',500);}timedCount();"], {type: 'application/javascript'});
    var blob = new Blob([primeFinder], {type: 'application/javascript'});
    
    var self = this;
    
    if(typeof(Worker) !== "undefined") {
        var w = new Worker(URL.createObjectURL(blob));
        var numberOfPrimes = 0;
        w.onmessage = function(event) {
            numberOfPrimes++;
            
            if(numberOfPrimes % 10000 == 0){
                self.workerCounter = "Biggest prime: " + event.data.toString() + " - " + numberOfPrimes + " found!";
                self.cd.detectChanges();
                
                self.authHttp.get('http://192.168.1.66:5000/api/values')
                .map(res => res.json())
                .subscribe(
                    data => {
                        
                    },
                    err => console.log(err)
                ); 
            }
        };
    }
  }
}
