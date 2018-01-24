using Showrunner.Data.Classes;
using Showrunner.Data.DatabaseConnection;
using Showrunner.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Showrunner.Data.Helpers
{
    public static class ApiHelper
    {
        public async static void UpdateShows(Show[] shows, ShowApi api, CancellationToken token, IProgress<int> progress = null)
        {
            if (shows == null || shows.Count() <= 0 || api == null)
                return;

            var total = shows.Count();
            int current = 0;

            using (var context = DbContextFactory.GetDbContext())
            {
                foreach (var show in shows)
                {
                    if (token.IsCancellationRequested)
                        return;

                    context.Shows.Attach(show);
                    Show apiResult = null;

                    if (show.ApiId.HasValue)
                        apiResult = await api.FindShow(show.ApiId.Value);
                    else
                        apiResult = await api.SearchAsync(show.Title);

                    if (apiResult != null)
                    {
                        show.CopyValues(apiResult);
                        var episodes = api.GetEpisodes(show.ApiId.Value).GetAwaiter().GetResult();

                        if (episodes.Any())
                            context.Episodes.RemoveRange(context.Episodes.Where(s => s.ShowId == show.Oid));

                        foreach (var episode in episodes)
                            episode.Show = show;

                        context.Episodes.AddRange(episodes);
                    }

                    current++;
                    if (progress != null)
                        progress.Report(current * 100 / total);
                }

                if (token.IsCancellationRequested)
                    return;

                await context.SaveChangesAsync();

                if (progress != null)
                    progress.Report(100);
            }
        }
    }
}
