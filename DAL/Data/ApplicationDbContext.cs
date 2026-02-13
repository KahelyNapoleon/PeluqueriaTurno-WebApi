using System;
using System.Collections.Generic;
using DAL.Identity;
using DomainLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public partial class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<EstadoTurno> EstadoTurnos { get; set; }

    public virtual DbSet<HistorialTurno> HistorialTurnos { get; set; }

    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<TipoServicio> TipoServicios { get; set; }

    public virtual DbSet<Turno> Turnos { get; set; }

    public virtual DbSet<TurnoServicio> TurnoServicios { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{ }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Identity Aggregate
        base.OnModelCreating(modelBuilder);

        //IdentityUser -> AppUser
        modelBuilder.Entity<AppUser>()
            .HasOne(u => u.Cliente)
            .WithOne()
            .HasForeignKey<AppUser>(u => u.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Clientes__71ABD08713A28857");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NroCelular)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Observaciones)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Preferencias)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EstadoTurno>(entity =>
        {
            entity.HasKey(e => e.EstadoTurnoId).HasName("PK__EstadoTu__10C65F06EAEE620F");

            entity.ToTable("EstadoTurno");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HistorialTurno>(entity =>
        {
            entity.HasKey(e => e.HistorialTurnoId).HasName("PK__Historia__1CF9757939A17C0F");

            entity.ToTable("HistorialTurno");

            entity.HasOne(d => d.EstadoTurnoActualNavigation).WithMany(p => p.HistorialTurnoEstadoTurnoActualNavigations)
                .HasForeignKey(d => d.EstadoTurnoActual)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Historial__Estad__76619304");

            entity.HasOne(d => d.EstadoTurnoAnteriorNavigation).WithMany(p => p.HistorialTurnoEstadoTurnoAnteriorNavigations)
                .HasForeignKey(d => d.EstadoTurnoAnterior)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Historial__Estad__756D6ECB");

            entity.HasOne(d => d.Turno).WithMany(p => p.HistorialTurnos)
                .HasForeignKey(d => d.TurnoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Historial_Turno");
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.MetodoPagoId).HasName("PK__MetodoPa__A8FEAF5471A25E67");

            entity.ToTable("MetodoPago");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.PagoId).HasName("PK__Pago__F00B61383B8FEBFF");

            entity.ToTable("Pago");

            entity.Property(e => e.MontoTotal).HasColumnType("decimal(20, 2)");

            entity.HasOne(d => d.MetodoPago).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.MetodoPagoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pago__MetodoPago__70A8B9AE");

            entity.HasOne(d => d.Turno).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.TurnoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pago__TurnoId__6FB49575");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.ServicioId).HasName("PK__Servicio__D5AEECC205ED5477");

            entity.ToTable("Servicio");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Observacion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(20, 2)");

            entity.HasOne(d => d.TipoServicio).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.TipoServicioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_TipoServicio");
        });

        modelBuilder.Entity<TipoServicio>(entity =>
        {
            entity.HasKey(e => e.TipoServicioId).HasName("PK__TipoServ__BC9FF47D782EAA7F");

            entity.ToTable("TipoServicio");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Turno>(entity =>
        {
            entity.HasKey(e => e.TurnoId).HasName("PK__Turno__AD3E2E9443AF8BC7");

            entity.ToTable("Turno");

            entity.Property(e => e.Detalle)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Cliente).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Turno_Cliente");

            entity.HasOne(d => d.EstadoTurno).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.EstadoTurnoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Turno_EstadoTurno");
        });

        modelBuilder.Entity<TurnoServicio>(entity =>
        {
            entity.HasKey(e => e.TurnoServicioId).HasName("PK__TurnoSer__F990823038421B8C");

            entity.ToTable("TurnoServicio");

            entity.Property(e => e.MontoAplicado).HasColumnType("decimal(20, 2)");

            entity.HasOne(d => d.Servicio).WithMany(p => p.TurnoServicios)
                .HasForeignKey(d => d.ServicioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TurnoServ__Servi__6CD828CA");

            entity.HasOne(d => d.Turno).WithMany(p => p.TurnoServicios)
                .HasForeignKey(d => d.TurnoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TurnoServ__Turno__6BE40491");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
