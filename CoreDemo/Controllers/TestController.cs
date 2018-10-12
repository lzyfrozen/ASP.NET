using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using CoreDemo.App_Code;
using CoreDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Dapper;
using System.Data.SqlClient;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreDemo.Controllers
{
    /// <summary>
    /// 测试Test
    /// </summary>
    [Route("api/[controller]/[action]")]
    //[Authorize(policy: "Client")]
    public class TestController : Controller
    {
        public IConfiguration Configuration { get; set; }
        private readonly MyDbContext _context;
        public TestController(MyDbContext context)
        {
            _context = context;
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("AppSettings.json");
            Configuration = builder.Build();
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

        [HttpPost]
        [AllowAnonymous]
        public void TestB()
        {
            var strConn = Configuration["database:connection:sqlerver_default"];

            var query = _context.Employees.Where(l => l.Name == "12345").ToList();
            var a = query.Any();

        }

        [HttpPost]
        [AllowAnonymous]
        public void TestA()
        {
            var strConn = Configuration["database:connection:sqlerver_default"];

            using (IDbConnection conn = new SqlConnection(strConn))
            {
                var sql = @"SELECT bin.BinCode,lot.La2,bin.PickSequence,det.OrdBatchNo FROM dbo.Stk_InventoryDet det 
                        LEFT JOIN dbo.Base_WarehouseBin bin ON det.BinId = bin.Id AND bin.IsDeleted = 0
                        LEFT JOIN dbo.Base_LotAttrValue lot ON det.LotValueId = lot.Id AND bin.IsDeleted = 0
                        WHERE det.InventoryDate = '2018-09-28' AND det.Qty - det.AllocatedQty - det.AdjustingQty - det.OtherLockQty > 0
                        AND det.CargoId = 'D7978F3C-A325-4551-B3B0-5396B8AE7ECD'
                        AND det.Sloc = '660dce9c-cc53-e611-b641-005056ba4c6d'
                        AND bin.BinUseType = '2'";
                //ORDER BY lot.La2,bin.PickSequence,det.OrdBatchNo";
                var qery = conn.Query<InvDet>(sql);
                var lst1 = qery.OrderBy(l => l.La2).ThenBy(l => l.PickSequence).ThenBy(l => l.OrdBatchNo);
                var lst2 = from q in qery
                           orderby q.La2 ascending, q.PickSequence ascending, q.OrdBatchNo ascending
                           select q;
                var qery1 = conn.Query<InvDet>($"{sql} ORDER BY lot.La2,bin.PickSequence,det.OrdBatchNo");
                var a1 = lst1.ToList().FirstOrDefault().BinCode;
                var a2 = lst2.ToList().FirstOrDefault().BinCode;
                var a3 = qery1.FirstOrDefault().BinCode;
                var str = $"{a1}--{a2}--{a3}";
            }

        }

        public class InvDet
        {
            public string BinCode { get; set; }
            public string La2 { get; set; }
            public string PickSequence { get; set; }
            public string OrdBatchNo { get; set; }

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
