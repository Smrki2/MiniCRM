using System.Collections;

namespace MiniCRM.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
        public ICollection<SalesLead> SalesLeads { get; set; } = new List<SalesLead>();
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
