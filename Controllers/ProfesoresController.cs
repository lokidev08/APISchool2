using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APISchool.Data;
using APISchool;

namespace APISchool.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfesoresController : ControllerBase
{
    private readonly ColegioDbContext _context;

    public ProfesoresController(ColegioDbContext context)
    {
        _context = context;
    }

    private static bool ValidarCedula(string cedula)
    {
        if (string.IsNullOrWhiteSpace(cedula) || cedula == "string") return false;
        cedula = cedula.Replace("-", "").Trim();
        if (cedula.Length != 11) return false;
        if (cedula.Distinct().Count() == 1) return false;
        int sum = 0;
        int[] multipliers = { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2 };
        for (int i = 0; i < 10; i++)
        {
            int valor = (cedula[i] - '0') * multipliers[i];
            if (valor > 9)
            {
                valor = (valor / 10) + (valor % 10);
            }
            sum += valor;
        }
        int digitoverificador = (10 - (sum % 10)) % 10;
        return digitoverificador == (cedula[10] - '0');
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Profesor>>> Get()
    {
        return await _context.Profesores.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Profesor>> Get(Guid id)
    {
        var profesor = await _context.Profesores.FindAsync(id);
        if (profesor == null) return NotFound();
        return profesor;
    }

    [HttpPost]
    public async Task<ActionResult<Profesor>> Post(ProfesorCreateRequest request)
    {
        var profesor = new Profesor
        {
            Name = request.Name,
            Apellido = request.Apellido,
            Sexo = request.Sexo,
            Cedula = request.Cedula,
            CreatedBy = request.CreatedBy,
            Status = request.Status
        };
        if (string.IsNullOrWhiteSpace(profesor.Name) || string.IsNullOrWhiteSpace(profesor.Apellido) || string.IsNullOrWhiteSpace(profesor.Sexo) || string.IsNullOrWhiteSpace(profesor.Cedula) || string.IsNullOrWhiteSpace(profesor.CreatedBy) || string.IsNullOrWhiteSpace(profesor.Status))
        {
            return BadRequest("Faltan campos obligatorios");
        }
        if (profesor.Name == "string" || profesor.Apellido == "string" || profesor.Sexo == "string" || profesor.Cedula == "string" || profesor.CreatedBy == "string" || profesor.Status == "string")
        {
            return BadRequest("Valores de prueba no permitidos");
        }
        if (profesor.Sexo != "M" && profesor.Sexo != "F")
        {
            return BadRequest("Sexo debe ser 'M' o 'F'");
        }
        if (profesor.Status != "Activo" && profesor.Status != "Inactivo")
        {
            return BadRequest("Status debe ser 'Activo' o 'Inactivo'");
        }
        if (!ValidarCedula(profesor.Cedula))
        {
            return BadRequest("Cédula no válida");
        }
        if (await _context.Profesores.AnyAsync(p => p.Cedula == profesor.Cedula))
        {
            return BadRequest("La cédula ya existe");
        }
         _context.Entry(profesor).State = EntityState.Added;
         _context.Profesores.Add(profesor);
         await _context.SaveChangesAsync();
        _context.Profesores.Add(profesor);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = profesor.Id }, profesor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, Profesor profesor)
    {
        if (id != profesor.Id) return BadRequest();
        if (string.IsNullOrWhiteSpace(profesor.Name) || string.IsNullOrWhiteSpace(profesor.Apellido) || string.IsNullOrWhiteSpace(profesor.Sexo) || string.IsNullOrWhiteSpace(profesor.Cedula) || string.IsNullOrWhiteSpace(profesor.CreatedBy) || string.IsNullOrWhiteSpace(profesor.Status))
        {
            return BadRequest("Faltan campos obligatorios");
        }
        if (profesor.Name == "string" || profesor.Apellido == "string" || profesor.Sexo == "string" || profesor.Cedula == "string" || profesor.CreatedBy == "string" || profesor.Status == "string")
        {
            return BadRequest("Valores de prueba no permitidos");
        }
        if (profesor.Sexo != "M" && profesor.Sexo != "F")
        {
            return BadRequest("Sexo debe ser 'M' o 'F'");
        }
        if (profesor.Status != "Activo" && profesor.Status != "Inactivo")
        {
            return BadRequest("Status debe ser 'Activo' o 'Inactivo'");
        }
        if (!ValidarCedula(profesor.Cedula))
        {
            return BadRequest("Cédula no válida");
        }
        if (await _context.Profesores.AnyAsync(p => p.Cedula == profesor.Cedula && p.Id != id))
        {
            return BadRequest("La cédula ya existe");
        }
         _context.Entry(profesor).State = EntityState.Modified;
        _context.Entry(profesor).State = EntityState.Modified;
        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Profesores.AnyAsync(e => e.Id == id)) return NotFound();
            throw;
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var profesor = await _context.Profesores.FindAsync(id);
        if (profesor == null) return NotFound();
        _context.Profesores.Remove(profesor);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
