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
using System.Runtime.InteropServices;

namespace WPFRAFIC.ViewModel
{
    public class AddNewEmployeeWindowVM: BaseVM
    {
        HttpClient httpClient = new HttpClient();
        AddNewEmployeeWindow addNewEmployeeWindow;
        public Employee Employee { get; set; } = new Employee();
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
                    Signal(nameof(Employee));
                    MessageBox.Show("Сотрудник успешно добавлен");
                    addNewEmployeeWindow.Close();
                }              
            });
        }

        internal void SetWindow(AddNewEmployeeWindow addNewEmployeeWindow)
        {
            this.addNewEmployeeWindow = addNewEmployeeWindow;
        }
    }
}

