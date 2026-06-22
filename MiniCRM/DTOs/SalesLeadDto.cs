namespace MiniCRM.DTOs
{
    public class SalesLeadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}