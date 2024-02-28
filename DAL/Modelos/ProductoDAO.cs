using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace DAL.Modelos
{
    /// <summary>
    /// Modelo virtual que representa la tabla Productos
    /// </summary>
    public class ProductoDAO
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]//Key
        public long id_producto { get; set; }
        public string nom_producto { get; set; }
        public string desc_producto { get; set; }
        public double precio_producto { get; set; }
        public int cant_producto { get; set; }
        public byte[]? img_producto { get; set; }

        public ICollection<CompraDAO> productos_compras { get; set; }//Relacion productos-compras

        public ICollection<CarritoDAO> productos_carritos { get; set; }//Relacion productos-carritos

    }
}