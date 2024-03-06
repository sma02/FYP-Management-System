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
        private DateTime? initialData = null;
        public string LabelText
        {
            get => labelText;
            set
            {
                labelText = value;
                TextBlockLabel.Text = value;
            }
        }
        public string? SelectedDate
        {
            get
            {
                if(DatePicker1.SelectedDate==null)
                {
                    return null;
                }
                return  DatePicker1.SelectedDate.Value.Date.ToShortDateString();
            }
            set
            {
                if (InitialData == null)
                {
                    InitialData = value;
                }
                DatePicker1.SelectedDate = Convert.ToDateTime(value);
            }
        }
        public string InputAttribute { get; set; }
        public bool IsModified { get { return InitialData != SelectedDate; } }
        public string QueryString { get { return InputAttribute + "=CONVERT(DATETIME,'" + SelectedDate+"',103)"; } }
        public string InitialData
        {
            get => initialData.ToString();
            set
            {
                initialData = Convert.ToDateTime(value);
            }
        }
        public DateEntry()
        {
            DataContext = this;
            InitializeComponent();
        }
    }
}
