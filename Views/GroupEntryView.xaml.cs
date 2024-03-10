using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Printing;
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
    /// Interaction logic for ManageGroupView.xaml
    /// </summary>
    public partial class GroupEntryView : Page
    {
        private bool provisonalStudentGroup;
        private List<int> provisionalStudentGroupIds;
        private bool updateMode = false;
        private int updateId;
        public event EventHandler UpdateNeeded;
        private int?[] advisors;
        private List<int> studentsIds;
        public GroupEntryView(object[]? itemArray = null)
        {
            DataContext = this;
            InitializeComponent();
            provisonalStudentGroup = false;
            advisors = new int?[3];
            AdvisorsEntrySection.Visibility = Visibility.Collapsed;
            AdvisorsRow.Height = new GridLength(0, GridUnitType.Pixel);
            if (itemArray != null)
            {
                updateId = (int)itemArray[0];
                updateMode = true;
                AddStudentsGroup.Visibility = Visibility.Collapsed;
                SqlDataReader reader = Utils.ReadData(@"SELECT Project.Id,Title
                                                         FROM GroupProject
                                                         JOIN Project
                                                         ON GroupProject.ProjectId=Project.Id
                                                         WHERE GroupProject.GroupId = " + updateId.ToString());
                if (reader.Read())
                {
                    ProjectIdLabel.InitialData = reader.GetInt32(0).ToString();
                    ProjectTitleLabel.TextData = reader.GetString(1);
                    AdvisorsEntrySection.Visibility = Visibility.Visible;
                    AdvisorsRow.Height = new GridLength(1000, GridUnitType.Pixel);
                }
                reader = Utils.ReadData(@"SELECT Person.Id
                                                ,CONCAT(FirstName,' ',LastName) Name
                                          	  ,l2.Value Designation
                                          FROM GroupProject
                                          JOIN ProjectAdvisor
                                          ON ProjectAdvisor.ProjectId = GroupProject.ProjectId
                                          JOIN Person
                                          ON Person.Id = ProjectAdvisor.AdvisorId
                                          JOIN Lookup l1
                                          ON l1.Id = Gender
                                          JOIN Lookup l2
                                          ON l2.Id = AdvisorRole AND l2.Value='Main Advisor'
                                          wHERE GroupProject.GroupId=" + updateId.ToString());
                if (reader.Read())
                {
                    MainAdvisorIdLabel.InitialData = reader.GetInt32(0).ToString();
                    MainAdvisorNameLabel.TextData = reader.GetString(1);
                    MainAdvisorDesignationLabel.TextData = reader.GetString(2);
                    advisors[0] = reader.GetInt32(0);
                }
                reader = Utils.ReadData(@"SELECT Person.Id
                                                ,CONCAT(FirstName,' ',LastName) Name
                                          	  ,l2.Value Designation
                                          FROM GroupProject
                                          JOIN ProjectAdvisor
                                          ON ProjectAdvisor.ProjectId = GroupProject.ProjectId
                                          JOIN Person
                                          ON Person.Id = ProjectAdvisor.AdvisorId
                                          JOIN Lookup l1
                                          ON l1.Id = Gender
                                          JOIN Lookup l2
                                          ON l2.Id = AdvisorRole AND l2.Value='Co-Advisror'
                                          wHERE GroupProject.GroupId=" + updateId.ToString());

                if (reader.Read())
                {
                    CoAdvisorIdLabel.InitialData = reader.GetInt32(0).ToString();
                    CoAdvisorNameLabel.TextData = reader.GetString(1);
                    CoAdvisorDesignationLabel.TextData = reader.GetString(2);
                    advisors[1] = reader.GetInt32(0);
                }
                reader = Utils.ReadData(@"SELECT Person.Id
                                                ,CONCAT(FirstName,' ',LastName) Name
                                          	  ,l2.Value Designation
                                          FROM GroupProject
                                          JOIN ProjectAdvisor
                                          ON ProjectAdvisor.ProjectId = GroupProject.ProjectId
                                          JOIN Person
                                          ON Person.Id = ProjectAdvisor.AdvisorId
                                          JOIN Lookup l1
                                          ON l1.Id = Gender
                                          JOIN Lookup l2
                                          ON l2.Id = AdvisorRole AND l2.Value='Industry Advisor'
                                          wHERE GroupProject.GroupId=" + updateId.ToString());
                if (reader.Read())
                {
                    IndustryAdvisorIdLabel.InitialData = reader.GetInt32(0).ToString();
                    IndustryAdvisorNameLabel.TextData = reader.GetString(1);
                    IndustryAdvisorDesignationLabel.TextData = reader.GetString(2);
                    advisors[2] = reader.GetInt32(0);
                }
                StudentsGroupDataGrid.ItemsSource = Utils.FillDataGrid(@"SELECT Student.Id
                                                                               ,RegistrationNo
                                                                               ,CONCAT(FirstName,' ',LastName) Name
                                                                               ,Contact,Email
                                                                               ,Lookup.Value Gender
                                                                               ,CONVERT(date,DateOfBirth,106) [Date of Birth]
                                                                     FROM GroupStudent
                                                                     JOIN Student
                                                                     ON Student.Id=GroupStudent.StudentId
                                                                     JOIN Person
                                                                     ON Person.Id=GroupStudent.StudentId
                                                                     JOIN Lookup
                                                                     ON Person.Gender=Lookup.Id
                                                                     WHERE Status=(SELECT Id FROM Lookup WHERE Lookup.Value='Active') AND 
                                                                           GroupId=" + updateId.ToString()).DefaultView;

            }
            ProjectDataGrid.ItemsSource = Utils.FillDataGrid(@"SELECT Project.Id,Title,Description
                                                               FROM Project
                                                               WHERE Project.Id NOT IN (SELECT ProjectId
                                                                                        FROM GroupProject)").DefaultView;
            MainAdvisorDataGrid.ItemsSource = Utils.FillDataGrid(@"SELECT Advisor.ID
                                                                         ,CONCAT(FirstName,' ',LastName)          
                                                                         ,l2.Value Designation
                                                                         ,l1.Value Gender
                                                                         ,CONVERT(VARCHAR,Salary) + 'Rs' Salary
                                                                         ,Contact
                                                                         ,Email
                                                                   FROM Person 
                                                                   JOIN Advisor 
                                                                   ON Advisor.ID=Person.ID AND LEFT(FirstName,1)<>'$'
                                                                   JOIN Lookup l1
                                                                   ON Gender=l1.Id AND l1.Category='GENDER'
                                                                   JOIN Lookup l2
                                                                   ON Designation=l2.Id AND l2.Category='DESIGNATION'").DefaultView;
            CoAdvisorDataGrid.ItemsSource = Utils.FillDataGrid(@"SELECT Advisor.ID
                                                                         ,CONCAT(FirstName,' ',LastName)          
                                                                         ,l2.Value Designation
                                                                         ,l1.Value Gender
                                                                         ,CONVERT(VARCHAR,Salary) + 'Rs' Salary
                                                                         ,Contact
                                                                         ,Email
                                                                   FROM Person 
                                                                   JOIN Advisor 
                                                                   ON Advisor.ID=Person.ID AND LEFT(FirstName,1)<>'$'
                                                                   JOIN Lookup l1
                                                                   ON Gender=l1.Id AND l1.Category='GENDER'
                                                                   JOIN Lookup l2
                                                                   ON Designation=l2.Id AND l2.Category='DESIGNATION'").DefaultView;
            IndustryAdvisorDataGrid.ItemsSource = Utils.FillDataGrid(@"SELECT Advisor.ID
                                                                         ,CONCAT(FirstName,' ',LastName)          
                                                                         ,l2.Value Designation
                                                                         ,l1.Value Gender
                                                                         ,CONVERT(VARCHAR,Salary) + 'Rs' Salary
                                                                         ,Contact
                                                                         ,Email
                                                                   FROM Person 
                                                                   JOIN Advisor 
                                                                   ON Advisor.ID=Person.ID AND LEFT(FirstName,1)<>'$'
                                                                   JOIN Lookup l1
                                                                   ON Gender=l1.Id AND l1.Category='GENDER'
                                                                   JOIN Lookup l2
                                                                   ON Designation=l2.Id AND l2.Category='DESIGNATION'").DefaultView;
        }
        private void ProjectDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProjectIdLabel.TextData = ((DataRowView)ProjectDataGrid.SelectedItem).Row[0].ToString();
            ProjectTitleLabel.TextData = ((DataRowView)ProjectDataGrid.SelectedItem).Row[1].ToString();
            AdvisorsEntrySection.Visibility = Visibility;
            AdvisorsRow.Height = new GridLength(1000, GridUnitType.Pixel);
        }
        private bool AdvisorAlreadyAssigned(int? id)
        {
            for (int i = 0; i < 3; i++)
            {
                if (id == advisors[i] && advisors[i] != null)
                {
                    return true;
                }
            }
            return false;
        }
        private void MainAdvisorDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AdvisorAlreadyAssigned(int.Parse(((DataRowView)MainAdvisorDataGrid.SelectedItem).Row[0].ToString())))
            {
                return;
            }
            MainAdvisorIdLabel.TextData = ((DataRowView)MainAdvisorDataGrid.SelectedItem).Row[0].ToString();
            MainAdvisorNameLabel.TextData = ((DataRowView)MainAdvisorDataGrid.SelectedItem).Row[1].ToString();
            MainAdvisorDesignationLabel.TextData = ((DataRowView)MainAdvisorDataGrid.SelectedItem).Row[2].ToString();
            advisors[0] = int.Parse((string)MainAdvisorIdLabel.TextData);
        }

        private void CoAdvisorDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AdvisorAlreadyAssigned(int.Parse(((DataRowView)CoAdvisorDataGrid.SelectedItem).Row[0].ToString())))
            {
                return;
            }
            CoAdvisorIdLabel.TextData = ((DataRowView)CoAdvisorDataGrid.SelectedItem).Row[0].ToString();
            CoAdvisorNameLabel.TextData = ((DataRowView)CoAdvisorDataGrid.SelectedItem).Row[1].ToString();
            CoAdvisorDesignationLabel.TextData = ((DataRowView)CoAdvisorDataGrid.SelectedItem).Row[2].ToString();
            advisors[1] = int.Parse((string)CoAdvisorIdLabel.TextData);
        }

        private void IndustryAdvisorDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AdvisorAlreadyAssigned(int.Parse(((DataRowView)IndustryAdvisorDataGrid.SelectedItem).Row[0].ToString())))
            {
                return;
            }
            IndustryAdvisorIdLabel.TextData = ((DataRowView)IndustryAdvisorDataGrid.SelectedItem).Row[0].ToString();
            IndustryAdvisorNameLabel.TextData = ((DataRowView)IndustryAdvisorDataGrid.SelectedItem).Row[1].ToString();
            IndustryAdvisorDesignationLabel.TextData = ((DataRowView)IndustryAdvisorDataGrid.SelectedItem).Row[2].ToString();
            advisors[2] = int.Parse((string)IndustryAdvisorIdLabel.TextData);
        }

        private void AddStudentsGroup_Click(object sender, RoutedEventArgs e)
        {
            StudentGroupEntryView page = new StudentGroupEntryView(true);
            NavigationService.Content = page;
            page.UpdateNeeded += StudentProvisionalGroup_Update;
        }

        private void StudentProvisionalGroup_Update(object? sender, EventArgs e)
        {
            this.studentsIds = ((StudentGroupEntryView)sender).ids;
            string studentsIds = string.Join(',', ((StudentGroupEntryView)sender).ids.Select(n => n.ToString()).ToArray());
            StudentsGroupDataGrid.ItemsSource = Utils.FillDataGrid(@"SELECT Student.Id
                                                                               ,RegistrationNo
                                                                               ,CONCAT(FirstName,' ',LastName) Name
                                                                               ,Contact,Email
                                                                               ,Lookup.Value Gender
                                                                               ,CONVERT(date,DateOfBirth,106) [Date of Birth]
                                                                     FROM Student
                                                                     JOIN Person
                                                                     ON Person.Id=Student.Id
                                                                     JOIN Lookup
                                                                     ON Lookup.Id=Gender
                                                                     WHERE LEFT(FirstName,1)<>'$'
                                                                       AND Student.Id IN (" + studentsIds + ")").DefaultView;
            provisionalStudentGroupIds = ((StudentGroupEntryView)sender).ids;
            provisonalStudentGroup = true;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string query = "";
            if (updateMode == false)
            {
                query += @"BEGIN TRANSACTION
                       INSERT INTO [Group](Created_On)
                       VALUES (GETDATE())
                       DECLARE @identity int = scope_identity();";
                if (ProjectIdLabel.TextData != null)
                {
                    query += @"INSERT INTO GroupProject(GroupId,ProjectId,AssignmentDate)
                          VALUES (@identity," + ProjectIdLabel.TextData + ",GETDATE());";
                    if (MainAdvisorIdLabel.TextData != null)
                        query += @"INSERT INTO ProjectAdvisor(AdvisorId,AdvisorRole,ProjectId,AssignmentDate)
                               VALUES (" + MainAdvisorIdLabel.TextData + ",(SELECT Id FROM Lookup WHERE Value='Main Advisor')," + ProjectIdLabel.TextData + ",GETDATE());";
                    if (CoAdvisorIdLabel.TextData != null)
                        query += @"INSERT INTO ProjectAdvisor(AdvisorId,AdvisorRole,ProjectId,AssignmentDate)
                               VALUES (" + CoAdvisorIdLabel.TextData + ",(SELECT Id FROM Lookup WHERE Value='Co-Advisror')," + ProjectIdLabel.TextData + ",GETDATE());";
                    if (IndustryAdvisorIdLabel.TextData != null)
                        query += @"INSERT INTO ProjectAdvisor(AdvisorId,AdvisorRole,ProjectId,AssignmentDate)
                               VALUES (" + IndustryAdvisorIdLabel.TextData + ",(SELECT Id FROM Lookup WHERE Value='Industry Advisor')," + ProjectIdLabel.TextData + ",GETDATE());";
                }
                if (provisonalStudentGroup == true)
                {
                    string studentsIdsString = string.Join(',', studentsIds.Select(n => n.ToString()).ToArray());
                    query += @"INSERT INTO GroupStudent(GroupId,StudentId,Status,AssignmentDate)
                          SELECT @identity,Person.Id,(SELECT Id FROM Lookup WHERE Value='Active'),GETDATE()
                          FROM Person
                          WHERE Person.Id IN (" + studentsIdsString + ")";
                }
                query += @"COMMIT TRANSACTION";
                Utils.ExecuteQuery(query);
            }
            else
            {
                if (ProjectIdLabel.IsModified)
                {
                    if (ProjectIdLabel.InitialData == null)
                        query += @"INSERT INTO GroupProject(GroupId,ProjectId,AssignmentDate)
                          VALUES (" + updateId + "," + ProjectIdLabel.TextData + ",GETDATE());";

                    else if (ProjectIdLabel.InitialData != null)
                        query += @"UPDATE GroupProject 
                               SET ProjectId=" + ProjectIdLabel.TextData + @",AssignmentDate=GETDATE()
                               WHERE GroupId=" + updateId + ";";
                }
                if (ProjectIdLabel.TextData != DBNull.Value)
                {
                    if (MainAdvisorIdLabel.IsModified)
                    {
                        if (MainAdvisorIdLabel.InitialData == null)
                            query += @"INSERT INTO ProjectAdvisor(AdvisorId,AdvisorRole,ProjectId,AssignmentDate)
                               VALUES (" + MainAdvisorIdLabel.TextData + ",(SELECT Id FROM Lookup WHERE Value='Main Advisor')," + ProjectIdLabel.TextData + ",GETDATE());";
                        else if (MainAdvisorIdLabel.InitialData != null)
                            query += @"UPDATE ProjectAdvisor
                                   SET AdvisorId=" + MainAdvisorIdLabel.TextData + @",AssignmentDate=GETDATE()
                                   WHERE AdvisorRole=(SELECT Id FROM Lookup WHERE Value='Main Advisor')
                                     AND ProjectId=" + ProjectIdLabel.TextData + ";";
                    }
                    if (CoAdvisorIdLabel.IsModified)
                    {
                        if (CoAdvisorIdLabel.InitialData == null)
                            query += @"INSERT INTO ProjectAdvisor(AdvisorId,AdvisorRole,ProjectId,AssignmentDate)
                               VALUES (" + CoAdvisorIdLabel.TextData + ",(SELECT Id FROM Lookup WHERE Value='Co-Advisror')," + ProjectIdLabel.TextData + ",GETDATE());";
                        else if (CoAdvisorIdLabel.InitialData != null)
                            query += @"UPDATE ProjectAdvisor
                                   SET AdvisorId=" + CoAdvisorIdLabel.TextData + @",AssignmentDate=GETDATE()
                                   WHERE AdvisorRole=(SELECT Id FROM Lookup WHERE Value='Co-Advisror')
                                     AND ProjectId=" + ProjectIdLabel.TextData + ";";
                    }
                    if (IndustryAdvisorIdLabel.IsModified)
                    {
                        if (IndustryAdvisorIdLabel.InitialData == null)
                            query += @"INSERT INTO ProjectAdvisor(AdvisorId,AdvisorRole,ProjectId,AssignmentDate)
                               VALUES (" + IndustryAdvisorIdLabel.TextData + ",(SELECT Id FROM Lookup WHERE Value='Industry Advisor')," + ProjectIdLabel.TextData + ",GETDATE());";
                        else if (IndustryAdvisorIdLabel.InitialData != null)
                            query += @"UPDATE ProjectAdvisor
                                   SET AdvisorId=" + IndustryAdvisorIdLabel.TextData + @",AssignmentDate=GETDATE()
                                   WHERE AdvisorRole=(SELECT Id FROM Lookup WHERE Value='Industry Advisor')
                                     AND ProjectId=" + ProjectIdLabel.TextData + ";";
                    }
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
