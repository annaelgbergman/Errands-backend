using System.ComponentModel.DataAnnotations;

namespace issue.Models
{
    public class IssueModel
    {
        public int Id { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modefied { get; set; }

        public StatusModel Status { get; set; }

        public CustomerModel Customer { get; set; }
    }
}
