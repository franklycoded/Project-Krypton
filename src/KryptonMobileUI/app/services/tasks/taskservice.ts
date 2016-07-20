// app/services/tasks/taskservice.ts
import {Injectable} from 'angular2/core';
import {Observable} from 'rxjs/Rx';
import {Headers, Http, Response} from 'angular2/http';
import {Task} from '../../models/tasks/task';

@Injectable()
export class TaskService {
    
    constructor (private http: Http){
    }
    
    public getNext() : Observable<Task> {
        return this.http.get("http://localhost:5000/api/jobitems/next")
                        .map(this.extractData)
                        .catch(this.handleError)
    }

    private extractData(res: Response) {
        let body = res.json();
        return new Task(body.JobItemId, body.Code, body.JsonData);
    }

    private handleError (error: any) {
        // In a real world app, we might use a remote logging infrastructure
        // We'd also dig deeper into the error to get a better message
        let errMsg = (error.message) ? error.message :
        error.status ? `${error.status} - ${error.statusText}` : 'Server error';
        
        console.error(errMsg); // log to console instead
        
        return Observable.throw(errMsg);
    }
}