namespace FourthCoffee.Employees;

public partial class Branch
{
    public int BranchId { get; set; }

    public string? BranchName { get; set; }

    public virtual ICollection<Employee> Employees { get; } = new List<Employee>();
}
