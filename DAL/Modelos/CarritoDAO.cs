using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL.Modelos
{
    /// <summary>
    /// Modelo virtual que representa la tabla Carritos
    /// </summary>
    public class CarritoDAO
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]//Key
        public long id_carrito { get; set; }

        public long id_usuario { get; set; }//FK
        public UsuarioDAO carrito_usuario_S { get; set; }//Relacion carrito-usuario

        public int cant_carrito { get; set; }


        public ICollection<ProductoDAO> carritos_productos { get; set; }//Relacion carritos-productos


    }
}
