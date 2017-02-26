using Liquid.IoC;
using Liquid.Metadata;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Liquid.Library.UI.Controllers
{
    public class LiquidControllerFactory : DefaultControllerFactory, IControllerFactory
    {
        private readonly IEntityMetadataProvider _metadataProvider;

        public LiquidControllerFactory()
        {
            _metadataProvider = ConfigurationProvider.GetService<IEntityMetadataProvider>();
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            foreach (var type in _metadataProvider.GetEntityTypes())
                if (type.Name == controllerName)
                    return CreateController(requestContext, type);

            return base.CreateController(requestContext, controllerName);
        }

        public IController CreateController(RequestContext requestContext, Type entityType)
        {
            var genericType = typeof(EntityController<>);
            var controllerType = genericType.MakeGenericType(entityType);
            return ConfigurationProvider.GetService(controllerType) as IController;
        }
    }
}