using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreDemo.App_Code;
using CoreDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreDemo.Controllers
{
    /// <summary>
    /// 测试Test
    /// </summary>
    [Route("api/[controller]/[action]")]
    [Authorize(policy: "Client")]
    public class TestController : Controller
    {
        private readonly MyDbContext _context;
        public TestController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/<controller>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<controller>/5
        //[HttpGet("{id}")]
        [HttpGet]
        public string Get()
        {
            return "1234567890";
        }

        /// <summary>
        /// 获取EmployeeList
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetList(Employee input)
        {
            var model = new HomePageViewModel();
            var empService = new EmployeeService(_context);
            model.Employees = empService.GetAll();
            return Json(JsonConvert.SerializeObject(model));
            //return RedirectToAction("List");
        }

        /// <summary>
        /// GetToken
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <param name="expiresSliding"></param>
        /// <param name="expiresAbsoulte"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public string GetToken(TokenModel tokenModel, TimeSpan expiresSliding, TimeSpan expiresAbsoulte)
        {
            return Token.IssueJWT(tokenModel, expiresSliding, expiresAbsoulte);
        }

        //// POST api/<controller>
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
