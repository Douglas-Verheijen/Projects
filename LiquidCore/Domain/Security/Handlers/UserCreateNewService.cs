using Liquid.Data;
using Liquid.IoC;
using Liquid.Services;
using System;

namespace Liquid.Domain.Security.Handlers
{
    public class UserCreateNewRequest : IServiceRequest<UserCreateNewResponse>
    {
        public User User { get; set; }
    }

    public class UserCreateNewResponse : IServiceResponse
    {
        public object Id { get; set; }
        public bool Success { get; set; }
    }

    [DefaultImplementation(typeof(IRequestHandler<UserCreateNewRequest, UserCreateNewResponse>))]
    class UserCreateNewRequestHandler : RequestHandler<UserCreateNewRequest, UserCreateNewResponse>
    {
        public UserCreateNewRequestHandler(IDataContext dataContext)
            : base(dataContext)
        {
        }

        protected override void BeginHandle(UserCreateNewRequest request, UserCreateNewResponse response)
        {
            base.BeginHandle(request, response);
            _changeTracker.TrackProperties(request.User);
        }

        protected override void HandleRequest(UserCreateNewRequest request, UserCreateNewResponse response)
        {
            var audit = new Liquid.Domain.Audit.AuditChange();
            audit.ModifiedOn = DateTimeOffset.Now;
            audit.NewValue = request.User.Username;

            _dataContext.Save(audit);
            _dataContext.Save(request.User);
            response.Id = request.User.Id;
            response.Success = true;
        }
    }
}
