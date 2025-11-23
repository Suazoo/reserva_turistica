using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using reserva_turisticas.Models;

namespace reserva_turisticas.Data;

public partial class ReservaTuristicaContext : DbContext
{
    public ReservaTuristicaContext()
    {
    }

    public ReservaTuristicaContext(DbContextOptions<ReservaTuristicaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoriaCliente> CategoriaClientes { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Contrato> Contratos { get; set; }

    public virtual DbSet<DisponibilidadTour> DisponibilidadTours { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<Guium> Guia { get; set; }

    public virtual DbSet<Habitacion> Habitacions { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<Idioma> Idiomas { get; set; }

    public virtual DbSet<Lugar> Lugars { get; set; }

    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }

    public virtual DbSet<Monedum> Moneda { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Paquete> Paquetes { get; set; }

    public virtual DbSet<PaqueteTour> PaqueteTours { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Reembolso> Reembolsos { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<ReservaServicio> ReservaServicios { get; set; }

    public virtual DbSet<ReservaTour> ReservaTours { get; set; }

    public virtual DbSet<SalidaTour> SalidaTours { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<ServicioHotel> ServicioHotels { get; set; }

    public virtual DbSet<Telefono> Telefonos { get; set; }

    public virtual DbSet<TipoHabitacion> TipoHabitacions { get; set; }

    public virtual DbSet<Tour> Tours { get; set; }

    public virtual DbSet<Transaccion> Transaccions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoriaCliente>(entity =>
        {
            entity.ToTable("CategoriaCliente");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.Prioridad)
                .HasMaxLength(45)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Cliente");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoriaClienteId).HasColumnName("CategoriaCliente_id");
            entity.Property(e => e.CodigoCliente).HasColumnName("Codigo_cliente");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_registro");
            entity.Property(e => e.PersonaId).HasColumnName("Persona_id");

            entity.HasOne(d => d.CategoriaCliente).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.CategoriaClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cliente_Categoria");

            entity.HasOne(d => d.Persona).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.PersonaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cliente_Persona");
        });

        modelBuilder.Entity<Contrato>(entity =>
        {
            entity.ToTable("Contrato");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro).HasColumnName("Fecha_Registro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.UltimaVersion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("Ultima_Version");
        });

        modelBuilder.Entity<DisponibilidadTour>(entity =>
        {
            entity.ToTable("Disponibilidad_Tour");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CupoDisponible).HasColumnName("Cupo_disponible");
            entity.Property(e => e.CupoTotal).HasColumnName("Cupo_Total");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Inicio");
            entity.Property(e => e.TourId).HasColumnName("Tour_id");

            entity.HasOne(d => d.Tour).WithMany(p => p.DisponibilidadTours)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Disponibilidad_Tour");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.ToTable("Empleado");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cargo)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.FechaIngreso).HasColumnName("Fecha_Ingreso");
            entity.Property(e => e.NumEmpleado).HasColumnName("Num_empleado");
            entity.Property(e => e.PersonaId).HasColumnName("Persona_id");

            entity.HasOne(d => d.Persona).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.PersonaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Empleado_Persona");

            entity.HasMany(d => d.Hotels).WithMany(p => p.Empleados)
                .UsingEntity<Dictionary<string, object>>(
                    "EmpleadoHotel",
                    r => r.HasOne<Hotel>().WithMany()
                        .HasForeignKey("HotelId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Hotel_E"),
                    l => l.HasOne<Empleado>().WithMany()
                        .HasForeignKey("EmpleadoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Empleado_H"),
                    j =>
                    {
                        j.HasKey("EmpleadoId", "HotelId");
                        j.ToTable("Empleado_Hotel");
                        j.IndexerProperty<int>("EmpleadoId").HasColumnName("Empleado_id");
                        j.IndexerProperty<int>("HotelId").HasColumnName("Hotel_id");
                    });
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.ToTable("Factura");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.FechaEmision).HasColumnName("Fecha_Emision");
            entity.Property(e => e.MonedaId).HasColumnName("Moneda_id");
            entity.Property(e => e.ReservaId).HasColumnName("Reserva_id");

            entity.HasOne(d => d.Moneda).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.MonedaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Factura_Moneda");

            entity.HasOne(d => d.Reserva).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.ReservaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Factura_Reserva");
        });

        modelBuilder.Entity<Guium>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CalificacionProm).HasColumnName("Calificacion_prom");
            entity.Property(e => e.EmpleadoId).HasColumnName("Empleado_id");
            entity.Property(e => e.Lincencia)
                .HasMaxLength(45)
                .IsUnicode(false);

            entity.HasOne(d => d.Empleado).WithMany(p => p.Guia)
                .HasForeignKey(d => d.EmpleadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Guia_Empleado");

            entity.HasMany(d => d.Paquetes).WithMany(p => p.Guia)
                .UsingEntity<Dictionary<string, object>>(
                    "GuiaPaquete",
                    r => r.HasOne<Paquete>().WithMany()
                        .HasForeignKey("PaqueteId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_GP_Paquete"),
                    l => l.HasOne<Guium>().WithMany()
                        .HasForeignKey("GuiaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_GP_Guia"),
                    j =>
                    {
                        j.HasKey("GuiaId", "PaqueteId");
                        j.ToTable("Guia_Paquete");
                        j.IndexerProperty<int>("GuiaId").HasColumnName("Guia_id");
                        j.IndexerProperty<int>("PaqueteId").HasColumnName("Paquete_id");
                    });
        });

        modelBuilder.Entity<Habitacion>(entity =>
        {
            entity.ToTable("Habitacion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CodigoInterno).HasColumnName("Codigo_Interno");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.HotelId).HasColumnName("Hotel_id");
            entity.Property(e => e.TipoHabitacionId).HasColumnName("Tipo_Habitacion_id");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Habitacions)
                .HasForeignKey(d => d.HotelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Habitacion_H");

            entity.HasOne(d => d.TipoHabitacion).WithMany(p => p.Habitacions)
                .HasForeignKey(d => d.TipoHabitacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Habitacion_T");
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.ToTable("Hotel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoriaEstrellas).HasColumnName("Categoria_estrellas");
            entity.Property(e => e.ChekinDesde)
                .HasColumnType("datetime")
                .HasColumnName("Chekin_Desde");
            entity.Property(e => e.ChekinHasta)
                .HasColumnType("datetime")
                .HasColumnName("Chekin_Hasta");
            entity.Property(e => e.Correo)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.IdCiudad).HasColumnName("idCiudad");
            entity.Property(e => e.IdDepartamento).HasColumnName("idDepartamento");
            entity.Property(e => e.IdPais).HasColumnName("idPais");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.Politicas).IsUnicode(false);
            entity.Property(e => e.ReferenciaDireccion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("referencia_direccion");
            entity.Property(e => e.Telefono)
                .HasMaxLength(45)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCiudadNavigation).WithMany(p => p.HotelIdCiudadNavigations)
                .HasForeignKey(d => d.IdCiudad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Hotel_Ciu");

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.HotelIdDepartamentoNavigations)
                .HasForeignKey(d => d.IdDepartamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Hotel_Dep");

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.HotelIdPaisNavigations)
                .HasForeignKey(d => d.IdPais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Hotel_Pais");
        });

        modelBuilder.Entity<Idioma>(entity =>
        {
            entity.ToTable("Idioma");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CodigoIso)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Codigo_iso");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false);

            entity.HasMany(d => d.Personas).WithMany(p => p.Idiomas)
                .UsingEntity<Dictionary<string, object>>(
                    "IdiomaPersona",
                    r => r.HasOne<Persona>().WithMany()
                        .HasForeignKey("PersonaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Idioma_Persona_Persona"),
                    l => l.HasOne<Idioma>().WithMany()
                        .HasForeignKey("IdiomaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Idioma_Persona_Idioma"),
                    j =>
                    {
                        j.HasKey("IdiomaId", "PersonaId");
                        j.ToTable("Idioma_Persona");
                        j.IndexerProperty<int>("IdiomaId").HasColumnName("Idioma_id");
                        j.IndexerProperty<int>("PersonaId").HasColumnName("Persona_id");
                    });
        });

        modelBuilder.Entity<Lugar>(entity =>
        {
            entity.HasKey(e => e.IdLugar);

            entity.ToTable("Lugar");

            entity.Property(e => e.IdLugar).HasColumnName("idLugar");
            entity.Property(e => e.LugarIdLugar).HasColumnName("Lugar_idLugar");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.LugarIdLugarNavigation).WithMany(p => p.InverseLugarIdLugarNavigation)
                .HasForeignKey(d => d.LugarIdLugar)
                .HasConstraintName("FK_Lugar_Lugar");
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.ToTable("Metodo_pago");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Monedum>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CodigoIso)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("Codigo_iso");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.Simbolo)
                .HasMaxLength(45)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.ToTable("Pago");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.FacturaId).HasColumnName("Factura_id");
            entity.Property(e => e.MetodoPagoId).HasColumnName("Metodo_pago_id");
            entity.Property(e => e.MonedaId).HasColumnName("Moneda_id");
            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Observacion).IsUnicode(false);

            entity.HasOne(d => d.Factura).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.FacturaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_Factura");

            entity.HasOne(d => d.MetodoPago).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.MetodoPagoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_Metodo");

            entity.HasOne(d => d.Moneda).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.MonedaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_Moneda");
        });

        modelBuilder.Entity<Paquete>(entity =>
        {
            entity.ToTable("Paquete");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.DuracionDias).HasColumnName("Duracion_dias");
            entity.Property(e => e.FechaFin)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Fin");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Inicio");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.PrecioPorPersona)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Precio_por_Persona");
            entity.Property(e => e.PrecioTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Precio_Total");

            entity.HasMany(d => d.Reservas).WithMany(p => p.Paquetes)
                .UsingEntity<Dictionary<string, object>>(
                    "PaqueteReserva",
                    r => r.HasOne<Reserva>().WithMany()
                        .HasForeignKey("ReservaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PR_Reserva"),
                    l => l.HasOne<Paquete>().WithMany()
                        .HasForeignKey("PaqueteId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PR_Paquete"),
                    j =>
                    {
                        j.HasKey("PaqueteId", "ReservaId");
                        j.ToTable("Paquete_Reserva");
                        j.IndexerProperty<int>("PaqueteId").HasColumnName("Paquete_id");
                        j.IndexerProperty<int>("ReservaId").HasColumnName("Reserva_id");
                    });
        });

        modelBuilder.Entity<PaqueteTour>(entity =>
        {
            entity.HasKey(e => new { e.PaqueteId, e.TourId });

            entity.ToTable("Paquete_Tour");

            entity.Property(e => e.PaqueteId).HasColumnName("Paquete_id");
            entity.Property(e => e.TourId).HasColumnName("Tour_id");
            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.DuracionEstimada).HasColumnName("Duracion_Estimada");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false);

            entity.HasOne(d => d.Paquete).WithMany(p => p.PaqueteTours)
                .HasForeignKey(d => d.PaqueteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PT_Paquete");

            entity.HasOne(d => d.Tour).WithMany(p => p.PaqueteTours)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PT_Tour");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.ToTable("Persona");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("Correo_electronico");
            entity.Property(e => e.Direccion)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.Dni).HasColumnName("DNI");
            entity.Property(e => e.PrimerApellido)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("Primer_apellido");
            entity.Property(e => e.PrimerNombre)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("Primer_nombre");
            entity.Property(e => e.SegundoApellido)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("Segundo_apellido");
            entity.Property(e => e.SegundoNombre)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("Segundo_nombre");
        });

        modelBuilder.Entity<Reembolso>(entity =>
        {
            entity.ToTable("Reembolso");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Motivo).IsUnicode(false);
            entity.Property(e => e.PagoId).HasColumnName("Pago_id");

            entity.HasOne(d => d.Pago).WithMany(p => p.Reembolsos)
                .HasForeignKey(d => d.PagoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reembolso_Pago");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.ToTable("Reserva");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CodigoReserva).HasColumnName("Codigo_reserva");
            entity.Property(e => e.ContratoId).HasColumnName("Contrato_id");
            entity.Property(e => e.ClienteId).HasColumnName("Cliente_id");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Creacion");
            entity.Property(e => e.MonedaId).HasColumnName("Moneda_id");
            entity.Property(e => e.Politicas).IsUnicode(false);
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Contrato).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.ContratoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reserva_Contrato");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Reservas) // Cliente no define colección Reservas en el modelo actual
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Reserva_Cliente");

            entity.HasOne(d => d.Moneda).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.MonedaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reserva_Moneda");
        });

        modelBuilder.Entity<ReservaServicio>(entity =>
        {
            entity.HasKey(e => new { e.ServicioId, e.ReservaId });

            entity.ToTable("Reserva_Servicio");

            entity.Property(e => e.ServicioId).HasColumnName("Servicio_id");
            entity.Property(e => e.ReservaId).HasColumnName("Reserva_id");
            entity.Property(e => e.FechaFin).HasColumnName("Fecha_FIN");
            entity.Property(e => e.FechaInicio).HasColumnName("Fecha_Inicio");
            entity.Property(e => e.PrecioUnidad).HasColumnName("Precio_unidad");

            entity.HasOne(d => d.Reserva).WithMany(p => p.ReservaServicios)
                .HasForeignKey(d => d.ReservaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RS_Reserva");

            entity.HasOne(d => d.Servicio).WithMany(p => p.ReservaServicios)
                .HasForeignKey(d => d.ServicioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RS_Servicio");
        });

        modelBuilder.Entity<ReservaTour>(entity =>
        {
            entity.HasKey(e => new { e.ReservaId, e.TourId });

            entity.ToTable("Reserva_Tour");

            entity.Property(e => e.ReservaId).HasColumnName("Reserva_id");
            entity.Property(e => e.TourId).HasColumnName("Tour_id");

            entity.HasOne(d => d.Reserva).WithMany(p => p.ReservaTours)
                .HasForeignKey(d => d.ReservaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RT_Reserva");

            entity.HasOne(d => d.Tour).WithMany(p => p.ReservaTours)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RT_Tour");
        });

        modelBuilder.Entity<SalidaTour>(entity =>
        {
            entity.ToTable("Salida_Tour");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CupoDisponible).HasColumnName("Cupo_disponible");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_inicio");
            entity.Property(e => e.TourId).HasColumnName("Tour_id");

            entity.HasOne(d => d.Tour).WithMany(p => p.SalidaTours)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Salida_Tour_Tour");

            entity.HasMany(d => d.Guia).WithMany(p => p.SalidaTours)
                .UsingEntity<Dictionary<string, object>>(
                    "TourSalidaGuium",
                    r => r.HasOne<Guium>().WithMany()
                        .HasForeignKey("GuiaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TSG_Guia"),
                    l => l.HasOne<SalidaTour>().WithMany()
                        .HasForeignKey("SalidaTourId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TSG_Salida"),
                    j =>
                    {
                        j.HasKey("SalidaTourId", "GuiaId");
                        j.ToTable("Tour_Salida_Guia");
                        j.IndexerProperty<int>("SalidaTourId").HasColumnName("Salida_Tour_id");
                        j.IndexerProperty<int>("GuiaId").HasColumnName("Guia_id");
                    });
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.ToTable("Servicio");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AplicaA)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("Aplica_a");
            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.Politicas).IsUnicode(false);
        });

        modelBuilder.Entity<ServicioHotel>(entity =>
        {
            entity.HasKey(e => new { e.ServicioId, e.HotelId });

            entity.ToTable("Servicio_Hotel");

            entity.Property(e => e.ServicioId).HasColumnName("Servicio_id");
            entity.Property(e => e.HotelId).HasColumnName("Hotel_id");
            entity.Property(e => e.CostoAdicional).HasColumnName("Costo_Adicional");
            entity.Property(e => e.Descripcion).IsUnicode(false);

            entity.HasOne(d => d.Hotel).WithMany(p => p.ServicioHotels)
                .HasForeignKey(d => d.HotelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Hotel_S");

            entity.HasOne(d => d.Servicio).WithMany(p => p.ServicioHotels)
                .HasForeignKey(d => d.ServicioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_H");
        });

        modelBuilder.Entity<Telefono>(entity =>
        {
            entity.ToTable("Telefono");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.PersonaId).HasColumnName("Persona_id");
            entity.Property(e => e.TipoTelefono)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("Tipo_Telefono");

            entity.HasOne(d => d.Persona).WithMany(p => p.Telefonos)
                .HasForeignKey(d => d.PersonaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Telefono_Persona");
        });

        modelBuilder.Entity<TipoHabitacion>(entity =>
        {
            entity.ToTable("Tipo_Habitacion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.ToTable("Tour");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.DuracionHora).HasColumnName("Duracion_Hora");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Transaccion>(entity =>
        {
            entity.ToTable("Transaccion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.FechaTransaccion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Transaccion");
            entity.Property(e => e.MonedaId).HasColumnName("Moneda_id");
            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PagoId).HasColumnName("Pago_id");
            entity.Property(e => e.Tipo)
                .HasMaxLength(1)
                .IsUnicode(false);

            entity.HasOne(d => d.Moneda).WithMany(p => p.Transaccions)
                .HasForeignKey(d => d.MonedaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaccion_Moneda");

            entity.HasOne(d => d.Pago).WithMany(p => p.Transaccions)
                .HasForeignKey(d => d.PagoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaccion_Pago");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.PersonaId).HasColumnName("Persona_id");

            entity.HasOne(d => d.Persona).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.PersonaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Persona");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
