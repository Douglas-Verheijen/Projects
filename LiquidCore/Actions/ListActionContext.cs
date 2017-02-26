using Liquid.Domain;
using Liquid.IoC;
using System.Collections.Generic;
using System.Linq;

namespace Liquid.Actions
{
    public interface IListActionContext : IFormActionContext
    {
        int Page { get; set; }
        int PageSize { get; set; }
        int Total { get; set; }
        bool Success { get; set; }

        ICollection<Entity> GetResults();
    }

    public interface IListActionContext<TEntity> : IFormActionContext<TEntity>, IListActionContext
        where TEntity : Entity
    {
        ICollection<TEntity> Results { get; set; }
    }

    [GenericImplementation(typeof(IListActionContext<>))]
    class ListActionContext<TEntity> : FormActionContext<TEntity>, IListActionContext<TEntity>
        where TEntity : Entity
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public bool Success { get; set; }

        public int Total { get; set; }

        public ICollection<TEntity> Results { get; set; }

        public ICollection<Entity> GetResults()
        {
            return Results.Cast<Entity>().ToList();
        }
    }
}
