namespace AirportSimulation.ImportExport.Contracts
{
    internal interface IExcelWriter
    {
        void WriteSettingsToExcelFile<T>(T settings, string fileLocation, string fileName);
    }
}