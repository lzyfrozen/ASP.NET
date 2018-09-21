using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Models
{
    public class EmployeeService
    {
        private MyDbContext _context;
        public EmployeeService(MyDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Employee> GetAll()
        {
            if (_context.Employees == null || _context.Employees.Count() == 0)
            {
                return new List<Employee>();
            }
            return _context.Employees.ToList();
        }

        public Employee Get(long id)
        {
            return _context.Employees.FirstOrDefault(l => l.Id == id);
        }
        public void Add(Employee emp)
        {
            _context.Employees.Add(emp);
            _context.SaveChanges();
        }
        public void Edit(Employee emp)
        {
            var entity = _context.Employees.FirstOrDefault(l => l.Id == emp.Id);
            entity.Code = emp.Code;
            entity.Name = emp.Name;
            entity.Remark = emp.Remark;
            _context.SaveChanges();
        }
        public void Delete(long id)
        {
            var emp = _context.Employees.FirstOrDefault(l => l.Id == id);
            _context.Employees.Remove(emp);
            _context.SaveChanges();
        }
    }
}
