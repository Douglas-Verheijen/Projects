namespace Liquid.Services
{
    public interface IServiceRequest<TResponse>
        where TResponse : IServiceResponse
    {
    }
}
