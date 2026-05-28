using Microsoft.EntityFrameworkCore;

namespace APISchool.Data;

public class ColegioDbContext : DbContext
{
    public ColegioDbContext(DbContextOptions<ColegioDbContext> options) : base(options)
    {
    }

    public DbSet<Curso> Cursos { get; set; } = null!;
    public DbSet<Seccion> Secciones { get; set; } = null!;
    public DbSet<Alumno> Alumnos { get; set; } = null!;
    public DbSet<Profesor> Profesores { get; set; } = null!;
    public DbSet<SeccionAlumno> SeccionesAlumnos { get; set; } = null!;
    public DbSet<Asignatura> Asignaturas { get; set; } = null!;
    public DbSet<AsignaturaSeccion> AsignaturasSecciones { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Curso>().ToTable("Cursos");
        modelBuilder.Entity<Seccion>().ToTable("Secciones");
        modelBuilder.Entity<Alumno>().ToTable("Alumnos");
        modelBuilder.Entity<Profesor>().ToTable("Profesores");
        modelBuilder.Entity<SeccionAlumno>().ToTable("SeccionesAlumnos");
        modelBuilder.Entity<Asignatura>().ToTable("Asignaturas");
        modelBuilder.Entity<AsignaturaSeccion>().ToTable("AsignaturasSecciones");

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
}
