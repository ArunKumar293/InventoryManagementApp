using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagementApp.Repository;
using Microsoft.AspNetCore;


namespace InventoryManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryDBContext _context;

        public InventoryController(InventoryDBContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            return await _context.Item.ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItems(int id)
        {
            var item = await _context.Item.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> DeleteItems(int id)
        {
            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Item.Remove(item);
            await _context.SaveChangesAsync();

            return item;
        }

        [HttpPost]
        public async Task<ActionResult<Item>> PostItems(Item item)
        {
            _context.Item.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetItems", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditItems(int id, Item item)
        {
            if (item.Id != id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
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

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.Id == id);
        }
    }
}
