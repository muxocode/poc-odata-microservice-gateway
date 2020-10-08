using crossapp.rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace host.domain.rules.Alumno
{
    public class NameFillRule : IRule<entities.Alumno>
    {
        public async Task<bool> Check(entities.Alumno obj)
        {
            if (obj.Nombre == null)
            {
                //TODO: Los textos deben ir por recursos
                throw new RuleException("El nombre no puede estar vacío");
            }

            return true;
        }
    }
}
