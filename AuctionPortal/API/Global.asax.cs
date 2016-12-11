using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Routing;
using BL.Bootstrap;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private readonly  IWindsorContainer _container = new WindsorContainer();
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            BootstrapContainer();
            MappingInit.ConfigureMapping();
        }

        private void BootstrapContainer()
        {
            _container.Install(new WebApiInstaller(), new BusinessLayerInstaller());
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator),new WindsorCompositionRoot(_container));
        }

        public override void Dispose()
        {
            _container.Dispose();
            base.Dispose();
        }
    }
}
