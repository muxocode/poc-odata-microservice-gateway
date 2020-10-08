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
    public class CentrosController : _base.ControllerBase<Centro>
    {
        public CentrosController(AlexiaLightContext dbLightContext, IRepository<Centro> repository, IUnitOfWork unitOfWork, ILogger<Centro> logger) :base(dbLightContext, repository, unitOfWork, logger)
        {

        }

        [HttpGet]
        [ODataRoute("Centros({key})/Alumnos")]
        public async Task<IActionResult> GetAlumnos([FromODataUri] Guid key)
        {
            try
            {
                var query = from c in this.DbLightContext.Centros
                            where c.Id == key
                            select c.Alumnos;

                return Ok(query);
            }
            catch (Exception oEx)
            {
                return SendError($"GET({key})/Alumnos", oEx);
            }
        }
    }
}
