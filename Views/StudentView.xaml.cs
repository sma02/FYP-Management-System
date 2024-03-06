using FYP_Management_System.Views.Components;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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
        private DataView tableView;
        private bool IsBusy;
        private string filterString;
        private DataTable table;
        public StudentView()
        {
            InitializeComponent();
            new Thread(new ThreadStart(() => 
            {       
                table = Utils.FillDataGrid(@"SELECT     Student.ID 
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
                                                  ON Gender=Lookup.Id AND Lookup.Category='GENDER'");
                this.Dispatcher.Invoke(() => {
            DG1.ItemsSource = table.DefaultView;
                });
            })).Start();
            searchBar.SearchAttributes = new List<string> { "FirstName", "LastName" };
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Content = new Views.Components.StudentEntryView();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
        }
        private void SearchBar_SearchRequested(object sender, EventArgs e)
        {
            string filterString = searchBar.FilterString;
            table.DefaultView.RowFilter = filterString;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedItem = (DataRowView)DG1.SelectedItem;
            NavigationService.Content = new StudentEntryView(selectedItem.Row.ItemArray);
        }

        private void DG1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(DG1.SelectedCells.Count>0)
            {
                EditButton.IsEnabled = true;
            }
        }

        private void DG1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView selectedItem = (DataRowView)DG1.SelectedItem;
            NavigationService.Content = new StudentEntryView(selectedItem.Row.ItemArray);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedItem = (DataRowView)DG1.SelectedItem;
            string? id = selectedItem.Row.ItemArray[0]?.ToString();
            ((DataView)DG1.ItemsSource).Table?.Rows.Remove(selectedItem.Row);
            Utils.ExecuteQuery(@"UPDATE GroupStudent SET Status = 4 WHERE StudentId = " +id + 
                ";UPDATE Person SET FirstName = '$' + FirstName WHERE Id = "+id);
        }
    }
}
