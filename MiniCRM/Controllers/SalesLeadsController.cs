using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniCRM.Data;
using MiniCRM.Models;
using MiniCRM.DTOs;

namespace MiniCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesLeadsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SalesLeadsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesLeadDto>>> GetSalesLeads()
        {
            var leads = await _context.SalesLeads
                .Include(s => s.Company)
                .ToListAsync();

            var result = leads.Select(s => new SalesLeadDto
            {
                Id = s.Id,
                Title = s.Title,
                Value = s.Value,
                Status = s.Status
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalesLeadDto>> GetSalesLead(int id)
        {
            var salesLead = await _context.SalesLeads
                .Include(s => s.Company)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (salesLead == null)
                return NotFound();

            var result = new SalesLeadDto
            {
                Id = salesLead.Id,
                Title = salesLead.Title,
                Value = salesLead.Value,
                Status = salesLead.Status
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<SalesLeadDto>> CreateSalesLead(SalesLead salesLead)
        {
            salesLead.Company = null;

            _context.SalesLeads.Add(salesLead);
            await _context.SaveChangesAsync();

            var result = new SalesLeadDto
            {
                Id = salesLead.Id,
                Title = salesLead.Title,
                Value = salesLead.Value,
                Status = salesLead.Status
            };

            return CreatedAtAction(nameof(GetSalesLead), new { id = salesLead.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSalesLead(int id, SalesLead salesLead)
        {
            if (id != salesLead.Id)
                return BadRequest();

            var existing = await _context.SalesLeads.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.Title = salesLead.Title;
            existing.Value = salesLead.Value;
            existing.Status = salesLead.Status;
            existing.CompanyId = salesLead.CompanyId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesLead(int id)
        {
            var salesLead = await _context.SalesLeads.FindAsync(id);

            if (salesLead == null)
                return NotFound();

            _context.SalesLeads.Remove(salesLead);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}