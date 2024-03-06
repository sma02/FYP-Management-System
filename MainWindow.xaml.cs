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
            ContentFrame.Content = new Views.ProjectView();
        }

        private void BtnManageAdvisors_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new Views.CrudManageView(@"SELECT     Advisor.ID 
                                                        ,FirstName
                                                        ,LastName
                                                        ,l2.Value Designation
                                                        ,l1.Value Gender
                                                        ,CONVERT(VARCHAR,Salary) + 'Rs' Salary
                                                        ,Contact
                                                        ,Email
                                                        ,CONVERT(VARCHAR,DateOfBirth,106) DateOfBirth
                                                  FROM Person 
                                                  JOIN Advisor 
                                                  ON Advisor.ID=Person.ID AND LEFT(FirstName,1)<>'$'
                                                  JOIN Lookup l1
                                                  ON Gender=l1.Id AND l1.Category='GENDER'
                                                  JOIN Lookup l2
                                                  ON Designation=l2.Id AND l2.Category='DESIGNATION'"
                                       ,
                                      @"UPDATE Person SET FirstName = '$' + FirstName WHERE Id = @Id"
                                      ,
                                      typeof(AdvisorEntryView)
                                      , new List<string> { "FirstName", "LastName" });
        }

        private void BtnManageGroups_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new Views.ManageGroupView("2");

        }

        private void BtnManageStudentGroups_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = new Views.ManageStudentGroupView();

        }
    }
}
