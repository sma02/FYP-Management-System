using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for ManageGroupView.xaml
    /// </summary>
    public partial class ManageGroupView : Page , INotifyPropertyChanged
    {
        private int groupId;
        public string GroupId { get => groupId.ToString();
            set {
                groupId = int.Parse(value);
                GroupIdLabel.TextData = value;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ManageGroupView()
        {
            DataContext = this;
            InitializeComponent();
            Utils.FillDataGrid("SELECT Id,Title FROM Project", ProjectDataGrid);
            Utils.FillDataGrid("SELECT Advisor.Id,Lookup.Value Designation,Salary FROM Advisor JOIN Lookup ON Lookup.Id=Designation WHERE Designation>=6 AND Designation<=7", MainAdvisorDataGrid);
            Utils.FillDataGrid("SELECT Advisor.Id,Lookup.Value Designation,Salary FROM Advisor JOIN Lookup ON Lookup.Id=Designation WHERE Designation>=8 AND Designation<=9", CoAdvisorDataGrid);
            Utils.FillDataGrid("SELECT Advisor.Id,Lookup.Value Designation,Salary FROM Advisor JOIN Lookup ON Lookup.Id=Designation WHERE Designation=10", IndustryAdvisorDataGrid);
        }

        public ManageGroupView(string groupId):this()
        {
            GroupId = groupId;
        }

        private void ProjectDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProjectNameLabel.TextData = ((DataRowView)ProjectDataGrid.SelectedItem).Row[1].ToString();
        }

        private void MainAdvisorDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainAdvisorIdLabel.TextData = ((DataRowView)MainAdvisorDataGrid.SelectedItem).Row[0].ToString();
            MainAdvisorDesignationLabel.TextData = ((DataRowView)MainAdvisorDataGrid.SelectedItem).Row[1].ToString();

        }

        private void CoAdvisorDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CoAdvisorIdLabel.TextData = ((DataRowView)CoAdvisorDataGrid.SelectedItem).Row[0].ToString();
            CoAdvisorDesignationLabel.TextData = ((DataRowView)CoAdvisorDataGrid.SelectedItem).Row[1].ToString();
        }

        private void IndustryAdvisorDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IndustryAdvisorIdLabel.TextData = ((DataRowView)IndustryAdvisorDataGrid.SelectedItem).Row[0].ToString();
            IndustryAdvisorDesignationLabel.TextData = ((DataRowView)IndustryAdvisorDataGrid.SelectedItem).Row[1].ToString();
        }
    }
}
