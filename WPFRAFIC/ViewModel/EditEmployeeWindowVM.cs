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
    public class EditEmployeeWindowVM : BaseVM
    {
        EditEmployeeWindow editEmployeeWindow;

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
        public CommandVM EditNewEmployee { get; }
        public EditEmployeeWindowVM()
        {
            EditNewEmployee = new CommandVM(async () =>
            {
                string arg = JsonSerializer.Serialize(Employee);
                var responce = await HttpClientSingle.HttpClient.PostAsync($"Admin/EditEmployee", new StringContent(arg, Encoding.UTF8, "application/json"));
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
                    MessageBox.Show("Сотрудник успешно обновлен");
                    editEmployeeWindow.Close();

                }
            });
        }

        internal void SetWindow(EditEmployeeWindow editEmployeeWindow)
        {
            this.editEmployeeWindow = editEmployeeWindow;
        }
    }
}

