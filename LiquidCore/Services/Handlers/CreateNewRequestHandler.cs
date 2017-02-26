using Liquid.Data;
using Liquid.Domain;
using Liquid.IoC;

namespace Liquid.Services.Handlers
{
    public class CreateNewRequest : IServiceRequest<CreateNewResponse>
    {
        public Entity Entity { get; set; }
    }

    public class CreateNewResponse : IServiceResponse
    {
        public object Id { get; set; }
        public bool Success { get; set; }
    }

    [DefaultImplementation(typeof(IRequestHandler<CreateNewRequest, CreateNewResponse>))]
    public class CreateNewRequestHandler : RequestHandler<CreateNewRequest, CreateNewResponse>
    {
        public CreateNewRequestHandler(IDataContext dataContext)
            : base(dataContext) { }

        protected override void HandleRequest(CreateNewRequest request, CreateNewResponse response)
        {
            _dataContext.Save(request.Entity);
            response.Id = request.Entity.Id;
            response.Success = true;
        }
    }
}
