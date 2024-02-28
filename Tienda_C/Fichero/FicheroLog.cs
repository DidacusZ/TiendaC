namespace Tienda_C.Fichero
{
    public class FicheroLog
    {
        public static void Log(string contenido)
        {
            try
            {
                //Ruta: \TiendaLog.txt
                string directorioBisabuelo = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.FullName;
                string ruta = Path.Combine(directorioBisabuelo, "TiendaLog.txt");

                // En el scope de Using los recursos utilizados se cerran automaticamente
                // Abrir el archivo de registro en modo de escritura, creándolo si no existe. 
                using (FileStream fs = new FileStream(ruta , FileMode.OpenOrCreate, FileAccess.Write))
                {
                    // Crear un StreamWriter para escribir en el archivo
                    using (StreamWriter escribir = new StreamWriter(fs))
                    {
                        // Mover al final del archivo
                        escribir.BaseStream.Seek(0, SeekOrigin.End);

                        // Reemplazar los saltos de línea en el mensaje por "|"
                        contenido = contenido.Replace(Environment.NewLine, " | ");
                        contenido = contenido.Replace("\r\n", " | ").Replace("\n", " | ").Replace("\r", " | ");

                        // Escribir la fecha y hora actual seguida del mensaje en una nueva línea
                        escribir.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " " + contenido);

                        // Limpiar el búfer y escribir los datos en el archivo
                        escribir.Flush();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] [EscribirLog - escribirEnFicheroLog()] Error al escribir en el fichero log:" + e.Message);
            }
        }
    }
}
