namespace AirportSimulation.ImportExport.Services
{
    using Contracts;
    using OfficeOpenXml;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class ExcelReader : IExcelReader
    {
        public T ParseExcelFileToObject<T>(string fileLocation, string fileName)
        {
            T settings = Activator.CreateInstance<T>();

            using (var helper = new ExcelPackage(new FileInfo(fileLocation + fileName)))
            {
                var sheet = helper.Workbook.Worksheets["SimulationSettings"];

                var keys = ReadColumn(sheet, 1);
                var values = ReadColumn(sheet, 2);

                for (int i = 0; i < keys.Count; i++)
                {
                    var key = keys[i];
                    var property = settings.GetType().GetProperty(key);
                    property.SetValue(settings, Convert.ChangeType(values[i], property.PropertyType));
                }
            }

            return settings;
        }

        private List<string> ReadColumn(ExcelWorksheet sheet, int col)
        {
            var columnValues = new List<string>();

            for (int row = sheet.Dimension.Start.Row; row <= sheet.Dimension.End.Row; row++)
            {
                columnValues.Add(sheet.Cells[row, col].Value.ToString());
            }

            return columnValues;
        }
    }
}
