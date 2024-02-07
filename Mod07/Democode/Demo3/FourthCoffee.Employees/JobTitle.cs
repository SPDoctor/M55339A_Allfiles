namespace FourthCoffee.Employees;

public partial class JobTitle
{
    public int JobTitleId { get; set; }

    public string? Job { get; set; }

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();
}
