using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        public async Task<ActionResult<ResponceTokenAndEmployee>> CheckAccountIsExist(EmployeeModel employee)
        {
            var newEmployee = new Employee { Id = employee.Id, IsBlocked = employee.IsBlocked, Lastlogin = employee.Lastlogin, Login = employee.Login, Password = employee.Password, RegistrationDate = employee.RegistrationDate, RoleId = employee.RoleId };

            if (string.IsNullOrEmpty(newEmployee.Login) || string.IsNullOrEmpty(newEmployee.Password))
                return BadRequest("Введите логин и пароль");
            
            var user = await _context.Employees.Include(s => s.Role).FirstOrDefaultAsync(s => s.Login == newEmployee.Login);
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
                    string role = user.Role.Title;
                    int id = user.Id;

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                        new Claim(ClaimTypes.Role, role)
                    };
                    var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    //кладём полезную нагрузку
                    claims: claims,
                    //устанавливаем время жизни токена 2 минуты
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

                    string token = new JwtSecurityTokenHandler().WriteToken(jwt);

                    return Ok(new ResponceTokenAndEmployee
                    {
                        Token = token,
                        Role = role,
                        Employee = user
                    });
                }
            }
        }

        public class ResponceTokenAndEmployee
        {
            public string Token { get; set; }
            public string Role { get; set; }

            public Employee Employee { get; set; }
        }
        public class AuthOptions
        {
            public const string ISSUER = "MyAuthServer"; // издатель токена
            public const string AUDIENCE = "MyAuthClient"; // потребитель токена
            const string KEY = "mysupersecret_secretsecretsecretkey!123";   // ключ для шифрации
            public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }

        [Authorize(Roles = "admin, loh")]
        [HttpPost("ChangeOldPassword")]
        public async Task<ActionResult> ChangeOldPassword(CheckPassword check)
        {
            var test = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(test))
                return BadRequest();
            if (!int.TryParse(test, out int userid))
                return BadRequest();

            var user = await _context.Employees.FirstOrDefaultAsync(s => s.Id == userid);
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
