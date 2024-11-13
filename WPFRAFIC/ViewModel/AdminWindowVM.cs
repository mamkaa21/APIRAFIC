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
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Windows.Controls.Primitives;

namespace WPFRAFIC.ViewModel
{
    public class AdminWindowVM : BaseVM
    {
        HttpClient httpClient = new HttpClient();
        private Employee employee;
        public Employee Employee
        {
            get => employee;
            set
            {
                employee = value;
                Signal(nameof(Employee));
            }
        }
        private List<Employee> employees;
        public List<Employee> Employees
        {
            get => employees;
            set
            {
                employees = value;
                Signal(nameof(Employees));
            }
        }

        public CommandVM OpenAddNewEmployee { get; }
        public CommandVM OpenEditEmployee { get; }
        public CommandVM UnblockEmployee { get; }
        public CommandVM OpenStatistic { get; }
        public CommandVM OpenEditSchedule { get; }
        public CommandVM OpenTasks { get; }

        JsonSerializerOptions options = new JsonSerializerOptions();
        public AdminWindowVM()
        {
            httpClient.BaseAddress = new Uri("http://localhost:5062/api/");
            options = new JsonSerializerOptions { ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles, PropertyNameCaseInsensitive = true };
            GetEmployees();
            OpenAddNewEmployee = new CommandVM(() =>
            {
                AddNewEmployeeWindow addNewEmployeeWindow = new AddNewEmployeeWindow();
                addNewEmployeeWindow.Show();
            });
            OpenEditEmployee = new CommandVM(() =>
            {
                EditEmployeeWindow editEmployeeWindow = new EditEmployeeWindow(Employee);
                editEmployeeWindow.Show();
            }
            );
            UnblockEmployee = new CommandVM(async () =>
            {
                string arg = JsonSerializer.Serialize(Employee);
                var responce = await httpClient.PostAsync($"Admin/UnblockEmployee", new StringContent(arg, Encoding.UTF8, "application/json"));
                if (responce.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    var result = await responce.Content.ReadAsStringAsync();
                    MessageBox.Show("Ошибка подключения");
                    return;
                }
                else
                {
                    var result = await responce.Content.ReadAsStringAsync();
                    MessageBox.Show("Вы разблокировали пользователя!");
                    GetEmployees();
                    Signal(nameof(Employees));
                    Signal(nameof(Employee));
                }
            });
        }
        public async void GetEmployees()
        {
            var responce = await httpClient.PostAsync($"Admin/GetEmployees", new StringContent("", Encoding.UTF8, "application/json"));
            if (responce.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var result = await responce.Content.ReadAsStringAsync();
                MessageBox.Show("Ошибка подключения");
                return;
            }
            else
            {
                var employees = await responce.Content.ReadFromJsonAsync<List<Employee>>(options);
                Employees = new List<Employee>(employees);
                Signal(nameof(Employees));
                Signal(nameof(Employee));

            }
        }
    }
}

