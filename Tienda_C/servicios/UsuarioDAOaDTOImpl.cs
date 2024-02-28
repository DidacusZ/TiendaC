using DAL.Modelos;
using Tienda_C.dto;
using Tienda_C.Fichero;

namespace Tienda_C.servicios
{
    /// <summary>
    /// Logica de la conversion de Usuarios de DAO a DTO
    /// </summary>
    public class UsuarioDAOaDTOImpl : UsuarioDAOaDTOInterfaz
    {
        public UsuarioDTO UsuarioDAOaDTO(UsuarioDAO usuarioDao)
        {
            try
            {
                FicheroLog.Log("[INFO] [UsuarioDAOaDTOImpl-UsuarioDAOaDTO]");
                UsuarioDTO usuarioDto = new UsuarioDTO();

                usuarioDto.id_usuario = usuarioDao.id_usuario;
                usuarioDto.nom_usuario = usuarioDao.nom_usuario;
                usuarioDto.email_usuario = usuarioDao.email_usuario;
                usuarioDto.clv_usuario = usuarioDao.clv_usuario;
                usuarioDto.mvl_usuario = usuarioDao.mvl_usuario;
                usuarioDto.img_usuario = usuarioDao.img_usuario;
                usuarioDto.rol_usuario = usuarioDao.rol_usuario;
                usuarioDto.token_recuperacion = usuarioDao.token_recuperacion;
                usuarioDto.fch_expiracion_token = usuarioDao.fch_expiracion_token;
                usuarioDto.cuenta_confirmada = usuarioDao.cuenta_confirmada;

                return usuarioDto;
            }
            catch (Exception)
            {
                FicheroLog.Log("[ERROR] [UsuarioDAOaDTOImpl-UsuarioDAOaDTO]");
            }
            return null;
        }

        public List<UsuarioDTO> ListaUsuarioDAOaDTO(List<UsuarioDAO> listaUsuarioDao)
        {
            try
            {
                FicheroLog.Log("[INFO] [UsuarioDAOaDTOImpl-ListaUsuarioDAOaDTO]");

                List<UsuarioDTO> listaUsuarioDto = new List<UsuarioDTO>();
                foreach (UsuarioDAO usuarioDao in listaUsuarioDao)
                {
                    listaUsuarioDto.Add(UsuarioDAOaDTO(usuarioDao));
                }
                return listaUsuarioDto;
            }
            catch (Exception)
            {
                FicheroLog.Log("[ERROR] [UsuarioDAOaDTOImpl-ListaUsuarioDAOaDTO]");
            }
            return null;
        }
    }
}
