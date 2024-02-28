using Microsoft.AspNetCore.Mvc;
using Tienda_C.dto;
using Tienda_C.Fichero;
using Tienda_C.servicios;

namespace Tienda_C.Controllers
{
    /// <summary>
    /// solicitudes necesarias para el cambio de clave
    /// </summary>
    [Route("credenciales")]
    public class SolicitarCambiarClaveController:Controller
    {
        private readonly UsuarioServicioInterfaz _usuarioS;

        public SolicitarCambiarClaveController(UsuarioServicioInterfaz usuarioS)
        {
            _usuarioS = usuarioS;
        }

        /// <summary>
        /// vista para solicitar el cambio de clave
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("solicitarCambiarClave")]
        public IActionResult SolicitarCambiarClaveGet()
        {
            try
            {
                FicheroLog.Log("[INFO] [SolicitarCambiarClaveController-SolicitarCambiarClaveGet()]");

                UsuarioDTO usuarioDTO = new UsuarioDTO();
                return View("~/Views/credenciales/solicitarCambiarClave.cshtml", usuarioDTO);
            }
            catch (Exception)
            {
                ViewData["error"] = "Error al procesar la solicitud";
                FicheroLog.Log("[ERROR] [SolicitarCambiarClave-SolicitarCambiarClaveGet()]");
                return View("~/Views/credenciales/solicitarCambiarClave.cshtml");
            }
        }


        /// <summary>
        /// solicitud post para el cambio de clave
        /// </summary>
        /// <param name="usuarioDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("solicitarCambiarClave")]
        public IActionResult SolicitarCambiarClavePost([Bind("email_usuario")] UsuarioDTO usuarioDto)
        {
            try
            {
                FicheroLog.Log("[INFO] [SolicitarCambiarClave-SolicitarCambiarClavePost()]");
                Console.WriteLine("email post: "+ usuarioDto.email_usuario);//no lo captura, nose porque
                bool envioConExito = _usuarioS.IniciarCambioClave(usuarioDto.email_usuario);
                Console.WriteLine("email post: "+envioConExito);
                if (envioConExito)
                {
                    ViewData["MensajeExitoMail"] = "Proceso de recuperación OK";
                    FicheroLog.Log("[INFO] [SolicitarCambiarClave-SolicitarCambiarClavePost()] "+ ViewData["MensajeExitoMail"]);
                    return View("~/Views/credenciales/inicioSesion.cshtml");
                }
                else
                {
                    ViewData["MensajeErrorMail"] = "cuenta de correo electrónico no encontrada.";
                }
                return View("~/Views/credenciales/solicitarCambiarClave.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud";
                FicheroLog.Log("[ERROR] [SolicitarCambiarClave-SolicitarCambiarClavePost()]");
                return View("~/Views/credenciales/solicitarCambiarClave.cshtml");
            }
        }

    }
}
