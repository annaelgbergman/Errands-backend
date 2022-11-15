using System.ComponentModel.DataAnnotations;

namespace issue.Models
{
    public class StatusRequest
    {
        [Required]
        public string Status { get; set; }
    }
}
