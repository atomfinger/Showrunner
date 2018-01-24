using Showrunner.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Showrunner.Data.Utils
{
    public static class FileImporterFactory
    {
        public static IFileImporter GetFileImporter(string FileType)
        {
            if (string.IsNullOrWhiteSpace(FileType))
                return null;

            if (FileType.ToLower() == ".csv")
                return new CsvImporter();

            throw new NotSupportedException();
        }
    }
}
