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
      var w = 640,
          h = 480,
          zone = document.getElementById("zone");
          
      zone.textContent = "";
      var time = (new Date()).getTime();
      console.log("started at " + time + " with w=" + w + ", h=" + h);
      this.jayTracer.traceTo(zone, w, h, this.scene);
      time = (new Date()).getTime() - time;
      console.log("time taken: " + time + "ms");
    }
}