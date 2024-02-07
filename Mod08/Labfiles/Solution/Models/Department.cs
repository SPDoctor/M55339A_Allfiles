using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Website.Models
{
    public class Department
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        [StringLength(20)]
        public string Street { get; set; }

        public bool Open { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [JsonIgnore]
        public virtual ICollection<Meeting> Meetings { get; set; }
    }
}
