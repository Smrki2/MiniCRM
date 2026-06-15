using Microsoft.EntityFrameworkCore;
using MiniCRM.Models;
namespace MiniCRM.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options ) : base( options ) { }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<SalesLead> SalesLeads { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
    }
}
