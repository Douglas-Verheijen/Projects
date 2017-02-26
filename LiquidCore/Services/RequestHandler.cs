using Liquid.Data;
using Liquid.Domain.Audit.Services;
using Liquid.IoC;
using System;

namespace Liquid.Services
{
    public interface IRequestHandler<TServiceRequest, TServiceResponse>
        where TServiceRequest : IServiceRequest<TServiceResponse>
        where TServiceResponse : IServiceResponse
    {
        TServiceResponse Execute(TServiceRequest request);
    }

    public abstract class RequestHandler<TServiceRequest, TServiceResponse> : IRequestHandler<TServiceRequest, TServiceResponse>
        where TServiceRequest : IServiceRequest<TServiceResponse>
        where TServiceResponse : IServiceResponse
    {
        protected readonly IDataContext _dataContext;
        protected readonly IEntityChangeTrackerService _changeTracker;

        public RequestHandler(IDataContext dataContext)
        {
            _dataContext = dataContext;
            _changeTracker = ConfigurationProvider.GetService<IEntityChangeTrackerService>();
        }

        public TServiceResponse Execute(TServiceRequest request)
        {
            var response = default(TServiceResponse);

            try 
	        {
                response = ConfigurationProvider.GetService<TServiceResponse>();

                BeginHandle(request, response);

                HandleRequest(request, response);

                AfterHandle(request, response);
	        }
	        catch (Exception ex)
	        {
                if (!HandleException(request, response, ex))
                    throw;
	        }

            return response;
        }

        protected virtual void BeginHandle(TServiceRequest request, TServiceResponse response)
        {
            _dataContext.BeginTransaction();
        }

        protected abstract void HandleRequest(TServiceRequest request, TServiceResponse response);

        protected virtual void AfterHandle(TServiceRequest request, TServiceResponse response)
        {
            if (_dataContext.HasPendingChanges())
                _dataContext.CommitChanges();
        }

        protected virtual bool HandleException(TServiceRequest request, TServiceResponse response, Exception ex)
        {
            return false;
        }
    }
}
