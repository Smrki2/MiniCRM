namespace MiniCRM.DTOs
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;

        public List<ContactDto> Contacts { get; set; } = new();
        public List<SalesLeadDto> SalesLeads { get; set; } = new();
        public List<TaskItemDto> Tasks { get; set; } = new();
    }
}