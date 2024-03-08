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

namespace FYP_Management_System
{
    /// <summary>
    /// Interaction logic for EvaluationEntryView.xaml
    /// </summary>
    public partial class EvaluationEntryView : Page
    {
        private bool updateMode = false;
        private int updateId;
        public event EventHandler UpdateNeeded;
        public EvaluationEntryView(object[]? itemArray = null)
        {
            InitializeComponent();
            if (itemArray != null)
            {
                updateMode = true;
                updateId = (int)itemArray[0];
                AddButton.Content = "Update";
                EvaluationNameEntry.Text = itemArray[1].ToString();
                TotalMarksEntry.Text = itemArray[2].ToString();
                TotalWeightageEntry.Text = itemArray[3].ToString();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (updateMode == false)
            {
                Utils.ExecuteQuery(@"INSERT INTO Evaluation(Name,TotalMarks,TotalWeightage)
                                     VALUES ('" + EvaluationNameEntry.Text + "'," + TotalMarksEntry.Text + "," + TotalWeightageEntry.Text + ")");
            }
            else
            {
                List<string> modifiedFields = new List<string>();
                if (EvaluationNameEntry.IsModified)
                    modifiedFields.Add(EvaluationNameEntry.QueryString);
                if (TotalMarksEntry.IsModified)
                    modifiedFields.Add(TotalMarksEntry.QueryString);
                if (TotalWeightageEntry.IsModified)
                    modifiedFields.Add(TotalWeightageEntry.QueryString);
                if (modifiedFields.Count != 0)
                {
                    Utils.ExecuteQuery(@"UPDATE Evaluation SET " + string.Join(",", modifiedFields) + " WHERE Id=" + updateId.ToString());
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
