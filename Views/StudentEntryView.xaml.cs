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

namespace FYP_Management_System.Views.Components
{
    /// <summary>
    /// Interaction logic for StudentEntryView.xaml
    /// </summary>
    public partial class StudentEntryView : Page
    {
        public StudentEntryView()
        {
            InitializeComponent();
            this.Loaded += (e, a) => { NavigationService.Navigating += NavigationService_Navigating; };
            List<string> genders = new List<string>();
            var conn = Configuration.getInstance().getConnection();
            SqlCommand command = new SqlCommand("SELECT Value FROM Lookup WHERE Category='GENDER'", conn);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                genders.Add(reader.GetString(0));
            }
            reader.Close();
            GenderEntry.Items = genders;
        }

        void NavigationService_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                e.Cancel = true;
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var conn = Configuration.getInstance().getConnection();
            SqlCommand command = new SqlCommand(@"BEGIN TRANSACTION
                                                  INSERT INTO Person(FirstName,LastName,Contact,Email,DateofBirth,Gender)
                                                  VALUES(@FirstName,@LastName,@Contact,@Email,@DateofBirth,(SELECT ID FROM Lookup WHERE Value=@Gender));
                                                  DECLARE @RecordId int = scope_identity();
                                                  INSERT INTO Student(Id,RegistrationNo)
                                                  VALUES(@RecordId,@RegistrationNo);
                                                  COMMIT TRANSACTION;", conn);
            command.Parameters.AddWithValue("@firstName", FirstNameEntry.Text);
            command.Parameters.AddWithValue("@LastName", LastNameEntry.Text);
            command.Parameters.AddWithValue("@Contact", ContactEntry.Text);
            command.Parameters.AddWithValue("@Email", EmailEntry.Text);
            command.Parameters.AddWithValue("@DateofBirth", DateEntry.SelectedDate);
            command.Parameters.AddWithValue("@Gender", GenderEntry.SelectedItem.ToString());
            command.Parameters.AddWithValue("@RegistrationNo", RegistrationNumberEntry.Text);
            command.ExecuteNonQuery();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Source= new Uri("/Views/StudentView.xaml", UriKind.Relative);
        }
    }
}
