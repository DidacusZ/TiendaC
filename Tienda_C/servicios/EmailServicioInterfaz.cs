namespace Tienda_C.servicios
{
    /// <summary>
    /// Metodos para el envio de emails
    /// </summary>
    public interface EmailServicioInterfaz
    {
        /// <summary>
        /// Envia email de cambio de contraseña
        /// </summary>
        /// <param name="emailDestino"></param>
        /// <param name="nombreUsuario"></param>
        /// <param name="token"></param>
        public void EmailCambioClave(string emailDestino, string nombreUsuario, string token);

        /// <summary>
        /// Envia email de confirmación
        /// </summary>
        /// <param name="emailDestino"></param>
        /// <param name="nombreUsuario"></param>
        /// <param name="token"></param>
        public void EmailConfirmacion(string emailDestino, string nombreUsuario, string token);
    }
}
