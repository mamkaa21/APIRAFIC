using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFRAFIC.Model;
using WPFRAFIC.ViewModel;

namespace WPFRAFIC.View
{
    /// <summary>
    /// Логика взаимодействия для EditEmployeeWindow.xaml
    /// </summary>
    public partial class EditEmployeeWindow : Window
    {
        public EditEmployeeWindow(Employee employee)
        {
            InitializeComponent();
            ((EditEmployeeWindowVM)DataContext).Employee = employee;
            ((EditEmployeeWindowVM)DataContext).SetWindow(this);
        }
    }
}
