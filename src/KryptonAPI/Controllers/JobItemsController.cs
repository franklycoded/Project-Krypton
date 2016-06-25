using System.Threading.Tasks;
using KryptonAPI.Data.Models.JobScheduler;
using KryptonAPI.DataContracts;
using KryptonAPI.DataContracts.JobScheduler;
using KryptonAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KryptonAPI.Controllers
{
    [Route("api/jobitems")]
    public class JobItemsController : Controller
    {    
        private readonly ICRUDManager<JobItem, JobItemDto> _jobItemsManager;
        
        public JobItemsController(ICRUDManager<JobItem, JobItemDto> jobItemsManager){
            _jobItemsManager = jobItemsManager;
        }
        
        [HttpGet("{id}")]
        //[Authorize(ActiveAuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetById(long id){
            var jobItemDto = await _jobItemsManager.GetById(id);

            if(jobItemDto == null) return NotFound();

            return Ok(jobItemDto);
        }

        [HttpPut("{id}")]
        //[Authorize(ActiveAuthenticationSchemes = "Bearer")]
        public IActionResult PutResult(long id, [FromBody] JobResultDto jobResult){
            
            
            return Ok(jobResult);
        }
        
        private static object GetData(int taskIndex, int divider){
            int w = 640;
            int h = 480;
            int startRow = h / divider * taskIndex; 
            var endRow = h / divider * (taskIndex + 1) - 1;
            
            var rawData = @"{ width: " + w + ", height: " + h + ", startRow: " + startRow + ", endRow: " + endRow + ", scene: {         background: [0, 0, 0],         shapes:     [             {                 id:       'infinity',                 type:     'plane',                 offset:   0,                 surface:  'checkerboard',                 normal:   [0, 1, 0],             },             {                 id:       'big-sphere',                 type:     'sphere',                 radius:   1,                 surface:  'shiny',                 centre:   [0, 1, 0],             },             {                 id:       'lil-sphere',                 type:     'sphere',                 radius:   0.5,                 surface:  'shiny',                 centre:   [-1, 0.5, 1.5],             },         ],         camera: {             position: [3, 2, 4],             lookAt:   [-1, 0.5, 0],         },         lights: [             {                 position: [-2, 2.5, 0],                 colour:   [0.49, 0.07, 0.07]             },             {                 position: [1.5, 2.5, 1.5],                 colour:   [0.07, 0.07, 0.49]             },             {                 position: [1.5, 2.5, -1.5],                 colour:   [0.07, 0.49, 0.07]             },             {                 position: [0, 3.5, 0],                 colour:   [0.21, 0.21, 0.35]             },         ]     } }";
            
            return JsonConvert.DeserializeObject<dynamic>(rawData);
        }
        
        private static string GetTask(){
            var taskText = "function getImageData(scene, /*ctx,*/ width, height, rowStart, rowFinish) {         this.prepareScene(scene);                  var aspectRatio = width / height;                  var data = new Array((rowFinish - rowStart + 1) * width * 4);                  for (var i = 0, n = (rowFinish - rowStart + 1) * width * 4, j = rowStart * width; i < n; i+=4) {             var y = Math.floor(j / width);             var x = (j % width) + 1;                          var yRec = (-y / height) + 0.5;             var xRec = ((x / width) - 0.5) * aspectRatio;             var chans = this.plotPixel(scene, xRec, yRec);                          data[i+0] = Math.floor(chans[0] * 255);             data[i+1] = Math.floor(chans[1] * 255);             data[i+2] = Math.floor(chans[2] * 255);             data[i+3] = 255;                              j++;         }         return data;       }          function vectorAdd(v1, v2) {         var res = [];         for (var i = 0; i < v1.length; i++) { res[i] = v1[i] + v2[i]; }         return res;       }       function vectorSub(v1, v2) {         var res = [];         for (var i = 0; i < v1.length; i++) { res[i] = v1[i] - v2[i]; }         return res;       };       function vectorNeg(v1) {         var res = [];         for (var i = 0; i < v1.length; i++) { res[i] = -v1[i]; }         return res;       }       function vectorScale(v1, x) {         var res = [];         for (var i = 0; i < v1.length; i++) { res[i] = v1[i] * x; }         return res;       }       function vectorDot(v1, v2) {         var res = 0;         for (var i = 0; i < v1.length; i++) { res += v1[i] * v2[i]; }         return res;       }       function vectorCross3(v1, v2) {         return [v1[1] * v2[2] - v1[2] * v2[1],                 v1[2] * v2[0] - v1[0] * v2[2],                 v1[0] * v2[1] - v1[1] * v2[0]];       }       function vectorBlend(v1, v2) {         var res = [];         for (var i = 0; i < v1.length; i++) { res[i] = v1[i] * v2[i]; }         return res;       }       function vectorLength(v1) {         var sum = 0.0;         for (var i = 0; i < v1.length; i++) { sum += (1*v1[i]) * (1*v1[i]); }         return Math.sqrt(sum);       }       function vectorNormalise(v1) {         var len = this.vectorLength(v1);         var res = [];         for (var i = 0; i < v1.length; i++) { res[i] = v1[i] / len }         return res;       }       function vectorSum() {         var vecs = [];         for (var i = 0; i < arguments.length; i++) vecs.push(arguments[i]);         return this.vectorSumArray(vecs);       }       function vectorSumArray(vecs) {         var res = [0,0,0];         for (var i = 0; i < vecs.length; i++) {             var v = vecs[i];             res[0] += v[0];             res[1] += v[1];             res[2] += v[2];         }         return res;       }       function plotPixel(scene, x, y) {                   var cam = scene.camera;                   var raySrc = cam.position;                   var rayDir = this.vectorNormalise(                           this.vectorAdd(                               cam.forward,                                this.vectorAdd(this.vectorScale(cam.right, x), this.vectorScale(cam.up, y))                           )                   );                   var chans = this.traceRay(scene, raySrc, rayDir, null, 1);                   for (var i = 0; i < chans.length; i++) {                       if (chans[i] < 0) chans[i] = 0;                       else if (chans[i] > 1) chans[i] = 1;                   }                   return chans;               }       function shapeIntersect(start, dir, shape) {                   switch (shape.type) {                       case 'plane':                           return this.intersectPlane(start, dir, shape);                       case 'sphere':                           return this.intersectSphere(start, dir, shape);                       default:                                                      return null;                   };               }       function intersectPlane(start, dir, plane) {                   var denom = this.vectorDot(dir, plane.normal);                   if (denom == 0) return;                   var res = plane.offset - this.vectorDot(start, plane.normal) / denom;                   if (res <= 0) return;                   return res;               };       function intersectSphere(start, dir, sphere) {                   var y = this.vectorSub(start, sphere.centre);                   var beta = this.vectorDot(dir, y),                       gamma = this.vectorDot(y, y) - sphere.radius * sphere.radius;                   var descriminant = beta * beta - gamma;                   if (descriminant <= 0) return;                   var sqrt = Math.sqrt(descriminant);                   if (-beta - sqrt > 0) return -beta - sqrt;                   else if (-beta + sqrt > 0) return -beta + sqrt;                   else return;               };       function shapeNormal(pos, shape) {                   switch (shape.type) {                       case 'plane':                           return shape.normal;                       case 'sphere':                           return this.sphereNormal(pos, shape);                       default:                           return [];                   };               };       function sphereNormal(pos, sphere) {                   return this.vectorScale(this.vectorSub(pos, sphere.centre), 1/sphere.radius);               };       function shade(pos, dir, shape, scene, contrib) {                   var mat = this.material(shape.surface, pos);                   var norm = this.shapeNormal(pos, shape);                   var reflect = mat[3];                   contrib = contrib * reflect;                   norm = (this.vectorDot(dir, norm) > 0) ? -norm : norm;                   var reflectDir = this.vectorSub(dir, this.vectorScale(norm, 2 * this.vectorDot(norm, dir)));                   var light = this.light(scene, shape, pos, norm, reflectDir, mat);                   if (contrib > 0.01) {                       return this.vectorSum(                           light,                           this.vectorScale(                               this.traceRay(scene, pos, reflectDir, shape, contrib),                               reflect                           )                       );                   }                   else {                       return light;                   }               };       function light(scene, shape, pos, norm, reflectDir, mat) {                   var colour = [mat[0],mat[1],mat[2]],                       reflect = mat[3],                       smooth = mat[4];                   var res = [];                   for (var i = 0; i < scene.lights.length; i++) {                       var lCol = scene.lights[i].colour,                           lPos = scene.lights[i].position;                       var lDir = this.vectorNormalise(this.vectorSub(lPos, pos));                       var lDist = this.vectorLength(this.vectorSub(lPos, pos));                       var tRay = this.testRay(scene, pos, lDir, shape);                       var skip = false;                       for (var j = 0; j < tRay.length; j++) if (tRay[j] < lDist) skip = true;                        if (skip) continue;                       var illum = this.vectorDot(lDir, norm);                       if (illum > 0) res.push(this.vectorScale(this.vectorBlend(lCol, colour), illum));                       var spec = this.vectorDot(lDir, reflectDir);                       if (spec > 0) res.push(this.vectorScale(lCol, Math.pow(spec, smooth) * reflect));                   }                   return this.vectorSumArray(res);               };       function material(name, pos) {                   if (name == 'shiny') return [1, 1, 1, 0.6, 50];                   else if (name == 'checkerboard') {                       return ((Math.floor(pos[0]) + Math.floor(pos[2])) % 2) == 0 ?                               [0, 0, 0, 0.7, 150] :                               [1, 1, 1, 0.1, 50];                   }                   return;               };       function testRay(scene, src, dir, curShape) {                   var res = [];                   for (var i = 0; i < scene.shapes.length; i++) {                       var shape = scene.shapes[i];                       if (shape.id == curShape.id) continue;                       var inter = this.shapeIntersect(src, dir, shape);                       if (inter != null) res.push(inter);                   }                   return res;               };              function traceRay(scene, src, dir, ignore, contrib) {                   var tmp = [];                   for (var i = 0; i < scene.shapes.length; i++) {                       var shape = scene.shapes[i];                       if (ignore && ignore.id == shape.id) continue;                       var dist = this.shapeIntersect(src, dir, shape);                       if (dist == null) continue;                      var pos = this.vectorAdd(src, this.vectorScale(dir, dist));                       tmp.push({dist: dist, pos: pos, shape: shape});                   }                   if (tmp.length == 0) return scene.background;                   else {                       tmp = tmp.sort(function (a, b) { return a.dist - b.dist; });                       return this.shade(tmp[0].pos, dir, tmp[0].shape, scene, contrib);                   }               };       function calculateBasis(scene) {                   var cam = scene.camera;                   cam.forward = this.vectorNormalise(this.vectorSub(cam.lookAt, cam.position));                   cam.right = this.vectorNormalise(this.vectorCross3(cam.forward, [0, -1, 0]));                   cam.up = this.vectorCross3(cam.forward, cam.right);               };       function prepareScene(scene) {                   this.calculateBasis(scene);               };";
            
            var commands = "this.onmessage = function(e) { console.log('Message received from main script, starting execution'); console.log(e.data); var data = getImageData(e.data.scene, e.data.width, e.data.height, e.data.startRow, e.data.endRow); postMessage({results: data}); };";
            
            return taskText + commands;
        }

        // // POST api/values
        // [HttpPost]
        // public void Post([FromBody]string value)
        // {
        // }

        // // PUT api/values/5
        // [HttpPut("{id}")]
        // public void Put(int id, [FromBody]string value)
        // {
        // }

        // // DELETE api/values/5
        // [HttpDelete("{id}")]
        // public void Delete(int id)
        // {
        // }
    }
}
