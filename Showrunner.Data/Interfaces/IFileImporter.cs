using Showrunner.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Showrunner.Data.Interfaces
{
    public interface IFileImporter
    {
        IEnumerable<Show> DoImport(StreamReader reader);
    }
}
