using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL.Modelos
{
    /// <summary>
    /// Modelo virtual que representa la tabla Usarios
    /// </summary>
    public class UsuarioDAO
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]//Key
        public long id_usuario { get; set; }

        [StringLength(50)]
        public string nom_usuario { get; set; }
        public string email_usuario { get; set; }
        public string mvl_usuario { get; set; }
        public string? rol_usuario { get; set; }
        public string clv_usuario { get; set; }
        public string? img_usuario { get; set; }

        public string? token_recuperacion { get; set; }
        public DateTime? fch_expiracion_token { get; set; }
        public bool cuenta_confirmada { get; set; }


        public CarritoDAO usuario_carrito_P { get; set; }//Relacion usuario-carrito
        public ICollection<CompraDAO> usuario_compras { get; set; }//Relacion usuario-compras
    }
}