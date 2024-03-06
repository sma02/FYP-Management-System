﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections;
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

namespace FYP_Management_System.Views.Components
{
    /// <summary>
    /// Interaction logic for StudentEntryView.xaml
    /// </summary>
    public partial class StudentEntryView : Page
    {
        private bool updateMode = false;
        private int updateId;
        public StudentEntryView(object[]? itemArray = null)
        {
            InitializeComponent();
            Loaded += (e, a) => { NavigationService.Navigating += NavigationService_Navigating; };
            List<string> genders = new List<string>();
            var conn = Configuration.getInstance().getConnection();
            SqlCommand command = new SqlCommand("SELECT Value FROM Lookup WHERE Category='GENDER'", conn);
            SqlDataReader reader = command.ExecuteReader();
            GenderEntry.ItemsRead = reader;
            if (itemArray != null)
            {
                updateMode = true;
                updateId = (int)itemArray[0];
                ButtonAdd.Content = "Update";
                RegistrationNumberEntry.Text = itemArray[1].ToString();
                FirstNameEntry.Text = itemArray[2].ToString();
                LastNameEntry.Text = itemArray[3].ToString();
                GenderEntry.SelectedItem = itemArray[4].ToString();
                ContactEntry.Text = itemArray[5].ToString();
                EmailEntry.Text = itemArray[6].ToString();
                DateEntry.SelectedDate = itemArray[7].ToString();
            }
        }

        void NavigationService_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                e.Cancel = true;
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var conn = Configuration.getInstance().getConnection();
            SqlCommand command;
            if (updateMode == false)
            {
                command = new SqlCommand(@"BEGIN TRANSACTION
                                                  INSERT INTO Person(FirstName,LastName,Contact,Email,DateofBirth,Gender)
                                                  VALUES(@FirstName,@LastName,@Contact,@Email,@DateofBirth,(SELECT ID FROM Lookup WHERE Value=@Gender));
                                                  DECLARE @RecordId int = scope_identity();
                                                  INSERT INTO Student(Id,RegistrationNo)
                                                  VALUES(@RecordId,@RegistrationNo);
                                                  COMMIT TRANSACTION;", conn);
                command.Parameters.AddWithValue("@FirstName", FirstNameEntry.Text);
                command.Parameters.AddWithValue("@LastName", LastNameEntry.Text);
                command.Parameters.AddWithValue("@Contact", ContactEntry.Text);
                command.Parameters.AddWithValue("@Email", EmailEntry.Text);
                command.Parameters.AddWithValue("@DateofBirth", DateEntry.SelectedDate);
                command.Parameters.AddWithValue("@Gender", GenderEntry.SelectedItem);
                command.Parameters.AddWithValue("@RegistrationNo", RegistrationNumberEntry.Text);
                command.ExecuteNonQuery();
            }
            else
            {
                List<string> modifiedFields = new List<string>();
                if (FirstNameEntry.IsModified)
                    modifiedFields.Add(FirstNameEntry.QueryString);
                if (LastNameEntry.IsModified)
                    modifiedFields.Add(LastNameEntry.QueryString);
                if (ContactEntry.IsModified)
                    modifiedFields.Add(ContactEntry.QueryString);
                if (GenderEntry.IsModified)
                    modifiedFields.Add("Gender = (SELECT Id FROM Lookup WHERE VALUE='"+GenderEntry.SelectedItem+"')");
                if (EmailEntry.IsModified)
                    modifiedFields.Add(EmailEntry.QueryString);
                if (DateEntry.IsModified)
                    modifiedFields.Add(DateEntry.QueryString);
                if (modifiedFields.Count==0)
                {
                    NavigationService.GoBack();
                    return;
                }
                string updateString = string.Join(",", modifiedFields);
                command = new SqlCommand(@"UPDATE Person SET "+updateString +" WHERE Id=@ID",conn);
                command.Parameters.AddWithValue("@ID", updateId);
            }
            command.ExecuteNonQuery();
            NavigationService.Content = new StudentView();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Content = new StudentView();
        }
    }
}
