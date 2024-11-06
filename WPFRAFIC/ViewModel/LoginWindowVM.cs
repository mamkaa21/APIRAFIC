
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
                if (responce.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    var result = await responce.Content.ReadAsStringAsync();
                    MessageBox.Show("Ошибка подключения");
                    return;
                }
                if (responce.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var result = await responce.Content.ReadAsStringAsync();
                    MessageBox.Show("Вы ввели неверный логин или пароль. Пожалуйста проверьте ещё раз введенные данные");
                    return;
                }
                else
                {
                    var result = await responce.Content.ReadAsStringAsync();
                    MessageBox.Show("Вы успешно авторизовались");                
                }              
                var responce2 = await httpClient.PostAsync($"Authorization/BlockAccountByThreeWrongPassword", new StringContent(arg, Encoding.UTF8, "application/json"));
                if (responce2.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    var result2 = await responce2.Content.ReadAsStringAsync();
                    MessageBox.Show("Ошибка подключения");
                    return;
                }
                if (responce2.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var result2 = await responce.Content.ReadAsStringAsync();
                    MessageBox.Show("Вы ввели неверный логин или пароль. Пожалуйста проверьте ещё раз введенные данные");
                    return;
                }
                else
                {
                    var result2 = await responce2.Content.ReadAsStringAsync();
                    MessageBox.Show("Вы заблокированы");
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.Show();
                }

            });
        }
    }
}
