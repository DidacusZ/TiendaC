using System.Net.Mail;
using Tienda_C.Fichero;

namespace Tienda_C.servicios
{
    /// <summary>
    /// Logica del envio de emails
    /// </summary>
    public class EmailServicioImpl : EmailServicioInterfaz
    {
        const string EMAIL_ORIGEN = "";
        const string CLAVE_ORIGEN = "";
        const string URL_DOMINIO = "http://localhost:5266";
        public void EmailCambioClave(string emailDestino, string nombreUsuario, string token)
        {
            try
            {
                FicheroLog.Log("[INFO] [EmailServicioImpl-EmailCambioClave()]");

                //Se crea la URL de recuperación con el token que se enviará al mail del user.
                string url = String.Format("{0}/credenciales/cambiarClave/?token={1}", URL_DOMINIO, token);

                //Hacemos que el texto del email sea un archivo html que se encuentra en la carpeta Plantilla.
                string directorioProyecto = System.IO.Directory.GetCurrentDirectory();
                string rutaArchivo = System.IO.Path.Combine(directorioProyecto, "email/cambiarClave.html");
                string htmlContent = System.IO.File.ReadAllText(rutaArchivo);
                //Asignamos el nombre de usuario que tendrá el cuerpo del mail y el URL de recuperación con el token al HTML.
                htmlContent = String.Format(htmlContent, nombreUsuario, url);

                MailMessage mensajeDelCorreo = new MailMessage(EMAIL_ORIGEN, emailDestino, "Cambiar contraseña", htmlContent);

                mensajeDelCorreo.IsBodyHtml = true;

                SmtpClient smtpCliente = new SmtpClient("smtp.gmail.com");
                smtpCliente.EnableSsl = true;
                smtpCliente.UseDefaultCredentials = false;
                smtpCliente.Port = 587;
                smtpCliente.Credentials = new System.Net.NetworkCredential(EMAIL_ORIGEN, CLAVE_ORIGEN);

                smtpCliente.Send(mensajeDelCorreo);

                smtpCliente.Dispose();
            }
            catch (Exception)
            {
                FicheroLog.Log("[ERROR] [EmailServicioImpl-EmailCambioClave()]");
            }
        }

        public void EmailConfirmacion(string emailDestino, string nombreUsuario, string token)
        {
            try
            {
                FicheroLog.Log("[INFO] [EmailServicioImpl-EmailConfirmacion()]");

                string url = String.Format("{0}/credenciales/confirmarCuenta/?token={1}", URL_DOMINIO, token);                

                //Hacemos que el texto del email sea un archivo html que se encuentra en la carpeta Plantilla.
                string directorioProyecto = System.IO.Directory.GetCurrentDirectory();
                string rutaArchivo = System.IO.Path.Combine(directorioProyecto, "email/confirmarCuenta.html");
                string htmlContent = System.IO.File.ReadAllText(rutaArchivo);
                //Asignamos el nombre de usuario que tendrá el cuerpo del email y el URL de recuperación con el token al HTML.
                htmlContent = String.Format(htmlContent, nombreUsuario, url);
                
                MailMessage mensajeDelCorreo = new MailMessage(EMAIL_ORIGEN, emailDestino, "Confirmar cuenta", htmlContent);

                mensajeDelCorreo.IsBodyHtml = true;

                SmtpClient smtpCliente = new SmtpClient("smtp.gmail.com");
                smtpCliente.EnableSsl = true;
                smtpCliente.UseDefaultCredentials = false;
                smtpCliente.Port = 587;
                smtpCliente.Credentials = new System.Net.NetworkCredential(EMAIL_ORIGEN, CLAVE_ORIGEN);

                smtpCliente.Send(mensajeDelCorreo);

                smtpCliente.Dispose();
            }
            catch (Exception)
            {
                FicheroLog.Log("[ERROR] [EmailServicioImpl-EmailConfirmacion()]");
            }
        }
    }
}
