namespace Grades.DataModel;

public partial class Grade
{
    public int Id { get; set; }

    public string Assessment { get; set; } = null!;

    public string? Comments { get; set; }

    public DateTime AssessmentDate { get; set; }

    public int SubjectId { get; set; }

    public Guid StudentUserId { get; set; }

    public virtual Student StudentUser { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
