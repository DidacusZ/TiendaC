using Npgsql;
using System;
using System.Data.SqlClient;
namespace Tienda_C
{
    public class EjecutarScriptSQL
    {
        public void EjecutarScript(string connectionString, string scriptPath)
        {
            try
            {
                // Leer el contenido del script SQL
                string script = System.IO.File.ReadAllText(scriptPath);

                // Crear una nueva conexión a la base de datos
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    // Abrir la conexión
                    connection.Open();

                    // Crear un nuevo comando SQL
                    using (NpgsqlCommand command = new NpgsqlCommand(script, connection))
                    {
                        // Ejecutar el script SQL
                        command.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("Script SQL ejecutado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar el script SQL: " + ex.Message);
            }
        }
    }
}