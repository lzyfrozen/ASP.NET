using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CoreDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CoreDemo.Controllers
{
    [Authorize]
    //[EnableCors("")]
    public class EmployeeController : Controller
    {
        private readonly MyDbContext _context;
        public EmployeeController(MyDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public JsonResult Index()
        {
            var model = new HomePageViewModel();
            var empService = new EmployeeService(_context);
            model.Employees = empService.GetAll();
            return Json(JsonConvert.SerializeObject(model));
            //return RedirectToAction("List");
        }

        [AllowAnonymous]
        public IActionResult List()
        {
            return View();
        }

        public IActionResult Detail(long id)
        {
            var empService = new EmployeeService(_context);
            var emp = empService.Get(id);
            return View(emp);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var emp = new Employee();
                emp.Code = model.Code;
                emp.Name = model.Name;
                emp.Remark = model.Remark;
                var empService = new EmployeeService(_context);
                empService.Add(emp);
                return RedirectToAction("Detail", new { id = emp.Id });
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            var model = new HomePageViewModel();
            var sqlData = new EmployeeService(_context);
            var emp = sqlData.Get(id);
            if (emp == null)
            {
                return RedirectToAction("Index");
            }
            return View(emp);
        }

        [HttpPost]
        public IActionResult Edit(Employee emp)
        {
            if (ModelState.IsValid)
            {
                var empService = new EmployeeService(_context);
                empService.Edit(emp);
                return RedirectToAction("Detail", new { id = emp.Id });
            }
            return View();
        }

        public IActionResult Delete(long id)
        {
            var empService = new EmployeeService(_context);
            empService.Delete(id);
            return RedirectToAction("Index");
        }
    }

    public class HomePageViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }
    }

    public class EmployeeEditViewModel
    {
        [Required, MaxLength(50)]
        public string Code { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(200)]
        public string Remark { get; set; }
    }
}