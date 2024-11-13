using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks.Dataflow;

namespace APIRAFIC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        readonly User02Context _context;

        public AdminController(User02Context _context)
        {
            this._context = _context;
        }

        [HttpPost("AddNewEmployee")]
        public async Task<ActionResult<Employee>> AddNewEmployee(EmployeeModel employee)
        {
            var newEmployee = new Employee { Id = employee.Id, IsBlocked = employee.IsBlocked, Lastlogin = employee.Lastlogin, Login = employee.Login, Password = employee.Password, RegistrationDate = employee.RegistrationDate, RoleId = employee.RoleId };

            if (string.IsNullOrEmpty(newEmployee.Login))
                return BadRequest("Вы не ввели логин");
            var check = await _context.Employees.FirstOrDefaultAsync(s => s.Login == newEmployee.Login);
            if (check == null)
            {
                newEmployee.RoleId = 2;
                newEmployee.RegistrationDate = DateTime.Now;
                _context.Employees.Add(newEmployee);
                await _context.SaveChangesAsync();
                return Ok("Новый сотрудник добавлен");
            }
            else
                return BadRequest("Такой аккаунт уже существует");
        }

        [HttpPost("EditEmployee")]
        public async Task<ActionResult<Employee>> EditEmployee(EmployeeModel employee)
        {
            var newEmployee = new Employee { Id = employee.Id, IsBlocked = employee.IsBlocked, Lastlogin = employee.Lastlogin, Login = employee.Login, Password = employee.Password, RegistrationDate = employee.RegistrationDate, RoleId = employee.RoleId };
            _context.Employees.Update(newEmployee); 
            await _context.SaveChangesAsync();
            return Ok("Сотрудник обновлен");
        }

        [HttpPost("UnblockEmployee")]
        public async Task<ActionResult<Employee>> UnblockEmployee(EmployeeModel employee)
        {
            var newEmployee = new Employee { Id = employee.Id, IsBlocked = employee.IsBlocked, Lastlogin = employee.Lastlogin, Login = employee.Login, Password = employee.Password, RegistrationDate = employee.RegistrationDate, RoleId = employee.RoleId  };
            var user = await _context.Employees.FirstOrDefaultAsync(s => s.Login == newEmployee.Login);
            user.IsBlocked = 0;
            await _context.SaveChangesAsync();
            return Ok("Сотрудник разблокирован!");
        }

        [HttpPost("GetEmployees")]
        public async Task<List<Employee>> GetEmployees()
        {
            var employees = _context.Employees.Include(s => s.Role).Where(s=>s.RoleId == 2).ToList();
            return employees;
        }
    }
}
