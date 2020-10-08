using crossapp.rule;
using entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace host.domain.rules.Centro
{
    public class NameFillRule : IRule<entities.Centro>
    {
        public async Task<bool> Check(entities.Centro obj)
        {
            if(obj.Nombre==null || obj.Nombre == String.Empty)
            {
                throw new RuleException("El nombre dle centro debe tener valor");
            }

            return true;
        }
    }
}
