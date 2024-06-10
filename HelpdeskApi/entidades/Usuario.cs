using System.Data;

namespace HelpdeskApi.entidades
{
    public class Usuario
    {

        public int id { get; set; }
        public string nombreUsuario { get; set; }
        public string apellidoUsuario { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public string nombreArea { get; set; }
        public string lugarSede { get; set; }
        public string celularUsuario { get; set; }
        public int idPerfil { get; set; }

    }

    public class NewUsuario
    {
        public int id { get; set; }
        public string nombreUsuario { get; set; }
        public string apellidoUsuario { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public int idArea { get; set; }
        public int idSede { get; set; }
        public string celularUsuario { get; set; }
        public int idPerfil { get; set; }

    }
    public class UsuarioSesion
    {
        public string usuario { get; set; }
        public string contrasena { get; set; }
    }

    public class UsuarioPassword
    {
        public int id { get; set; }
        public string contrasena { get; set; }
        public string contrasenaActual { get; set; } // Añadido para la contraseña actual
    }

}
