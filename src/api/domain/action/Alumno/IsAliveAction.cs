using data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace host.domain.action.Alumno
{
    public class IsAliveAction : crossapp.action.IUpdateAction<entities.Alumno>
    {
        public AlexiaLightContext Context { get; }

        public IsAliveAction(AlexiaLightContext context)
        {
            Context = context;
        }

        public Action Create(entities.Alumno obj)
        {
            return () =>
            {
                //Esto es un ejemplo de control de concurrencia
                if (this.Context.Alumnos.Find(obj.Id).FechaBaja != null)
                {
                    throw new InvalidOperationException("El alumno ya está dado de baja, no se puede modificar");
                }
            };
        }
    }
}
