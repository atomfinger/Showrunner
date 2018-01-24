using Showrunner.Data.Interfaces;
using Showrunner.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Showrunner.Data.Utils
{
    public class CsvImporter : IFileImporter
    {
        public IEnumerable<Show> DoImport(StreamReader reader)
        {
            var showNames = new List<string>();
            bool isFirst = true;
            while(!reader.EndOfStream)
            {
                if (isFirst)
                {
                    isFirst = false;
                    continue;
                }

                var line = reader.ReadLine();
                var values = line.Split(',');
                if (values.Length < 6)
                    continue;
                showNames.Add(values[5]);
            }

            var shows = new List<Show>();
            foreach (var showName in showNames)
                shows.Add(new Show() { Title = showName });

            return shows;
        }
    }
}
