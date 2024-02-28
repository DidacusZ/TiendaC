using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tienda_C.dto;
using Tienda_C.Fichero;
using Tienda_C.servicios;

namespace Tienda_C.Controllers
{
    /// <summary>
    /// controlador de administración
    /// </summary>

    [Authorize(Roles = "ROLE_ADMIN")]
    [Route("privada")]
    public class AdministracionController:Controller    
    {
        private readonly UsuarioServicioInterfaz _usuarioS;
        //private readonly ImagenServicioInterfaz _imagenS;

        public AdministracionController(UsuarioServicioInterfaz usuarioS)//, ImagenServicioInterfaz imagenS)
        {
            _usuarioS = usuarioS;
            //_imagenS = imagenS;
        }

        /// <summary>
        /// muestra todos usuarios
        /// </summary>
        /// <returns></returns>
        
        [HttpGet]
        [Route("administracion")]
        public IActionResult Administracion()
        {
            try
            {
                FicheroLog.Log("[INFO] [AdministracionController-Administracion()]");
                List<UsuarioDTO> usuarios = new List<UsuarioDTO>();

                ViewBag.Usuarios = _usuarioS.TodosUsuarios();

                return View("~/Views/privada/administracion.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Ocurrió un error al obtener la lista de usuarios";

                FicheroLog.Log("[INFO] [AdministracionController-Administracion()]");
                return View("~/Views/privada/bienvenida.cshtml");
            }
        }

        /// <summary>
        /// elimian usuario por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("eliminarUsuario/{id}")]
        public IActionResult EliminarUsuario(long id)
        {
            try
            {
                FicheroLog.Log("[INFO] [AdministracionController-EliminarUsuario()]");

                UsuarioDTO usuario = _usuarioS.BuscarPorId(id);
                List<UsuarioDTO> usuarios = _usuarioS.TodosUsuarios();

                string emailUsuarioActual = User.Identity.Name;

                //int adminsRestantes = _usuarioS.contarUsuariosPorRol("ROLE_ADMIN");

                if (emailUsuarioActual == usuario.email_usuario)
                {
                    ViewData["noTePuedesEliminar"] = "Un administrador no puede eliminarse a sí mismo";
                    ViewBag.Usuarios = usuarios;
                    return View("~/Views/privada/administracion.cshtml");
                }

                _usuarioS.EliminarUsuario(id);
                ViewData["eliminacionCorrecta"] = "El usuario se ha eliminado correctamente";
                ViewBag.Usuarios = _usuarioS.TodosUsuarios();

                return View("~/Views/privada/administracion.cshtml");

            }
            catch (Exception e)
            {
                ViewData["error"] = "Ocurrió un error al eliminar el usuario";
                FicheroLog.Log("[ERROR] [AdministracionController-EliminarUsuario()]");
                return View("~/Views/privada/bienvenida.cshtml");
            }
        }


        /// <summary>
        /// muestra vista editarUsuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("mostrarEditarUsuario/{id}")]
        public IActionResult EditarUsuarioGet(long id)
        {
            try
            {
                FicheroLog.Log("[INFO] [AdministracionController-MostrarEditarUsuario()]");

                UsuarioDTO usuarioDTO = _usuarioS.BuscarPorId(id);

                if (usuarioDTO == null)
                {
                    return View("~/Views/privada/administracion.cshtml");
                }
                return View("~/Views/privada/editarUsuario.cshtml", usuarioDTO);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Ocurrió un error al obtener el usuario para editar";
                FicheroLog.Log("[ERROR] [AdministracionController-MostrarEditarUsuario()]");
                return View("~/Views/privada/bienvenida.cshtml");
            }
        }

        [HttpPost]
        [Route("editarUsuario")]
        public IActionResult EditarUsuarioPost(long id, string nombre, string movil, string rol, IFormFile imagen)
        {
            try
            {
                FicheroLog.Log("[INFO] [AdministracionController-EditarUsuario()]");

                UsuarioDTO usuarioDTO = _usuarioS.BuscarPorId(id);
                usuarioDTO.nom_usuario = nombre;
                usuarioDTO.mvl_usuario = movil;

                if (rol.Equals("Administrador"))
                {
                    usuarioDTO.rol_usuario = "ROLE_ADMIN";
                }
                else
                {
                    usuarioDTO.rol_usuario = rol;
                }
                /*
                if (imagen != null && imagen.Length > 0)
                {
                    var nombreArchivo = "nombre_universamente_único_para_la_imagen.jpg"; // Puedes generar un nombre único aquí

                    using (var memoryStream = new MemoryStream())
                    {
                        imagen.CopyTo(memoryStream);
                        var imagenData = memoryStream.ToArray();
                        var rutaImagen = _imagenS.GuardarImagen("imagenes", nombreArchivo, imagenData);
                        // Aquí puedes hacer lo que necesites con la ruta de la imagen guardada
                        usuarioDTO.img_usuario = rutaImagen;
                    }
                }
                else
                {
                    UsuarioDTO usuarioActualDTO = _usuarioS.BuscarPorId(id);
                    //byte[] fotoActual = usuarioActualDTO.img_usuario;
                    //usuarioDTO.img_usuario = fotoActual;
                }
                */
                _usuarioS.EditarUsuario(usuarioDTO);

                ViewData["EdicionCorrecta"] = "El Usuario se ha editado correctamente";
                ViewBag.Usuarios = _usuarioS.TodosUsuarios();

                return View("~/Views/privada/administracion.cshtml");
            }
            catch (Exception)
            {
                ViewData["Error"] = "error al editar el usuario";
                FicheroLog.Log("[ERROR] [AdministracionController-EditarUsuario()]");
                return View("~/Views/privada/bienvenida.cshtml");
            }
        }


        [HttpGet]
        [Route("mostrarCrearCuenta")]
        public IActionResult CrearcuentaGet()
        {
            try
            {
                FicheroLog.Log("[INFO] [AdministracionController-CrearcuentaGet()]");

                UsuarioDTO usuarioDTO = new UsuarioDTO();
                ViewData["esRegistroDeAdmin"] = true;

                return View("~/Views/credenciales/registro.cshtml", usuarioDTO);

            }
            catch (Exception e)
            {
                ViewData["error"] = "Error";
                FicheroLog.Log("[ERROR] [AdministracionController-CrearcuentaGet()]");
                return View("~/Views/credenciales/registro.cshtml");
            }
        }



        [HttpPost]
        [Route("crearcuenta")]
        public IActionResult CrearcuentaPost(UsuarioDTO usuarioDTO)
        {
            try
            {
                FicheroLog.Log("[INFO] [AdministracionController-CrearcuentaPost()]");

                UsuarioDTO nuevoUsuario = _usuarioS.GuardarUsuario(usuarioDTO);

                if (nuevoUsuario.email_usuario == "EmailNoConfirmado")
                {
                    ViewData["EmailNoConfirmado"] = "Ya existe un usuario registrado con ese email con la cuenta sin verificar";
                    ViewBag.Usuarios = _usuarioS.TodosUsuarios();
                    FicheroLog.Log("[INFO] [AdministracionController-CrearcuentaPost()]" + ViewData["EmailNoConfirmado"]);
                    return View("~/Views/privada/administracion.cshtml");
                }
                else if (nuevoUsuario.email_usuario == "EmailRepetido")
                {
                    ViewData["EmailRepetido"] = "Ya existe un usuario con ese email registrado en el sistema";
                    ViewBag.Usuarios = _usuarioS.TodosUsuarios();
                    FicheroLog.Log("[INFO] [AdministracionController-CrearcuentaPost()]" + ViewData["EmailRepetido"]);
                    return View("~/Views/credenciales/registro.cshtml");
                }
                else
                {
                    ViewData["MensajeRegistroExitoso"] = "Registro del nuevo usuario OK";
                    ViewBag.Usuarios = _usuarioS.TodosUsuarios();
                    FicheroLog.Log("[INFO] [AdministracionController-CrearcuentaPost()]" + ViewData["MensajeRegistroExitoso"]);
                    return View("~/Views/privada/administracion.cshtml");
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al crear";
                FicheroLog.Log("[ERROR] [AdministracionController-CrearcuentaPost()]");
                return View("~/Views/privada/administracion.cshtml");
            }
        }


    }
}
