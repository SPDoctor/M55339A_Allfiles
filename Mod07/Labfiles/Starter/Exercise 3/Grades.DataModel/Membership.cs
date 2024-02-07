namespace Grades.DataModel;

public partial class Membership
{
    public Guid ApplicationId { get; set; }

    public Guid UserId { get; set; }

    public string Password { get; set; } = null!;

    public int PasswordFormat { get; set; }

    public string PasswordSalt { get; set; } = null!;

    public string? Email { get; set; }

    public string? PasswordQuestion { get; set; }

    public string? PasswordAnswer { get; set; }

    public bool IsApproved { get; set; }

    public bool IsLockedOut { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime LastLoginDate { get; set; }

    public DateTime LastPasswordChangedDate { get; set; }

    public DateTime LastLockoutDate { get; set; }

    public int FailedPasswordAttemptCount { get; set; }

    public DateTime FailedPasswordAttemptWindowStart { get; set; }

    public int FailedPasswordAnswerAttemptCount { get; set; }

    public DateTime FailedPasswordAnswerAttemptWindowsStart { get; set; }

    public string? Comment { get; set; }

    public virtual Application Application { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
