namespace Tienda_C.servicios
{
    /// <summary>
    /// Metodos para la encriptación
    /// </summary>
    public interface EncriptarServicioInterfaz
    {
        /// <summary>
        /// Encripta un string
        /// </summary>
        /// <param name="clave"></param>lo que se quiere encriptar
        /// <returns></returns>
        public string Encriptar(string clave);
    }
}
