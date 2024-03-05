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
    /// Interaction logic for SearchBar.xaml
    /// </summary>
    public partial class SearchBar : UserControl
    {
        public string Text { get=>SearchTextBox.Text; set=>SearchTextBox.Text=value; }
        public List<string> SearchAttributes { get; set; }
        public string FilterString {
            get
            {
                string[] strings = SearchAttributes.ToArray();
                for (int i=0;i<SearchAttributes.Count;i++)
                {
                    strings[i] += " LIKE '*" + Text + "*'";
                }
               string filter =  String.Join(" OR ", strings);
                return filter;
            } }
        public event EventHandler<EventArgs> SearchRequested;
        public SearchBar()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SearchRequested?.Invoke(this, new EventArgs());
        }

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchRequested?.Invoke(this, new EventArgs());
            }
        }
    }
}
