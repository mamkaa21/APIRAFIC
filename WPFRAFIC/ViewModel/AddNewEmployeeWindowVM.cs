using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFRAFIC.View;
using WPFRAFIC.Model;
using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace WPFRAFIC.ViewModel
{
    public class AddNewEmployeeWindowVM
    {
        HttpClient httpClient = new HttpClient();
        public Employee Employee { get; set; } = new Employee();
        //public List<Employee> Employees { get; set; } = new List<Employee>();
        public CommandVM AddNewEmployee { get; }
        public AddNewEmployeeWindowVM()
        {
            httpClient.BaseAddress = new Uri("http://localhost:5062/api/");
            AddNewEmployee = new CommandVM(async () =>
            {
                string arg = JsonSerializer.Serialize(Employee);
                var responce = await httpClient.PostAsync($"Admin/AddNewEmployee", new StringContent(arg, Encoding.UTF8, "application/json"));
                if (responce.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    var result = await responce.Content.ReadAsStringAsync();
                    MessageBox.Show("Ошибка подключения");
                    return;
                }             
                else
                {
                    var result = await responce.Content.ReadAsStringAsync();
                    MessageBox.Show("Сотрудник успешно добавлен");                  
                }              
            });
        }
    }
}

