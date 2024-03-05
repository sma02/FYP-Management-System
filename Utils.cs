using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FYP_Management_System
{
    public static class Utils
    {
        private static SqlDataReader reader;
        public static DataTable FillDataGrid(string query)
        {
            closeReader();
            var conn = Configuration.getInstance().getConnection();
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        public static void ExecuteQuery(string query)
        {
            closeReader();
            var conn = Configuration.getInstance().getConnection();
            SqlCommand command = new SqlCommand(query, conn);
            command.ExecuteNonQuery();
        }
        public static SqlDataReader ReadData(string query)
        {
            closeReader();
            var conn = Configuration.getInstance().getConnection();
            SqlCommand command = new SqlCommand(query, conn);
            reader = command.ExecuteReader();
            return reader;
        }
        private static void closeReader()
        {
            if (reader != null)
            {
                reader.Close();
                reader = null;
            }
        }
    }
}
