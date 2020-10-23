using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Iso810.Entities;

namespace Iso810
{
    public partial class MainContext : DbContext
    {
        public MainContext()
        {
        }

        public MainContext(DbContextOptions<MainContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Asignaturas> Asignaturas { get; set; }
        public virtual DbSet<AsignaturasEstudiantes> AsignaturasEstudiantes { get; set; }
        public virtual DbSet<Escuelas> Escuelas { get; set; }
        public virtual DbSet<Estudiantes> Estudiantes { get; set; }
        public virtual DbSet<Grados> Grados { get; set; }
        public virtual DbSet<Provincias> Provincias { get; set; }
        public virtual DbSet<Secciones> Secciones { get; set; }
        public virtual DbSet<SeccionesGrados> SeccionesGrados { get; set; }
        public virtual DbSet<Sectores> Sectores { get; set; }
        public virtual DbSet<StudentsView> StudentsView { get; set; }
        public virtual DbSet<Tandas> Tandas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=Iso810;Trusted_Connection=False;User Id=SA;Password=P4ssw0rd");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asignaturas>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Tanda)
                    .WithMany(p => p.Asignaturas)
                    .HasForeignKey(d => d.TandaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Asignatur__Tanda__398D8EEE");
            });

            modelBuilder.Entity<AsignaturasEstudiantes>(entity =>
            {
                entity.ToTable("Asignaturas_Estudiantes");

                entity.Property(e => e.CondicionAcademica)
                    .IsRequired()
                    .HasColumnName("Condicion_academica")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Asignatura)
                    .WithMany(p => p.AsignaturasEstudiantes)
                    .HasForeignKey(d => d.AsignaturaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Asignatur__Asign__3D5E1FD2");

                entity.HasOne(d => d.Estudiante)
                    .WithMany(p => p.AsignaturasEstudiantes)
                    .HasForeignKey(d => d.EstudianteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Asignatur__Estud__3C69FB99");
            });

            modelBuilder.Entity<Escuelas>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Direccion)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Provincia)
                    .WithMany(p => p.Escuelas)
                    .HasForeignKey(d => d.ProvinciaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Escuelas__Provin__30F848ED");

                entity.HasOne(d => d.Sector)
                    .WithMany(p => p.Escuelas)
                    .HasForeignKey(d => d.SectorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Escuelas__Sector__300424B4");
            });

            modelBuilder.Entity<Estudiantes>(entity =>
            {
                entity.HasKey(e => e.Matricula)
                    .HasName("PK__Estudian__0FB9FB4E7EDDBC22");

                entity.Property(e => e.Matricula).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SeccionGradoId).HasColumnName("Seccion_GradoId");

                entity.HasOne(d => d.Escuela)
                    .WithMany(p => p.Estudiantes)
                    .HasForeignKey(d => d.EscuelaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Estudiant__Escue__33D4B598");

                entity.HasOne(d => d.SeccionGrado)
                    .WithMany(p => p.Estudiantes)
                    .HasForeignKey(d => d.SeccionGradoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Estudiant__Secci__34C8D9D1");
            });

            modelBuilder.Entity<Grados>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Provincias>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Secciones>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SeccionesGrados>(entity =>
            {
                entity.ToTable("Secciones_Grados");

                entity.HasOne(d => d.Grado)
                    .WithMany(p => p.SeccionesGrados)
                    .HasForeignKey(d => d.GradoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Secciones__Grado__2D27B809");

                entity.HasOne(d => d.Seccion)
                    .WithMany(p => p.SeccionesGrados)
                    .HasForeignKey(d => d.SeccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Secciones__Secci__2C3393D0");
            });

            modelBuilder.Entity<Sectores>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StudentsView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Students_view");

                entity.Property(e => e.Asignatura)
                    .HasColumnName("ASIGNATURA")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Calificacion).HasColumnName("CALIFICACION");

                entity.Property(e => e.CodigoDelCentro).HasColumnName("CODIGO DEL CENTRO");

                entity.Property(e => e.CondicionAcademica)
                    .HasColumnName("CONDICION ACADEMICA")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Grado)
                    .HasColumnName("GRADO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Matricula).HasColumnName("MATRICULA");

                entity.Property(e => e.NombreDelCentro)
                    .IsRequired()
                    .HasColumnName("NOMBRE DEL CENTRO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreDelEstudiante)
                    .HasColumnName("NOMBRE DEL ESTUDIANTE")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Provincia)
                    .HasColumnName("PROVINCIA")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Seccion)
                    .HasColumnName("SECCION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sector)
                    .HasColumnName("SECTOR")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tanda)
                    .HasColumnName("TANDA")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tandas>(entity =>
            {
                entity.Property(e => e.HoraFin)
                    .HasColumnName("Hora_fin")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.HoraInicio)
                    .HasColumnName("Hora_inicio")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
