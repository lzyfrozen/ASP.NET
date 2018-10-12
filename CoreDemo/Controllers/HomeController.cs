using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static CoreDemo.Controllers.TestController;

namespace CoreDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult TestC()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult TestC(InvDet input)//[FromBody]
        {
            input.OrdBatchNo = Uri.UnescapeDataString(input.OrdBatchNo);
            var strjson = JsonConvert.SerializeObject(input);
            var obj = JsonConvert.DeserializeObject<InvDet>(strjson);
            return Json(obj);
        }
    }
}