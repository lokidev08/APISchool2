using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APISchool.Data;
using APISchool;

namespace APISchool.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlumnosSeccionController : ControllerBase
{
    private readonly ColegioDbContext _context;

    public AlumnosSeccionController(ColegioDbContext context)
    {
        _context = context;
    }
    [HttpGet("por-seccion/{id}")]
    public async Task<ActionResult<IEnumerable<object>>> GetAlumnosPorSeccion(Guid id)
    {
        var seccionExiste = await _context.Secciones.AnyAsync(s => s.Id == id);
        if (!seccionExiste) return NotFound();

        var alumnos = await _context.SeccionesAlumnos
            .Where(sa => sa.IdSeccion == id)
            .Join(_context.Alumnos,
                sa => sa.IdAlumno,
                alumno => alumno.Id,
                (sa, alumno) => new
                {
                    alumno.Id,
                    alumno.Nombre,
                    alumno.Sexo,
                    Edad = DateTime.Today.Year - alumno.FechaNacimiento.Year
                })
            .ToListAsync();

        return Ok(alumnos);
    }

}