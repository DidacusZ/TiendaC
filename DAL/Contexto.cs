using DAL.Modelos;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    /// <summary>
    /// Contexto de la Base de Datos necesario establecer
    /// </summary>
    public class Contexto : DbContext
    {
        //Representacion de entidades
        public DbSet<UsuarioDAO> Usuarios { get; set; }
        public DbSet<ProductoDAO> Productos { get; set; }
        public DbSet<CompraDAO> Compras { get; set; }
        public DbSet<CarritoDAO> Carritos { get; set; }


        //Opciones de configuración relacionadoas con la base de datos, como la cadena de conexion ...
        public Contexto(DbContextOptions<Contexto> opciones) : base(opciones) { }

        //Se utiliza para inicializar el contexto de la base de datos con las opciones proporcionadas
        //Tambien para configurar las opciones de DbContext de forma manual
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        //Confiracion del modelo de datos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {//relaciones

            //Sirve para asegurarte de que se ejecuten todas las configuraciones proporcionadas por la clase base
            //antes de agregar mis propias configuraciones personalizadas
            //base.OnModelCreating(modelBuilder);

            //Esquemas y nombre de las tablas en BD
            modelBuilder.Entity<UsuarioDAO>().ToTable("usuarios", schema: "gestion_usuarios");
            modelBuilder.Entity<ProductoDAO>().ToTable("productos", schema: "gestion_logica_negocio");
            modelBuilder.Entity<CarritoDAO>().ToTable("carritos", schema: "gestion_logica_negocio");
            modelBuilder.Entity<CompraDAO>().ToTable("compras", schema: "gestion_logica_negocio");

            //Relacion carrito-usuario
            modelBuilder.Entity<UsuarioDAO>()
            .HasOne(usuP => usuP.usuario_carrito_P)
            .WithOne(carrS => carrS.carrito_usuario_S)
            .HasForeignKey<CarritoDAO>(usuP => usuP.id_usuario);

            //Relacion usuario-compras
            modelBuilder.Entity<CompraDAO>()
            .HasOne(uno => uno.compras_usuario)
            .WithMany(muchos => muchos.usuario_compras)
            .HasForeignKey(uno => uno.id_usuario);

            //Relacion productos-compras
            modelBuilder.Entity<ProductoDAO>()
            .HasMany(l => l.productos_compras)
            .WithMany(l => l.compras_productos)
            .UsingEntity(
            "compras_productos",
            l => l.HasOne(typeof(CompraDAO)).WithMany().HasForeignKey("id_compra").HasPrincipalKey(nameof(CompraDAO.id_compra)),//PK
            r => r.HasOne(typeof(ProductoDAO)).WithMany().HasForeignKey("id_producto").HasPrincipalKey(nameof(ProductoDAO.id_producto)),
            j => j.HasKey("id_producto", "id_compra"));

            //Relacion carritos-productos
            modelBuilder.Entity<CarritoDAO>()
            .HasMany(l => l.carritos_productos)
            .WithMany(l => l.productos_carritos)
            .UsingEntity(
            "productos_carritos",
            l => l.HasOne(typeof(ProductoDAO)).WithMany().HasForeignKey("id_producto").HasPrincipalKey(nameof(ProductoDAO.id_producto)),//PK
            r => r.HasOne(typeof(CarritoDAO)).WithMany().HasForeignKey("id_carrito").HasPrincipalKey(nameof(CarritoDAO.id_carrito)),
            j => j.HasKey("id_carrito", "id_producto"));

        }
    }
}