using QLNSService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QLNSService.Models;

namespace QLNSService.Controllers
{
    public class ValuesController : ApiController
    { 

        // POST api/values
        public ModelSelectItem[] Get()
        {
           var  db = new QLNSEntities();
             var query = db.TaiKhoans ;
            //return db.TaiKhoans.ToList();
             return query.Select(x => new ModelSelectItem()  ).ToArray();
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
            var a = 1;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
            var a = 1;
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            var a = 1;
        }


    }
}