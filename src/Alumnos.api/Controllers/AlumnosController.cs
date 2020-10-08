using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using crossapp.repository;
using crossapp.unitOfWork;
using data;
using entities;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace host.Controllers
{
    public class AlumnosController : _base.ControllerBase<Alumno>
    {
        public AlumnosController(AlexiaLightContext dbLightContext, IRepository<Alumno> repository, IUnitOfWork unitOfWork, ILogger<Alumno> logger) :base(dbLightContext, repository, unitOfWork, logger)
        {

        }

        [HttpGet]
        [ODataRoute("Alumnos({key})/Centro")]
        public async Task<IActionResult> GetCentro([FromODataUri] Guid key)
        {
            try
            {
                /*
                  
                Esto no hace falta!!! :-)  
                 
                var query = from a in this.DbContext.Alumnos
                            join c in this.DbContext.Centros
                            on a.Centro.Id equals c.Id
                            where a.Id == key
                            select c;
                */

                var query = from a in this.DbLightContext.Alumnos
                            where a.Id == key
                            select a.Centro;

                return Ok(query);
            }
            catch (Exception oEx)
            {
                return SendError($"GET({key})/Centro", oEx);
            }
        }

        [HttpGet]
        [ODataRoute("Alumnos({key})/AlumnoAsignaturas")]
        public async Task<IActionResult> GetAlumnoAsignaturas([FromODataUri] Guid key)
        {
            try
            {
                /*
                 * Podemos pisar la query del modelo
                 * 
                 * Incluso haciendo bruto lo resuelve medianamente bien
                 * 
                 */


                var query = from a in this.DbLightContext.Alumnos
                            where a.Id == key
                            select a.AlumnoAsignaturas.Select(x=>x.Asignatura);

                return Ok(query);
            }
            catch (Exception oEx)
            {
                return SendError($"GET({key})/Centro", oEx);
            }
        }
    }
}
