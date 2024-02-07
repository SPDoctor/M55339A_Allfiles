namespace Grades.DataModel;

public partial class AspnetWebEventEvent
{
    public string EventId { get; set; } = null!;

    public DateTime EventTimeUtc { get; set; }

    public DateTime EventTime { get; set; }

    public string EventType { get; set; } = null!;

    public decimal EventSequence { get; set; }

    public decimal EventOccurrence { get; set; }

    public int EventCode { get; set; }

    public int EventDetailCode { get; set; }

    public string? Message { get; set; }

    public string? ApplicationPath { get; set; }

    public string? ApplicationVirtualPath { get; set; }

    public string MachineName { get; set; } = null!;

    public string? RequestUrl { get; set; }

    public string? ExceptionType { get; set; }

    public string? Details { get; set; }
}
