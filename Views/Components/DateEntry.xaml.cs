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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FYP_Management_System.Views.Components
{
    /// <summary>
    /// Interaction logic for DateEntry.xaml
    /// </summary>
    public partial class DateEntry : UserControl
    {
        private string labelText;
        public string LabelText
        {
            get => labelText;
            set
            {
                labelText = value;
                TextBlockLabel.Text = value;
            }
        }
        public string SelectedDate
        {
            get => DatePicker1.SelectedDate.Value.Date.ToShortDateString();
        }
        public DateEntry()
        {
            InitializeComponent();
        }
    }
}
