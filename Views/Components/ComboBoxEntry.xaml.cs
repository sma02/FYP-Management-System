using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static System.Net.Mime.MediaTypeNames;

namespace FYP_Management_System.Views.Components
{
    /// <summary>
    /// Interaction logic for ComboBoxEntry.xaml
    /// </summary>
    public partial class ComboBoxEntry : UserControl
    {
        public string LabelText
        {
            get => TextBlockLabel.Text;
            set
            {
                TextBlockLabel.Text = value;
            }
        }
        public object SelectedItem
        {
            get => ComboBox1.SelectedItem;
        }
        public List<string> Items
        {
            get => (List<string>)ComboBox1.ItemsSource; 
            set
            {
                ComboBox1.ItemsSource = value;
            }
        }
        public SqlDataReader ItemsRead
        {
            set
            {
                List<string> items = new List<string>();
                while (value.Read())
                {
                    items.Add(value.GetString(0));
                }
                value.Close();
                Items = items;
            }
        }
        public ComboBoxEntry()
        {
            InitializeComponent();
        }

        private void ComboBox1_Drop(object sender, DragEventArgs e)
        {

        }
    }
}
