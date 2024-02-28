using DAL.Modelos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tienda_C.dto;
using Tienda_C.servicios;
using Tienda_C.Fichero;

namespace Tienda_C.Controllers
{
    /// <summary>
    /// Controlador para hacer solicitudes get y post del inicio de sesión
    /// </summary>
    
    public class InicioSesionController:Controller
    {
        private readonly UsuarioServicioInterfaz _usuarioS;

        public InicioSesionController(UsuarioServicioInterfaz usuarioS)
        {
            _usuarioS = usuarioS;
        }

        /// <summary>
        /// muestra la vista inicio de sesión y pasa un UsuarioDTO vacío a la vista
        /// </summary>
        /// <returns></returns>
        ///
        [HttpGet]
        [Route("credenciales/inicioSesion")]
        public IActionResult InicioSesionGet()
        {
            try
            {
                FicheroLog.Log("[INFO] [InicioSesionController-InicioSesionGet()]");
                UsuarioDTO usuarioDTO = new UsuarioDTO();
                return View("~/Views/credenciales/inicioSesion.cshtml", usuarioDTO);

            }
            catch (Exception)
            {
                ViewData["error"] = "Error al procesar la solicitud";
                FicheroLog.Log("[ERROR] [InicioSesionController-InicioSesionGet()]");
                return View("~/Views/credenciales/inicioSesion.cshtml");
            }
        }

        [HttpPost]
        [Route("credenciales/inicioSesion")]
        public IActionResult InicioSesionPost(UsuarioDTO usuarioDto)
        {
            try
            {
                FicheroLog.Log("[INFO] [InicioSesionController-InicioSesionPost()]");

                bool credencialesValidas = _usuarioS.IniciarSesion(usuarioDto.email_usuario, usuarioDto.clv_usuario);
                if (credencialesValidas)
                {
                    UsuarioDTO u = _usuarioS.BuscarUsuarioPorEmail(usuarioDto.email_usuario);

                    // Se crea una lista de claims para almacenar información sobre la identidad del usuario que ha iniciado sesión
                    var claims = new List<Claim>
                    {
                        // Se agrega un claim que representa el nombre del usuario, utilizando su dirección de correo electrónico
                        new Claim(ClaimTypes.Name, usuarioDto.email_usuario),
                    };

                    // Se verifica si el usuario tiene un rol asignado
                    if (!string.IsNullOrEmpty(u.rol_usuario))
                    {
                        // Si el usuario tiene un rol, se agrega un claim que representa su rol
                        claims.Add(new Claim(ClaimTypes.Role, u.rol_usuario));
                    }

                    // Se crea una identidad de reclamaciones utilizando los claims creados anteriormente
                    var identidadDeReclamaciones = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Se utiliza el método SignInAsync para firmar la identidad de reclamaciones y establecer una cookie de autenticación en el navegador del usuario
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidadDeReclamaciones));

                    return RedirectToAction("Bienvenida", "InicioSesion");
                }
                else
                {
                    ViewData["MensajeErrorInicioSesion"] = "Credenciales inválidas o cuenta no confirmada";
                    FicheroLog.Log("[ERROR] [InicioSesionController-InicioSesionPost()] Credenciales inválidas o cuenta no confirmada");

                    return View("~/Views/credenciales/inicioSesion.cshtml");
                }
            }
            catch (Exception)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                FicheroLog.Log("[ERROR] [InicioSesionController-InicioSesionPost()]");

                return View("~/Views/credenciales/inicioSesion.cshtml");
            }
        }


        /// <summary>
        /// confirmar la cuenta
        /// </summary>
        /// <param name="token"></param> token de confirmación de cuenta
        /// <returns></returns>
        [HttpGet]
        [Route("credenciales/confirmarCuenta")]
        public IActionResult ConfirmarCuenta([FromQuery] string token)
        {
            try
            {
                FicheroLog.Log("[INFO] [InicioSesionController-ConfirmarCuenta()]");
                bool esConfirmada =  _usuarioS.ConfirmarCuenta(token);

                if (esConfirmada)
                {
                    ViewData["CuentaVerificada"] = "confirmada correctamente";
                }
                else
                {
                    ViewData["yaEstabaVerificada"] = "usuario registrado y confirmado";
                }

                return View("~/Views/credenciales/inicioSesion.cshtml");
            }
            catch (Exception)
            {
                ViewData["error"] = "Error al procesar la solicitud";
                FicheroLog.Log("[ERROR] [InicioSesionController-ConfirmarCuenta()]");
                return View("~/Views/credenciales/inicioSesion.cshtml");
            }
        }


        [Authorize]
        [HttpGet]
        [Route("/privada/bienvenida")]
        public IActionResult Bienvenida()
        {
            UsuarioDTO u = _usuarioS.BuscarUsuarioPorEmail(User.Identity.Name);
            ViewBag.UsuarioDTO = u;
            FicheroLog.Log("[INFO] [InicioSesionController-Bienvenida()]");
            return View("~/Views/privada/bienvenida.cshtml");
        }



        [HttpGet]
        [Route("/cerrarSesion")]
        public IActionResult CerrarSesion()
        {
            // Cierra la sesión del usuario
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirige al usuario a la página de inicio o a donde quieras después de cerrar sesión
            return RedirectToAction("Index", "Home");
        }
    }
}
