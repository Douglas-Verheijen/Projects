using Liquid.Data;
using Liquid.IoC;
using Liquid.Library.Domain.Inventory;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace Liquid.Library.Api
{
    [ServiceContract]
    public interface IBookApiService
    {
        [OperationContract]
        bool CreateNew(Book book);

        [OperationContract]
        ICollection<Book> List();
    }

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BookApiService : IBookApiService
    {
        public bool CreateNew(Book book)
        {
            return false;
        }

        public ICollection<Book> List()
        {
            var dataContext = ConfigurationProvider.GetService<IDataContext>();
            var query = dataContext.Query<Book>();
            return query != null ? query.ToList() : new List<Book>();
        }
    }
}
