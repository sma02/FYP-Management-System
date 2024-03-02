using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
    /// Interaction logic for DataLabel.xaml
    /// </summary>
    public partial class DataLabel : UserControl,INotifyPropertyChanged
    {
        private string textData;
        private string textLabel;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string TextData
        {
            get => textData; 
            set
            {
                textData = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextData)));
            }
        }

        public string TextLabel
        {
            get => textLabel; set
            {
                textLabel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextLabel)));

            }
        }

        public DataLabel()
        {
            DataContext = this;
            InitializeComponent();
        }
    }
}
