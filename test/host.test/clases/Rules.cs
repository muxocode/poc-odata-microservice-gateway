using crossapp.rule;
using host.domain.rules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace host.test.clases
{
    public class CorrectRule : IRule<TestUser>
    {
        public async Task<bool> Check(TestUser obj)
        {
            return await Task.Run(() => true);
        }
    }

    public class WrongRule : IRule<TestUser>
    {
        public async Task<bool> Check(TestUser obj)
        {
            return await Task.Run(() => false);
        }
    }

    public class ExceptionRule : IRule<TestUser>
    {
        public async Task<bool> Check(TestUser obj)
        {
            return await Task.Run(() =>
            {
                throw new RuleException("Error provocado");
                return false;
            }
            );
        }
    }
}
