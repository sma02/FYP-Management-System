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

namespace FYP_Management_System.Views
{
    /// <summary>
    /// Interaction logic for StudentView.xaml
    /// </summary>
    public partial class StudentView : Page
    {
        public StudentView()
        {
            InitializeComponent();
            var conn = Configuration.getInstance().getConnection();
            SqlCommand command = new SqlCommand(@"SELECT RegistrationNo
                                                        ,FirstName
                                                        ,LastName
                                                        ,Lookup.Value Gender
                                                        ,Contact,Email
                                                        ,CONVERT(VARCHAR,DateOfBirth,106) DateOfBirth
                                                  FROM Person 
                                                  JOIN Student 
                                                  ON Student.ID=Person.ID
                                                  JOIN Lookup
                                                  ON Gender=Lookup.Id AND Lookup.Category='GENDER'", conn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            DG1.ItemsSource = table.DefaultView;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Content = new Views.Components.StudentEntryView();
        }
    }
}
