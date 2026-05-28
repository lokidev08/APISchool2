using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APISchool.Data;
using APISchool;

namespace APISchool.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeccionesController : ControllerBase
{
    private readonly ColegioDbContext _context;

    public SeccionesController(ColegioDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Seccion>>> Get()
    {
        return await _context.Secciones.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Seccion>> Get(Guid id)
    {
        var seccion = await _context.Secciones.FindAsync(id);
        if (seccion == null) return NotFound();
        return seccion;
    }

    [HttpPost]
    public async Task<ActionResult<Seccion>> Post(SeccionCreateRequest request)
    {
        var seccion = new Seccion
        {
            IdCurso = request.IdCurso,
            IdProfesorEncargado = request.IdProfesorEncargado,
            Name = request.Name,
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin,
            CreatedBy = request.CreatedBy,
            Status = request.Status
        };
        if (string.IsNullOrWhiteSpace(seccion.Name) || seccion.IdCurso == Guid.Empty || seccion.IdProfesorEncargado == Guid.Empty || string.IsNullOrWhiteSpace(seccion.CreatedBy) || string.IsNullOrWhiteSpace(seccion.Status))
        {
            return BadRequest("Faltan campos obligatorios");
        }
        if (seccion.Name == "string" || seccion.CreatedBy == "string" || seccion.Status == "string")
        {
            return BadRequest("Valores de prueba no permitidos");
        }
        if (seccion.IdProfesorEncargado != Guid.Empty)
        {
            var profesor = await _context.Profesores.FindAsync(seccion.IdProfesorEncargado);
            if (profesor == null) return BadRequest("Profesor encargado no existe");
        }
        if (seccion.IdCurso != Guid.Empty)
        {
            var curso = await _context.Cursos.FindAsync(seccion.IdCurso);
            if (curso == null) return BadRequest("Curso no existe");
        }
        if (seccion.Status != "Activo" && seccion.Status != "Inactivo")
        {
            return BadRequest("Status debe ser 'Activo' o 'Inactivo'");
        }
        if (seccion.FechaInicio >= seccion.FechaFin)
        {   
            return BadRequest("Fecha de inicio debe ser anterior a fecha de fin");
        }
        if (await _context.Secciones.AnyAsync(s => s.Name == seccion.Name && s.Id != seccion.Id && s.IdCurso == seccion.IdCurso))
        {
            return BadRequest("El nombre de la sección ya existe en este curso");
        }
         _context.Entry(seccion).State = EntityState.Added;
        _context.Secciones.Add(seccion);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = seccion.Id }, seccion);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, Seccion seccion)
    {
        if (id != seccion.Id) return BadRequest();
        if (string.IsNullOrWhiteSpace(seccion.Name) || seccion.IdCurso == Guid.Empty || seccion.IdProfesorEncargado == Guid.Empty || string.IsNullOrWhiteSpace(seccion.CreatedBy) || string.IsNullOrWhiteSpace(seccion.Status))
        {
            return BadRequest("Faltan campos obligatorios");
        }
        if (seccion.Name == "string" || seccion.CreatedBy == "string" || seccion.Status == "string")
        {
            return BadRequest("Valores de prueba no permitidos");
        }
        if (seccion.IdProfesorEncargado != Guid.Empty)
        {
            var profesor = await _context.Profesores.FindAsync(seccion.IdProfesorEncargado);
            if (profesor == null) return BadRequest("Profesor encargado no existe");
        }
        if (seccion.IdCurso != Guid.Empty)
        {
            var curso = await _context.Cursos.FindAsync(seccion.IdCurso);
            if (curso == null) return BadRequest("Curso no existe");
        }
        if (seccion.Status != "Activo" && seccion.Status != "Inactivo")
        {
            return BadRequest("Status debe ser 'Activo' o 'Inactivo'");
        }
        if (seccion.FechaInicio >= seccion.FechaFin)
        {
            return BadRequest("Fecha de inicio debe ser anterior a fecha de fin");
        }
        if (await _context.Secciones.AnyAsync(s => s.Name == seccion.Name && s.Id != seccion.Id && s.IdCurso == seccion.IdCurso))
        {
            return BadRequest("El nombre de la sección ya existe en este curso");
        }
        _context.Entry(seccion).State = EntityState.Modified;
        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Secciones.AnyAsync(e => e.Id == id)) return NotFound();
            throw;
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var seccion = await _context.Secciones.FindAsync(id);
        if (seccion == null) return NotFound();
        _context.Secciones.Remove(seccion);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
