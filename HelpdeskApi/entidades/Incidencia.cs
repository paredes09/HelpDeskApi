namespace HelpdeskApi.entidades
{
    public class incidenciasUsuarios
    {
        public int id { get; set; }
        public string nombreUsuario { get; set; }
        public int idUsuario { get; set; }
        public string nTicket { get; set; }
        public string asunto { get; set; }
        public string detalles { get; set; }
        public string nombreEstado { get; set; }
        public string nombreArea { get; set; }
        public string colorEstado { get; set; }
        public DateTime fechaIncidencia { get; set; }
        public string? respuesta { get; set; } 
    }


    // crear una nueva incidencia
    public class IncidenciaAdd
    {
        public int idUsuario { get; set; }
        public string asunto { get; set; }
        public string detalles { get; set; }
    
    }
}
