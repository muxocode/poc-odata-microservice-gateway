using crossapp.rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace host.domain.rules.Centro
{
    public class NameLengthRule : IRule<entities.Centro>
    {
        public async Task<bool> Check(entities.Centro obj)
        {
            if (obj.Nombre?.Length<10)
            {
                throw new RuleException("El nombre del centro debe ser mayor que 10");
            }

            return true;
        }
    }
}
