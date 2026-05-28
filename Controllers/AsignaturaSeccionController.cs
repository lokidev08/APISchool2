using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APISchool.Data;
using APISchool;

namespace APISchool.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AsignaturaSeccionController : ControllerBase
{
    private readonly ColegioDbContext _context;

    public AsignaturaSeccionController(ColegioDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AsignaturaSeccion>>> Get()
    {
        return await _context.AsignaturasSecciones.ToListAsync();
    }
    [HttpPost]
    public async Task<ActionResult<AsignaturaSeccion>> Post(AsignaturaSeccionCreateRequest request)
    {
        var asigSec = new AsignaturaSeccion
        {
            IdAsignatura = request.IdAsignatura,
            IdSeccion = request.IdSeccion,
            CreatedBy = request.CreatedBy,
            Status = request.Status
        };
        if (asigSec.IdAsignatura == Guid.Empty || asigSec.IdSeccion == Guid.Empty || string.IsNullOrWhiteSpace(asigSec.CreatedBy) || string.IsNullOrWhiteSpace(asigSec.Status))
        {
            return BadRequest("Faltan campos obligatorios");
        }
        if (asigSec.CreatedBy == "string" || asigSec.Status == "string")
        {
            return BadRequest("Valores de prueba no permitidos");
        }
        if (asigSec.IdAsignatura != Guid.Empty)
        {
            var asignatura = await _context.Asignaturas.FindAsync(asigSec.IdAsignatura);
            if (asignatura == null) return BadRequest("Asignatura no existe");
        }
        if (asigSec.IdSeccion != Guid.Empty)
        {
            var seccion = await _context.Secciones.FindAsync(asigSec.IdSeccion);
            if (seccion == null) return BadRequest("Sección no existe");
        }
        if (asigSec.Status != "Activo" && asigSec.Status != "Inactivo")
        {
            return BadRequest("Status debe ser 'Activo' o 'Inactivo'");
        }
        if (await _context.AsignaturasSecciones.AnyAsync(a => a.IdAsignatura == asigSec.IdAsignatura && a.IdSeccion == asigSec.IdSeccion))
        {
            return BadRequest("Ya existe una asignatura con esa sección");
        }
        _context.AsignaturasSecciones.Add(asigSec);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = asigSec.Id }, asigSec);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AsignaturaSeccion>> Get(Guid id)
    {
        var asigSec = await _context.AsignaturasSecciones.FindAsync(id);
        if (asigSec == null) return NotFound();
        return Ok(asigSec);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, AsignaturaSeccion asigSec)
    {
        if (id != asigSec.Id) return BadRequest();
        if (asigSec.IdAsignatura == Guid.Empty || asigSec.IdSeccion == Guid.Empty || string.IsNullOrWhiteSpace(asigSec.CreatedBy) || string.IsNullOrWhiteSpace(asigSec.Status))
        {
            return BadRequest("Faltan campos obligatorios");
        }
        if (asigSec.CreatedBy == "string" || asigSec.Status == "string")
        {
            return BadRequest("Valores de prueba no permitidos");
        }
        if (asigSec.IdAsignatura != Guid.Empty)
        {
            var asignatura = await _context.Asignaturas.FindAsync(asigSec.IdAsignatura);
            if (asignatura == null) return BadRequest("Asignatura no existe");
        }
        if (asigSec.IdSeccion != Guid.Empty)
        {
            var seccion = await _context.Secciones.FindAsync(asigSec.IdSeccion);
            if (seccion == null) return BadRequest("Sección no existe");
        }
        if (asigSec.Status != "Activo" && asigSec.Status != "Inactivo")
        {
            return BadRequest("Status debe ser 'Activo' o 'Inactivo'");
        }
        _context.Entry(asigSec).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var asigSec = await _context.AsignaturasSecciones.FindAsync(id);
        if (asigSec == null) return NotFound();
        _context.AsignaturasSecciones.Remove(asigSec);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}