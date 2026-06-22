using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCRM.Data;
using MiniCRM.DTOs;
using MiniCRM.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies()
        {
            var companies = await _context.Companies
                .Include(c => c.Contacts)
                .Include(c => c.SalesLeads)
                .Include(c => c.Tasks)
                .ToListAsync();

            var result = companies.Select(c => new CompanyDto
            {
                Id = c.Id,
                Name = c.Name,
                Industry = c.Industry,
                Website = c.Website,

                Contacts = c.Contacts.Select(x => new ContactDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Phone = x.Phone,

                    CompanyId = c.Id,
                    CompanyName = c.Name
                }).ToList(),

                SalesLeads = c.SalesLeads.Select(x => new SalesLeadDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Value = x.Value,
                    Status = x.Status
                }).ToList(),

                Tasks = c.Tasks.Select(x => new TaskItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    DueDate = x.DueDate,
                    IsCompleted = x.IsCompleted
                }).ToList()
            });

            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            var result = new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Industry = company.Industry,
                Website = company.Website,
                Contacts = new(),
                SalesLeads = new(),
                Tasks = new()
            };

            return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(int id)
        {
            var company = await _context.Companies
                .Include(c => c.Contacts)
                .Include(c => c.SalesLeads)
                .Include(c => c.Tasks)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null)
                return NotFound();

            var result = new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Industry = company.Industry,
                Website = company.Website,

                Contacts = company.Contacts.Select(x => new ContactDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Phone = x.Phone,

                    CompanyId = company.Id,
                    CompanyName = company.Name
                }).ToList(),

                SalesLeads = company.SalesLeads.Select(x => new SalesLeadDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Value = x.Value,
                    Status = x.Status
                }).ToList(),

                Tasks = company.Tasks.Select(x => new TaskItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    DueDate = x.DueDate,
                    IsCompleted = x.IsCompleted
                }).ToList()
            };

            return Ok(result);
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
