// app/services/tasks/taskrunner.ts
import {Injectable} from 'angular2/core';
import {Observable} from 'rxjs/Rx';
import {TaskService} from './taskservice';
import {Task} from '../../models/tasks/task';
import {TaskResult} from '../../models/tasks/taskresult';

@Injectable()
export class TaskRunnerService {
    isBusy: boolean;

    constructor(private taskService: TaskService){
        this.isBusy = false;
    }

    private runTask(task: Task) : Observable<Task>{
        return Observable.create(observer => {
        var blob = new Blob([task.Code], {type: 'application/javascript'});
        
        console.log("starting worker for task " + task.JobItemId);
        var worker = new Worker(URL.createObjectURL(blob));
        worker.postMessage(task.JsonData);
        worker.onmessage = function(event) {
            
            observer.next(new TaskResult(task.JobItemId, event.data.results, true, null));
            observer.complete();
            
            console.log("worker finished");
            worker.terminate();
        }; 
        });
    }

    private feedTasks(): Observable<Task> {
        var self = this;
        
        // return Observable
        //         .interval(100)
        //         .flatMap(() => {
        //             return self.taskService.getNext();
        //         })
        //         .skipWhile(() => self.isBusy)
        //         .catch(() => {
        //             return Observable.throw("myerror");
        //         });

        return Observable.create(observer => {
            setInterval(function(){
                if(!self.isBusy){
                    self.isBusy = true;
                    self.taskService.getNext().subscribe(
                        task => observer.next(task),
                        error => {
                            console.log(error);
                            self.isBusy = false;
                        }
                    );
                }
            }, 1000);
        });
    }

    public runWorker(): Observable<string> {
        var self = this;
        
        return Observable.create(observer => {
            observer.next("Starting worker...");

            self.feedTasks()
                    .subscribe(
                        task => {
                            console.log("executing task:");
                            console.log(task);
                            self.isBusy = false;
                        }
                    )
        });
    }
}