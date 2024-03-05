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
    /// Interaction logic for StudentGroupEntryView.xaml
    /// </summary>
    public partial class StudentGroupEntryView : Page
    {
        private DataTable assignedStudentsDataTable;
        private DataTable availableStudentsDataTable;
        private int groupId;
        public StudentGroupEntryView()
        {
            InitializeComponent();
        }
        public StudentGroupEntryView(int groupId) : this()
        {
            GroupIdLabel.TextData = groupId.ToString();
            this.groupId = groupId;
            assignedStudentsDataTable = Utils.FillDataGrid(@"SELECT Student.Id,RegistrationNo,CONCAT(FirstName,' ',LastName) Name,Contact,Email,Lookup.Value Gender,CONVERT(date,DateOfBirth,106) [Date of Birth]
                                 FROM GroupStudent
                                 JOIN Student
                                 ON Student.Id=GroupStudent.StudentId
                                 JOIN Person
                                 ON Person.Id=GroupStudent.StudentId
                                 JOIN Lookup
                                 ON Person.Gender=Lookup.Id
                                 WHERE Status=(SELECT Id FROM Lookup WHERE Lookup.Value='Active') AND 
                                       GroupId=" + groupId.ToString());
            availableStudentsDataTable = Utils.FillDataGrid(@"SELECT Student.Id,RegistrationNo,CONCAT(FirstName,' ',LastName) Name,Contact,Email,Lookup.Value Gender, CONVERT(DATE,DateOfBirth,106) DateOfBirth
                                 FROM (SELECT Student.Id
                                 FROM Student
                                 	EXCEPT
                                 (SELECT StudentId
                                 FROM GroupStudent
                                 WHERE GroupStudent.Status=(SELECT Id FROM Lookup WHERE Lookup.Value='Active'))) i
                                 JOIN Student
                                 ON Student.Id=i.Id
                                 JOIN Person
                                 ON Person.Id=i.Id
                                 JOIN Lookup
                                 ON Person.Gender=Lookup.Id");
            AssignedStudentsDataGrid.ItemsSource = assignedStudentsDataTable.DefaultView;
            AvailableStudentsDataGrid.ItemsSource = availableStudentsDataTable.DefaultView;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (AvailableStudentsDataGrid.SelectedItem != null)
            {
                DataRowView selectedItem = (DataRowView)AvailableStudentsDataGrid.SelectedItem;
                SqlDataReader reader = Utils.ReadData("SELECT CONVERT(bit,COUNT(1)) FROM GroupStudent WHERE StudentId=" + selectedItem.Row.ItemArray[0].ToString());
                reader.Read();
                if (reader.GetBoolean(0))
                {
                    Utils.ExecuteQuery(@"UPDATE GroupStudent SET Status=(SELECT Id FROM Lookup WHERE Lookup.Value='Active') WHERE GroupStudent.StudentId=" + selectedItem.Row.ItemArray[0].ToString());
                }
                else
                {
                    Utils.ExecuteQuery(@"INSERT INTO GroupStudent
                                     VALUES(" + groupId.ToString() + "," + selectedItem.Row.ItemArray[0].ToString() + ",3,GETDATE())");
                }
                assignedStudentsDataTable.Rows.Add(selectedItem.Row.ItemArray);
                availableStudentsDataTable.Rows.Remove(selectedItem.Row);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (AssignedStudentsDataGrid.SelectedItem != null)
            {
                DataRowView selectedItem = (DataRowView)AssignedStudentsDataGrid.SelectedItem;
                Utils.ExecuteQuery(@"UPDATE GroupStudent SET Status=(SELECT Id FROM Lookup WHERE Lookup.Value='Inactive') WHERE GroupStudent.StudentId=" + selectedItem.Row.ItemArray[0].ToString());
                availableStudentsDataTable.Rows.Add(selectedItem.Row.ItemArray);
                assignedStudentsDataTable.Rows.Remove(selectedItem.Row);
            }
        }
    }
}
