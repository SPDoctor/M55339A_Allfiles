﻿using Grades.DataModel;
using GradesPrototype.Services;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace GradesPrototype.Controls
{
    /// <summary>
    /// Interaction logic for AssignStudentDialog.xaml
    /// </summary>
    public partial class AssignStudentDialog : Window
    {
        public AssignStudentDialog()
        {
            InitializeComponent();
        }

        // Refresh the display of unassigned students
        private void Refresh()
        {
            // Find all unassigned students - they have a TeacherID of zero
            SessionContext.DBContext.Students.Load<Student>();
            var unassignedStudents = from s in SessionContext.DBContext.Students.Local
                                     where s.TeacherUserId == null
                                     select s;




            // If there are no unassigned students, then display the "No unassigned students" message
            // and hide the list of unassigned students
            if (unassignedStudents.Count() == 0)
            {
                txtMessage.Visibility = Visibility.Visible;
                list.Visibility = Visibility.Collapsed;
            }
            else
            {
                // If there are unassigned students, hide the "No unassigned students" message 
                // and display the list of unassigned students
                txtMessage.Visibility = Visibility.Collapsed;
                list.Visibility = Visibility.Visible;

                // Bind the ItemsControl on the dialog to the list of unassigned students
                // The names of the students will appear in the ItemsControl on the dialog
                list.ItemsSource = unassignedStudents;
            }
        }

        private void AssignStudentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        // Enroll a student in the teacher's class
        private void Student_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Determine which student the user clicked using the StudentID held in the Tag property of the Button that the user clicked
                Button studentClicked = sender as Button;
                Guid studentID = (Guid)studentClicked.Tag;

                // Find this student in the Students collection
                Student student = (from s in SessionContext.DBContext.Students
                                   where s.UserId == studentID
                                   select s).First();

                // Prompt the user to confirm that they wish to add this student to their class
                string message = String.Format("Add {0} {1} to your class?", student.FirstName, student.LastName);
                MessageBoxResult reply = MessageBox.Show(message, "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);

                // If the user confirms, add the student to their class
                if (reply == MessageBoxResult.Yes)
                {
                    // Get the ID of the currently logged-on teacher
                    Guid teacherID = SessionContext.CurrentTeacher.UserId;

                    // Enrolling functionality to be added in Exercise 3 



                    // Refresh the display - the newly assigned student should disappear from the list of unassigned students
                    Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error enrolling student", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            // Close the dialog box
            this.Close();
        }
    }
}