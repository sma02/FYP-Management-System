using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
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

namespace FYP_Management_System.Views
{
    /// <summary>
    /// Interaction logic for AdvisorEntryView.xaml
    /// </summary>
    public partial class AdvisorEntryView : Page
    {
        private bool updateMode = false;
        private int updateId;
        public event EventHandler UpdateNeeded;
        public AdvisorEntryView(object[]? itemArray = null)
        {
            InitializeComponent();
            GenderEntry.ItemsRead = Utils.ReadData("SELECT Value FROM Lookup WHERE Category = 'GENDER'");
            DesignationEntry.ItemsRead = Utils.ReadData("SELECT Value FROM Lookup WHERE Category = 'DESIGNATION'");
            if (itemArray != null)
            {
                updateMode = true;
                updateId = (int)itemArray[0];
                ButtonAdd.Content = "Update";
                FirstNameEntry.Text = itemArray[1].ToString();
                LastNameEntry.Text = itemArray[2].ToString();
                DesignationEntry.SelectedItem = itemArray[3].ToString();
                GenderEntry.SelectedItem = itemArray[4].ToString();
                SalaryEntry.Text = itemArray[5].ToString().Replace("Rs","");
                ContactEntry.Text = itemArray[6].ToString();
                EmailEntry.Text = itemArray[7].ToString();
                DateEntry.SelectedDate = itemArray[8].ToString().IsNullOrEmpty() ? itemArray[8].ToString():null;
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var conn = Configuration.getInstance().getConnection();
            SqlCommand command;
            if (updateMode == false)
            {
                command = new SqlCommand(@"BEGIN TRANSACTION
                                                  INSERT INTO Person(FirstName,LastName,Contact,Email,DateofBirth,Gender)
                                                  VALUES(@FirstName,@LastName,@Contact,@Email,@DateofBirth,(SELECT ID FROM Lookup WHERE Value=@Gender));
                                                  DECLARE @RecordId int = scope_identity();
                                                  INSERT INTO Advisor(Id,Designation,Salary)
                                                  VALUES(@RecordId,(SELECT ID FROM Lookup WHERE Value=@Designation),@Salary)
                                                  COMMIT TRANSACTION;", conn);
                command.Parameters.AddWithValue("@FirstName", FirstNameEntry.Text);
                command.Parameters.AddWithValue("@LastName", LastNameEntry.Text);
                command.Parameters.AddWithValue("@Contact", ContactEntry.Text);
                command.Parameters.AddWithValue("@Email", EmailEntry.Text);
                command.Parameters.AddWithValue("@DateofBirth", DateEntry.SelectedDate);
                command.Parameters.AddWithValue("@Gender", GenderEntry.SelectedItem);
                command.Parameters.AddWithValue("@Designation", DesignationEntry.SelectedItem);
                command.Parameters.AddWithValue("@Salary", SalaryEntry.Text);
            }
            else
            {
                List<string> modifiedFields = new List<string>();
                List<string> modifiedFields2 = new List<string>();
                if (FirstNameEntry.IsModified)
                    modifiedFields.Add(FirstNameEntry.QueryString);
                if (LastNameEntry.IsModified)
                    modifiedFields.Add(LastNameEntry.QueryString);
                if (ContactEntry.IsModified)
                    modifiedFields.Add(ContactEntry.QueryString);
                if (GenderEntry.IsModified)
                    modifiedFields.Add("Gender = (SELECT Id FROM Lookup WHERE VALUE='" + GenderEntry.SelectedItem + "')");
                if (EmailEntry.IsModified)
                    modifiedFields.Add(EmailEntry.QueryString);
                if (DateEntry.IsModified)
                    modifiedFields.Add(DateEntry.QueryString);
                if (DesignationEntry.IsModified)
                    modifiedFields2.Add("Designation = (SELECT Id FROM Lookup WHERE VALUE='" + DesignationEntry.SelectedItem + "')");
                if (SalaryEntry.IsModified)
                    modifiedFields2.Add(SalaryEntry.QueryString);
                if (modifiedFields.Count == 0 && modifiedFields2.Count == 0)
                {
                    NavigationService.GoBack();
                    return;
                }
                string updateString = string.Join(",", modifiedFields);
                string updateString2 = string.Join(",", modifiedFields2);
                List<string> queries = new List<string>();
                if (modifiedFields.Count != 0)
                    queries.Add(@"UPDATE Person SET " + updateString + "WHERE Id=@ID");
                if(modifiedFields2.Count != 0)
                    queries.Add("UPDATE Advisor SET " +updateString2 + "WHERE Id=@ID");
                command = new SqlCommand(string.Join(";", queries), conn);
                command.Parameters.AddWithValue("@ID", updateId);
            }
            command.ExecuteNonQuery();
            UpdateNeeded?.Invoke(this, EventArgs.Empty);
            NavigationService.GoBack();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
