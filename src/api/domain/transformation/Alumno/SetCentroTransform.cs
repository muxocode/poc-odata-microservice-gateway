using crossapp.transformation;
using data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace host.domain.transformation.Alumno
{
    public class SetCentroTransform : ITransformation<entities.Alumno>
    {
        public SetCentroTransform(AlexiaContext context)
        {
            Context = context;
        }

        public AlexiaContext Context { get; }

        public async Task Do(entities.Alumno obj)
        {
            //Asignación del centro
            //Esto debería saberse mediante lo claims
            //Sería recomendable mapear IdCentro
            obj.Centro = this.Context.Centros.First();
        }
    }
}
