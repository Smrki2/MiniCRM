namespace MiniCRM.Models
{
    public class SalesLead
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public string Status { get; set; } = "New";
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
