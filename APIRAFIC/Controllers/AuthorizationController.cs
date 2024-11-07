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
        public async Task<ActionResult<Employee>> CheckAccountIsExist(Employee employee)
        {
            if (string.IsNullOrEmpty(employee.Login) || string.IsNullOrEmpty(employee.Password))
                return BadRequest("Введите логин и пароль");
            
            var user = await _context.Employees.FirstOrDefaultAsync(s => s.Login == employee.Login);
            if (user == null)
            {
                return NotFound("Такой пользователь не найден");
            }
            else
            {
                
                if (employee.Login != user.Login)
                {
                    return NotFound("Неверный логин");
                }
                else 
                {
                    if (employee.Password != user.Password)
                    {
                        var current_c = blockedUsers.AddBadLogin(user.Id);
                        if (current_c >= 3)
                        {
                            user.IsBlocked = 1;
                            await _context.SaveChangesAsync();
                            return BadRequest("Вы заблокированы. Обратитесь к администратору");
                        }
                        /*current_c += 1;
                        if (current_c >= 3)
                        {
                            employee.IsBlocked = 1;
                            await _context.SaveChangesAsync();
                            return BadRequest("Вы заблокированы. Обратитесь к администратору");
                        }*/
                        return NotFound($"Вы ввели неверный пароль. Пожалуйста проверьте еще раз введенные данные. Использована {current_c} попытки из 3");
                    }
                    
                    else
                    {
                        if(employee.IsBlocked == 1)
                        {
                            return BadRequest("Вы заблокированы. Обратитесь к администратору");
                        }
                        user.Lastlogin = DateTime.Now;
                        await _context.SaveChangesAsync();
                        return Ok(employee);
                    }
                }      
            }
            //var check = await _context.Employees.FirstOrDefaultAsync(s => s.Login == employee.Login
            //&& s.Password == employee.Password);
            //if (check != null)
            //{
            //    return Ok(check);
            //}
            //if (check == null)
            //{
            //    return NotFound("Вы ввели неверный логин или пароль. Пожалуйста проверьте еще раз введенные данные");
            //}
            ////check.last = DateTime.Now;
            ////
            //return check;
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

        //[HttpPost("BlockAccountByThreeWrongPassword")]
        //public async Task<ActionResult<Employee>> BlockAccountByThreeWrongPassword(Employee employee)
        //{
        //    if (string.IsNullOrEmpty(employee.Login) || string.IsNullOrEmpty(employee.Password))
        //        return BadRequest("Введите логин и пароль");
        //    var check = await _context.Employees.FirstOrDefaultAsync(s => s.Login == employee.Login
        //    && s.Password == employee.Password);
        //    int c = 0;
        //    if (check == null)
        //    {
        //        c++;
        //        return NotFound("Вы ввели неверный логин или пароль. Пожалуйста проверьте еще раз введенные данные");
        //    }
        //    if (c == 3)
        //    {
        //        return Ok("Вы заблокированы. Обратитесь к администратору");
        //    }
        //    return check;

        //}

        //[HttpPost("BlockAccountForNoAuthorizationByMonth")]
        //public ActionResult<Employee> BlockAccountForNoAuthorizationByMonth(Employee employee)
        //{

        //}


    }
}
