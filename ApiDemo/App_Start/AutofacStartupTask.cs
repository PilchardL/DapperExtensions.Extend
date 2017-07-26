using Autofac;
using Autofac.Integration.WebApi;
using DapperExtensions.Extend;
using System.Web.Http;

namespace ApiDemo.App_Start
{
    public class AutofacStartupTask
    {
        public static void Execute(HttpConfiguration config)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly).PropertiesAutowired().InstancePerRequest();//注册API
            RegisterInterface(builder);
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        static void RegisterInterface(ContainerBuilder builder)
        {
            System.AppDomain.CurrentDomain.SetData("debug", true);
            builder.Register<IDapperContext>(c => new DapperContext("db")).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RespositoryBase<>)).As(typeof(IRespositoryBase<>));
        }
    }
}