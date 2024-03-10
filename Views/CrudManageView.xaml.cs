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
using System.Windows.Forms;
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
        private string? deleteQuery;
        private Type addEditPage;
        private Type? secondaryPage;
        private string? secondaryQuery;
        private string? secondaryDeleteQuery;
        private List<string>? secondarySearchAttributes;
        private int? secondaryId;
        public CrudManageView(string readQuery,string? deleteQuery,Type addEditPage,List<string> searchAttributes,bool CanAdd = true,string? secondaryQuery = null, string? secondaryDeleteQuery = null, Type? secondaryPage = null,List<string>? secondarySearchAttributes = null)
        {
            InitializeComponent();
            this.addEditPage = addEditPage;
            this.readQuery = readQuery;
            this.secondaryQuery = secondaryQuery;
            this.secondaryDeleteQuery = secondaryDeleteQuery;
            this.secondaryPage = secondaryPage;
            this.secondarySearchAttributes = secondarySearchAttributes;
            this.secondaryId = null;
            if(deleteQuery==null)
            {
                DeleteButton.Visibility = Visibility.Collapsed;
            }
            if(CanAdd==false)
            {
                AddButton.Visibility = Visibility.Collapsed;
            }
            this.deleteQuery = deleteQuery;
            populateTable();
            searchBar.SearchAttributes = searchAttributes;
        }
        public CrudManageView(object[]? itemarray,string query,string? deleteQuery, Type addEditPage,List<string> searchAttributes)
        {
            InitializeComponent();
            this.readQuery = query + itemarray[0].ToString();
            this.secondaryId = Convert.ToInt32(itemarray[0].ToString());
            this.addEditPage = addEditPage;
            this.deleteQuery = deleteQuery;
            this.secondaryQuery = null;
            if (deleteQuery == null)
            {
                DeleteButton.Visibility = Visibility.Collapsed;
            }
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
            object? page;
            if (secondaryId!=null)
                page = Activator.CreateInstance(addEditPage, new object[] { null,secondaryId });
            else
                page = Activator.CreateInstance(addEditPage, new object[] { null });
            NavigationService.Content = page;
            var eventInfo = page?.GetType().GetEvent("UpdateNeeded");
            eventInfo?.AddEventHandler(page, new EventHandler(Handler_UpdateNeeded));
        }
        private void SearchBar_SearchRequested(object sender, EventArgs e)
        {
            string filterString = searchBar.FilterString;
            table.DefaultView.RowFilter = filterString;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedItem = (DataRowView)DG1.SelectedItem;
            object? page;
            if (secondaryQuery != null)
                page = Activator.CreateInstance(addEditPage, new object[] { selectedItem.Row.ItemArray,secondaryQuery,secondaryDeleteQuery,secondaryPage,secondarySearchAttributes });
            else
                if (secondaryId != null)
                    page = Activator.CreateInstance(addEditPage, new object[] { selectedItem.Row.ItemArray, secondaryId });
                else
                    page = Activator.CreateInstance(addEditPage, new object[] { selectedItem.Row.ItemArray });
            NavigationService.Content = page;
            var eventInfo = page?.GetType().GetEvent("UpdateNeeded");
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
                DeleteButton.IsEnabled = true;
            }
        }

        private void DG1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DG1.SelectedItem != null)
            {
                DataRowView selectedItem = (DataRowView)DG1.SelectedItem;
                object? page;
                if (secondaryQuery != null)
                    page = Activator.CreateInstance(addEditPage, new object[] { selectedItem.Row.ItemArray, secondaryQuery,secondaryDeleteQuery,secondaryPage,secondarySearchAttributes });
                else
                    if (secondaryId != null)
                        page = Activator.CreateInstance(addEditPage, new object[] { selectedItem.Row.ItemArray, secondaryId });
                    else
                        page = Activator.CreateInstance(addEditPage, new object[] { selectedItem.Row.ItemArray });
                NavigationService.Content = page;
                var eventInfo = page.GetType().GetEvent("UpdateNeeded");
                eventInfo?.AddEventHandler(page, new EventHandler(Handler_UpdateNeeded));
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedItem = (DataRowView)DG1.SelectedItem;
            string? id = selectedItem.Row.ItemArray[0]?.ToString();
            ((DataView)DG1.ItemsSource).Table?.Rows.Remove(selectedItem.Row);
            string a = deleteQuery.Replace("@Id", id).Replace("@GroupId", secondaryId.ToString());
            if (secondaryId!=null)
                Utils.ExecuteQuery(deleteQuery.Replace("@Id", id).Replace("@GroupId",secondaryId.ToString()));
            else
                Utils.ExecuteQuery(deleteQuery.Replace("@Id", id));
        }
    }
}
