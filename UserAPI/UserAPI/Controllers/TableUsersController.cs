using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAPI.Models;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableUsersController : ControllerBase
    {
        private readonly dfug8uq2aj17f1Context _context;

        public TableUsersController(dfug8uq2aj17f1Context context)
        {
            _context = context;
        }

        // GET: api/TableUsers
        [HttpGet]
        public IEnumerable<TableUser> GetTableUser()
        {
            return _context.TableUser;
        }

        // GET: api/TableUsers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTableUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tableUser = await _context.TableUser.FindAsync(id);

            if (tableUser == null)
            {
                return NotFound();
            }

            return Ok(tableUser);
        }

        // PUT: api/TableUsers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTableUser([FromRoute] int id, [FromBody] TableUser tableUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tableUser.Userid)
            {
                return BadRequest();
            }

            _context.Entry(tableUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TableUserExists(id))
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

        // POST: api/TableUsers
        [HttpPost]
        public async Task<IActionResult> PostTableUser([FromBody] TableUser tableUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TableUser.Add(tableUser);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TableUserExists(tableUser.Userid))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTableUser", new { id = tableUser.Userid }, tableUser);
        }

        // DELETE: api/TableUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTableUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tableUser = await _context.TableUser.FindAsync(id);
            if (tableUser == null)
            {
                return NotFound();
            }

            _context.TableUser.Remove(tableUser);
            await _context.SaveChangesAsync();

            return Ok(tableUser);
        }

        private bool TableUserExists(int id)
        {
            return _context.TableUser.Any(e => e.Userid == id);
        }
    }
}