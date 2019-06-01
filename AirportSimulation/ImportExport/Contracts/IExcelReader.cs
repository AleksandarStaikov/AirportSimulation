namespace AirportSimulation.ImportExport.Contracts
{
    public interface IExcelReader
    {
        T ParseExcelFileToObject<T>(string fileLocation, string fileName);
    }
}