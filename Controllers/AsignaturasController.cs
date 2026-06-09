using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APISchool.Data;
using APISchool;

namespace APISchool.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AsignaturasController : ControllerBase
{
    private readonly ColegioDbContext _context;

    public AsignaturasController(ColegioDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Asignatura>>> Get()
    {
        return await _context.Asignaturas.ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Asignatura>> Get(Guid id)
    {
        var asignatura = await _context.Asignaturas.FindAsync(id);
        if (asignatura == null) return NotFound();
        return asignatura;
    }
    [HttpPost]
    public async Task<ActionResult<Asignatura>> Post(AsignaturaCreateRequest request)
    {
        var asignatura = new Asignatura
        {
            Nombre = request.Nombre,
            CreatedBy = request.CreatedBy,
            Status = request.Status
        };
        if (string.IsNullOrWhiteSpace(request.Nombre) || string.IsNullOrWhiteSpace(request.CreatedBy) || string.IsNullOrWhiteSpace(request.Status))
        {
            return BadRequest("Faltan campos obligatorios");
        }
        if (request.Nombre == "string" || request.CreatedBy == "string" || request.Status == "string")
        {
            return BadRequest("Valores de prueba no permitidos");
        }
        if (request.Status != "Activo" && request.Status != "Inactivo")
        {
            return BadRequest("Status debe ser 'Activo' o 'Inactivo'");
        }
        if (await _context.Asignaturas.AnyAsync(a => a.Nombre == request.Nombre))
         {
             return BadRequest("El nombre de la asignatura ya existe");
         }
         _context.Entry(asignatura).State = EntityState.Added;
         _context.Asignaturas.Add(asignatura);
         await _context.SaveChangesAsync();
         return CreatedAtAction(nameof(Get), new { id = asignatura.Id }, asignatura);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, Asignatura asignatura)
    {
        if (id != asignatura.Id) return BadRequest();
        if (string.IsNullOrWhiteSpace(asignatura.Nombre) || string.IsNullOrWhiteSpace(asignatura.CreatedBy) || string.IsNullOrWhiteSpace(asignatura.Status))
        {
            return BadRequest("Faltan campos obligatorios");
        }
        if (asignatura.Nombre == "string" || asignatura.CreatedBy == "string" || asignatura.Status == "string")
        {
            return BadRequest("Valores de prueba no permitidos");
        }
        if (asignatura.Status != "Activo" && asignatura.Status != "Inactivo")
        {
            return BadRequest("Status debe ser 'Activo' o 'Inactivo'");
        }
        _context.Entry(asignatura).State = EntityState.Modified;
        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Asignaturas.AnyAsync(e => e.Id == id)) return NotFound();
            throw;
        }
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var asignatura = await _context.Asignaturas.FindAsync(id);
        if (asignatura == null) return NotFound();
        _context.Asignaturas.Remove(asignatura);
        await _context.SaveChangesAsync();
        return NoContent();
    }
} 