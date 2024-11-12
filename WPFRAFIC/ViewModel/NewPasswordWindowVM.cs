
using APIRAFIC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using WPFRAFIC.Model;
using WPFRAFIC.View;

namespace WPFRAFIC.ViewModel
{
    public class NewPasswordWindowVM : BaseVM
    {
        HttpClient httpClient = new HttpClient();
        private Employee employee;

        public Employee Employee
        {
            get => employee;
            set
            {
                employee = value;
                CheckPassword.IdEmployee = employee.Id;
                Signal();
            }
        }
        public CheckPassword CheckPassword { get; set; } = new CheckPassword();
        public CommandVM ChangePassword { get; }
        public NewPasswordWindowVM()
        {
            httpClient.BaseAddress = new Uri("http://localhost:5062/api/");
            ChangePassword = new CommandVM(async () =>
            {
                string arg = JsonSerializer.Serialize(CheckPassword);
                var responce = await httpClient.PostAsync($"Authorization/ChangeOldPassword", new StringContent(arg, Encoding.UTF8, "application/json"));
                if (responce.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var result = await responce.Content.ReadAsStringAsync();
                    MessageBox.Show(result.ToString());
                    return;
                }
                if (responce.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var result = await responce.Content.ReadAsStringAsync();
                    MessageBox.Show(result.ToString());
                    return;
                }
                if (responce.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await responce.Content.ReadAsStringAsync();
                    MessageBox.Show(result.ToString());
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
