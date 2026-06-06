using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace APISchool.Data;

public class ColegioDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ColegioDbContext(
        DbContextOptions<ColegioDbContext> options,
        IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Curso> Cursos { get; set; } = null!;
    public DbSet<Seccion> Secciones { get; set; } = null!;
    public DbSet<Alumno> Alumnos { get; set; } = null!;
    public DbSet<Profesor> Profesores { get; set; } = null!;
    public DbSet<SeccionAlumno> SeccionesAlumnos { get; set; } = null!;
    public DbSet<Asignatura> Asignaturas { get; set; } = null!;
    public DbSet<AsignaturaSeccion> AsignaturasSecciones { get; set; } = null!;
    public DbSet<Usuario> Usuarios { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Curso>().ToTable("Cursos", table =>
        {
            table.HasTrigger("TR_Cursos_Insert");
            table.HasTrigger("TR_Cursos_Update");
            table.HasTrigger("TR_Cursos_Delete");
        });
        modelBuilder.Entity<Seccion>().ToTable("Secciones", table =>
        {
            table.HasTrigger("TR_Secciones_Insert");
            table.HasTrigger("TR_Secciones_Update");
            table.HasTrigger("TR_Secciones_Delete");
        });
        modelBuilder.Entity<Alumno>().ToTable("Alumnos", table =>
        {
            table.HasTrigger("TR_Alumnos_Insert");
            table.HasTrigger("TR_Alumnos_Update");
            table.HasTrigger("TR_Alumnos_Delete");
        });
        modelBuilder.Entity<Profesor>().ToTable("Profesores", table =>
        {
            table.HasTrigger("TR_Profesores_Insert");
            table.HasTrigger("TR_Profesores_Update");
            table.HasTrigger("TR_Profesores_Delete");
        });
        modelBuilder.Entity<SeccionAlumno>().ToTable("SeccionesAlumnos", table =>
        {
            table.HasTrigger("TR_SeccionesAlumnos_Insert");
            table.HasTrigger("TR_SeccionesAlumnos_Update");
            table.HasTrigger("TR_SeccionesAlumnos_Delete");
        });
        modelBuilder.Entity<Asignatura>().ToTable("Asignaturas", table =>
        {
            table.HasTrigger("TR_Asignaturas_Insert");
            table.HasTrigger("TR_Asignaturas_Update");
            table.HasTrigger("TR_Asignaturas_Delete");
        });
        modelBuilder.Entity<AsignaturaSeccion>().ToTable("AsignaturasSecciones", table =>
        {
            table.HasTrigger("TR_AsignaturasSecciones_Insert");
            table.HasTrigger("TR_AsignaturasSecciones_Update");
            table.HasTrigger("TR_AsignaturasSecciones_Delete");
        });

        modelBuilder.Entity<Usuario>().ToTable("Usuarios");

        modelBuilder.Entity<Curso>().HasKey(e => e.Id);
        modelBuilder.Entity<Seccion>().HasKey(e => e.Id);
        modelBuilder.Entity<Alumno>().HasKey(e => e.Id);
        modelBuilder.Entity<Profesor>().HasKey(e => e.Id);
        modelBuilder.Entity<SeccionAlumno>().HasKey(e => e.Id);
        modelBuilder.Entity<Asignatura>().HasKey(e => e.Id);
        modelBuilder.Entity<AsignaturaSeccion>().HasKey(e => e.Id);

        modelBuilder.Entity<Seccion>()
            .HasOne<Curso>()
            .WithMany()
            .HasForeignKey(s => s.IdCurso)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SeccionAlumno>()
            .HasOne<Seccion>()
            .WithMany()
            .HasForeignKey(sa => sa.IdSeccion)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SeccionAlumno>()
            .HasOne<Alumno>()
            .WithMany()
            .HasForeignKey(sa => sa.IdAlumno)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

        await Database.ExecuteSqlRawAsync(
            "EXEC sp_set_session_context @key=N'Usuario', @value={0}",
            username ?? "Sistema",
            cancellationToken);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
