using Microsoft.EntityFrameworkCore;
using Website.Models;

namespace Server.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<EmployeeRequirements> EmployeesRequirements { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
               new Department
               {
                   Id = 1,
                   Name = "Technology",
                   Street = "610 St Stephens Gate",
                   Open = true,
                   PhoneNumber = "(20)-4266-8812"
               },
               new Department
               {
                   Id = 2,
                   Name = "Languages",
                   Street = "106 Bamborough Road",
                   Open = true,
                   PhoneNumber = "(07)-0116-7991"
               },
               new Department
               {
                   Id = 3,
                   Name = "Science",
                   Street = "99 Brubeck Avenue",
                   Open = false,
                   PhoneNumber = "(03)-4584-9768"
               },
               new Department
               {
                   Id = 4,
                   Name = "Art",
                   Street = "20 Fitzmaurice Road",
                   Open = true,
                   PhoneNumber = "(03)-8622-6758"
               },
               new Department
               {
                   Id = 5,
                   Name = "Mathematics",
                   Street = "26 Fitzmaurice Road",
                   Open = true,
                   PhoneNumber = "(03)-8622-6342"
               },
               new Department
               {
                   Id = 6,
                   Name = "Sport",
                   Street = "1 Dunstable Road",
                   Open = true,
                   PhoneNumber = "(03)-8622-6166"
               });

            modelBuilder.Entity<EmployeeRequirements>().HasData(
               new EmployeeRequirements
               {
                   Id = 1,
                   JobTitle = "Cleaner",
                   JobDescription = "Clean classrooms, common areas, and other facilities.",
                   MinimumAge = 18,
                   PricePerHour = 15
               },
               new EmployeeRequirements
               {
                   Id = 2,
                   JobTitle = "Canteen Staff",
                   JobDescription = "Prepare and serve food to teachers and students in our restaurant.",
                   MinimumAge = 18,
                   PricePerHour = 22
               },
               new EmployeeRequirements
               {
                   Id = 3,
                   JobTitle = "Deputy Headmaster",
                   JobDescription = "Responsible for the efficient running of the school and standing in for the Headmaster when unavailable.",
                   MinimumAge = 35,
                   PricePerHour = 50
               },
               new EmployeeRequirements
               {
                   Id = 4,
                   JobTitle = "Mathematics Teacher",
                   JobDescription = "Teach students mathematics up to year 12",
                   MinimumAge = 25,
                   PricePerHour = 45
               },
               new EmployeeRequirements
               {
                   Id = 5,
                   JobTitle = "Geography Teacher",
                   JobDescription = "Teach students geography up to year 10",
                   MinimumAge = 25,
                   PricePerHour = 42
               });

            modelBuilder.Entity<Meeting>().HasData(
               new Meeting
               {
                   Id = 1,
                   FirstName = "Terence",
                   LastName = "Smith",
                   PhoneNumber = "657-313-4997",
                   ReservationTime = DateTime.Now,
                   Participants = 4,
                   DepartmentId = 1
               },
               new Meeting
               {
                   Id = 2,
                   FirstName = "David",
                   LastName = "Brown",
                   PhoneNumber = "661-330-3205",
                   ReservationTime = DateTime.Now,
                   Participants = 4,
                   DepartmentId = 1
               },
               new Meeting
               {
                   Id = 3,
                   FirstName = "Mike",
                   LastName = "Johnson",
                   PhoneNumber = "601-987-2384",
                   ReservationTime = DateTime.Now,
                   Participants = 4,
                   DepartmentId = 1
               });
        }
    }
}
