using DAL.Modelos;
using Tienda_C.dto;

namespace Tienda_C.servicios
{
    /// <summary>
    /// Metodos para convertir Usuarios de DTO a DAO
    /// </summary>
    public interface UsuarioDTOaDAOInterfaz
    {
        /// <summary>
        /// Convierte un Usuario de DTO a DAO
        /// </summary>
        /// <param name="usuarioDto"></param>
        /// <returns></returns>
        UsuarioDAO UsuarioDTOaDAO(UsuarioDTO usuarioDto);

        /// <summary>
        /// Convierte una lista de Usuarios de DTO a DAO
        /// </summary>
        /// <param name="usuarioDto"></param>
        /// <returns></returns>
        List<UsuarioDAO> ListaUsuarioDTOaDAO(List<UsuarioDTO> listaUsuarioDto);
        
    }
}
