import {Page} from 'ionic-angular';
import {JayTracerService} from '../../services/tracer/jaytracer';

@Page({
  templateUrl: 'build/pages/page3/page3.html'
})
export class Page3 {
  scene: any;
  
  constructor(private jayTracer: JayTracerService) {
    this.scene = {
        background: [0, 0, 0],
        shapes:     [
            {
                id:       "infinity",
                type:     "plane",
                offset:   0,
                surface:  "checkerboard",
                normal:   [0, 1, 0],
            },
            {
                id:       "big-sphere",
                type:     "sphere",
                radius:   1,
                surface:  "shiny",
                centre:   [0, 1, 0],
            },
            {
                id:       "lil-sphere",
                type:     "sphere",
                radius:   0.5,
                surface:  "shiny",
                centre:   [-1, 0.5, 1.5],
            },
        ],
        camera: {
            position: [3, 2, 4],
            lookAt:   [-1, 0.5, 0],
        },
        lights: [
            {
                position: [-2, 2.5, 0],
                colour:   [0.49, 0.07, 0.07]
            },
            {
                position: [1.5, 2.5, 1.5],
                colour:   [0.07, 0.07, 0.49]
            },
            {
                position: [1.5, 2.5, -1.5],
                colour:   [0.07, 0.49, 0.07]
            },
            {
                position: [0, 3.5, 0],
                colour:   [0.21, 0.21, 0.35]
            },
        ]
    };
  }
  
  render() {
      var w = 2000,
          h = 1500,
          zone = document.getElementById("zone");
          
      zone.textContent = "";
      var time = (new Date()).getTime();
      console.log("started at " + time + " with w=" + w + ", h=" + h);
      
      var can = document.createElement("canvas");
      can.width = w;
      can.height = h;
      zone.appendChild(can);
      var ctx = can.getContext("2d");
      
      var id;
      if(ctx.createImageData){
        id = ctx.createImageData(w, h);
      }  
      else if(ctx.getImageData){
        id = ctx.getImageData(0, 0, w, h);
      }
      else{
        id = { 'width' : w, 'height' : h, 'data' : new Array(w*h*4) };
      }                          
      
      var rowStart = 0;
      var rowFinish = 1500;
      
      var data = this.jayTracer.getImageData(this.scene, w, h, rowStart, rowFinish);
      
      for(var i = 0; i < data.length; i++){
        id.data[i + (rowStart * w)] = data[i];
      }
      
      ctx.putImageData(id, 0, 0);
      
      time = (new Date()).getTime() - time;
      console.log("time taken: " + time + "ms");
    }
}