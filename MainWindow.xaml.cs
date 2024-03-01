using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace FYP_Management_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Configuration.getInstance().getConnection();
            InitializeComponent();
        }
        private void BtnManageStudents_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Source = new Uri("/Views/StudentView.xaml", UriKind.Relative);

        }

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Source = new Uri("/Views/StudentEntryView.xaml", UriKind.Relative);
        }

        private void BtnManageProjects_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Source = new Uri("/Views/ProjectView.xaml", UriKind.Relative);
        }

        private void BtnManageAdvisors_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Source = new Uri("/Views/AdvisorView.xaml", UriKind.Relative);
        }
    }
}
