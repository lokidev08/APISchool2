using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APISchool.Data;
using APISchool;

namespace APISchool.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlumnosController : ControllerBase
{
    private readonly ColegioDbContext _context;

    public AlumnosController(ColegioDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Alumno>>> Get()
    {
        return await _context.Alumnos.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Alumno>> Get(Guid id)
    {
        var alumno = await _context.Alumnos.FindAsync(id);
        if (alumno == null) return NotFound();
        return alumno;
    }

    [HttpPost]
    public async Task<ActionResult<Alumno>> Post(AlumnoCreateRequest request)
    {
        var alumno = new Alumno
        {
            Name = request.Name,
            Apellido = request.Apellido,
            Sexo = request.Sexo,
            FechaNacimiento = request.FechaNacimiento,
            NumeroIdentificacion = request.NumeroIdentificacion,
            CreatedBy = request.CreatedBy,
            Status = request.Status
        };
        if (string.IsNullOrWhiteSpace(alumno.Name) || string.IsNullOrWhiteSpace(alumno.Apellido) || string.IsNullOrWhiteSpace(alumno.Sexo) || string.IsNullOrWhiteSpace(alumno.NumeroIdentificacion) || string.IsNullOrWhiteSpace(alumno.CreatedBy) || string.IsNullOrWhiteSpace(alumno.Status))
        {
            return BadRequest("Faltan campos obligatorios");
        }
        if (alumno.Name == "string" || alumno.Apellido == "string" || alumno.NumeroIdentificacion == "string" || alumno.CreatedBy == "string" || alumno.Status == "string")
        {
            return BadRequest("Valores de prueba no permitidos");
        }
        if (alumno.Sexo != "M" && alumno.Sexo != "F")
        {
            return BadRequest("Sexo debe ser 'M' o 'F'");
        }
        if (alumno.Status != "Activo" && alumno.Status != "Inactivo")
        {
            return BadRequest("Status debe ser 'Activo' o 'Inactivo'");
        }
        if (alumno.FechaNacimiento >= DateTime.Now)
        {
            return BadRequest("Fecha de nacimiento no puede ser futura ni hoy");
        }
        if (alumno.FechaNacimiento <= DateTime.Now.AddYears(-100))
        {
            return BadRequest("Fecha de nacimiento no puede ser hace más de 100 años");
        }
        if (await _context.Alumnos.AnyAsync(a => a.NumeroIdentificacion == alumno.NumeroIdentificacion))
        {
            return BadRequest("Número de identificación ya existe");
        }
         _context.Entry(alumno).State = EntityState.Added;
         _context.Alumnos.Add(alumno);
        _context.Alumnos.Add(alumno);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = alumno.Id }, alumno);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, Alumno alumno)
    {
        if (id != alumno.Id) return BadRequest();
        if (string.IsNullOrWhiteSpace(alumno.Name) || string.IsNullOrWhiteSpace(alumno.Apellido) || string.IsNullOrWhiteSpace(alumno.Sexo) || string.IsNullOrWhiteSpace(alumno.NumeroIdentificacion) || string.IsNullOrWhiteSpace(alumno.CreatedBy) || string.IsNullOrWhiteSpace(alumno.Status))
        {
            return BadRequest("Faltan campos obligatorios");
        }
        if (alumno.Name == "string" || alumno.Apellido == "string" || alumno.NumeroIdentificacion == "string" || alumno.CreatedBy == "string" || alumno.Status == "string")
        {
            return BadRequest("Valores de prueba no permitidos");
        }
        if (alumno.Sexo != "M" && alumno.Sexo != "F")
        {
            return BadRequest("Sexo debe ser 'M' o 'F'");
        }
        if (alumno.Status != "Activo" && alumno.Status != "Inactivo")
        {
            return BadRequest("Status debe ser 'Activo' o 'Inactivo'");
        }
        if (alumno.FechaNacimiento >= DateTime.Now)
        {
            return BadRequest("Fecha de nacimiento no puede ser futura ni hoy");
        }
        if (alumno.FechaNacimiento <= DateTime.Now.AddYears(-100))
        {
            return BadRequest("Fecha de nacimiento no puede ser hace más de 100 años");
        }
        if (await _context.Alumnos.AnyAsync(a => a.NumeroIdentificacion == alumno.NumeroIdentificacion && a.Id != id))
        {
            return BadRequest("Número de identificación ya existe");
        }
         _context.Entry(alumno).State = EntityState.Modified;
         _context.Entry(alumno).State = EntityState.Modified;
        _context.Entry(alumno).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Alumnos.AnyAsync(e => e.Id == id)) return NotFound();
            throw;
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var alumno = await _context.Alumnos.FindAsync(id);
        if (alumno == null) return NotFound();
        _context.Alumnos.Remove(alumno);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
