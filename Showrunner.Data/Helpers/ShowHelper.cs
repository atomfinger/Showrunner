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
    public static class ShowHelper
    {
        public async static void UpdateShows(ShowrunnerDbContext context, Show[] shows, ShowApi api, CancellationToken token, IProgress<int> progress = null)
        {
            if (shows == null || shows.Count() <= 0 || api == null)
                return;

            var total = shows.Count();
            int current = 0;

            var genres = context.Genres.ToDictionary(s => s.Description);
            var networks = context.Networks.ToDictionary(s => s.ApiId);

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

                    if (apiResult.Network != null)
                    {
                        var network = apiResult.Network;
                        if (networks.ContainsKey(network.ApiId))
                        {
                            network = networks[network.ApiId];
                        }
                        else
                        {
                            networks[network.ApiId] = network;
                            context.Set<Network>().Add(network);
                        }

                        show.Network = network;
                    }

                    if (apiResult.Genres.Count > 0)
                    {
                        if (show.Genres.Count > 0)
                            show.Genres.Clear();

                        foreach (var iGenre in apiResult.Genres)
                        {
                            var genre = iGenre;
                            if (genres.ContainsKey(genre.Description))
                            {
                                genre = genres[genre.Description];
                            }
                            else
                            {
                                genres[genre.Description] = genre;
                                context.Set<Genre>().Add(genre);
                            }

                            show.Genres.Add(genre);
                        }
                    }

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

        public static IDictionary<DayOfWeek, IList<Episode>> GetNextWeekSchedule(string countryCode, ShowApi api)
        {
            var schedule = new Dictionary<DayOfWeek, IList<Episode>>();

            int diff = (7 + (DateTime.Today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var date = DateTime.Today.AddDays(7).AddDays(-1 * diff).Date;

            for (int i = 0; i < 7; i++)
            {
                schedule[date.DayOfWeek] = new List<Episode>(api.GetScheduledEpisodes(date, countryCode).GetAwaiter().GetResult());
                date = date.AddDays(1);
            }

            return schedule;
        }
    }
}
