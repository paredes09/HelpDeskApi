using HelpdeskApi;
using HelpdeskApi.entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace inmovil2.Controllers
{
    [ApiController]
    [Route("api/usarios")]
    public class UsuarioController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        public UsuarioController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("listarUsuarios")]
        public async Task<ActionResult> getusuario()
        {
            var listarUsuarios = await context.usuarios.FromSqlRaw("EXEC listarUsuarios").ToListAsync();
            return Ok(listarUsuarios);

        }
        [HttpGet]
        [Route("listarUsuarioNombre")]
        public async Task<ActionResult> getUsuarioNombre(string nombreUsuario)
        {
            var listarUsuarioNombre = await context.usuarios.FromSqlRaw("EXEC buscarUsuarioPorNombre @nombreUsuario",
            new SqlParameter("@nombreUsuario", nombreUsuario)).ToListAsync();
            return Ok(listarUsuarioNombre);
        }

        [HttpGet]
        [Route("SesionUsuario")]
        public async Task<ActionResult> getSesionUsuario(string usuario, string contraseña)
        {
            var contraseñahash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(contraseña));
            contraseña = Convert.ToBase64String(contraseñahash);

            var sesionUsuario = await context.usuarioSesion.FromSqlRaw("EXEC sesionUsuario @usuario, @contraseña",
            new SqlParameter("@usuario", usuario),
            new SqlParameter("@contraseña", contraseña)).ToListAsync();

            var result = sesionUsuario.Count;

            if (result > 0)
            {
                return Ok(sesionUsuario);
            }
            else
            {
                return BadRequest("Usuario o contraseña incorrectos");
            }
        }

        [HttpPost]
        [Route("crearUsuario")]
        public async Task<ActionResult> postUsuario([FromBody] NewUsuario usuario)
        {
            try
            {
                var UsuarioExistente = await context.newUsuarios.FromSqlRaw("select * from usuarios").Where(x => x.usuario == usuario.usuario).FirstOrDefaultAsync();
                if (UsuarioExistente == null)
                {
                    var contraseñahash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(usuario.contrasena));
                    usuario.contrasena = Convert.ToBase64String(contraseñahash);
                    var registrarUsuario = await context.Database.ExecuteSqlRawAsync("exec registarUsuario @nombreUsuario, @apellidoUsuario, @usuario, @contraseña, @idArea, @idSede, @celularUsuario, @idPerfil",
                    new SqlParameter("@nombreUsuario", usuario.nombreUsuario),
                    new SqlParameter("@apellidoUsuario", usuario.apellidoUsuario),
                    new SqlParameter("@usuario", usuario.usuario),
                    new SqlParameter("@contraseña", usuario.contrasena),
                    new SqlParameter("@idArea", usuario.idArea),
                    new SqlParameter("@idSede", usuario.idSede),
                    new SqlParameter("@celularUsuario", usuario.celularUsuario),
                    new SqlParameter("@idPerfil", usuario.idPerfil));
                    return Ok(registrarUsuario);
                }
                else
                {
                    return BadRequest("El usuario ya existe, no seas pendejo");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut]
        [Route("actualizarUsuario")]
        public async Task<ActionResult> putUsuario([FromBody] NewUsuario usuario)
        {
            try
            {
                var UsuarioExistente = await context.newUsuarios.FromSqlRaw("select * from usuarios").Where(x => x.usuario == usuario.usuario).FirstOrDefaultAsync();
                if (UsuarioExistente == null)
                {
                    var contraseñahash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(usuario.contrasena));
                    usuario.contrasena = Convert.ToBase64String(contraseñahash);
                    var actualizarUsuario = await context.Database.ExecuteSqlRawAsync("exec actualizarUsuario @id, @nombreUsuario, @apellidoUsuario, @usuario, @contraseña, @idArea, @idSede, @celularUsuario, @idPerfil",
                    new SqlParameter("@id", usuario.id),
                    new SqlParameter("@nombreUsuario", usuario.nombreUsuario),
                    new SqlParameter("@apellidoUsuario", usuario.apellidoUsuario),
                    new SqlParameter("@usuario", usuario.usuario),
                    new SqlParameter("@contraseña", usuario.contrasena),
                    new SqlParameter("@idArea", usuario.idArea),
                    new SqlParameter("@idSede", usuario.idSede),
                    new SqlParameter("@celularUsuario", usuario.celularUsuario),
                    new SqlParameter("@idPerfil", usuario.idPerfil));

                    return Ok(actualizarUsuario);
                }
                else
                {
                    return BadRequest("No puedes colocar el mismo nombre de usuario");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPut]
        [Route("actualizarContraseña")]
        public async Task<ActionResult> PutContraseña([FromBody] UsuarioPassword usuario)
        {
            try
            {
                var contrasenaActualHash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(usuario.contrasenaActual));
                var contrasenaActualBase64 = Convert.ToBase64String(contrasenaActualHash);

                var nuevaContrasenaHash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(usuario.contrasena));
                usuario.contrasena = Convert.ToBase64String(nuevaContrasenaHash);

                var mensaje = new SqlParameter
                {
                    ParameterName = "@mensaje",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Size = 100,
                    Direction = System.Data.ParameterDirection.Output
                };

                await context.Database.ExecuteSqlRawAsync("exec actualizarPassword @id, @contrasenaactual, @contrasena, @mensaje OUTPUT",
                    new SqlParameter("@id", usuario.id),
                    new SqlParameter("@contrasenaactual", contrasenaActualBase64),
                    new SqlParameter("@contrasena", usuario.contrasena),
                    mensaje);

                return Ok(mensaje.Value.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}