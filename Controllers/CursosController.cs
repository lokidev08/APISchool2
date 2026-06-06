using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APISchool.Data;
using APISchool;

namespace APISchool.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CursosController : ControllerBase
{
    private readonly ColegioDbContext _context;

    public CursosController(ColegioDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Curso>>> Get()
    {
        return await _context.Cursos.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Curso>> Get(Guid id)
    {
        var curso = await _context.Cursos.FindAsync(id);
        if (curso == null) return NotFound();
        return curso;
    }

    [HttpGet("{anio}/por-anio")]
    public async Task<ActionResult<Curso>> Get(string anio)
    {
        var curso = await _context.Cursos.FirstOrDefaultAsync(c => c.anio == anio);
        if (curso == null) return NotFound();
        return curso;
    }

    [HttpPost]
    public async Task<ActionResult<Curso>> Post(CursoCreateRequest request)
    {
        var curso = new Curso
        {
            Nombre = request.Nombre,
            anio = request.anio,
            CreatedBy = request.CreatedBy,
            Status = request.Status
        };
        if (string.IsNullOrWhiteSpace(curso.Nombre) || string.IsNullOrWhiteSpace(curso.anio) || string.IsNullOrWhiteSpace(curso.CreatedBy) || string.IsNullOrWhiteSpace(curso.Status))
        {
            return BadRequest("Faltan campos obligatorios");
        }
        if (curso.Nombre == "string" || curso.anio == "string" || curso.CreatedBy == "string" || curso.Status == "string")
        {
            return BadRequest("Valores de prueba no permitidos");
        }
        if (curso.Status != "Activo" && curso.Status != "Inactivo")
        {
            return BadRequest("Status debe ser 'Activo' o 'Inactivo'");
        }
        if (await _context.Cursos.AnyAsync(c => c.Nombre == curso.Nombre && c.anio == curso.anio))
         {
             return BadRequest("Ya existe un curso con el mismo nombre y año");
         }
          _context.Entry(curso).State = EntityState.Added;
         _context.Cursos.Add(curso);
        _context.Cursos.Add(curso);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = curso.Id }, curso);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, Curso curso)
    {
        if (id != curso.Id) return BadRequest();
        if (string.IsNullOrWhiteSpace(curso.Nombre) || string.IsNullOrWhiteSpace(curso.anio) || string.IsNullOrWhiteSpace(curso.CreatedBy) || string.IsNullOrWhiteSpace(curso.Status))
        {
            return BadRequest("Faltan campos obligatorios");
        }
        if (curso.Nombre == "string" || curso.anio == "string" || curso.CreatedBy == "string" || curso.Status == "string")
        {
            return BadRequest("Valores de prueba no permitidos");
        }
        if (curso.Status != "Activo" && curso.Status != "Inactivo")
        {
            return BadRequest("Status debe ser 'Activo' o 'Inactivo'");
        }
        if (await _context.Cursos.AnyAsync(c => c.Nombre == curso.Nombre && c.anio == curso.anio))
        {
            return BadRequest("Ya existe un curso con el mismo nombre y año");
        }
        _context.Entry(curso).State = EntityState.Modified;
        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Cursos.AnyAsync(e => e.Id == id)) return NotFound();
            throw;
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var curso = await _context.Cursos.FindAsync(id);
        if (curso == null) return NotFound();
        if (await _context.Secciones.AnyAsync(s => s.IdCurso == id))
        {
            return BadRequest("No se puede eliminar el curso porque tiene secciones asociadas");
        }
        _context.Cursos.Remove(curso);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

