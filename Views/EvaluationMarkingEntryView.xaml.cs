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
    /// Interaction logic for EvaluationMarkingEntryView.xaml
    /// </summary>
    public partial class EvaluationMarkingEntryView : Page
    {
        private bool updateMode = false;
        private int updateId;
        private int? groupId;
        public event EventHandler UpdateNeeded;
        public EvaluationMarkingEntryView(object[]? itemArray = null, int? groupId = null)
        {
            InitializeComponent();
            this.groupId = groupId;
            if (itemArray != null)
            {
                updateId = (int)itemArray[0];
                updateMode = true;
                AddButton.Content = "Update";
                //EvaluationNameEntry.Val = itemArray[1].ToString();
                ObtainedMarksEntry.Text = itemArray[2].ToString();
                TotalMarksEntry.Text = itemArray[3].ToString();
                TotalWeightageEntry.Text = itemArray[4].ToString();
                EvaluationEntryDataGrid.Visibility = Visibility.Collapsed;
                EvaluationSelection.TextLabel = "Evaluation Name: ";
                SqlDataReader reader = Utils.ReadData(@"SELECT Name
                                                                FROM Evaluation
                                                                WHERE Evaluation.Id=" + updateId.ToString());
                reader.Read();
                EvaluationSelection.TextData = reader.GetString(0);
            }
            EvaluationEntryDataGrid.ItemsSource = Utils.FillDataGrid(@"SELECT Id
                                                                             ,Name
                                                                             ,TotalMarks [Total Marks]
                                                                             ,TotalWeightage [Total Weightage]
                                                                       FROM Evaluation
                                                                       WHERE LEFT(Name,1)<>'$'").DefaultView;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (updateMode == false)
            {
                if (EvaluationEntryDataGrid.SelectedItem != null)
                {
                    DataRowView selectedItem = (DataRowView)EvaluationEntryDataGrid.SelectedItem;
                    Utils.ExecuteQuery(@"INSERT INTO GroupEvaluation(GroupId,EvaluationId,ObtainedMarks,EvaluationDate)
                                     VALUES (" + groupId.ToString() + "," + selectedItem.Row.ItemArray[0].ToString() + "," + ObtainedMarksEntry.Text + ",GETDATE())");
                }
            }
            else
            {
                if (ObtainedMarksEntry.IsModified)
                    Utils.ExecuteQuery(@"UPDATE GroupEvaluation 
                                         SET ObtainedMarks = " + ObtainedMarksEntry.Text + ",EvaluationDate=GETDATE() WHERE GroupId=" + groupId.ToString() + " AND EvaluationId=" + updateId.ToString());
            }
                UpdateNeeded?.Invoke(this, EventArgs.Empty);
                NavigationService.GoBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void EvaluationEntryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EvaluationEntryDataGrid.SelectedItem != null)
            {
                DataRowView selectedItem = (DataRowView)EvaluationEntryDataGrid.SelectedItem;
                TotalMarksEntry.Text = selectedItem.Row[2].ToString();
                TotalWeightageEntry.Text = selectedItem.Row[3].ToString();
            }
        }
    }
}
