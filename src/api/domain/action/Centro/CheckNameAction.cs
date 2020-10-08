using data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace host.domain.action.Centro
{
    public class CheckNameAction : crossapp.action.IUpdateAction<entities.Centro>, crossapp.action.IInsertAction<entities.Centro>
    {
        public AlexiaLightContext Context { get; }

        public CheckNameAction(AlexiaLightContext context)
        {
            Context = context;
        }

        public Action Create(entities.Centro obj)
        {
            return () =>
            {
                //Esto es un ejemplo de control de concurrencia
                if (this.Context.Centros.Where(x=>x.Nombre.ToUpper()==obj.Nombre.ToUpper()).Any())
                {
                    throw new InvalidOperationException("Ya existe un centro con ese nombre");
                }
            };
        }
    }
}
