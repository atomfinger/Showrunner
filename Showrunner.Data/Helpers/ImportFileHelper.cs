using Showrunner.Data.DatabaseConnection;
using Showrunner.Data.Models;
using Showrunner.Data.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Showrunner.Data.Helpers
{
    public static class ImportFileHelper
    {
        public static IEnumerable<Show> Import(string filePath)
        {
            var shows = ImportFile(filePath);
            var savedShows = new List<Show>();       

            using (var context = DbContextFactory.GetDbContext())
            {
                var showTitles = context.Shows.Select(s => s.Title).ToList();

                foreach (var show in shows)
                {
                    if (showTitles.Contains(show.Title))
                        continue;

                    context.Shows.Add(show);
                    savedShows.Add(show);
                }

                context.SaveChanges();
            }

            return savedShows;
        }

        private static IEnumerable<Show> ImportFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            var fileImporter = FileImporterFactory.GetFileImporter(Path.GetExtension(filePath));

            using (var reader = new StreamReader(filePath))
            {
                return fileImporter.DoImport(reader);
            }
        }
    }
}
