using Liquid.Actions;
using Liquid.Data;
using Liquid.IoC;

namespace Liquid.Services.Handlers
{
    public class EntityActionProcessRequest : IServiceRequest<EntityActionProcessResponse>
    {
        public IFormActionContext ActionContext { get; set; }
    }

    public class EntityActionProcessResponse : IServiceResponse
    {
        public object Id { get; set; }
        public bool Success { get; set; }
    }

    [DefaultImplementation(typeof(IRequestHandler<EntityActionProcessRequest, EntityActionProcessResponse>))]
    public class EntityActionProcessRequestHandler : RequestHandler<EntityActionProcessRequest, EntityActionProcessResponse>
    {
        public EntityActionProcessRequestHandler(IDataContext dataContext)
            : base(dataContext) { }

        protected override void HandleRequest(EntityActionProcessRequest request, EntityActionProcessResponse response)
        {
            var actionContext = request.ActionContext;
            actionContext.Process();

            response.Id = actionContext.GetEntity().Id;
            response.Success = !actionContext.HasErrors();
        }
    }
}
