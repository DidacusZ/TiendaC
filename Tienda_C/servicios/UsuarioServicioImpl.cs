using DAL;
using DAL.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Tienda_C.dto;
using Tienda_C.Fichero;


namespace Tienda_C.servicios
{
    /// <summary>
    /// Lógica de los métodos para la gestión de usuario
    /// </summary>
    public class UsuarioServicioImpl : UsuarioServicioInterfaz
    {
        //variables privadas (es muy importante poner _ porque sino no se hace bien la inyección de dependencias)
        private readonly Contexto _contexto;
        private readonly EncriptarServicioInterfaz _encriptarS;
        private readonly UsuarioDTOaDAOInterfaz _usuarioDTOaDAO;
        private readonly UsuarioDAOaDTOInterfaz _usuarioDAOaDTO;
        private readonly EmailServicioInterfaz _emailS;
        //private readonly IServicioEmail _servicioEmail;


        public UsuarioServicioImpl(Contexto contexto, EncriptarServicioInterfaz encriptarServicio, UsuarioDTOaDAOInterfaz usuarioDTOaDAO, UsuarioDAOaDTOInterfaz usuarioDAOaDTO, EmailServicioInterfaz emailS)
        {
            _contexto = contexto;
            _encriptarS = encriptarServicio;
            _usuarioDTOaDAO = usuarioDTOaDAO;
            _usuarioDAOaDTO = usuarioDAOaDTO;
            _emailS = emailS;
        }

        public UsuarioDTO GuardarUsuario(UsuarioDTO usuarioDto)
        {
            try
            {
                FicheroLog.Log("[INFO] [UsuarioServicioImpl-GuardarUsuario()]");
                var usuarioExiste = _contexto.Usuarios.FirstOrDefault(u => u.email_usuario == usuarioDto.email_usuario && !u.cuenta_confirmada);

                if (usuarioExiste != null)
                {
                    usuarioDto.email_usuario = "EmailNoConfirmado";
                    return usuarioDto;
                }

                var emailExiste = _contexto.Usuarios.FirstOrDefault(u => u.email_usuario == usuarioDto.email_usuario && u.cuenta_confirmada);

                if (emailExiste != null)
                {
                    usuarioDto.email_usuario = "EmailRepetido";
                    return usuarioDto;
                }

                usuarioDto.clv_usuario = _encriptarS.Encriptar(usuarioDto.clv_usuario);
                UsuarioDAO usuarioDao = _usuarioDTOaDAO.UsuarioDTOaDAO(usuarioDto);
                usuarioDao.rol_usuario = "ROLE_USER";
                string token = GenerarToken();
                usuarioDao.token_recuperacion = token;

                _contexto.Usuarios.Add(usuarioDao);
                _contexto.SaveChanges();

                string nombreUsuario = usuarioDao.nom_usuario;
                _emailS.EmailConfirmacion(usuarioDto.email_usuario, nombreUsuario, token);

                return usuarioDto;
            }
            catch (Exception)
            {
                FicheroLog.Log("[ERROR] [UsuarioServicioImpl-GuardarUsuario()]");
                return null;
            }
        }


        public bool IniciarSesion(string email, string clave)
        {
            try
            {
                FicheroLog.Log("[INFO] [UsuarioServicioImpl-IniciarSesion()]");

                string claveEncriptada = _encriptarS.Encriptar(clave);
                UsuarioDAO? usuarioDao = _contexto.Usuarios.FirstOrDefault(u => u.email_usuario == email && u.clv_usuario == claveEncriptada);
                if (usuarioDao == null)
                {
                    FicheroLog.Log("[INFO] [RegistroController-IniciarSesion()] email no encontrado");
                    return false;
                }
                if (!usuarioDao.cuenta_confirmada)
                {
                    FicheroLog.Log("[INFO] [RegistroController-IniciarSesion()] cuenta no confirmada");
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                FicheroLog.Log("[ERROR] [UsuarioServicioImpl-IniciarSesion()]");
                return false;
            }

        }


        public UsuarioDTO BuscarUsuarioPorEmail(string email)
        {
            try
            {
                FicheroLog.Log("[INFO] [UsuarioServicioImpl-BuscarUsuarioPorEmail()]");

                UsuarioDTO usuarioDTO = new UsuarioDTO();
                var usuarioDao = _contexto.Usuarios.FirstOrDefault(u => u.email_usuario == email);
                //nulo=no existe
                
                if (usuarioDao != null)
                {
                    //si existe
                    usuarioDTO = _usuarioDAOaDTO.UsuarioDAOaDTO(usuarioDao);
                }
                return usuarioDTO;
            }
            catch (Exception)
            {
                FicheroLog.Log("[ERROR] [UsuarioServicioImpl-BuscarUsuarioPorEmail()]");
                return null;
            }

        }


        public UsuarioDTO BuscarUsuarioPorToken(string token)
        {
            try
            {
                FicheroLog.Log("[INFO] [UsuarioServicioImpl-BuscarUsuarioPorToken()]");

                UsuarioDAO? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.token_recuperacion == token);

                if (usuarioExistente != null)
                {
                    UsuarioDTO usuario = _usuarioDAOaDTO.UsuarioDAOaDTO(usuarioExistente);
                    return usuario;
                }
                else
                {
                    FicheroLog.Log("[INFO] [UsuarioServicioImpl-BuscarUsuarioPorToken()] no existe usuario con token: "+token);
                    return null;
                }
            }
            catch (ArgumentNullException e)
            {
                FicheroLog.Log("[ERROR] [UsuarioServicioImpl-BuscarUsuarioPorToken()]");
                return null;
            }
        }

        public bool ConfirmarCuenta(string token)
        {
            try
            {
                FicheroLog.Log("[INFO] [UsuarioServicioImpl-ConfirmarCuenta()]");

                UsuarioDAO? usuarioExistente = _contexto.Usuarios.Where(u => u.token_recuperacion == token).FirstOrDefault();

                if (usuarioExistente != null && !usuarioExistente.cuenta_confirmada)
                {
                    // Entra en esta condición si el usuario existe y su cuenta no se ha confirmado
                    usuarioExistente.cuenta_confirmada = true;
                    usuarioExistente.token_recuperacion = null;
                    _contexto.Usuarios.Update(usuarioExistente);
                    _contexto.SaveChanges();
                    FicheroLog.Log("[INFO] [UsuarioServicioImpl-ConfirmarCuenta()] La cuenta se ha confirmado correctament");
                    return true;
                }
                else
                {
                    FicheroLog.Log("[INFO] [UsuarioServicioImpl-ConfirmarCuenta()] cuenta no confirmada o inexistente");
                    return false;
                }
            }
            catch (Exception)
            {
                FicheroLog.Log("[ERROR] [UsuarioServicioImpl-ConfirmarCuenta()]");
                return false;
            }
        }


        //privados

        /// <summary>
        /// metodo encargado de generar un token
        /// </summary>
        /// <returns></returns>
        private string GenerarToken()
        {
            try
            {
                FicheroLog.Log("[INFO] [UsuarioServicioImpl-GenerarToken()]");

                using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
                {
                    byte[] tokenBytes = new byte[30];
                    rng.GetBytes(tokenBytes);

                    return BitConverter.ToString(tokenBytes).Replace("-", "").ToLower();
                }
            }
            catch (Exception)
            {
                FicheroLog.Log("[ERROR] [UsuarioServicioImpl-GenerarToken()]");
                return null;
            }

        }


        public bool IniciarCambioClave(string email)
        {
            try
            {
                Console.WriteLine("email: "+email);
                FicheroLog.Log("[INFO] [UsuarioServicioImpl-IniciarCambioClave()]");

                UsuarioDAO? usuarioDao = _contexto.Usuarios.FirstOrDefault(u => u.email_usuario == email);

                if (usuarioDao != null)
                {
                    // Generar el token y establecer la fecha de expiración
                    string token = GenerarToken();

                    DateTime fechaExpiracion = DateTime.Now.AddMinutes(10);
                    // DateTime.SpecifyKind(fechaExpiracion, DateTimeKind.Utc) cambia el tipo de Kind del objeto DateTime
                    // para que sea DateTimeKind.Utc, lo que significa que se considera que la fecha y hora son en el
                    // horario de tiempo universal coordinado (UTC), que es una forma común de manejar fechas y horas en aplicaciones.
                    fechaExpiracion = DateTime.SpecifyKind(fechaExpiracion, DateTimeKind.Utc);//sino lo guardo así no me lo deja guardar en BD

                    // Actualizar el usuario con el nuevo token y la fecha de expiración
                    usuarioDao.token_recuperacion = token;
                    usuarioDao.fch_expiracion_token = fechaExpiracion;

                    // Actualizar el usuario en la base de datos
                    _contexto.Usuarios.Update(usuarioDao);
                    _contexto.SaveChanges();

                    // Enviar el correo de recuperación
                    string nombreUsuario = usuarioDao.nom_usuario;
                    _emailS.EmailCambioClave(email, nombreUsuario, token);

                    return true;
                }
                else
                {
                    FicheroLog.Log("[INFO] [UsuarioServicioImpl-IniciarCambioClave()] no existe "+ email);
                    return false;
                }
            }
            catch (DbUpdateException dbe)
            {
                FicheroLog.Log("[ERROR] [UsuarioServicioImpl-IniciarCambioClave()] BD");
                Console.WriteLine("[Error UsuarioServicioImpl - iniciarProcesoRecuperacion()] Error de persistencia al actualizar la bbdd: " + dbe.Message);
                return false;
            }
            catch (Exception ex)
            {
                FicheroLog.Log("[ERROR] [UsuarioServicioImpl-IniciarCambioClave()] Rec");
                Console.WriteLine("[Error UsuarioServicioImpl - iniciarProcesoRecuperacion()] Error al iniciar el proceso de recuperación: " + ex.Message);
                return false;
            }
        }

        public bool CambiarClaveConToken(UsuarioDTO usuarioDto)
        {
            try
            {
                FicheroLog.Log("[INFO] [UsuarioServicioImpl-CambiarClaveConToken()]");

                UsuarioDAO? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.token_recuperacion == usuarioDto.token_recuperacion);

                if (usuarioExistente != null)
                {
                    string nuevaContraseña = _encriptarS.Encriptar(usuarioDto.clv_usuario);
                    usuarioExistente.clv_usuario = nuevaContraseña;
                    usuarioExistente.token_recuperacion = null; //borramos token
                    _contexto.Usuarios.Update(usuarioExistente);
                    _contexto.SaveChanges();

                    return true;
                }
            }
            catch (DbUpdateException dbe)
            {
                Console.WriteLine("[Error UsuarioServicioImpl - modificarContraseñaConToken()] Error de persistencia al actualizar la bbdd: " + dbe.Message);
            }
            catch (ArgumentNullException e)
            {
                FicheroLog.Log("[ERROR] [UsuarioServicioImpl-CambiarClaveConToken()]");
                Console.WriteLine("[Error UsuarioServicioImpl - verificarCredenciales()] Error al modificar contraseña del usuario: " + e.Message);
                return false;
            }
            return false;
        }

        public List<UsuarioDTO> TodosUsuarios()
        {
            FicheroLog.Log("[INFO] [UsuarioServicioImpl-TodosUsuarios()]");
            var usuariosOrdenados = _contexto.Usuarios.OrderBy(u => u.id_usuario).ToList();
            return _usuarioDAOaDTO.ListaUsuarioDAOaDTO(usuariosOrdenados);
        }

        public UsuarioDTO BuscarPorId(long id)
        {
            try
            {
                FicheroLog.Log("[INFO] [UsuarioServicioImpl-BuscarPorId()]");

                UsuarioDAO? usuarioDao = _contexto.Usuarios.FirstOrDefault(u => u.id_usuario == id);
                if (usuarioDao != null)
                {
                    return _usuarioDAOaDTO.UsuarioDAOaDTO(usuarioDao);
                }
            }
            catch (Exception)
            {
                FicheroLog.Log("[ERROR] [UsuarioServicioImpl-BuscarPorId()]");
            }
            return null;
        }

        public void EliminarUsuario(long id)
        {
            try
            {
                FicheroLog.Log("[INFO] [UsuarioServicioImpl-EliminarUsuario()]");

                UsuarioDAO? usuarioDao = _contexto.Usuarios.Find(id);
                if (usuarioDao != null)
                {
                    _contexto.Usuarios.Remove(usuarioDao);
                    _contexto.SaveChanges();
                }
            }
            catch (Exception)
            {
                FicheroLog.Log("[ERROR] [UsuarioServicioImpl-EliminarUsuario()]");
            }
        }

        public void EditarUsuario(UsuarioDTO usuarioDto)
        {
            try
            {
                FicheroLog.Log("[INFO] [UsuarioServicioImpl-EditarUsuario()]");

                UsuarioDAO? usuarioDao = _contexto.Usuarios.Find(usuarioDto.id_usuario);

                if (usuarioDao != null)
                {
                    usuarioDao.nom_usuario = usuarioDto.nom_usuario;
                    usuarioDao.mvl_usuario = usuarioDto.mvl_usuario;
                    usuarioDao.rol_usuario = usuarioDto.rol_usuario;
                    usuarioDao.img_usuario = usuarioDto.img_usuario;

                    _contexto.Usuarios.Update(usuarioDao);
                    _contexto.SaveChanges();
                    FicheroLog.Log("[INFO] [UsuarioServicioImpl-EditarUsuario()] usuario editado");
                }
                FicheroLog.Log("[INFO] [UsuarioServicioImpl-EditarUsuario()] usuario no encontrado");
            }
            catch (Exception )
            {
                FicheroLog.Log("[ERROR] [UsuarioServicioImpl-EditarUsuario()]");
            }
        }
    }
}