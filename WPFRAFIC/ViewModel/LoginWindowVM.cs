
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using WPFRAFIC.View;
using WPFRAFIC.Model;
using System.Net.Http.Json;

namespace WPFRAFIC.ViewModel
{
    public class LoginWindowVM: BaseVM
    {
        HttpClient httpClient = new HttpClient();
        
        public Employee Employee { get; set; } = new Employee();
        public CommandVM LogIn { get; }
        public LoginWindowVM()
        {
            httpClient.BaseAddress = new Uri("http://localhost:5062/api/");
            LogIn = new CommandVM(async () =>
            {
                string arg = JsonSerializer.Serialize(Employee);
                var responce = await httpClient.PostAsync($"Authorization/CheckAccountIsExist", new StringContent(arg, Encoding.UTF8, "application/json"));
               
                if (responce.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var result = await responce.Content.ReadAsStringAsync();
                    MessageBox.Show("Вы ввели неверный логин или пароль. Пожалуйста проверьте ещё раз введенные данные. Помните, у вас есть три попытки ввести верный пароль.");
                    return;
                }
                if(responce.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var result = await responce.Content.ReadAsStringAsync();
                    MessageBox.Show("Вы заблокированы. Обратитесь к администратору.");
                    return;
                }
                if (responce.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Employee = await responce.Content.ReadFromJsonAsync<Employee>();
                    MessageBox.Show("Вы успешно авторизовались");
                    if (Employee.Lastlogin == null)
                    {
                        NewPasswordWindow newPasswordWindow = new NewPasswordWindow(Employee);
                        newPasswordWindow.Show();
                    }
                    if (Employee.RoleId == 1)
                    {
                        AdminWindow adminWindow = new AdminWindow();
                        adminWindow.Show();
                    }
                    else
                    {
                        EmployeeWindow employeeWindow = new EmployeeWindow();
                        employeeWindow.Show();
                    }
                    
                }
                else
                {
                    var result = await responce.Content.ReadAsStringAsync();
                    MessageBox.Show("Ошибка подключения");
                    return;
                }              
                

            });
        }
    }
}
