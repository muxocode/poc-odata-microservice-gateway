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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace host.Controllers._base
{
    [Route("[controller]")]
    [EnableQuery()]
    public abstract class ControllerBase<T> : ODataController where T : entities._base.EntityBase
    {
        protected AlexiaLightContext DbLightContext { get; }
        protected IRepository<T> Repository { get; }
        protected IUnitOfWork UnitOfWork { get; }
        protected ILogger Logger { get; }

        protected ControllerBase(AlexiaLightContext dbLightContext, IRepository<T> repository,IUnitOfWork unitOfWork, ILogger<T> logger)
        {
            DbLightContext = dbLightContext;
            Repository = repository;
            UnitOfWork = unitOfWork;
            Logger = logger;
        }

        protected IActionResult SendError(string name, Exception ex)
        {
            var text = $"Error on {name} in {this.Request.Path}";
            this.Logger.LogError(text, ex);
            return BadRequest(text);
        }

        // GET
        [HttpGet]
        public virtual async Task<IActionResult> Get()
        {
            try
            {
                return Ok(this.DbLightContext.Set<T>());
            }
            catch (Exception oEx)
            {
                return SendError("GET", oEx);
            }
        }

        // GET/key
        [HttpGet]
        public virtual async Task<IActionResult> Get([FromODataUri] Guid key)
        {
            try
            {
                return Ok(this.DbLightContext.Find<T>(key));
            }
            catch (Exception oEx)
            {
                return SendError($"GET({key})", oEx);
            }
        }


        // POST
        [HttpPost]
        public virtual async Task<IActionResult> Post(T item)
        {
            try
            {
                IActionResult result;
                if (!ModelState.IsValid)
                {
                    result = BadRequest(ModelState);
                }
                else
                {
                    await this.Repository.Add(item);
                    await this.UnitOfWork.SaveChanges();


                    return Created(item);
                }

                return result;
            }
            catch (Exception oEx)
            {
                return SendError($"POST [{JsonSerializer.Serialize(item)}]", oEx);
            }
        }

        // PUT
        [HttpPut]
        public virtual async Task<IActionResult> Put([FromODataUri] Guid key, T item)
        {
            try
            {
                IActionResult result;
                if (!ModelState.IsValid)
                {
                    result = BadRequest(ModelState);
                }
                else
                {
                    if (!key.Equals(item.Id))
                    {
                        result = BadRequest();
                    }
                    else
                    {
                        await this.Repository.Update(item);
                        await this.UnitOfWork.SaveChanges();


                        result = Updated(item);
                    }
                }

                return result;
            }
            catch (Exception oEx)
            {
                return SendError($"PUT({key})[{JsonSerializer.Serialize(item)}]", oEx);
            }
        }

        //PATCH
        [HttpPatch]
        public virtual async Task<IActionResult> Patch([FromODataUri] Guid key, Delta<T> item)
        {
            try
            {
                IActionResult result;

                if (!ModelState.IsValid)
                {
                    result = BadRequest(ModelState);
                }
                else
                {
                    var entity = await this.Repository.Get(key);
                    if (entity == null)
                    {
                        result = NotFound();
                    }
                    else
                    {
                        item.Patch(entity);

                        await this.Repository.Update(entity);
                        await this.UnitOfWork.SaveChanges();

                        result = Updated(item);
                    }
                }

                return result;
            }
            catch (Exception oEx)
            {
                return SendError($"Patch({key})[{JsonSerializer.Serialize(item)}]", oEx);
            }
        }

        // DELETE
        [HttpDelete]
        public virtual async Task<IActionResult> Delete([FromODataUri] Guid key)
        {
            try
            {
                IActionResult result;

                var entity = await this.Repository.Get(key);
                if (entity == null)
                {
                    result = NotFound();
                }
                else
                {
                    await this.Repository.Remove(entity);
                    await this.UnitOfWork.SaveChanges();

                    result = StatusCode((int)System.Net.HttpStatusCode.NoContent);
                }

                return result;
                
            }
            catch (Exception oEx)
            {
                return SendError($"Delete({key})", oEx);
            }
        }
    }
}
