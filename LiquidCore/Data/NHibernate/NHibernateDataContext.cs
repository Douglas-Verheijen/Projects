using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Liquid.Domain.Security;
using Liquid.IoC;
using Liquid.Metadata;
using NHibernate;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Liquid.Data.NHibernate
{
    [DefaultImplementation(typeof(IDataContext))]
    class NHibernateDataContext : IDataContext
    {
        private static ISessionFactory _factory;
        private static ISession _session;

        private ISessionFactory Factory
        {
            get
            {
                if (_factory == null)
                    _factory = CreateSessionFactory();
                return _factory;
            }
        }

        private IEnumerable<HbmMapping> GetMappings()
        {
            var persistentObjectType = typeof(IPersistentObject);
            var mapper = new NHibernateModelMapper();

            var metadataProvider = ConfigurationProvider.GetService<IEntityMetadataProvider>();
            var entityTypes = metadataProvider.GetEntityTypes();
            if (entityTypes.Any())
                yield return mapper.CompileMappingFor(entityTypes);
        }

        public virtual ISessionFactory CreateSessionFactory()
        {
            var directoryName = AppDomain.CurrentDomain.BaseDirectory;
            if (!(directoryName.EndsWith("\\bin") || directoryName.EndsWith("\\bin\\Debug")))
                directoryName += "\\bin";

            var configurationString = ConfigurationManager.ConnectionStrings["DefaultConnection"];
            if (configurationString == null)
                throw new Exception("Connections string 'DefaultConnection' has not been specified in the web.config.");

            var connectionString = MsSqlConfiguration.MsSql2012.ConnectionString(configurationString.ConnectionString);
            var database = Fluently.Configure().Database(connectionString);
            var configuration = database.Mappings(x =>
            {
                foreach (var assemblyFile in Directory.GetFiles(directoryName, "Liquid*.dll"))
                    x.FluentMappings.AddFromAssembly(Assembly.LoadFrom(assemblyFile));
            });

            return configuration.BuildSessionFactory();
        }

        public void Save(IPersistentObject obj)
        {
            if (_session != null)
                _session.SaveOrUpdate(obj);
        }

        public T Load<T>(object id)
            where T : IPersistentObject
        {
            if (_session != null)
                return _session.Load<T>(id);
            return default(T);
        }

        public void Delete<T>(T obj)
            where T : IPersistentObject
        {
            if (_session != null)
                _session.Delete(obj);
        }

        public DataContextTransaction BeginTransaction()
        {
            if (_session == null)
            {
                _session = Factory.OpenSession();
                _session.BeginTransaction();
                return new DataContextTransaction(this);
            }

            return null;
        }

        public void CommitChanges()
        {
            if (_session != null)
            {
                _session.Transaction.Commit();
                _session.Dispose();
                _session = null;
            }
        }

        public bool HasPendingChanges()
        {
            return _session != null && _session.IsDirty();
        }

        public IQueryable<T> Query<T>()
            where T : IPersistentObject
        {
            if (_session != null)
                return _session.Query<T>();
            return null;
        }

        public void InvalidateChanges()
        {
            _session.Dispose();
            _session = null;
        }
    }
}
