using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace DAL.Modelos
{
    /// <summary>
    /// Modelo virtual que representa la tabla Compras
    /// </summary>
    public class CompraDAO
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]//Key
        public long id_compra { get; set; }
        public long id_usuario { get; set; }//FK
        public UsuarioDAO compras_usuario { get; set; }//Relacion compras-usuario
        public int cant_compra { get; set; }
        public DateTime? fch_compra { get; set; }

        public ICollection<ProductoDAO> compras_productos { get; set; }//Relacion compras-productos

    }
}