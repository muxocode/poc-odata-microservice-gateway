using crossapp.rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace host.domain.rules
{
    public class RuleProcessor<T> : crossapp.rule.IRuleProcessor<T> where T : class
    {
        public IEnumerable<IRule<T>> Rules { get; set; }

        public RuleProcessor(IEnumerable<IRule<T>> rules)
        {
            this.Rules = rules;
        }

        public async Task<bool> CheckRules(T entity)
        {
            var bResult = true;
            if (this.Rules != null)
            {
                var aTasks = new List<Task<bool>>();
                var aErrors = new List<string>();

                foreach (var rule in Rules)
                {
                    aTasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            return await rule.Check(entity);
                        }
                        catch (RuleException oEx)
                        {
                            aErrors.Add(oEx.Message);
                            return false;
                        }
                    }));
                }

                var result = await Task.WhenAll(aTasks);

                if (aErrors.Any())
                {
                    var msg = String.Join(Environment.NewLine, aErrors);
                    throw new InvalidOperationException(msg);
                }


                foreach (var item in result)
                {
                    bResult &= item;
                }
            }
            return bResult;
        }
    }
}
