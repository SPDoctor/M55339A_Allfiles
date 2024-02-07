namespace FourthCoffee.Employees;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public int? Branch { get; set; }

    public int? JobTitle { get; set; }

    public virtual Branch? BranchNavigation { get; set; }

    public virtual JobTitle? JobTitleNavigation { get; set; }
}
