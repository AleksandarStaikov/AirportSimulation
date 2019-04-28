namespace AirportSimulation.Core
{
    using Autofac;
    using System.Reflection;
    using System;
    using Autofac.Core;
    using ImportExport;
    using NLog;
    using AirportSimulation.Core.Contracts;
    using LinkNodes;
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

            #region LocalRegistrations

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => typeof(IService).IsAssignableFrom(t))
                .SingleInstance()
                .AsImplementedInterfaces();

            builder.RegisterType<Engine>()
                .InstancePerDependency()
                .AsImplementedInterfaces();

            builder.RegisterType<ChainLinkFactory>()
                .SingleInstance()
                .AsImplementedInterfaces();

            builder.RegisterType<TimerService>()
                .SingleInstance()
                .AsImplementedInterfaces();

            builder.RegisterType<CheckInDesk>();
            builder.RegisterType<Psc>();
            builder.RegisterType<Asc>();
            builder.RegisterType<Mpa>();
            builder.RegisterType<BSU>();
            builder.RegisterType<Aa>();
            builder.RegisterType<OneToOneConveyor>();
            builder.RegisterType<CheckInDispatcher>();
            builder.RegisterType<BagCollector>();

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
