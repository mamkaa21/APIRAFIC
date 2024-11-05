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
    public class AdminWindowVM: BaseVM
    {
        HttpClient httpClient = new HttpClient();
        public Employee Employee { get; set; }
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public CommandVM OpenAddNewEmployee { get; }
        public CommandVM OpenEditEmployee { get; }
        public CommandVM OpenStatistic { get; }
        public CommandVM OpenEditSchedule { get; }
        public CommandVM OpenTasks { get; }
        public AdminWindowVM()
        {
            httpClient.BaseAddress = new Uri("http://localhost:5062/api/");
            OpenAddNewEmployee = new CommandVM(async () =>
            {
                AddNewEmployeeWindow addNewEmployeeWindow = new AddNewEmployeeWindow();
                addNewEmployeeWindow.Show();
            });
            OpenEditEmployee = new CommandVM(() =>
            {                                   
                EditEmployeeWindow editEmployeeWindow = new EditEmployeeWindow();
                editEmployeeWindow.Show();
            }
            );
        }
    }
}

