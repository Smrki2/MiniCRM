namespace MiniCRM.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public bool IsCompleted {  get; set; }
        public int CompanyId {  get; set; }
        public Company? Company { get; set; }
    }
}
