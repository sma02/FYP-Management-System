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
    /// Interaction logic for AdvisorView.xaml
    /// </summary>
    public partial class AdvisorView : UserControl
    {
        public AdvisorView()
        {
            InitializeComponent();
            var conn = Configuration.getInstance().getConnection();
            SqlCommand command = new SqlCommand(@"SELECT *
                                                  FROM Advisor", conn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            DG1.ItemsSource = table.DefaultView;
        }
    }
}
