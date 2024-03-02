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
        public StudentGroupEntryView(int groupId):this()
        {
            GroupIdLabel.TextData = groupId.ToString();
            this.groupId = groupId;
            assignedStudentsDataTable = Utils.FillDataGrid(@"SELECT RegistrationNo,CONCAT(FirstName,' ',LastName) Name,Contact,Email,Lookup.Value Gender,CONVERT(date,DateOfBirth,106) [Date of Birth]
                                 FROM GroupStudent
                                 JOIN Student
                                 ON Student.Id=GroupStudent.StudentId
                                 JOIN Person
                                 ON Person.Id=GroupStudent.StudentId
                                 JOIN Lookup
                                 ON Person.Gender=Lookup.Id
                                 WHERE GroupId=" + groupId.ToString(), AssignedStudentsDataGrid);
            availableStudentsDataTable = Utils.FillDataGrid(@"SELECT RegistrationNo,CONCAT(FirstName,' ',LastName) Name,Contact,Email,Lookup.Value Gender, CONVERT(DATE,DateOfBirth,106) DateOfBirth
                                 FROM (SELECT Student.Id
                                 FROM Student
                                 	EXCEPT
                                 (SELECT StudentId
                                 FROM GroupStudent
                                 WHERE GroupStudent.Status=3)) i
                                 JOIN Student
                                 ON Student.Id=i.Id
                                 JOIN Person
                                 ON Person.Id=i.Id
                                 JOIN Lookup
                                 ON Person.Gender=Lookup.Id", AvailableStudentsDataGrid);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if(AvailableStudentsDataGrid.SelectedItem != null)
            {
                assignedStudentsDataTable.Rows.Add(((DataRowView)AvailableStudentsDataGrid.SelectedItem).Row.ItemArray);
                availableStudentsDataTable.Rows.Remove(((DataRowView)AvailableStudentsDataGrid.SelectedItem).Row);
            }
        }
    }
}
