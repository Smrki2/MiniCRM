using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCRM.Data;
using MiniCRM.Models;

namespace MiniCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            return await _context.Companies
                .Include(c => c.Contacts)
                .Include(c => c.SalesLeads)
                .Include(c => c.Tasks)
                .ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<Company>> CreateCompany(Company company)
        {
            _context.Companies.Add(company);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCompany),
                new { id = company.Id },
                company);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var company = await _context.Companies
                .Include(c => c.Contacts)
                .Include(c => c.SalesLeads)
                .Include(c => c.Tasks)
                .FirstOrDefaultAsync(c => c.Id == id);

            if(company == null)
            {
                return NotFound();
            }
            return company;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, Company company)
        {
            if(id != company.Id)
            {
                return BadRequest();
            }
            _context.Entry(company).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!CompanyExists(id))
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if(company == null)
            {
                return NotFound();
            }
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(c => c.Id == id);
        }
    }
}
