
using System;
using APISchool.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace APISchool.Migrations
{
    [DbContext(typeof(ColegioDbContext))]
    [Migration("20260523130533_InitialCreate")]
    partial class InitialCreate
    {

        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "10.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Alumno", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AlumnoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("CreatedAt")
                        .HasColumnType("date");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateOnly>("FechaNacimiento")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumeroIdentificacion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sexo")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(5)");

                    b.HasKey("Id");

                    b.HasIndex("AlumnoId");

                    b.ToTable("Alumnos", (string)null);
                });

            modelBuilder.Entity("Curso", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("CreatedAt")
                        .HasColumnType("date");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<Guid?>("CursoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(5)");

                    b.HasKey("Id");

                    b.HasIndex("CursoId");

                    b.ToTable("Cursos", (string)null);
                });

            modelBuilder.Entity("Profesor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cedula")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("CreatedAt")
                        .HasColumnType("date");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ProfesorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Sexo")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(5)");

                    b.HasKey("Id");

                    b.HasIndex("ProfesorId");

                    b.ToTable("Profesores", (string)null);
                });

            modelBuilder.Entity("Seccion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("CreatedAt")
                        .HasColumnType("date");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateOnly>("FechaFin")
                        .HasColumnType("date");

                    b.Property<DateOnly>("FechaInicio")
                        .HasColumnType("date");

                    b.Property<Guid>("IdCurso")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfesorEncargado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("SeccionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(5)");

                    b.HasKey("Id");

                    b.HasIndex("IdCurso");

                    b.HasIndex("SeccionId");

                    b.ToTable("Secciones", (string)null);
                });

            modelBuilder.Entity("SeccionAlumno", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("CreatedAt")
                        .HasColumnType("date");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<Guid>("IdAlumno")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdSeccion")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SeccionAlumnoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(5)");

                    b.HasKey("Id");

                    b.HasIndex("IdAlumno");

                    b.HasIndex("IdSeccion");

                    b.HasIndex("SeccionAlumnoId");

                    b.ToTable("SeccionesAlumnos", (string)null);
                });

            modelBuilder.Entity("Alumno", b =>
                {
                    b.HasOne("Alumno", null)
                        .WithMany("Alumnos")
                        .HasForeignKey("AlumnoId");
                });

            modelBuilder.Entity("Curso", b =>
                {
                    b.HasOne("Curso", null)
                        .WithMany("Cursos")
                        .HasForeignKey("CursoId");
                });

            modelBuilder.Entity("Profesor", b =>
                {
                    b.HasOne("Profesor", null)
                        .WithMany("Profesores")
                        .HasForeignKey("ProfesorId");
                });

            modelBuilder.Entity("Seccion", b =>
                {
                    b.HasOne("Curso", null)
                        .WithMany()
                        .HasForeignKey("IdCurso")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Seccion", null)
                        .WithMany("Secciones")
                        .HasForeignKey("SeccionId");
                });

            modelBuilder.Entity("SeccionAlumno", b =>
                {
                    b.HasOne("Alumno", null)
                        .WithMany()
                        .HasForeignKey("IdAlumno")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Seccion", null)
                        .WithMany()
                        .HasForeignKey("IdSeccion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SeccionAlumno", null)
                        .WithMany("SeccionesAlumnos")
                        .HasForeignKey("SeccionAlumnoId");
                });

            modelBuilder.Entity("Alumno", b =>
                {
                    b.Navigation("Alumnos");
                });

            modelBuilder.Entity("Curso", b =>
                {
                    b.Navigation("Cursos");
                });

            modelBuilder.Entity("Profesor", b =>
                {
                    b.Navigation("Profesores");
                });

            modelBuilder.Entity("Seccion", b =>
                {
                    b.Navigation("Secciones");
                });

            modelBuilder.Entity("SeccionAlumno", b =>
                {
                    b.Navigation("SeccionesAlumnos");
                });
#pragma warning restore 612, 618
        }
    }
}
