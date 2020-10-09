using crossapp.action;
using crossapp.rule;
using crossapp.transformation;
using crossapp.unitOfWork;
using data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace host.domain.repostory._base
{
    public class RepositoryGeneric<T> : crossapp.repository.IRepository<T> where T : entities._base.EntityBase
    {
        public AlexiaContext Context { get; set; }
        public IRuleProcessor<T> RuleProcessor { get; }
        public IEnumerable<IInsertAction<T>> InsertActions { get; }
        public IEnumerable<IUpdateAction<T>> UpdateActions { get; }
        public IEnumerable<IDeleteAction<T>> DeleteActions { get; }
        public IEnumerable<ITransformation<T>> Transformations { get; }
        public IUnitOfWork UnitOfWork { get; }

        public RepositoryGeneric(
            AlexiaContext context, 
            IRuleProcessor<T> ruleProcessor, 
            IEnumerable<IInsertAction<T>> insertActions, 
            IEnumerable<IUpdateAction<T>> updateActions, 
            IEnumerable<IDeleteAction<T>> deleteActions,
            IEnumerable<ITransformation<T>> transformations,
            IUnitOfWork unitOfWork
            )
        {
            Context = context;
            RuleProcessor = ruleProcessor;
            InsertActions = insertActions;
            UpdateActions = updateActions;
            DeleteActions = deleteActions;
            Transformations = transformations;
            UnitOfWork = unitOfWork;
        }

        public async Task Add(T item)
        {
            await this.RuleProcessor.CheckRules(item);

            var aTask = new List<Task>();

            this.InsertActions
                .Select(x=>x.Create(item))
                .ToList()
                .ForEach(x=> aTask.Add(this.UnitOfWork.AddDBAction(x)));

            this.Transformations.ToList().ForEach(x => aTask.Add(x.Do(item)));

            Task.WaitAll(aTask.ToArray());

            item.Id = Guid.NewGuid();

            this.Context.Set<T>().Add(item);
        }

        public void Dispose()
        {
            this.Context = null;
        }

        public async Task<T> Get<TKey>(TKey key)
        {
            return this.Context.Set<T>().Find(key);
        }

        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> expression = null)
        {
            var query = this.Context.Set<T>() as IEnumerable<T>;

            if (expression != null)
            {
                query = query.Where(expression.Compile());                   
            }

            return query;
        }

        public async Task Remove(T obj)
        {
            this.DeleteActions
                .Select(x => x.Create(obj))
                .ToList()
                .ForEach(x => this.UnitOfWork.AddDBAction(x));

            this.Context.Set<T>().Remove(obj);
        }

        public async Task Update(T obj)
        {
            await this.RuleProcessor.CheckRules(obj);

            var aTask = new List<Task>();


            this.UpdateActions
                .Select(x => x.Create(obj))
                .ToList()
                .ForEach(x => aTask.Add(this.UnitOfWork.AddDBAction(x)));

            this.Transformations.ToList().ForEach(x => aTask.Add(x.Do(obj)));

            Task.WaitAll(aTask.ToArray());

            this.Context.Set<T>().Update(obj);
        }
    }
}
