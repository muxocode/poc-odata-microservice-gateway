using crossapp.rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace host.domain.rules.Alumno
{
    public class NameLenghtRule : IRule<entities.Alumno>
    {
        public async Task<bool> Check(entities.Alumno obj)
        {
            if (obj.Nombre.Length < 5)
            {
                //TODO: Los textos deben ir por recursos
                throw new RuleException("El nombre debe medir más de 5");
            }

            return true;
        }
    }
}
