using DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Tienda_C.servicios;

namespace Tienda_C
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //Agregamos la cadena de conexion
            builder.Services.AddDbContext<Contexto>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("CadenaConexionPostgreSQL")));

            //inyección de dependencias

            builder.Services.AddScoped<UsuarioServicioInterfaz, UsuarioServicioImpl>();
            builder.Services.AddScoped<EncriptarServicioInterfaz, EncriptarServicioImpl>();
            builder.Services.AddScoped<UsuarioDTOaDAOInterfaz, UsuarioDTOaDAOImpl>();
            builder.Services.AddScoped<UsuarioDAOaDTOInterfaz, UsuarioDAOaDTOImpl>();
            builder.Services.AddScoped<EmailServicioInterfaz, EmailServicioImpl>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/credenciales/inicioSesion";
            });

            var app = builder.Build();


            //script
            //cadena de conexión desde la configuración
            string connectionString = builder.Configuration.GetConnectionString("CadenaConexionPostgreSQL");

            //llamar al método para ejecutar el script SQL
            string scriptPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.FullName+"/Datos_C.sql";

            var ejecutarScript = new EjecutarScriptSQL();

            //descomentar esta linea si se quiere ejecutar el script
            //ejecutarScript.EjecutarScript(connectionString, scriptPath);


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
