using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CovidApi.Models;

namespace CovidApi.Controllers
{
    [Route("api/thisisanawsomenewdotnetcore32api")]
    [ApiController]
    public class CovidItemsController : ControllerBase
    {
        private readonly CovidContext _context;

        public CovidItemsController(CovidContext context)
        {
            _context = context;
        }

        // GET: api/CovidItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CovidItemDTO>>> GetCovidItems()
        {
            return await _context.CovidItems.Select(item => ItemToDTO(item)).ToListAsync();
        }

        // GET: api/CovidItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CovidItemDTO>> GetCovidItem(long id)
        {
            var covidItem = await _context.CovidItems.FindAsync(id);

            if (covidItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(covidItem);
        }

        // PUT: api/CovidItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCovidItem(long id, CovidItemDTO covidItemDTO)
        {
            if (id != covidItemDTO.Id)
            {
                return BadRequest();
            }

            var covidItem = new CovidItem {
                Id = covidItemDTO.Id,
                IsComplete = covidItemDTO.IsComplete,
                Name= covidItemDTO.Name
            };

            _context.Entry(covidItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CovidItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CovidItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CovidItemDTO>> PostCovidItem(CovidItemDTO covidItemDTO)
        {
            var covidItem = new CovidItem {
                IsComplete = covidItemDTO.IsComplete,
                Name= covidItemDTO.Name
            };

            _context.CovidItems.Add(covidItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCovidItem), new { id = covidItemDTO.Id }, ItemToDTO(covidItem));
        }

        // DELETE: api/CovidItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CovidItemDTO>> DeleteCovidItem(long id)
        {
            var covidItem = await _context.CovidItems.FindAsync(id);
            if (covidItem == null)
            {
                return NotFound();
            }

            _context.CovidItems.Remove(covidItem);
            await _context.SaveChangesAsync();

            return ItemToDTO(covidItem);
        }

        private bool CovidItemExists(long id)
        {
            return _context.CovidItems.Any(e => e.Id == id);
        }

        private static CovidItemDTO ItemToDTO(CovidItem covidItem) => new CovidItemDTO
        {
            Id = covidItem.Id,
            Name = covidItem.Name,
            IsComplete = covidItem.IsComplete
        };
    }
}
