using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Data;
using Paragraph = iTextSharp.text.Paragraph;
using System.Data.Common;

namespace FYP_Management_System.Views
{
    /// <summary>
    /// Interaction logic for ReportsGenerationView.xaml
    /// </summary>
    public partial class ReportsGenerationView : Page
    {
        private string destination = "Reports/";
        public ReportsGenerationView()
        {
            InitializeComponent();
        }
        private void generateReportFromTable(string pdfName,string query)
        {
            Directory.CreateDirectory("Reports");
            FileStream fs = new FileStream("Reports/"+pdfName, FileMode.Create, FileAccess.Write, FileShare.None);
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            DataTable table = Utils.FillDataGrid(query);
            document.Open();
            PdfPTable pdfTable = new PdfPTable(table.Columns.Count);
            foreach (DataColumn column in table.Columns)
            {
                pdfTable.AddCell(new Phrase(column.ColumnName));
            }
            foreach (DataRow row in table.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    pdfTable.AddCell(new Phrase(item.ToString()));

                }
            }
            document.Add(pdfTable);
            document.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory("Reports");
            FileStream fs = new FileStream("Reports/ProjectsInfo.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            DataTable table = Utils.FillDataGrid(@"SELECT Project.Title
                                                         	,(SELECT CONCAT(FirstName,' ',LastName)
                                                         	  FROM ProjectAdvisor 
                                                         	  JOIN Person 
                                                         	  ON Person.Id=ProjectAdvisor.AdvisorId 
                                                         	  WHERE ProjectAdvisor.AdvisorRole=(SELECT Id FROM Lookup WHERE Value='Main Advisor')
															  	AND GroupProject.ProjectId=ProjectAdvisor.ProjectId) [Main Advisor]
                                                         	 ,(SELECT CONCAT(FirstName,' ',LastName)
                                                         	  FROM ProjectAdvisor 
                                                         	  JOIN Person 
                                                         	  ON Person.Id=ProjectAdvisor.AdvisorId 
                                                         	  WHERE ProjectAdvisor.AdvisorRole=(SELECT Id FROM Lookup WHERE Value='Co-Advisror')
																AND GroupProject.ProjectId=ProjectAdvisor.ProjectId) [Co-Advisor]
                                                         	 ,(SELECT CONCAT(FirstName,' ',LastName)
                                                         	  FROM ProjectAdvisor 
                                                         	  JOIN Person 
                                                         	  ON Person.Id=ProjectAdvisor.AdvisorId 
                                                         	  WHERE ProjectAdvisor.AdvisorRole=(SELECT Id FROM Lookup WHERE Value='Industry Advisor')
																AND GroupProject.ProjectId=ProjectAdvisor.ProjectId) [Industry Advisor]
															                                                     	  ,(SELECT STUFF((SELECT ', ' +Student.RegistrationNo
                                                         			                     FROM GroupStudent
                                                         			                     JOIN Student
                                                         			                     ON Student.Id=GroupStudent.StudentId
                                                         			                     WHERE g.GroupId=GroupStudent.GroupId 
                                                         								 AND GroupStudent.Status=(SELECT Id FROM Lookup WHERE Lookup.Value='Active')
                                                         			                     FOR XML PATH('')),1,1,'') [Registration Numbers]
                                                                                          FROM GroupStudent g
                                                         								 WHERE g.GroupId=[Group].Id
                                                                                          GROUP BY g.GroupId) [Registration Numbers]
                                                         FROM [Group]
                                                         JOIN GroupProject
                                                         ON GroupProject.GroupId=[Group].Id
                                                         JOIN Project
                                                         ON Project.Id=GroupProject.ProjectId");
            document.Open();
            PdfPTable pdfTable = new PdfPTable(table.Columns.Count + 5 - 1);
            for(int i=0;i<table.Columns.Count-1;i++)
            { 
                pdfTable.AddCell(new Phrase(table.Columns[i].ColumnName));
            }
            pdfTable.AddCell(new Phrase("Student 1"));
            pdfTable.AddCell(new Phrase("Student 2"));
            pdfTable.AddCell(new Phrase("Student 3"));
            pdfTable.AddCell(new Phrase("Student 4"));
            pdfTable.AddCell(new Phrase("Student 5"));

            foreach (DataRow row in table.Rows)
            {
                for(int i=0;i<row.ItemArray.Count()-1;i++)
                {
                    pdfTable.AddCell(new Phrase(row.ItemArray[i].ToString()));
                }
               List<string> regNo =  row.ItemArray[row.ItemArray.Count() - 1].ToString().Split(',').ToList();
                for(int i=0;i<5;i++)
                {
                    if (i< regNo.Count)
                        pdfTable.AddCell(new Phrase(regNo[i]));
                    else
                        pdfTable.AddCell(new Phrase());
                }
            }
            document.Add(pdfTable);
            document.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            generateReportFromTable("Marksheet.pdf", @"SELECT RegistrationNo
                                                   	     ,ObtainedMarks
                                                   	     ,Evaluation.Name [Evaluation]
                                                   	     ,Project.Title [Project Name]
                                                   FROM GroupStudent
                                                   JOIN GroupEvaluation
                                                   ON GroupStudent.GroupId=GroupEvaluation.GroupId
                                                   JOIN Student
                                                   ON Student.Id=GroupStudent.StudentId
                                                   JOIN GroupProject
                                                   ON GroupProject.GroupId=GroupStudent.GroupId
                                                   JOIN Project
                                                   ON Project.Id=GroupProject.ProjectId
                                                   JOIN Evaluation
                                                   ON Evaluation.Id=GroupEvaluation.EvaluationId
                                                   ORDER BY RegistrationNo");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            generateReportFromTable("AdvisorsAssignments.pdf", @"SELECT CONCAT(FirstName,' ',LastName) [Advisor Name],Lookup.Value Desgination,COUNT(AdvisorId) [No. of Assignments]
                                                                 FROM ProjectAdvisor
                                                                 JOIN Person
                                                                 ON Person.Id=ProjectAdvisor.AdvisorId
                                                                 JOIN Advisor
                                                                 ON Advisor.Id=Person.Id
                                                                 JOIN Lookup
                                                                 ON Lookup.Id=Advisor.Designation
                                                                 GROUP BY AdvisorId,CONCAT(FirstName,' ',LastName),Lookup.Value");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            generateReportFromTable("UnderPerformingStudent.pdf", @"SELECT RegistrationNo,Evaluation.Name Evaluation,CONVERT(VARCHAR,ROUND((CONVERT(FLOAT,ObtainedMarks)/TotalMarks)*100,2)) + '%' Percentage
                                                                    FROM GroupStudent
                                                                    JOIN GroupEvaluation
                                                                    ON GroupEvaluation.GroupId=GroupStudent.GroupId
                                                                    JOIN Evaluation
                                                                    ON Evaluation.Id=GroupEvaluation.EvaluationId
                                                                    JOIN Student
                                                                    ON Student.Id=GroupStudent.StudentId
                                                                    WHERE (ObtainedMarks/TotalMarks)*100 < 33");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            generateReportFromTable("GroupChangingStudents.pdf", @"SELECT RegistrationNo,CONCAT(FirstName,' ',LastName) Name,COUNT(*) [Number of Changes]
                                                                   FROM GroupStudent
                                                                   JOIN Student
                                                                   ON Student.Id=GroupStudent.StudentId
                                                                   JOIN Person
                                                                   ON Person.Id=GroupStudent.StudentId
                                                                   WHERE GroupStudent.Status=(SELECT Id FROM Lookup WHERE Value='Inactive')
                                                                   GROUP BY RegistrationNo,CONCAT(FirstName,' ',LastName)");
        }
    }
}
