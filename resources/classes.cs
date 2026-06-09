using System.ComponentModel.DataAnnotations.Schema;

public class Mandatory
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column(TypeName = "nvarchar(20)")]
    public required string CreatedBy { get; set; }

    [Column(TypeName = "nvarchar(5)")]
    public required string Status { get; set; }
}

public class Curso : Mandatory
{
    public required string Nombre { get; set; }
    public required string anio { get; set; }
}

public class Seccion : Mandatory
{
    public Guid IdCurso { get; set; }

    public required string Nombre { get; set; }

    public required Guid IdProfesorEncargado { get; set; }

    public required DateTime FechaInicio { get; set; }

    public required DateTime FechaFin { get; set; }
}

public class Alumno : Mandatory
{
    public required string Nombre { get; set; }

    public required string Apellido { get; set; }

    [Column(TypeName = "nvarchar(1)")]
    public required string Sexo { get; set; }

    public required DateTime FechaNacimiento { get; set; }

    public required string NumeroIdentificacion { get; set; }
}

public class Profesor : Mandatory
{
    public required string Nombre { get; set; }

    public required string Apellido { get; set; }

    [Column(TypeName = "nvarchar(1)")]
    public required string Sexo { get; set; }

    public required string Cedula { get; set; }
}

public class SeccionAlumno : Mandatory
{
    public Guid IdSeccion { get; set; }

    public Guid IdAlumno { get; set; }
}

public class AlumnoCreateRequest
{
    public required string Nombre { get; set; }
    public required string Apellido { get; set; }
    public required string Sexo { get; set; }
    public required DateTime FechaNacimiento { get; set; }
    public required string NumeroIdentificacion { get; set; }
    public required string CreatedBy { get; set; }
    public required string Status { get; set; }
}

public class CursoCreateRequest
{
    public required string Nombre { get; set; }
    public required string anio { get; set; }
    public required string CreatedBy { get; set; }
    public required string Status { get; set; }
}

public class ProfesorCreateRequest
{
    public required string Nombre { get; set; }
    public required string Apellido { get; set; }
    public required string Sexo { get; set; }
    public required string Cedula { get; set; }
    public required string CreatedBy { get; set; }
    public required string Status { get; set; }
}

public class SeccionCreateRequest
{
    public required Guid IdCurso { get; set; }
    public required string Nombre { get; set; }
    public required Guid IdProfesorEncargado { get; set; }
    public required DateTime FechaInicio { get; set; }
    public required DateTime FechaFin { get; set; }
    public required string CreatedBy { get; set; }
    public required string Status { get; set; }
}

public class SeccionAlumnoCreateRequest
{
    public required Guid IdSeccion { get; set; }
    public required Guid IdAlumno { get; set; }
    public required string CreatedBy { get; set; }
    public required string Status { get; set; }
}

public class Asignatura : Mandatory
{
    public required string Nombre { get; set; }
}

public class AsignaturaCreateRequest
{
    public required string Nombre { get; set; }
    public required string CreatedBy { get; set; }
    public required string Status { get; set; }
}

public class AsignaturaSeccion : Mandatory
{
    public Guid IdAsignatura { get; set; }
    public Guid IdSeccion { get; set; }
    public Guid IdProfesor { get; set; }
}

public class AsignaturaSeccionCreateRequest
{
    public required Guid IdAsignatura { get; set; }
    public required Guid IdSeccion { get; set; }
    public required Guid IdProfesor { get; set; }
    public required string CreatedBy { get; set; }
    public required string Status { get; set; }
}

public class LoginRequest
{
    public required string Username { get; set; } = "";
    public required string Password { get; set; } = "";
}

public class TokenValidationRequest
{
    public string? Token { get; set; }
}

public class Usuario
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string Nombre { get; set; }
    public required bool Status { get; set; }
}
