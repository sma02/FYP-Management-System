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
                TextBoxText.Text = value;
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
            InitializeComponent();
        }

    }
}
