using DAL.Modelos;
using Tienda_C.dto;

namespace Tienda_C.servicios
{
    /// <summary>
    /// Metodos para convertir Usuarios de DAO a DTO
    /// </summary>
    public interface UsuarioDAOaDTOInterfaz
    {
        /// <summary>
        /// Convierte un Usuario de DAO a DTO
        /// </summary>
        /// <param name="usuarioDao"></param>
        /// <returns></returns>
        UsuarioDTO UsuarioDAOaDTO(UsuarioDAO usuarioDao);

        /// <summary>
        /// Convierte una lista de Usuarios de DAO a DTO
        /// </summary>
        /// <param name="listausuarioDao"></param>
        /// <returns></returns>
        List<UsuarioDTO> ListaUsuarioDAOaDTO(List<UsuarioDAO> listaUsuarioDao);
    }
}
