using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace School
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Connection to the School database
        private SchoolDBContext schoolContext;

        // Field for tracking the currently selected teacher
        private Teacher teacher;

        #region Predefined code

        public MainWindow()
        {
            InitializeComponent();
        }

        // Connect to the database and display the list of teachers when the window appears
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            schoolContext = new SchoolDBContext();
            schoolContext.Database.EnsureCreated();
            schoolContext.Teachers.Load();
            if (schoolContext.Teachers.Count() == 0)
            {
                schoolContext.SeedDatabase();
                schoolContext.Teachers.Load();
            }
            schoolContext.Students.Load();
            teachersList.DataContext = schoolContext.Teachers?.Local.ToObservableCollection();
        }

        // When the user selects a different teacher, fetch and display the students for that teacher
        private void teachersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Find the teacher that has been selected
            teacher = teachersList.SelectedItem as Teacher;

            // Use databinding to display these students
            studentsList.DataContext = teacher.Students;
        }

        #endregion

        // When the user presses a key, determine whether to add a new student to a class, remove a student from a class, or modify the details of a student
        private void studentsList_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                // If the user pressed Enter, edit the details for the currently selected student
                case Key.Enter:
                    Student student = studentsList.SelectedItem as Student;

                    // Use the StudentsForm to display and edit the details of the student
                    StudentForm sf = new StudentForm();

                    // Set the title of the form and populate the fields on the form with the details of the student           
                    sf.Title = "Edit Student Details";
                    sf.firstName.Text = student.FirstName;
                    sf.lastName.Text = student.LastName;
                    sf.dateOfBirth.Text = student.DateOfBirth.ToString("d", CultureInfo.InvariantCulture); // Format the date to omit the time element

                    // Display the form
                    if (sf.ShowDialog().Value)
                    {
                        // When the user closes the form, copy the details back to the student
                        student.FirstName = sf.firstName.Text;
                        student.LastName = sf.lastName.Text;
                        student.DateOfBirth = DateTime.Parse(sf.dateOfBirth.Text, CultureInfo.InvariantCulture);
                        studentsList.Items.Refresh();
                        saveChanges.IsEnabled = true;
                    }
                    break;

                case Key.Insert:
                    sf = new StudentForm();

                    // Set the title of the form           
                    sf.Title = "New Student for Class " + teacher.Class;

                    // Display the form
                    if (sf.ShowDialog().Value)
                    {
                        // When the user closes the form, use details to create new student
                        Student newStudent = new Student();
                        newStudent.FirstName = sf.firstName.Text;
                        newStudent.LastName = sf.lastName.Text;
                        newStudent.DateOfBirth = DateTime.Parse(sf.dateOfBirth.Text, CultureInfo.InvariantCulture);
                        teacher.Students.Add(newStudent);
                        saveChanges.IsEnabled = true;
                    }
                    break;

                case Key.Delete:
                    student = studentsList.SelectedItem as Student;
                    // display the confirmation dialog
                    MessageBoxResult response = MessageBox.Show(
                       string.Format("Remove {0}", student.FirstName + " " + student.LastName),
                       "Confirm",
                       MessageBoxButton.YesNo,
                       MessageBoxImage.Question,
                        MessageBoxResult.No);
                    // remove student if user clicked 'Yes'
                    if (response == MessageBoxResult.Yes)
                    {
                        teacher.Students.Remove(student);
                        saveChanges.IsEnabled = true;
                    }
                    break;
            }
        }

        #region Predefined code

        private void studentsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        // Save changes back to the database and make them permanent
        private void saveChanges_Click(object sender, RoutedEventArgs e)
        {
            schoolContext.SaveChanges();
        }

        #endregion
    }

    [ValueConversion(typeof(string), typeof(Decimal))]
    class AgeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              System.Globalization.CultureInfo culture)
        {
            // Convert the date of birth provided in the value parameter and convert to the age of the student in years
            // TODO: Exercise 4: Task 2a: Check that the value provided is not null. If it is, return an empty string
            // TODO: Exercise 4: Task 2b: Convert the value provided into a DateTime value
            // TODO: Exercise 4: Task 2c: Work out the difference between the current date and the value provided
            // TODO: Exercise 4: Task 2d: Convert this result into a number of years
            // TODO: Exercise 4: Task 2e: Convert the number of years into a string and return it

            return "";
        }

        #region Predefined code

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
