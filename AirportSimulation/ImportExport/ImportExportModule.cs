namespace AirportSimulation.ImportExport
{
    using Autofac;
    using Contracts;
    using Services;

    public class ImportExportModule : Module 
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ExcelReader>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            builder.RegisterType<ExcelWriter>()
                .AsImplementedInterfaces()
                .InstancePerDependency();
        }
    }
}
