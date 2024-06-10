using HelpdeskApi.entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskApi.Controllers
{
    [ApiController]
    [Route("api/incidencias")]
    public class IncidenciaController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public IncidenciaController(ApplicationDbContext context)
        {
            this.context = context;
        }
        // listar incidencias por id
        [HttpGet]
        [Route("listarIncidenciasId")]
        public async Task<ActionResult> getIncidencia(string id)
        {
            var listarIncidenciasId = await context.incidenciasUsuarios.FromSqlRaw("EXEC incideciasPorUsuarios @id",
            new SqlParameter("@id", id)).ToListAsync();  
            return Ok(listarIncidenciasId);
        }
        // listar incidencias
        [HttpGet]
        [Route("listarIncidencias")]
        public async Task<ActionResult> getIncidencia()
        {
            var listarIncidencias = await context.incidenciasUsuarios.FromSqlRaw("listarIncidencias").ToListAsync();
            return Ok(listarIncidencias);
        }

        // Insertar incidencia
        [HttpPost]
        [Route("crearIncidencia")]
        public async Task<ActionResult> postIncidencia([FromBody] IncidenciaAdd incidenciaAdds)
        {
            try
            {
                var registrarIncidencia = await context.Database.ExecuteSqlRawAsync("EXEC registrarIncidencia @idUsuario,@asunto,@detalles",
                new SqlParameter("@idUsuario", incidenciaAdds.idUsuario),
                new SqlParameter("@asunto",incidenciaAdds.asunto),
                new SqlParameter("@detalles", incidenciaAdds.detalles));
                return Ok(registrarIncidencia);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }   
    }
}
