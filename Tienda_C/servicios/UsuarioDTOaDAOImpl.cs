using DAL.Modelos;
using Tienda_C.dto;
using Tienda_C.Fichero;

namespace Tienda_C.servicios
{
    /// <summary>
    /// Logica de la conversion de Usuarios de DTO a DAO
    /// </summary>
    public class UsuarioDTOaDAOImpl : UsuarioDTOaDAOInterfaz
    {
        public UsuarioDAO UsuarioDTOaDAO(UsuarioDTO usuarioDto)
        {
            try
            {
                FicheroLog.Log("[INFO] [UsuarioDTOaDAOImpl-UsuarioDTOaDAO]");
                UsuarioDAO usuarioDao = new UsuarioDAO();

                usuarioDao.id_usuario=usuarioDto.id_usuario;
                usuarioDao.nom_usuario=usuarioDto.nom_usuario;
                usuarioDao.email_usuario=usuarioDto.email_usuario;
                usuarioDao.clv_usuario=usuarioDto.clv_usuario;
                usuarioDao.mvl_usuario=usuarioDto.mvl_usuario;
                usuarioDao.cuenta_confirmada = usuarioDto.cuenta_confirmada;

                //guardar imagen
                if (usuarioDto.img_usuario == null)
                {
                    // Lee los bytes de la imagen por defecto      byte[]
                    usuarioDao.img_usuario = "~/imagenes/fotoPerfil.jpg";
                }
                else
                {
                    usuarioDao.img_usuario = usuarioDto.img_usuario;
                }

                return usuarioDao;
            }
            catch (Exception)
            {
                FicheroLog.Log("[ERROR] [UsuarioDTOaDAOImpl-UsuarioDTOaDAO]");
            }
            return null;
        }

        public List<UsuarioDAO> ListaUsuarioDTOaDAO(List<UsuarioDTO> listaUsuarioDto)
        {
            try
            {
                FicheroLog.Log("[INFO] [UsuarioDTOaDAOImpl-ListaUsuarioDTOaDAO]");
                List<UsuarioDAO> listaUsuarioDao = new List<UsuarioDAO>();
                foreach (UsuarioDTO usuarioDto in listaUsuarioDto)
                {
                    listaUsuarioDao.Add(UsuarioDTOaDAO(usuarioDto));
                }
                return listaUsuarioDao;
            }
            catch (Exception)
            {
                FicheroLog.Log("[ERROR] [UsuarioDTOaDAOImpl-ListaUsuarioDTOaDAO]");
            }
            return null;
        }


    }
}
