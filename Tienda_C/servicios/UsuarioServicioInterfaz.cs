using Tienda_C.dto;

namespace Tienda_C.servicios
{
    /// <summary>
    /// Metodos para la gestión de usuario
    /// </summary>
    public interface UsuarioServicioInterfaz
    {

        /// <summary>
        /// Guarda un usuario en BD
        /// </summary>
        /// <param name="usuarioDto"></param>información de usuario que se vva a guardar
        /// <returns></returns>
        public UsuarioDTO GuardarUsuario(UsuarioDTO usuarioDto);

        /// <summary>
        /// inciar sesión en la app comprobando si el email y la contraseña coinciden en BD
        /// </summary>
        /// <param name="email"></param>email usuario
        /// <param name="clave"></param>contraseña usuario
        /// <returns></returns>
        public bool IniciarSesion(string email, string clave);

        /// <summary>
        /// busca un usuario por su email
        /// </summary>
        /// <param name="email"></param>email usuario
        /// <returns></returns>
        UsuarioDTO BuscarUsuarioPorEmail(string email);

        /// <summary>
        /// busca un usuario por su token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UsuarioDTO BuscarUsuarioPorToken(string token);

        /// <summary>
        /// Confirma la cuenta
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool ConfirmarCuenta(string token);

        /// <summary>
        /// inicia el prroceso para cabiar la contraseña
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IniciarCambioClave(string email);

        /// <summary>
        /// cambia contraseña con el token
        /// </summary>
        /// <param name="usuarioDto"></param>
        /// <returns></returns>
        public bool CambiarClaveConToken(UsuarioDTO usuarioDto);





        //administración

        /// <summary>
        /// muestra todos los usuarios ordenados por id
        /// </summary>
        /// <returns></returns>
        public List<UsuarioDTO> TodosUsuarios();

        /// <summary>
        /// busca un usuario por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UsuarioDTO BuscarPorId(long id);

        /// <summary>
        /// borra un usuario por su id
        /// </summary>
        /// <param name="id"></param>
        public void EliminarUsuario(long id);

        /// <summary>
        /// edita un usuario
        /// </summary>
        /// <param name="usuarioDto"></param>
        public void EditarUsuario(UsuarioDTO usuarioDto);

    }
}
