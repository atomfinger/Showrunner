using Showrunner.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Showrunner.Data.Classes
{
    public abstract class ShowApi
    {
        public abstract Task<Show> SearchAsync(string showName);
        public abstract Task<Show> FindShow(int id);
        public abstract Task<IEnumerable<Episode>> GetEpisodes(int showId);
        public abstract Task<IEnumerable<Episode>> GetScheduledEpisodes(DateTime date, string countryCode);
    }
}
