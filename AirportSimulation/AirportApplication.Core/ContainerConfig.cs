namespace AirportSimulation.Core
{
    using Autofac;       
    using System.Reflection;
    using System;
    using Autofac.Core;
    using ImportExport;
    using NLog;
    using AirportSimulation.Core.Contracts;
    using Services;

    public static class ContainerConfig
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static IContainer _container;

        public static IContainer Configure()
        {
            Logger.Info("Building IoC and registering types.");

            var builder = new ContainerBuilder();

            // TODO: Register types according to their life time scope. 
            // This is just an example:
            var assembly = Assembly.GetCallingAssembly();
            builder.RegisterAssemblyTypes(assembly)
                   .Where(t => typeof(IService).IsAssignableFrom(t))
                   .SingleInstance()
                   .AsImplementedInterfaces();

            #region LocalRegistrations

            builder.RegisterType<TimerService>()
                .SingleInstance()
                .AsImplementedInterfaces();

            #endregion

            builder.RegisterModule<ImportExportModule>();

            _container = builder.Build();

            Logger.Info("IoC was built successfully.");

            return _container;
        }

        public static T Resolve<T>()
        {
            if (_container == null)
            {
                throw new Exception("Autofac hasn't been started!");
            }

            return _container.Resolve<T>(new Parameter[0]);
        }

        public static T Resolve<T>(Parameter[] parameters)
        {
            if (_container == null)
            {
                throw new Exception("Autofac hasn't been started!");
            }

            return _container.Resolve<T>(parameters);
        }

        public static void Stop()
        {
            _container?.Dispose();
        }
    }
}
