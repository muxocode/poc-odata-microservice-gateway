using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlumnosService.entities;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace gateway.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GateWayController : ControllerBase
    {
        private readonly ILogger<GateWayController> _logger;

        public GateWayController(ILogger<GateWayController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Alumnos=$"{this.Request.GetDisplayUrl()}/Alumnos",
                Alumnos_BuscarPorNombre = $"{Request.GetDisplayUrl()}/Alumnos/<name>",
                AlumnosConCentro = $"{Request.GetDisplayUrl()}/AlumnosConCentro",
                Asignaturas = $"{Request.GetDisplayUrl()}/Asignaturas",
                Query = $"{Request.GetDisplayUrl()}/Query",
                FirstAlumnoCentro = $"{Request.GetDisplayUrl()}/FirstAlumnoCentro"
            });
        }

        [HttpGet("Alumnos")]
        public IActionResult GetAlumnos()
        {
            var uri = new Uri("https://localhost:44307/api");
            var context = new AlumnosService.Default.Container(uri);

            var query = context.Alumnos.Execute();

            return Ok(query);
        }

        [HttpGet("Alumnos/{name}")]
        public IActionResult GetPorNombre(string name)
        {
            var uri = new Uri("https://localhost:44307/api");
            var context = new AlumnosService.Default.Container(uri);

            var query = context.Alumnos
                .Where(x => x.Nombre.StartsWith(name));
      
            return Ok(query);
        }

        [HttpGet("AlumnosConCentro")]
        public IActionResult GetAlumnosConCentro()
        {
            var uri = new Uri("https://localhost:44307/api");
            var context = new AlumnosService.Default.Container(uri);

            var query = context.Alumnos
                .Expand(x=>x.Centro);
               
            return Ok(query);
        }


        [HttpGet("Asignaturas")]
        public IActionResult GetAsignaturas()
        {
            var uri = new Uri("https://localhost:44307/api");
            var context = new AlumnosService.Default.Container(uri);

            var query = context.Asignaturas.Execute();
               
            return Ok(query);
        }

        [HttpGet("FirstAlumnoCentro")]
        public IActionResult GetFirstAlumnoCentro()
        {
            var uri = new Uri("https://localhost:44307/api");
            var context = new AlumnosService.Default.Container(uri);

            var query = context.Alumnos.First();
               
            return Ok(query.Centro);
        }

        [HttpGet("Query")]
        public async Task<IActionResult> GetQuery()
        {
            var uri = new Uri("https://localhost:44307/api");
            var context = new AlumnosService.Default.Container(uri);

            var centros = context.Centros.ExecuteAsync();

            var Alumnos = context.Alumnos
                .Expand(x => x.Centro)
                .ExecuteAsync();



            var query = from a in await Alumnos
                        join c in await centros
                        on a.Centro.Id equals c.Id
                        select new
                        {
                            Alumno = $"{a.Nombre} {a.Apellido1} {a.Apellido2}",
                            Centro = c.Nombre
                        };

            return Ok(query);
        }
    }
}