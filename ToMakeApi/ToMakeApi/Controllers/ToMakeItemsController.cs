using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToMakeApi.Models;

namespace ToMakeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToMakeItemsController : ControllerBase
    {
        private readonly ToMakeItemContext _context;

        public ToMakeItemsController(ToMakeItemContext context)
        {
            _context = context;
        }

        // GET: api/ToMakeItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToMakeItem>>> GetToMakeItems()
        {
            return await _context.ToMakeItems.ToListAsync();
        }

        // GET: api/ToMakeItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToMakeItem>> GetToMakeItem(long id)
        {
            var toMakeItem = await _context.ToMakeItems.FindAsync(id);

            if (toMakeItem == null)
            {
                return NotFound();
            }

            return toMakeItem;
        }

        // PUT: api/ToMakeItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToMakeItem(long id, ToMakeItem toMakeItem)
        {
            if (id != toMakeItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(toMakeItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToMakeItemExists(id))
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

        // POST: api/ToMakeItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ToMakeItem>> PostToMakeItem(ToMakeItem toMakeItem)
        {
            _context.ToMakeItems.Add(toMakeItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToMakeItem", new { id = toMakeItem.Id }, toMakeItem);
        }

        // DELETE: api/ToMakeItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToMakeItem(long id)
        {
            var toMakeItem = await _context.ToMakeItems.FindAsync(id);
            if (toMakeItem == null)
            {
                return NotFound();
            }

            _context.ToMakeItems.Remove(toMakeItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ToMakeItemExists(long id)
        {
            return _context.ToMakeItems.Any(e => e.Id == id);
        }
    }
}
