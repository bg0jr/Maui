using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.ComponentModel.Composition.Hosting;
using MefContrib.Integration.Unity;
using System.ComponentModel.Composition.Primitives;
using System.Security;

namespace Maui.Tools.Studio
{
    public class IoCContainer
    {
        private AggregateCatalog myCatalog;

        /// <summary/>
        public IoCContainer()
        {
            Container = new UnityContainer();

            ConfigureMefSupport();
        }

        [SecuritySafeCritical]
        private void ConfigureMefSupport()
        {
            Container.EnableCompositionIntegration();

            myCatalog = new AggregateCatalog();
            Container.RegisterCatalog( myCatalog );
        }

        /// <summary/>
        public IUnityContainer Container
        {
            get;
            private set;
        }

        /// <summary/>
        public void AddPlugins( ComposablePartCatalog catalog )
        {
            myCatalog.Catalogs.Add( catalog );
        }

        /// <summary/>
        public void SetupDefaults()
        {
            RegisterDefaultImplementations();
            RegisterDefaultPlugins();
        }

        private void RegisterDefaultImplementations()
        {
        }

        private void RegisterDefaultPlugins()
        {
            var catalog = new AssemblyCatalog( typeof( IoCContainer ).Assembly );
            AddPlugins( catalog );
        }
    }
}
