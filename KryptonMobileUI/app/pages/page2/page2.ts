import {Page} from 'ionic-angular';
import {AuthHttp} from 'angular2-jwt';
import {AuthService} from '../../services/auth/auth';
import {JayTracerService} from '../../services/tracer/jaytracer';
import 'rxjs/add/operator/map';

@Page({
  templateUrl: 'build/pages/page2/page2.html',
})
export class Page2 {
  message: string
  
  constructor(private authHttp: AuthHttp, private auth: AuthService) {

  }
  
  sendPing(event){  
    console.log("sending ping");
    
    // Here we use authHttp to make an authenticated
    // request to the server. Change the endpoint up for
    // one that points to your own server.
    this.authHttp.get('http://192.168.15.114:5000/api/values')
      .map(res => res.json())
      .subscribe(
        data => {
          console.log("received data:");
          console.log(data);
          this.message += " " + data;
        },
        err => console.log(err)
      );
  }
}
