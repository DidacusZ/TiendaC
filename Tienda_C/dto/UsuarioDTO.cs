using System.ComponentModel.DataAnnotations;

namespace Tienda_C.dto
{
    /// <summary>
    /// Se usa para capturar informacion del usuario de la vista
    /// </summary>
    public class UsuarioDTO
    {
        public long id_usuario { get; set; }
        public string nom_usuario { get; set; }
        public string email_usuario { get; set; }
        public string mvl_usuario { get; set; }
        public string rol_usuario { get; set; }
        public string clv_usuario { get; set; }
        public string? img_usuario { get; set; }
        public string token_recuperacion { get; set; }
        public DateTime? fch_expiracion_token { get; set; }
        public bool cuenta_confirmada { get; set; }

        //constructor
        public UsuarioDTO()
        {
        }

    }
}
