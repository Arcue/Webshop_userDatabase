using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using UserAPI.Helpers;
using UserAPI.Models;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly dfug8uq2aj17f1Context _context;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(dfug8uq2aj17f1Context context, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<TableUser> GetTableUser()
        {
            return _context.TableUser;
        }

        // GET: api/Users/5
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

        // PUT: api/Users/
        //Redigerar en användare
        [HttpPut]
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

        // POST: api/Users
        //Registrerar en ny användare
        [HttpPost]
        public async Task<IActionResult> PostTableUser(string jsonString)
        {
            TableUser newUser = JsonConvert.DeserializeObject<TableUser>(jsonString);

            //Kollar att username och email inte registrerats förut
            if (UsernameAndEmailExists(newUser.Username, newUser.Email))
            {
                //Hittade användarman eller email redan
                return BadRequest("Username or email already exists");
            } else
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _context.TableUser.Add(newUser);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    
                }
            }
            return CreatedAtAction("GetTableUser", new { id = newUser.Userid }, newUser);
        }

        // DELETE: api/Users/5
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

        //Kollar om Username redan finns in listan
        private bool UsernameAndEmailExists(String userName, String email)
        {
            bool itExists = false;
            if (_context.TableUser.Any(e => e.Username == userName))
            {
                itExists = true;
            }
            if (_context.TableUser.Any(e => e.Email == email))
            {
                itExists = true;
            }

            return itExists;
        }






    }
}