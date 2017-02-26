using Liquid.Data;
using Liquid.IoC;
using Liquid.Security.Authentication;
using Liquid.Security.Configuration;
using Liquid.Security.Domain;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Liquid.Security
{
    public class SecurityProvider : ConfigurationProvider
    {
        private const string _configurationFilename = "Environment.Unity.config";
        private static volatile IUnityContainer _configurationContainer;
        private static volatile AuthenticationSettings _settings;
        private static object _syncRoot = new object();

        public static AuthenticationMode Mode
        {
            get
            {
                if (_settings == null)
                {
                    lock (_syncRoot)
                    {
                        var dataContext = GetService<IDataContext>();
                        using (dataContext.BeginTransaction())
                            _settings = dataContext.Query<AuthenticationSettings>().FirstOrDefault();
                    }
                }

                return _settings.Mode;
            }
        }

        public static void Initialize()
        {
            lock (_syncRoot)
            {
                _configurationContainer = new UnityContainer();

                var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configurationFilename);
                var fileMap = new ExeConfigurationFileMap() { ExeConfigFilename = filename };
                var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                foreach (var section in configuration.Sections.OfType<UnityConfigurationSection>())
                    section.Configure(_configurationContainer);
            }
        }

        public static IFacebookAuthenticationSettings FacebookSettings
        {
            get
            {
                var authenticationSettings = _configurationContainer.Resolve<IFacebookAuthenticationSettings>();
                return authenticationSettings;
            }
        }

        public static ITwitterAuthenticationSettings TwitterSettings
        {
            get
            {
                var authenticationSettings = _configurationContainer.Resolve<ITwitterAuthenticationSettings>();
                return authenticationSettings;
            }
        }

        public static IGoogleAuthenticationSettings GoogleSettings
        {
            get
            {
                var authenticationSettings = _configurationContainer.Resolve<IGoogleAuthenticationSettings>();
                return authenticationSettings;
            }
        }

        public static IEmailConfiguration Email
        {
            get
            {
                var authenticationSettings = _configurationContainer.Resolve<IEmailConfiguration>();
                return authenticationSettings;
            }
        }
    }
}
