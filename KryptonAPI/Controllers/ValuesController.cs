using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using KryptonAPI.Models;

namespace KryptonAPI.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private static int numRequests = 0;
        private static object numLock = new object();
        
        // GET: api/values
        [HttpGet]
        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        public IEnumerable<string> Get()
        {
            //System.Console.WriteLine("user name: " + User.Identity.Name);
            
            /*
            using (var db = new BloggingContext())
            {
                db.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);

                Console.WriteLine();
                Console.WriteLine("All blogs in database:");
                foreach (var blog in db.Blogs)
                {
                    Console.WriteLine(" - {0}", blog.Url);
                }
            }
            */
            
            lock(numLock){
                numRequests++;
                Console.WriteLine("Number of requests so far: " + numRequests.ToString());
            }
            
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
