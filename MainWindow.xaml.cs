using FYP_Management_System.Views;
using FYP_Management_System.Views.Components;
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
            ContentFrame.Content = new Views.CrudManageView(@"SELECT     Student.ID 
                                                        ,RegistrationNo
                                                        ,FirstName
                                                        ,LastName
                                                        ,Lookup.Value Gender
                                                        ,Contact
                                                        ,Email
                                                        ,CONVERT(VARCHAR,DateOfBirth,106) DateOfBirth
                                                  FROM Person 
                                                  JOIN Student 
                                                  ON Student.ID=Person.ID AND LEFT(FirstName,1)<>'$'
                                                  JOIN Lookup
                                                  ON Gender=Lookup.Id AND Lookup.Category='GENDER'"
                                                  ,
                                                  @"UPDATE GroupStudent SET Status = 4 WHERE StudentId = @Id;
                                                    UPDATE Person SET FirstName = '$' + FirstName WHERE Id = @Id"
                                                  ,
                                                  typeof(StudentEntryView)
                                                  ,new List<string>{"FirstName","LastName" });
        }

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new Views.Components.StudentEntryView();
        }

        private void BtnManageProjects_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new Views.CrudManageView(@"SELECT Id,Title,Description
                                                              FROM Project
                                                              WHERE LEFT(Title,1)<>'$'"
                                                            , @"UPDATE Project SET Title = '$' + Title WHERE Id = @Id"
                                                            , typeof(ProjectEntryView)
                                                            , new List<string> { "Title" });
        }
        private void BtnManageAdvisors_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new Views.CrudManageView(@"SELECT     Advisor.ID 
                                                        ,FirstName
                                                        ,LastName
                                                        ,l1.Value Designation
                                                        ,(SELECT Value FROM Lookup WHERE Lookup.Id=Gender) Gender
                                                        ,CONVERT(VARCHAR,Salary) + 'Rs' Salary
                                                        ,Contact
                                                        ,Email
                                                        ,CONVERT(VARCHAR,DateOfBirth,106) DateOfBirth
                                                  FROM Person 
                                                  JOIN Advisor 
                                                  ON Advisor.ID=Person.ID AND LEFT(FirstName,1)<>'$'
                                                  JOIN Lookup l1
                                                  ON Designation=l1.Id AND l1.Category='DESIGNATION'"
                                       ,
                                      @"UPDATE Person SET FirstName = '$' + FirstName WHERE Id = @Id"
                                      ,
                                      typeof(AdvisorEntryView)
                                      , new List<string> { "FirstName", "LastName" });
        }

        private void BtnManageGroups_Click(object sender, RoutedEventArgs e)
        {
            // ContentFrame.Content = new Views.ManageGroupView();
            ContentFrame.Content =  new CrudManageView(@"SELECT [Group].Id
                                                         	, Project.Title
                                                         	,(SELECT CONCAT(FirstName,' ',LastName)
                                                         	  FROM ProjectAdvisor 
                                                         	  JOIN Person 
                                                         	  ON Person.Id=ProjectAdvisor.AdvisorId 
                                                         	  WHERE ProjectAdvisor.AdvisorRole=(SELECT Id FROM Lookup WHERE Value='Main Advisor')
															  	AND GroupProject.ProjectId=ProjectAdvisor.ProjectId) [Main Advisor]
                                                         	 ,(SELECT CONCAT(FirstName,' ',LastName)
                                                         	  FROM ProjectAdvisor 
                                                         	  JOIN Person 
                                                         	  ON Person.Id=ProjectAdvisor.AdvisorId 
                                                         	  WHERE ProjectAdvisor.AdvisorRole=(SELECT Id FROM Lookup WHERE Value='Co-Advisror')
																AND GroupProject.ProjectId=ProjectAdvisor.ProjectId) [Co-Advisor]
                                                         	 ,(SELECT CONCAT(FirstName,' ',LastName)
                                                         	  FROM ProjectAdvisor 
                                                         	  JOIN Person 
                                                         	  ON Person.Id=ProjectAdvisor.AdvisorId 
                                                         	  WHERE ProjectAdvisor.AdvisorRole=(SELECT Id FROM Lookup WHERE Value='Industry Advisor')
																AND GroupProject.ProjectId=ProjectAdvisor.ProjectId) [Industry Advisor]
															                                                     	  ,(SELECT STUFF((SELECT ', ' +Student.RegistrationNo
                                                         			                     FROM GroupStudent
                                                         			                     JOIN Student
                                                         			                     ON Student.Id=GroupStudent.StudentId
                                                         			                     WHERE g.GroupId=GroupStudent.GroupId 
                                                         								 AND GroupStudent.Status=(SELECT Id FROM Lookup WHERE Lookup.Value='Active')
                                                         			                     FOR XML PATH('')),1,1,'') [Registration Numbers]
                                                                                          FROM GroupStudent g
                                                         								 WHERE g.GroupId=[Group].Id
                                                                                          GROUP BY g.GroupId) [Registration Numbers]
                                                         FROM [Group]
                                                         JOIN GroupProject
                                                         ON GroupProject.GroupId=[Group].Id
                                                         JOIN Project
                                                         ON Project.Id=GroupProject.ProjectId", null, typeof(GroupEntryView), new List<string> { "[Registration Numbers]" });
        }

        private void BtnManageStudentGroups_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new CrudManageView(@"SELECT [Group].Id
                                                         	, Project.Title
                                                         	,(SELECT STUFF((SELECT ', ' +Student.RegistrationNo
                                                         			                     FROM GroupStudent
                                                         			                     JOIN Student
                                                         			                     ON Student.Id=GroupStudent.StudentId
                                                         			                     WHERE g.GroupId=GroupStudent.GroupId 
                                                         								 AND GroupStudent.Status=(SELECT Id FROM Lookup WHERE Lookup.Value='Active')
                                                         			                     FOR XML PATH('')),1,1,'') [Registration Numbers]
                                                                                          FROM GroupStudent g
                                                         								 WHERE g.GroupId=[Group].Id
                                                                                          GROUP BY g.GroupId) [Registration Numbers]
                                                         FROM [Group]
                                                         JOIN GroupProject
                                                         ON GroupProject.GroupId=[Group].Id
                                                         JOIN Project
                                                         ON Project.Id=GroupProject.ProjectId", null, typeof(StudentGroupEntryView),new List<string>{ "[Registration Numbers]" },CanAdd: false);

        }

        private void BtnManageEvaluations_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new CrudManageView(@"SELECT *
                                                        FROM Evaluation
                                                        WHERE LEFT(Name,1)<>'$'"
                                                        , @"UPDATE Evaluation SET Name = '$' + Name WHERE Id = @Id"
                                                        , typeof(EvaluationEntryView)
                                                        , new List<string> { "Name" });
        }

        private void BtnMarkEvaluations_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new CrudManageView(@"SELECT [Group].Id
                                                         	, Project.Title
                                                         	,(SELECT CONCAT(FirstName,' ',LastName)
                                                         	  FROM ProjectAdvisor 
                                                         	  JOIN Person 
                                                         	  ON Person.Id=ProjectAdvisor.AdvisorId 
                                                         	  WHERE ProjectAdvisor.AdvisorRole=(SELECT Id FROM Lookup WHERE Value='Main Advisor')
															  	AND GroupProject.ProjectId=ProjectAdvisor.ProjectId) [Main Advisor]
                                                         	 ,(SELECT CONCAT(FirstName,' ',LastName)
                                                         	  FROM ProjectAdvisor 
                                                         	  JOIN Person 
                                                         	  ON Person.Id=ProjectAdvisor.AdvisorId 
                                                         	  WHERE ProjectAdvisor.AdvisorRole=(SELECT Id FROM Lookup WHERE Value='Co-Advisror')
																AND GroupProject.ProjectId=ProjectAdvisor.ProjectId) [Co-Advisor]
                                                         	 ,(SELECT CONCAT(FirstName,' ',LastName)
                                                         	  FROM ProjectAdvisor 
                                                         	  JOIN Person 
                                                         	  ON Person.Id=ProjectAdvisor.AdvisorId 
                                                         	  WHERE ProjectAdvisor.AdvisorRole=(SELECT Id FROM Lookup WHERE Value='Industry Advisor')
																AND GroupProject.ProjectId=ProjectAdvisor.ProjectId) [Industry Advisor]
															                                                     	  ,(SELECT STUFF((SELECT ', ' +Student.RegistrationNo
                                                         			                     FROM GroupStudent
                                                         			                     JOIN Student
                                                         			                     ON Student.Id=GroupStudent.StudentId
                                                         			                     WHERE g.GroupId=GroupStudent.GroupId 
                                                         								 AND GroupStudent.Status=(SELECT Id FROM Lookup WHERE Lookup.Value='Active')
                                                         			                     FOR XML PATH('')),1,1,'') [Registration Numbers]
                                                                                          FROM GroupStudent g
                                                         								 WHERE g.GroupId=[Group].Id
                                                                                          GROUP BY g.GroupId) [Registration Numbers]
                                                         FROM [Group]
                                                         JOIN GroupProject
                                                         ON GroupProject.GroupId=[Group].Id
                                                         JOIN Project
                                                         ON Project.Id=GroupProject.ProjectId"
                                                       , null
                                                       , typeof(CrudManageView)
                                                       ,new List<string> { "FirstName","LastName","RegistrationNo"}
                                                       , false
                                                       , @"SELECT EvaluationId [Id]
                                                                 ,Name
                                                                 ,ObtainedMarks [Obtained Marks]
                                                                 ,TotalMarks [Total Marks]
                                                                 ,TotalWeightage [Total Weightage]
                                                           FROM GroupEvaluation
                                                           JOIN Evaluation
                                                           ON Evaluation.Id=GroupEvaluation.EvaluationId
                                                           WHERE GroupEvaluation.GroupId="
                                                       ,@"DELETE FROM GroupEvaluation
                                                          WHERE GroupId=@GroupId AND EvaluationId=@Id" 
                                                       ,typeof(EvaluationMarkingEntryView)
                                                       , new List<string> { "Name" });
        }
    }
}
