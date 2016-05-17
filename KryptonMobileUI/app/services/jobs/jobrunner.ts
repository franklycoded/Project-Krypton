// app/services/tracer/jobrunner.ts
import {Injectable} from 'angular2/core';
import {Observable} from 'rxjs/Rx';
import {JobResult} from '../../models/jobs/jobResult'

@Injectable()
export class JobRunnerService {
  isBusy: boolean
  
  constructor() {
    this.isBusy = false;
  }
  
  public runJob(jobId: number, codeToExecute: string) : Observable<JobResult>{
    var self = this;
    
    if(self.isBusy) return null;
    
    self.isBusy = true;
    
    return Observable.create(observer => {
      var blob = new Blob([codeToExecute], {type: 'application/javascript'});
      
      console.log("starting worker");
      var worker = new Worker(URL.createObjectURL(blob));
      worker.onmessage = function(event) {
         
        //  var resultArray = [];
        //  for (var i = 0; i < event.data.results.length; i++) {
        //      resultArray.push(event.data.results[i]);
        //  }
         
         observer.next(new JobResult(jobId, event.data.results));
         observer.complete();
         
        self.isBusy = false;
        console.log("worker finished");
        worker.terminate();
      }; 
    });
  }
}