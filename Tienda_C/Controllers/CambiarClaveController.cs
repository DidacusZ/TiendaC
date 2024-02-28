using Microsoft.AspNetCore.Mvc;
using Tienda_C.dto;
using Tienda_C.Fichero;
using Tienda_C.servicios;

namespace Tienda_C.Controllers
{
    [Route("credenciales")]
    public class CambiarClaveController : Controller
    {

        private readonly UsuarioServicioInterfaz _usuarioS;

        public CambiarClaveController(UsuarioServicioInterfaz usuarioS)
        {
            _usuarioS = usuarioS;
        }

        [HttpGet]
        [Route("cambiarClave")]
        public IActionResult CambiarClaveGet([FromQuery(Name = "token")] string token)
        {
            try
            {
                FicheroLog.Log("[INFO] [CambiarClaveController-CambiarClaveGet()]");
                UsuarioDTO usuarioDTO = _usuarioS.BuscarUsuarioPorToken(token);
                Console.WriteLine("token get: " + token);

                if (usuarioDTO != null)
                {
                    ViewData["UsuarioDTO"] = usuarioDTO;
                    ViewData["UsuarioDto_token"] = usuarioDTO.token_recuperacion;
                }
                else
                {
                    ViewData["MensajeErrorTokenValidez"] = "Enlace de recuperación no es válido o el usuario no se ha encontrado";
                    FicheroLog.Log("[INFO] [CambiarClaveController-CambiarClaveGet()] " + ViewData["MensajeErrorTokenValidez"]);
                    return View("~/Views/credenciales/SolicitarCambiarClave.cshtml");
                }
                return View("~/Views/credenciales/cambiarClave.cshtml");
            }
            catch (Exception)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                FicheroLog.Log("[ERROR] [CambiarClaveController-CambiarClaveGet()]");
                return View("~/Views/credenciales/SolicitarCambiarClave.cshtml");
            }
        }


        [HttpPost]
        [Route("cambiarClave")]
        public IActionResult CambiarClavePost(UsuarioDTO usuarioDto)
        {
            try
            {
                FicheroLog.Log("[INFO] [CambiarClaveController-CambiarClavePost()]");
                Console.WriteLine("token: " + usuarioDto.token_recuperacion);//no hay
                UsuarioDTO usuarioExistente = _usuarioS.BuscarUsuarioPorToken(usuarioDto.token_recuperacion);//no encuentra

                if (usuarioExistente == null)
                {
                    ViewData["MensajeErrorTokenValidez"] = "El enlace de recuperación no es válido";
                    //EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController. " + ViewData["MensajeErrorTokenValidez"]);
                    return View("~/Views/credenciales/SolicitarCambiarClave.cshtml");
                }

                if (usuarioExistente.fch_expiracion_token.HasValue && usuarioExistente.fch_expiracion_token.Value < DateTime.Now)
                {
                    ViewData["MensajeErrorTokenExpirado"] = "El enlace de recuperación ha expirado";
                    //EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController. " + ViewData["MensajeErrorTokenExpirado"]);
                    return View("~/Views/credenciales/SolicitarCambiarClave.cshtml");
                }

                bool modificadaPassword = _usuarioS.CambiarClaveConToken(usuarioDto);

                if (modificadaPassword)
                {
                    ViewData["ContraseñaModificadaExito"] = "Contraseña modificada OK";
                    //EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController. " + ViewData["ContraseñaModificadaExito"]);
                    return View("~/Views/credenciales/inicioSesion.cshtml");
                }
                else
                {
                    ViewData["ContraseñaModificadaError"] = "Error al cambiar de contraseña";
                    //EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método ProcesarRecuperacionContraseña() de la clase RecuperarPasswordController. " + ViewData["ContraseñaModificadaError"]);
                    return View("~/Views/credenciales/SolicitarCambiarClave.cshtml");
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                FicheroLog.Log("[ERROR] [CambiarClaveController-CambiarClavePost()]");
                return View("~/Views/credenciales/SolicitarCambiarClave.cshtml");
            }
        }
    }
}