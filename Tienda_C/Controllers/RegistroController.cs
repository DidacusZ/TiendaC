using Microsoft.AspNetCore.Mvc;
using Tienda_C.dto;
using Tienda_C.Fichero;
using Tienda_C.servicios;

namespace Tienda_C.Controllers
{
    /// <summary>
    /// Controlador para hacer solicitudes get y post del registro
    /// </summary>
    [Route("credenciales/registro")]
    public class RegistroController:Controller
    {

        private readonly UsuarioServicioInterfaz _usuarioS;

        public RegistroController(UsuarioServicioInterfaz usuarioS)
        {
            _usuarioS = usuarioS;
        }

        /// <summary>
        /// muestra la vista registro y pasa un UsuarioDTO vacío a la vista
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RegistroGet()
        {
            try
            {
                FicheroLog.Log("[INFO] [RegistroController-RegistroGet()]");
                UsuarioDTO usuarioDto = new UsuarioDTO();
                return View("~/Views/credenciales/registro.cshtml", usuarioDto);

            }
            catch (Exception)
            {
                //pasar datos de controlador a vista
                //se almacena un dato en el diccionario ViewData
                //luego se accede usando la clave:error
                ViewData["error"] = "Error al procesar la solicitud";
                FicheroLog.Log("[ERROR] [RegistroController-RegistroGet()]");
                return View("~/Views/credenciales/registro.cshtml");
            }
        }
        

        /// <summary>
        /// guarda el usuariro y controla los errores del registro
        /// </summary>
        /// <param name="usuarioDTO"></param> usuario recibido
        /// <returns></returns>
        [HttpPost]
        public IActionResult RegistrarPost(UsuarioDTO usuarioDTO)
        {
            try
            {
                FicheroLog.Log("[INFO] [RegistroController-RegistrarPost()]");
                UsuarioDTO usuarioDto = _usuarioS.GuardarUsuario(usuarioDTO);

                if (usuarioDto.email_usuario == "EmailNoConfirmado")
                {
                    ViewData["EmailNoConfirmado"] = "usuario registrado con ese email";
                    FicheroLog.Log("[INFO] [RegistroController-RegistrarPost()] " + ViewData["EmailNoConfirmado"]);
                    return View("~/Views/credenciales/inicioSesion.cshtml");
                }
                else if (usuarioDto.email_usuario == "EmailRepetido")
                {
                    ViewData["EmailRepetido"] = "existe un usuario con ese email";
                    FicheroLog.Log("[INFO] [RegistroController-RegistrarPost()] " + ViewData["EmailRepetido"]);
                    return View("~/Views/credenciales/registro.cshtml");
                }
                else
                {
                    ViewData["MensajeRegistroExitoso"] = "Registro OK";
                    FicheroLog.Log("[INFO] [RegistroController-RegistrarPost()] " + ViewData["MensajeRegistroExitoso"]);
                    return View("~/Views/credenciales/inicioSesion.cshtml");
                }

            }
            catch (Exception)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                FicheroLog.Log("[ERROR] [RegistroController-RegistrarPost()]");
                return View("~/Views/credenciales/registro.cshtml");
            }
        }
    }
}
