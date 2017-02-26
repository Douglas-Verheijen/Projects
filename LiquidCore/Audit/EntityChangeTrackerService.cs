using Liquid.Data;
using Liquid.IoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Liquid.Domain.Audit.Services
{
    public interface IEntityChangeTrackerService
    {
        void CreateRecords<TEntity>(TEntity entity, string actionName)
            where TEntity : Entity;


        void TrackProperties(Entity entity);
    }

    [DefaultImplementation(typeof(IEntityChangeTrackerService))]
    class EntityChangeTrackerService : IEntityChangeTrackerService
    {
        private static ICollection<KeyValuePair<Entity, KeyValuePair<string, object>>> _properties;
        private readonly IDataContext _dataContext;

        static EntityChangeTrackerService()
        {
            _properties = new Collection<KeyValuePair<Entity, KeyValuePair<string, object>>>();
        }

        public EntityChangeTrackerService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void CreateRecords<TEntity>(TEntity entity, string actionName)
            where TEntity : Entity
        {
            var type = entity.GetType();
            var originalEntity = ConfigurationProvider.GetService<TEntity>();
            var originalProperties = _properties.Where(x => x.Key == entity).Select(x => x.Value);

            if (actionName == "CreateNew")
            {
                foreach (var property in type.GetProperties())
                {
                    object newValue = property.GetValue(entity);
                    object originalValue = property.GetValue(originalEntity);
                    CreateAuditChangeRecord(entity, newValue, originalValue, property.Name, actionName);
                }
            }
            else
            {
                foreach (var property in type.GetProperties())
                {
                    object newValue = property.GetValue(entity);
                    object originalValue = originalProperties.FirstOrDefault(x => x.Key == property.Name).Value;
                    CreateAuditChangeRecord(entity, newValue, originalValue, property.Name, actionName);
                }
            }
        }

        private void CreateAuditChangeRecord<TEntity>(TEntity entity, object newValue, object originalValue, string propertyName, string actionName)
            where TEntity : Entity
        {
            if (newValue != null && !newValue.Equals(originalValue) || originalValue != null && !originalValue.Equals(newValue))
            {
                var auditChange = ConfigurationProvider.GetService<AuditChange>();
                auditChange.EntityId = entity.Id as Guid?;
                auditChange.EntityClrType = typeof(TEntity).FullName;
                auditChange.ActionName = actionName;
                auditChange.PropertyName = propertyName;
                auditChange.OriginalValue = originalValue != null ? originalValue.ToString() : null;
                auditChange.NewValue = newValue != null ? newValue.ToString() : null;
                //auditChange.ModifiedBy = _dataContext.Query<User>().FirstOrDefault();
                auditChange.ModifiedOn = DateTimeOffset.Now;

                //_dataContext.Save(auditChange);
            }
        }

        public void TrackProperties(Entity entity)
        {
            var type = entity.GetType();
            foreach (var property in type.GetProperties())
            {
                var value = property.GetValue(entity);
                var valuePair = new KeyValuePair<string, object>(property.Name, value);
                var propertyPair = new KeyValuePair<Entity, KeyValuePair<string, object>>(entity, valuePair);
                _properties.Add(propertyPair);
            }
        }
    }
}
