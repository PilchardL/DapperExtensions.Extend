using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DapperExtensions.Extend;
using ApiDemo.Models;

namespace ApiDemo.Controllers
{
    public class UserController : ApiController
    {
        private SqlQueryBuilder builder;

        public UserController()
        {
            builder = new SqlQueryBuilder();
        }
        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            //var sql = context.Join<user, user>((x, y) => x.Id == y.Id && x.Name == x.Name);
            var sql = builder.Select<user>(x => x.Id);
            return Json(sql);
        }
    }
}
