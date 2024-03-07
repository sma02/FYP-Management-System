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
    /// Interaction logic for TextEntry.xaml
    /// </summary>
    public partial class TextEntry : UserControl
    {
        private int maxLength;
        private string initialData = null;
        public string LabelText
        {
            get => TextBlockLabel.Text; 
            set
            {
                TextBlockLabel.Text = value;
            }
        }
        public string Text
        {
            get => TextBoxText.Text; 
            set
            {
                if(InitialData == null)
                {
                    InitialData = value;
                }
                TextBoxText.Text = value;
            }
        }
        public string InputAttribute { get; set; }
        public bool IsModified { get { return InitialData != Text; } }
        public string QueryString { get {
                string text = Text;
                if (text.Contains('\''))
                    text = text.Replace("'", "''");
                return InputAttribute + "='" + text + "'"; } }
        public string InitialData
        {
            get => initialData;
            set
            {
                initialData = value;
                Text = value;
            }
        }
        public int MaxLength
        {
            get => maxLength; 
            set
            {
                maxLength = value;
                TextBoxText.MaxLength = value;
            }
        }

        public TextEntry()
        {
            DataContext = this;
            InitializeComponent();
        }

    }
}
