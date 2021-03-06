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

    public runWorker(): Observable<string> {
        var self = this;
        
        return Observable.create(observer => {
            observer.next("Starting worker...");

            self.feedTasks()
                .flatMap((task) => self.executeTask(task))
                .map((result: TaskResult) => {
                    console.log("calculated, posting result");
                    console.log(result);
                    return result;
                })
                .flatMap((taskResult) => this.taskService.postTaskResult(taskResult))
                .map((result: boolean) => {
                    console.log("post result: " + result);
                    return result;
                })
                .subscribe(
                    result => {
                        console.log("subscribed result:" + result);
                        self.isBusy = false;
                    },
                    error => {
                        console.log("error while executing task");
                        console.log(error);
                        self.isBusy = false;
                    }
                );
        });
    }

    private executeTask(task: Task) : Observable<Task>{
        return Observable.create(observer => {
            // Parsing jsonData
            var parsedData = null;
            try {
                parsedData = JSON.parse(task.jsonData);
            }
            catch(e){
                observer.next(new TaskResult(task.jobItemId, null, false, "Error while parsing JsonData: " + e.toString()));
                observer.complete();
                return;
            }
            
            try {
                // Creating executing blob
                var blob = new Blob([task.code], {type: 'application/javascript'});
                var worker = new Worker(URL.createObjectURL(blob));
                
                worker.onmessage = function(event) {
                    observer.next(new TaskResult(task.jobItemId, JSON.stringify(event.data.taskResult), true, null));
                    observer.complete();
                    worker.terminate();
                };
    	        
                worker.onerror = function(err){
                    observer.next(new TaskResult(task.jobItemId, null, false, "Error while executing task: " + err.message));
                    observer.complete();
                    worker.terminate();
                }

                worker.postMessage(parsedData);
            } catch(e) {
                observer.next(new TaskResult(task.jobItemId, null, false, "Error while running task: " + e.toString()));
                observer.complete();
                return;
            }
        });
    }

    private feedTasks(): Observable<Task> {
        var self = this;

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
}