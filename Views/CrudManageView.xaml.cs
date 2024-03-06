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
using static System.Windows.Forms.LinkLabel;

namespace FYP_Management_System.Views
{
    /// <summary>
    /// Interaction logic for StudentView.xaml
    /// </summary>
    public partial class CrudManageView : Page
    {
        private DataTable table;
        private string readQuery;
        private string deleteQuery;
        private Type addEditPage;
        public CrudManageView(string readQuery,string deleteQuery,Type addEditPage,List<string> searchAttributes)
        {
            InitializeComponent();
            this.addEditPage = addEditPage;
            this.readQuery = readQuery;
            this.deleteQuery = deleteQuery;
            populateTable();
            searchBar.SearchAttributes = searchAttributes;
        }
        private void populateTable()
        {
            new Thread(new ThreadStart(() =>
            {
                table = Utils.FillDataGrid(readQuery);
                Dispatcher.Invoke(() => {
                    DG1.ItemsSource = table.DefaultView;
                });
            })).Start();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Content = Activator.CreateInstance(addEditPage, new object[] { null });
        }
        private void SearchBar_SearchRequested(object sender, EventArgs e)
        {
            string filterString = searchBar.FilterString;
            table.DefaultView.RowFilter = filterString;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
                        DataRowView selectedItem = (DataRowView)DG1.SelectedItem;
            object? page = Activator.CreateInstance(addEditPage, new object[] { selectedItem.Row.ItemArray });
            NavigationService.Content = page;
            var eventInfo = page.GetType().GetEvent("UpdateNeeded");
            eventInfo?.AddEventHandler(page,new EventHandler(Handler_UpdateNeeded));
        }

        private void Handler_UpdateNeeded(object sender, object e)
        {
            populateTable();
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
            object? page = Activator.CreateInstance(addEditPage, new object[] { selectedItem.Row.ItemArray });
            NavigationService.Content = page;
            var eventInfo = page.GetType().GetEvent("UpdateNeeded");
            eventInfo?.AddEventHandler(page, new EventHandler(Handler_UpdateNeeded));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedItem = (DataRowView)DG1.SelectedItem;
            string? id = selectedItem.Row.ItemArray[0]?.ToString();
            ((DataView)DG1.ItemsSource).Table?.Rows.Remove(selectedItem.Row);
            Utils.ExecuteQuery(deleteQuery.Replace("@Id", id));
        }
    }
}
