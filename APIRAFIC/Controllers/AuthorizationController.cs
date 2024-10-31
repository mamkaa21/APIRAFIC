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

        public AuthorizationController(User02Context _context)
        {
            this._context = _context;
        }

        [HttpPost("CheckAccountIsExist")]
        public async Task<ActionResult<Employee>> CheckAccountIsExist(Employee employee)
        {
            if (string.IsNullOrEmpty(employee.Login) || string.IsNullOrEmpty(employee.Password))
                return BadRequest("Введите логин и пароль");
            var check = await _context.Employees.FirstOrDefaultAsync(s => s.Login == employee.Login
            && s.Password == employee.Password);
            if (check != null)
            {
                return Ok(check);
            }
            if (check == null)
            {
                return NotFound("Вы ввели неверный логин или пароль. Пожалуйста проверьте еще раз введенные данные");
            }
            return check;


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
                    return BadRequest("Введите старый пароль");
                var checkOld = await _context.Employees.FirstOrDefaultAsync(s => s.Password == check.PasswordOld);
                if (checkOld.Password == check.Password1)
                {
                    return BadRequest("Новый пароль не должен совпадать со старым");
                }
                if (check.Password1 != check.Password2)
                {
                    return BadRequest("Неверный пароль");
                }
                checkOld.Password = check.Password2;
                await _context.SaveChangesAsync();
                return Ok("Пароль успешно изменен");
            }


        }
    }
}
