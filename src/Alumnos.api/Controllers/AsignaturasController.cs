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
    public class AsignaturasController : _base.ControllerBase<Asignatura>
    {
        public AsignaturasController(AlexiaLightContext dbLightContext, IRepository<Asignatura> repository, IUnitOfWork unitOfWork, ILogger<Asignatura> logger) :base(dbLightContext, repository, unitOfWork, logger)
        {

        }
    }
}
