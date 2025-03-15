using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Models;
using VietausWebAPI.WebAPI.DatabaseContext;
//using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;

namespace VietausWebAPI.WebAPI.Controllers.v1
{
    //[ApiVersion("1.0")]
    /// <summary>
    /// 
    /// </summary>

    [ApiController]
    [Route("api/[Controller]")]
    //[Authorize] //Này là cấp quyền cho local nhưng có thể cấp quyền chó global bằng cách đăng ký trong programsk
    [AllowAnonymous]

    public class CitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cities>>> GetCities()
        {
            return await _context.Cities.ToListAsync();
        }

        // GET: api/Cities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cities>> GetCities(Guid id)
        {
            var cities = await _context.Cities.FindAsync(id);

            if (cities == null)
            {
                return NotFound();
            }

            return cities;
        }

        // PUT: api/Cities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCities(Guid id, Cities cities)
        {
            if (id != cities.Id)
            {
                return BadRequest();
            }

            _context.Entry(cities).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CitiesExists(id))
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

        // POST: api/Cities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cities>> PostCities(Cities cities)
        {
            _context.Cities.Add(cities);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCities", new { id = cities.Id }, cities);
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCities(Guid id)
        {
            var cities = await _context.Cities.FindAsync(id);
            if (cities == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(cities);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CitiesExists(Guid id)
        {
            return _context.Cities.Any(e => e.Id == id);
        }
    }
}
