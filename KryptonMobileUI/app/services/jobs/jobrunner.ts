// app/services/tracer/jobrunner.ts
import {Injectable} from 'angular2/core';
import {Observable} from 'rxjs/Rx';
import {JobResult} from '../../models/jobs/jobResult'
import {JobDto} from '../../models/jobs/jobDto'
import {AuthHttp} from 'angular2-jwt';
import {AuthService} from '../../services/auth/auth';
import {Headers} from 'angular2/http';

@Injectable()
export class JobRunnerService {
  isBusy: boolean
  
  constructor(private authHttp: AuthHttp, private auth: AuthService) {
    this.isBusy = false;
  }
  
  private runJob(jobId: number, codeToExecute: string) : Observable<JobResult>{
    return Observable.create(observer => {
      var blob = new Blob([codeToExecute], {type: 'application/javascript'});
      
      console.log("starting worker for job " + jobId);
      var worker = new Worker(URL.createObjectURL(blob));
      worker.onmessage = function(event) {
         
         observer.next(new JobResult(jobId, event.data.results));
         observer.complete();
        
        console.log("worker finished");
        worker.terminate();
      }; 
    });
  }
  
  public runWorker() : Observable<string> {
    var self = this;
    var headers = new Headers({
    'Content-Type': 'application/json'});
    
    return Observable.create(observer => {
      observer.next("Starting worker...");
      
      var interval = setInterval(function(){
        if(!self.isBusy){
          self.isBusy = true;
          observer.next("Not busy");
          self.authHttp.get('http://192.168.1.66:5000/api/jobs/getNext')
            .map(res => res.json())
            .toPromise()
            .then(
              data => {
                observer.next("Busy with job " + data.Id)
                console.log("Received job with id " + data.Id);
                
                var jobRunSubscription = self.runJob(data.Id, data.Task).subscribe(jobResult => {
                  console.log("Job with id " + jobResult.jobId + " finished");
                  observer.next("Finished calculating job: " + jobResult.jobId);
                  jobRunSubscription.unsubscribe();
                  self.isBusy = false;
                  
                  var resultDto = new JobDto(jobResult.jobId, "Success", jobResult.payload);
                  
                  self.authHttp.put(
                    'http://192.168.1.66:5000/api/jobs/' + jobResult.jobId,
                    JSON.stringify(resultDto), {headers: headers})
                    .toPromise()
                    .then(() => {
                      //self.isBusy = false;
                    })
                    .catch(err => {
                      console.log(err);
                      //self.isBusy = false;
                    });  
                });
              })
              .catch(
                err => {
                  if(err.status === 404){
                    observer.next("No jobs available"); 
                  } else {
                    console.log(err);
                  }
                  
                  self.isBusy = false;
                }
            );
        }
      }, 100);
    });
  }
}