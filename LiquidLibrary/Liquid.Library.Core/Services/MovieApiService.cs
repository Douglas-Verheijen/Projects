using Liquid.Actions;
using Liquid.Data;
using Liquid.IoC;
using Liquid.Library.Domain.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace Liquid.Library.Api
{
    [ServiceContract]
    public interface IMovieApiService
    {
        [OperationContract]
        void CreateNew(Movie movie);

        [OperationContract]
        ICollection<Movie> List();
    }

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class MovieApiService : IMovieApiService
    {
        public void CreateNew(Movie movie)
        {
            var actionContext = ConfigurationProvider.GetService<ICreateNewActionContext<Movie>>();
            actionContext.Entity = movie;
            actionContext.Process();

            if (actionContext.Errors.Any())
                throw new Exception(string.Join("\n", actionContext.Errors));
        }

        public ICollection<Movie> List()
        {
            var dataContext = ConfigurationProvider.GetService<IDataContext>();
            var query = dataContext.Query<Movie>();
            return query != null ? query.ToList() : new List<Movie>();
        }
    }
}
