using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Turismo_Bova.Models;

namespace Turismo_Bova.Data
{
    public class AppDBContext : DbContext
    {
        private readonly PasswordHasher<Usuario> _passwordHasher;
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Asignacion_Ruta_Vehiculo_Horario> asignacion_Ruta_Vehiculo_Horarios { get; set; }
        public DbSet<Cliente> clientes { get; set; }
        public DbSet<Contrato> contratos { get; set; }
        public DbSet<Cotizacion> cotizaciones { get; set; }
        public DbSet<Empleado> empleados { get; set; }
        public DbSet<Factura> facturas { get; set; }
        public DbSet<Horario> horarios { get; set; }
        public DbSet<Mantenimiento> mantenimientos { get; set; }
        public DbSet<Pago> pagos { get; set; }
        public DbSet<Pedido> pedidos { get; set; }
        public DbSet<Pedido_Producto> pedido_Productos { get; set; }
        public DbSet<Producto> productos { get; set; }
        public DbSet<Proveedor> proveedores { get; set; }
        public DbSet<Reporte> reportes { get; set; }
        public DbSet<Rol> roles { get; set; }
        public DbSet<Ruta> rutas { get; set; }
        public DbSet<Servicio> servicios { get; set; }
        public DbSet<Tarea> tareas { get; set; }
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<Vehiculo> vehiculos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Administrador" }
            );

            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Nombre = "Admin",
                    Correo = "admin@turismobova.com",
                    Contraseña = "123",
                    RolId = 1
                }
                
            );
            
            modelBuilder.Entity<Asignacion_Ruta_Vehiculo_Horario>(tb =>
            {
                tb.HasOne(tb => tb.ruta)
                .WithMany(tb => tb.asignacion_Ruta_Vehiculo_Horario)
                .HasForeignKey(tb => tb.RutaId)
                .OnDelete(DeleteBehavior.Restrict);

                tb.HasOne(tb => tb.vehiculo)
                .WithMany(tb => tb.asignacion_Ruta_Vehiculo_Horario)
                .HasForeignKey(tb => tb.VehiculoId)
                .OnDelete(DeleteBehavior.Restrict);

                tb.HasOne(tb => tb.horario)
                .WithMany(tb => tb.asignacion_Ruta_Vehiculo_Horario)
                .HasForeignKey(tb => tb.HorarioId)
                .OnDelete(DeleteBehavior.Restrict);

                tb.HasOne(tb => tb.empleado)
                .WithMany(tb => tb.asignacion_Ruta_Vehiculo_Horario)
                .HasForeignKey(tb => tb.EmpleadoId)
                .OnDelete(DeleteBehavior.Restrict);

            });
            

            modelBuilder.Entity<Contrato>(tb =>
            {
                tb.HasOne(tb => tb.cliente)
                .WithMany(tb => tb.contrato)
                .HasForeignKey(tb => tb.ClienteId);

                tb.HasOne(tb => tb.servicio)
                .WithMany(tb => tb.contrato)
                .HasForeignKey(tb => tb.ServicioId);
            });

            modelBuilder.Entity<Cotizacion>(tb =>
            {
                tb.HasOne(tb => tb.cliente)
                .WithMany(tb => tb.cotizacion)
                .HasForeignKey(tb => tb.ClienteId);
            });

            modelBuilder.Entity<Factura>(tb =>
            {
                tb.HasOne(tb => tb.contrato)
                .WithOne(tb => tb.factura)
                .HasForeignKey<Factura>(tb => tb.ContratoId);
            });

            modelBuilder.Entity<Horario>(tb =>
            {
                tb.HasOne(tb => tb.ruta)
                .WithMany(tb => tb.horario)
                .HasForeignKey(tb => tb.RutaId);
            });

            modelBuilder.Entity<Mantenimiento>(tb =>
            {
                tb.HasOne(tb => tb.vehiculo)
                .WithMany(tb => tb.mantenimiento)
                .HasForeignKey(tb => tb.VehiculoId);

                tb.HasOne(tb => tb.empleado)
                .WithMany(tb => tb.mantenimiento)
                .HasForeignKey(tb => tb.EmpleadoId);

                tb.HasOne(tb => tb.proveedor)
                .WithMany(tb => tb.mantenimiento)
                .HasForeignKey(tb => tb.ProveedorId);
            });

            modelBuilder.Entity<Pago>(tb =>
            {
                tb.HasOne(tb => tb.contrato)
                .WithMany(tb => tb.pago)
                .HasForeignKey(tb => tb.ContratoId);
            });

            modelBuilder.Entity<Pedido>(tb =>
            {
                tb.HasOne(tb => tb.proveedor)
                .WithMany(tb => tb.pedido)
                .HasForeignKey(tb => tb.ProveedorId);
            });

            modelBuilder.Entity<Pedido_Producto>(tb =>
            {
                tb.HasOne(tb => tb.pedido)
                .WithMany(tb => tb.pedido_producto)
                .HasForeignKey(tb => tb.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

                tb.HasOne(tb => tb.producto)
                .WithMany(tb => tb.pedido_producto)
                .HasForeignKey(tb => tb.ProductoId)
                .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Producto>(tb =>
            {
                tb.HasOne(tb => tb.proveedor)
                .WithMany(tb => tb.producto)
                .HasForeignKey(tb => tb.ProveedorId);
            });

            modelBuilder.Entity<Reporte>(tb =>
            {
                tb.HasOne(tb => tb.empleado)
                .WithMany(tb => tb.reporte)
                .HasForeignKey(tb => tb.EmpleadoId);
            });

            modelBuilder.Entity<Ruta>(tb =>
            {
                tb.HasOne(tb => tb.servicio)
                .WithMany(tb => tb.ruta)
                .HasForeignKey(tb => tb.ServicioId);
            });

            modelBuilder.Entity<Tarea>(tb =>
            {
                tb.HasOne(tb => tb.empleado)
                .WithMany(tb => tb.tarea)
                .HasForeignKey(tb => tb.EmpleadoId)
                .OnDelete(DeleteBehavior.Restrict);

                tb.HasOne(tb => tb.mantenimiento)
                .WithMany(tb => tb.tarea)
                .HasForeignKey(tb => tb.MantenimientoId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Usuario>(tb =>
            {
                tb.HasOne(tb => tb.rol)
                .WithMany(tb => tb.usuario)
                .HasForeignKey(tb => tb.RolId);

                tb.HasOne(tb => tb.Empleado)
                .WithOne(tb => tb.usuario)
                .HasForeignKey<Usuario>(tb => tb.EmpleadoId);

                tb.HasOne(tb => tb.Cliente)
                .WithOne(tb => tb.usuario)
                .HasForeignKey<Usuario>(tb => tb.ClienteId);
            });

            


            modelBuilder.Entity<Asignacion_Ruta_Vehiculo_Horario>().ToTable("Asignacion_Ruta_Vehiculo_Horario");
            modelBuilder.Entity<Cliente>().ToTable("Cliente");
            modelBuilder.Entity<Contrato>().ToTable("Contrato");
            modelBuilder.Entity<Cotizacion>().ToTable("Cotizacion");
            modelBuilder.Entity<Empleado>().ToTable("Empleado");
            modelBuilder.Entity<Factura>().ToTable("Factura");
            modelBuilder.Entity<Horario>().ToTable("Horario");
            modelBuilder.Entity<Mantenimiento>().ToTable("Mantenimiento");
            modelBuilder.Entity<Pago>().ToTable("Pago");
            modelBuilder.Entity<Pedido>().ToTable("Pedido");
            modelBuilder.Entity<Pedido_Producto>().ToTable("Pedido_Producto");
            modelBuilder.Entity<Producto>().ToTable("Producto");
            modelBuilder.Entity<Proveedor>().ToTable("Proveedor");
            modelBuilder.Entity<Reporte>().ToTable("Reporte");
            modelBuilder.Entity<Rol>().ToTable("Rol");
            modelBuilder.Entity<Ruta>().ToTable("Ruta");
            modelBuilder.Entity<Servicio>().ToTable("Servicio");
            modelBuilder.Entity<Tarea>().ToTable("Tarea");
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<Vehiculo>().ToTable("Vehiculo");
        }
    }
}
