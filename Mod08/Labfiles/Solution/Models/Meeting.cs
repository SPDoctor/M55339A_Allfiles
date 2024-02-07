using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Website.Models
{
    public class Meeting
    {
        public int Id { get; set; }

        [Display(Name = "First name")]
        [StringLength(60, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [StringLength(60, MinimumLength = 3)]
        public string LastName { get; set; }

        [Display(Name = "Phone number"), DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Preferred meeting time"), DataType(DataType.DateTime)]
        public DateTime ReservationTime { get; set; }

        [Display(Name = "Number of participants")]
        [Range(1, 20)]
        public int Participants { get; set; }

        [Required(ErrorMessage = "Please select a school department.")]
        public int DepartmentId { get; set; }

        [Display(Name = "School department")]
        [JsonIgnore]
        public virtual Department Department { get; set; }
    }
}
