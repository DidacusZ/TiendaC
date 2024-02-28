using System.Security.Cryptography;
using System.Text;
using Tienda_C.Fichero;

namespace Tienda_C.servicios
{
    /// <summary>
    /// Logica de la encriptacion
    /// </summary>
    public class EncriptarServicioImpl : EncriptarServicioInterfaz
    {
        public string Encriptar(string clave)
        {
            try
            {
                FicheroLog.Log("[INFO] [EncriptarServicioImpl-Encriptar()]");

                // Se utiliza SHA256 para realizar el hash de la clave proporcionada
                using (SHA256 sha256 = SHA256.Create())
                {
                    // Se convierte la cadena de clave en un array de bytes utilizando UTF-8
                    byte[] bytes = Encoding.UTF8.GetBytes(clave);

                    // Se calcula el hash de los bytes utilizando SHA256
                    byte[] hashBytes = sha256.ComputeHash(bytes);

                    // Se convierte el hash de bytes en una cadena hexadecimal sin guiones y en minúsculas
                    string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                    // Se devuelve el hash en forma de cadena
                    return hash;
                }
            }
            catch (Exception)
            {
                FicheroLog.Log("[ERROR] [EncriptarServicioImpl-Encriptar()]");
                return null;
            }
        }
    }
}
