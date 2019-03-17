namespace AirportSimulation.ImportExport.Services
{
    using Contracts;
    using OfficeOpenXml;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class ExcelWriter : IExcelWriter
    {
        public void WriteSettingsToExcelFile<T>(T settings, string fileLocation, string fileName)
        {
            using (var helper = new ExcelPackage())
            {
                var sheet = helper.Workbook.Worksheets.Add("SimulationSettings");

                var properties = settings
                    .GetType()
                    .GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                    .ToDictionary(prop => prop.Name, prop => prop.GetValue(settings, null));

                var keys = new List<string>();
                var values = new List<string>();

                foreach (KeyValuePair<string, object> kvp in properties)
                {
                    keys.Add(kvp.Key);
                    values.Add(kvp.Value.ToString());
                }

                PrintColumn(sheet, 1, 1, keys);
                PrintColumn(sheet, 2, 1, values);

                helper.SaveAs(new FileInfo(fileLocation + fileName));
            }
        }

        private void PrintColumn(ExcelWorksheet sheet, int column, int startRow, List<string> valuesToWrite)
        {
            for (int i = startRow; i < startRow + valuesToWrite.Count; i++)
            {
                sheet.Cells[i, column].Value = valuesToWrite[i - startRow];
            }
        }
    }
}
