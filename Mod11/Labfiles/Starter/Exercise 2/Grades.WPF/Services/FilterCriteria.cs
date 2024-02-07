using Grades.DataModel;

namespace Grades.WPF.Services
{
    public class FilterCriteria
    {
        public Subject CurrentSubject { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
