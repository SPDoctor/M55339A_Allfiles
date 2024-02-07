using Microsoft.EntityFrameworkCore;

namespace School
{
    public class SchoolDBContext : DbContext
    {
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=..\..\..\school.db");
        }

        public void SeedDatabase()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
            Teachers?.Add(new Teacher() { Id = 1, FirstName = "Esther", LastName = "Valle", Class = "3C" });
            Teachers?.Add(new Teacher() { Id = 2, FirstName = "David", LastName = "Waite", Class = "4B" });
            Teachers?.Add(new Teacher() { Id = 3, FirstName = "Belinda", LastName = "Newman", Class = "2A" });

            Students?.Add(new Student() { FirstName = "Kevin", LastName = "Liu", DateOfBirth = new DateTime(2005, 10, 10), TeacherId = 1 });
            Students?.Add(new Student() { FirstName = "Martin", LastName = "Weber", DateOfBirth = new DateTime(2005, 9, 7), TeacherId = 1 });
            Students?.Add(new Student() { FirstName = "George", LastName = "Li", DateOfBirth = new DateTime(2005, 8, 10), TeacherId = 1 });
            Students?.Add(new Student() { FirstName = "Lisa", LastName = "Miller", DateOfBirth = new DateTime(2005, 5, 4), TeacherId = 1 });
            Students?.Add(new Student() { FirstName = "Run", LastName = "Liu", DateOfBirth = new DateTime(2005, 10, 23), TeacherId = 1 });
            Students?.Add(new Student() { FirstName = "Sean", LastName = "Stewart", DateOfBirth = new DateTime(2003, 2, 18), TeacherId = 2 });
            Students?.Add(new Student() { FirstName = "Olinda", LastName = "Turner", DateOfBirth = new DateTime(2003, 5, 17), TeacherId = 2 });
            Students?.Add(new Student() { FirstName = "Jon", LastName = "Orton", DateOfBirth = new DateTime(2002, 8, 10), TeacherId = 2 });
            Students?.Add(new Student() { FirstName = "Toby", LastName = "Nixon", DateOfBirth = new DateTime(2002, 12, 15), TeacherId = 2 });
            Students?.Add(new Student() { FirstName = "Daniela", LastName = "Guinot", DateOfBirth = new DateTime(2002, 8, 1), TeacherId = 2 });
            Students?.Add(new Student() { FirstName = "Vijay", LastName = "Sundaram", DateOfBirth = new DateTime(2007, 2, 3), TeacherId = 3 });
            Students?.Add(new Student() { FirstName = "Eric", LastName = "Gruber", DateOfBirth = new DateTime(2007, 5, 26), TeacherId = 3 });
            Students?.Add(new Student() { FirstName = "Chris", LastName = "Meyer", DateOfBirth = new DateTime(2006, 5, 9), TeacherId = 3 });
            Students?.Add(new Student() { FirstName = "Yuhong", LastName = "Li", DateOfBirth = new DateTime(2007, 5, 28), TeacherId = 3 });
            Students?.Add(new Student() { FirstName = "Yan", LastName = "Li", DateOfBirth = new DateTime(2007, 3, 31), TeacherId = 3 });


            SaveChanges();
        }
    }
}
