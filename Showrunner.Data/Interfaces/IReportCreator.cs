using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Showrunner.Data.Interfaces
{
    public interface IReportCreator
    {
        string GenerateReportCSV();
        string GenerateReportText();
    }
}
