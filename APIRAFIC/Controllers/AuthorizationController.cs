using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace APIRAFIC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        readonly User02Context _context;
        private readonly BlockedUsers blockedUsers;

        public AuthorizationController(User02Context _context, BlockedUsers blockedUsers)
        {
            this._context = _context;
            this.blockedUsers = blockedUsers;
        }

        [HttpPost("CheckAccountIsExist")]
        public async Task<ActionResult<Employee>> CheckAccountIsExist(EmployeeModel employee)
        {
            var newEmployee = new Employee { Id = employee.Id, IsBlocked = employee.IsBlocked, Lastlogin = employee.Lastlogin, Login = employee.Login, Password = employee.Password, RegistrationDate = employee.RegistrationDate, RoleId = employee.RoleId };

            if (string.IsNullOrEmpty(newEmployee.Login) || string.IsNullOrEmpty(newEmployee.Password))
                return BadRequest("Введите логин и пароль");

            var user = await _context.Employees.FirstOrDefaultAsync(s => s.Login == newEmployee.Login);
            if (user == null)
                return NotFound("Такой пользователь не найден. Проверьте введенные данные.");
            else
            {
                if (newEmployee.Password != user.Password)
                {
                    var current_c = blockedUsers.AddBadLogin(user.Id);
                    if (current_c >= 3 || user.Lastlogin != null && DateTime.Now >= user.Lastlogin.Value.AddMonths(1) || DateTime.Now >= user.RegistrationDate.Value.AddMonths(1))
                    {
                        user.IsBlocked = 1;
                        await _context.SaveChangesAsync();
                        return BadRequest("Вы заблокированы. Обратитесь к администратору");
                    }
                    return NotFound($"Вы ввели неверный пароль. Пожалуйста проверьте еще раз введенные данные. Использована {current_c} попытки из 3");
                }
                else
                {
                    if (user.IsBlocked == 1)
                        return BadRequest("Вы заблокированы. Обратитесь к администратору");
                    if (user.Lastlogin != null)
                    {
                        user.Lastlogin = DateTime.Now;
                        await _context.SaveChangesAsync();
                    }
                    return Ok(user);
                }
            }
        }

        [HttpPost("ChangeOldPassword")]
        public async Task<ActionResult> ChangeOldPassword(CheckPassword check)
        {
            var user = await _context.Employees.FirstOrDefaultAsync(s => s.Id == check.IdEmployee);
            if (user == null)
                return NotFound("Такого пользователя нет");
            else
            {
                if (string.IsNullOrEmpty(check.PasswordOld))
                    return NotFound("Введите старый пароль");

                var checkOld = await _context.Employees.FirstOrDefaultAsync(s => s.Password == check.PasswordOld);
                if (checkOld.Password == check.Password1)
                    return BadRequest("Новый пароль не должен совпадать со старым");

                if (check.Password1 != check.Password2)
                    return BadRequest("Неверный пароль. Пароли не совпадают.");

                checkOld.Password = check.Password2;
                await _context.SaveChangesAsync();
                return Ok("Пароль успешно изменен");
            }
        }
    }
}
