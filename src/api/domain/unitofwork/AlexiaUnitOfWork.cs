using data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Transactions;

namespace host.domain.unitofwork
{
    public class AlexiaUnitOfWork : crossapp.unitOfWork.IUnitOfWork
    {
        AlexiaContext Context { get; set; }
        List<Action> Actions { get; }
        public AlexiaUnitOfWork(AlexiaContext context)
        {
            this.Context = context;
            this.Actions = new List<Action>();
        }

        public void Dispose()
        {
            this.Context = null;
        }

        public async Task SaveChanges()
        {
            using (var scope = new TransactionScope())
            {
                    this.Actions.ForEach(x => x());

                    this.Context.SaveChanges();

                    // Commit transaction if all commands succeed, transaction will auto-rollback
                    // when disposed if either commands fails
                    scope.Complete();
            }
        }

        public async Task AddPendingAction(Action action)
        {
            this.Actions.Add(action);
        }
    }
}
