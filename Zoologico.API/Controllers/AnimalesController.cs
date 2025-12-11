using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zoologico.Modelos;

namespace Zoologico.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalesController : ControllerBase
    {
        private readonly ZoologicoAPIContext _context;

        public AnimalesController(ZoologicoAPIContext context)
        {
            _context = context;
        }

        // GET: api/Animales
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<Animal>>>> GetAnimales()
        {
            try
            {
                var data = await _context.Animales.ToListAsync();
                return ApiResult<List<Animal>>.Ok(data);
            }
            catch (Exception ex)
            {
                return ApiResult<List<Animal>>.Fail(ex.Message);
            }
        }

        // GET: api/Animales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<Animal>>> GetAnimal(int id)
        {
            try
            {
                var animal = await _context
                    .Animales
                    .Include(e => e.Raza)
                    .Include(e => e.Especie)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (animal == null)
                {
                    return ApiResult<Animal>.Fail("DAtos no encontrados");
                }

                return ApiResult<Animal>.Ok(animal);
            }
            catch (Exception ex)
            {
                return ApiResult<Animal>.Fail(ex.Message);
            }
        }

        // PUT: api/Animales/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult<Animal>>> PutAnimal(int id, Animal animal)
        {
            if (id != animal.Id)
            {
                return ApiResult<Animal>.Fail("No coinciden los identificadores");
            }

            _context.Entry(animal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!AnimalExists(id))
                {
                    return ApiResult<Animal>.Fail("Datos no encontrados");
                }
                else
                {
                    return ApiResult<Animal>.Fail(ex.Message);
                }
            }

            return ApiResult<Animal>.Ok(null);
        }

        // POST: api/Animales
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApiResult<Animal>>> PostAnimal(Animal animal)
        {
            try
            {
                _context.Animales.Add(animal);
                await _context.SaveChangesAsync();

                return ApiResult<Animal>.Ok(animal);
            }
            catch (Exception ex)
            {
                return ApiResult<Animal>.Fail(ex.Message);
            }
        }

        // DELETE: api/Animales/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult<Animal>>> DeleteAnimal(int id)
        {
            try
            {
                var animal = await _context.Animales.FindAsync(id);
                if (animal == null)
                {
                    return ApiResult<Animal>.Fail("Datos no encontrados");
                }

                _context.Animales.Remove(animal);
                await _context.SaveChangesAsync();

                return ApiResult<Animal>.Ok(null);
            }
            catch (Exception ex)
            {
                return ApiResult<Animal>.Fail(ex.Message);
            }
        }

        private bool AnimalExists(int id)
        {
            return _context.Animales.Any(e => e.Id == id);
        }
    }
}
