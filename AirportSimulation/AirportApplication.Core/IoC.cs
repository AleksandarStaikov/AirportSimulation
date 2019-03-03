namespace AirportApplication.Core
{
    using Autofac;       
    using AirportApplication.Core.Services.Interfaces;
    using System.Reflection;
    using System;
    using Autofac.Core;

    public static class IoC
    {
        public static IContainer Container { get; private set; }

        public static void Start()
        {
            var builder = new ContainerBuilder();
            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly)
                   .Where(t => typeof(IService).IsAssignableFrom(t))
                   .SingleInstance()
                   .AsImplementedInterfaces();

            Container = builder.Build();
        }

        public static T Resolve<T>()
        {
            if (Container == null)
            {
                throw new Exception("Autofac hasn't been started!");
            }

            return Container.Resolve<T>(new Parameter[0]);
        }

        public static void Stop()
        {
            Container.Dispose();
        }
    }
}
