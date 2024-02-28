using System.IO;
using Microsoft.AspNetCore.Hosting;
namespace Tienda_C.servicios
{

    public class ImagenServicioImpl:ImagenServicioInterfaz
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ImagenServicioImpl(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string GuardarImagen(string nombreCarpeta,string nombreArchivo, byte[] imagenData)
        {
            
            var carpetaImagenes = Path.Combine(_hostingEnvironment.WebRootPath, nombreCarpeta);
            var rutaImagen = Path.Combine(carpetaImagenes, nombreArchivo);

            // Verificar si la carpeta de imágenes existe, si no, crearla
            if (!Directory.Exists(carpetaImagenes))
            {
                Directory.CreateDirectory(carpetaImagenes);
            }

            // Guardar la imagen en la ruta especificada
            using (var fileStream = new FileStream(rutaImagen, FileMode.Create))
            {
                fileStream.Write(imagenData, 0, imagenData.Length);
            }

            // Devolver la ruta de la imagen guardada
            return rutaImagen;
        }

        public object GuardarImagen(string v, string nombreArchivo, object value)
        {
            throw new NotImplementedException();
        }
    }
}
