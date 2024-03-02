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
    /// Interaction logic for ManageStudentGroupView.xaml
    /// </summary>
    public partial class ManageStudentGroupView : Page
    {
        public ManageStudentGroupView()
        {
            InitializeComponent();
            Utils.FillDataGrid(@"SELECT GroupId,STUFF((SELECT ', ' +Student.RegistrationNo
			                     FROM GroupStudent
			                     JOIN Student
			                     ON Student.Id=GroupStudent.StudentId
			                     WHERE g.GroupId=GroupStudent.GroupId
			                     FOR XML PATH('')),1,1,'') [Registration Numbers]
                                 FROM GroupStudent g
                                 GROUP BY g.GroupId", ManageStudentDataGrid);
        }

        private void ManageStudentDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int groupId = Convert.ToInt32(((DataRowView)ManageStudentDataGrid.SelectedItem).Row[0]);
            NavigationService.Navigate(new StudentGroupEntryView(groupId));
        }
    }
}
