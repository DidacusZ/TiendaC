namespace Tienda_C.servicios
{
    /// <summary>
    /// 
    /// </summary>
    public interface ImagenServicioInterfaz
    {
        /// <summary>
        /// guarda imagen en ruta especifica
        /// </summary>
        /// <param name="nombreCarpeta"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="imagenData"></param>
        /// <returns></returns>
        public string GuardarImagen(string nombreCarpeta, string nombreArchivo, byte[] imagenData);
    }
}
