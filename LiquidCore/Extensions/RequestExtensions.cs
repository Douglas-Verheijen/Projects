using Liquid.IoC;
using Liquid.Services;

namespace Liquid
{
    public static class RequestExtensions
    {
        public static TResponse Handle<TResponse>(this IServiceRequest<TResponse> request)
            where TResponse : IServiceResponse
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var handler = ConfigurationProvider.GetService(handlerType.FullName);
            return (TResponse)handlerType.GetMethod("Execute").Invoke(handler, new object[] { request });
        }
    }
}
