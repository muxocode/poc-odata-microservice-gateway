using crossapp.rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace host.domain.rules.Alumno
{
    public class BirthJuniorRule : IRule<entities.Alumno>
    {
        public async Task<bool> Check(entities.Alumno obj)
        {
            if ((DateTime.Now - obj.FechaNacimiento).TotalDays > 365*18)
            {
                //TODO: Los textos deben ir por recursos
                throw new RuleException("El alumno ya está crecidito");
            }

            return true;
        }
    }
}
