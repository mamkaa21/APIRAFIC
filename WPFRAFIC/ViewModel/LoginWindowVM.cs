
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
        public Employee Employee { get; set; } = new Employee();
        LoginWindow loginWindow;
        public CommandVM LogIn { get; }
        public LoginWindowVM()
        {
            LogIn = new CommandVM(async () =>
            {
                string arg = JsonSerializer.Serialize(Employee);
                var responce = await HttpClientSingle.HttpClient.PostAsync($"Authorization/CheckAccountIsExist", new StringContent(arg, Encoding.UTF8, "application/json"));
               
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
                        loginWindow.Close();
                    }
                    else
                    {
                        if (Employee.RoleId == 1)
                        {
                            AdminWindow adminWindow = new AdminWindow();
                            adminWindow.Show();
                            loginWindow.Close();

                        }
                        else
                        {
                            EmployeeWindow employeeWindow = new EmployeeWindow();
                            employeeWindow.Show();
                            loginWindow.Close();
                        }
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

        internal void SetWindow(LoginWindow loginWindow)
        {
            this.loginWindow = loginWindow;
        }
    }
}
