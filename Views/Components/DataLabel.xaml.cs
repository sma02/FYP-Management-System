using Microsoft.IdentityModel.Tokens;
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
using static System.Net.Mime.MediaTypeNames;

namespace FYP_Management_System.Views.Components
{
    /// <summary>
    /// Interaction logic for DataLabel.xaml
    /// </summary>
    public partial class DataLabel : UserControl,INotifyPropertyChanged
    {
        private string? textData;
        private string? textLabel;
        private string initialData = null;
        public event PropertyChangedEventHandler? PropertyChanged;

        public object? TextData
        {
            get
            {
                if (textData.IsNullOrEmpty())
                {
                    return DBNull.Value;
                }
                return textData;
            }

            set
            {
                textData = (string?)value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextData)));
            }
        }
        public string InputAttribute { get; set; }
        public bool IsModified { get { return InitialData != TextData; } }
        public string QueryString
        {
            get
            {
                object text = TextData;
                if (text == DBNull.Value)
                {
                    return InputAttribute + "=NULL ";
                }
                if (((string)text).Contains('\''))
                    text = ((string)text).Replace("'", "''");
                return InputAttribute + "='" + ((string)text) + "'";
            }
        }
        public string? InitialData
        {
            get => initialData;
            set
            {
                initialData = value;
                TextData = value;
            }
        }
        public string? TextLabel
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
