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
    /// Interaction logic for ProjectEntryView.xaml
    /// </summary>
    public partial class ProjectEntryView : Page
    {
        private bool updateMode = false;
        private int updateId;
        public event EventHandler UpdateNeeded;
        public ProjectEntryView(object[]? itemArray = null)
        {
            InitializeComponent();
            if (itemArray != null)
            {
                updateMode = true;
                updateId = (int)itemArray[0];
                AddButton.Content = "Update";
                ProjectTitleEntry.Text = itemArray[1].ToString();
                ProjectDescriptionEntry.Text = itemArray[2].ToString();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if(updateMode==false)
            {
                Utils.ExecuteQuery(@"INSERT INTO Project(Title,Description)
                                     VALUES ('" + ProjectTitleEntry.Text + "','" + ProjectDescriptionEntry.Text + "')");
            }
            else
            {
                List<string> modifiedFields = new List<string>();
                if (ProjectTitleEntry.IsModified)
                    modifiedFields.Add(ProjectTitleEntry.QueryString);
                if (ProjectDescriptionEntry.IsModified)
                    modifiedFields.Add(ProjectDescriptionEntry.QueryString);
                if (modifiedFields.Count != 0)
                {
                    Utils.ExecuteQuery(@"UPDATE Project SET " + string.Join(",", modifiedFields) + " WHERE Id="+updateId.ToString());
                }
            }
            UpdateNeeded?.Invoke(this, EventArgs.Empty);
            NavigationService.GoBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
