﻿using GradesPrototype.Data;
using GradesPrototype.Services;
using System.Windows;
using System.Windows.Controls;

namespace GradesPrototype.Views
{
    /// <summary>
    /// Interaction logic for StudentProfile.xaml
    /// </summary>
    public partial class StudentProfile : UserControl
    {
        public StudentProfile()
        {
            InitializeComponent();
        }

        #region Event Members
        public event EventHandler Back;
        #endregion

        #region Events
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // If the user is not a teacher, do nothing
            if (SessionContext.UserRole != Role.Teacher)
            {
                return;
            }

            // If the user is a teacher, raise the Back event
            // The MainWindow page has a handler that catches this event and returns to the Students page
            if (Back != null)
            {
                Back(sender, e);
            }
        }
        #endregion

        // TODO: Exercise 1: Task 4d: Display the details for the current student including the grades for the student
        // The name of the student is available in the CurrentStudent property of the global context
        // Grades data is hardcoded in the XAML code for the StudentProfile view in this version of the prototype
        public void Refresh()
        {

        }
    }
}
