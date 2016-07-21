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
            // Parsing jsonData
            var parsedData = null;
            try {
                parsedData = JSON.parse(task.JsonData);
            }
            catch(e){
                observer.next(new TaskResult(task.JobItemId, null, false, "Error while parsing JsonData: " + e.toString()));
                observer.complete();
                return;
            }
            
            // Creating executing blob
            var blob = new Blob([task.Code], {type: 'application/javascript'});
            var worker = new Worker(URL.createObjectURL(blob));
            worker.postMessage(parsedData);
            
            worker.onmessage = function(event) {
                observer.next(new TaskResult(task.JobItemId, event.data.taskResult, true, null));
                observer.complete();
                worker.terminate();
            }; 
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

    public runWorker(): Observable<string> {
        var self = this;
        
        return Observable.create(observer => {
            observer.next("Starting worker...");

            self.feedTasks().subscribe(
                task => {
                    console.log("executing task:");
                    console.log(task);
                    self.runTask(task).subscribe(
                        result => {
                            console.log(result);
                            self.isBusy = false;
                        },
                        error => {
                            console.log("error while executing task");
                            console.log(error);
                            self.isBusy = false;
                        }
                    )
                }
                )
        });
    }
}