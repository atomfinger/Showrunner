using Showrunner.Data.Classes;
using Showrunner.Data.DatabaseConnection;
using Showrunner.Data.Enums;
using Showrunner.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Showrunner.Data.Helpers
{
    public static class ReportHelpers
    {
        public static string GetNextWeekScheduleReport(ReportType type, ShowApi api, string countryCode = "US")
        {
            var result = ShowHelper.GetNextWeekSchedule(countryCode, api);
            switch (type)
            {
                case ReportType.CSV: return GetNextWeekScheduleReportCsv(result);
                case ReportType.Text: return GetNextWeekScheduleReportTxt(result);
            }

            throw new NotSupportedException();
        }

        private static string GetNextWeekScheduleReportCsv(IDictionary<DayOfWeek, IList<Episode>> dataSource)
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine("SHOW_NAME;MONDAY;TUESDAY;WEDENSDAY;THURSDAY;FRIDAY;SATURDAY;SUNDAY");

            foreach (var kv in dataSource)
            {
                foreach (var episode in kv.Value)
                {
                    var info = episode.GetSeasonAndEpisodeText();
                    switch (kv.Key)
                    {
                        case DayOfWeek.Monday: report.AppendLine($"{episode.Show.Title};{info};;;;;;"); break;
                        case DayOfWeek.Tuesday: report.AppendLine($"{episode.Show.Title};;{info};;;;;"); break;
                        case DayOfWeek.Wednesday: report.AppendLine($"{episode.Show.Title};;;{info};;;;"); break;
                        case DayOfWeek.Thursday: report.AppendLine($"{episode.Show.Title};;;;{info};;;"); break;
                        case DayOfWeek.Friday: report.AppendLine($"{episode.Show.Title};;;;;{info};;"); break;
                        case DayOfWeek.Saturday: report.AppendLine($"{episode.Show.Title};;;;;;{info};"); break;
                        case DayOfWeek.Sunday: report.AppendLine($"{episode.Show.Title};;;;;;;{info}"); break;
                    }
                }
            }

            return report.ToString();
        }

        private static string GetNextWeekScheduleReportTxt(IDictionary<DayOfWeek, IList<Episode>> dataSource)
        {
            StringBuilder report = new StringBuilder();

            foreach (var kv in dataSource)
            {
                report.AppendLine($"-----------{kv.Key.ToString()}-----------");

                foreach (var episode in kv.Value)
                    report.AppendLine($"{episode.Show.Title} ({episode.GetSeasonAndEpisodeText()})");

                report.AppendLine();
            }

            return report.ToString();
        }

        public static string TopTenShowsReport(ReportType type)
        {
            using (var context = DbContextFactory.GetDbContext())
            {
                var shows = context.Shows.OrderByDescending(s => s.Rating).Where(s => s.Rating.HasValue).Take(10);
                switch (type)
                {
                    case ReportType.CSV: return TopTenShowsReportCsv(shows);
                    case ReportType.Text: return TopTenShowsReportText(shows);
                }

                throw new NotSupportedException();
            }
        }

        private static string TopTenShowsReportCsv(IEnumerable<Show> shows)
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine("SHOW_NAME;RATING");
            foreach (var show in shows)
                report.AppendLine($"{show.Title};{Math.Round(show.Rating.Value, 1).ToString()}");
            return report.ToString();
        }

        private static string TopTenShowsReportText(IEnumerable<Show> shows)
        {
            StringBuilder report = new StringBuilder();
            foreach (var show in shows)
                report.AppendLine($"{Math.Round(show.Rating.Value, 1).ToString()} - {show.Title}");

            if (shows.Count() == 0)
                report.AppendLine("No shows has ratings");

            return report.ToString();
        }

        public static string TopNetworksReport(ReportType type)
        {
            StringBuilder report = new StringBuilder();
            using (var context = DbContextFactory.GetDbContext())
            {
                var networkInfos = context.Networks.Select(n =>
                new
                {
                    Network = n,
                    AverageRating = n.Shows.Where(s => s.Rating.HasValue).Average(s => s.Rating),
                    TopShow = n.Shows.Where(s => s.Rating.HasValue).OrderByDescending(s => s.Rating).FirstOrDefault(),
                    ShowCount = n.Shows.Count(),
                });

                if (type == ReportType.CSV)
                    report.AppendLine("AVERAGE_RATING;NETWORK;TOP_RATED_SHOW;TOP_RATING;SHOW_COUNT");

                foreach (var info in networkInfos.OrderByDescending(s => s.AverageRating))
                {
                    if (info.TopShow == null || !info.AverageRating.HasValue)
                        continue;

                    switch (type)
                    {
                        case ReportType.CSV:
                            report.AppendLine(TopNetworksReportCsv(info.Network, info.AverageRating, info.TopShow, info.ShowCount));
                            break;
                        case ReportType.Text:
                            report.AppendLine(TopNetworksReportText(info.Network, info.AverageRating, info.TopShow, info.ShowCount));
                            break;
                    }
                }

                return report.ToString();
            }
        }

        private static string TopNetworksReportCsv(Network network, decimal? rating, Show topShow, int showCount)
        {
            return $"{Math.Round(rating.Value, 1).ToString()};{network.Name};{topShow.Title};{Math.Round(topShow.Rating.Value, 1).ToString()};{showCount}";
        }

        private static string TopNetworksReportText(Network network, decimal? rating, Show topShow, int showCount)
        {
            return $"{network.Name} (Avg: {Math.Round(rating.Value, 1).ToString()}) - TOP: {topShow.Title} ({Math.Round(topShow.Rating.Value, 1).ToString()}) - TOTAL: {showCount}";
        }

        public static string ShowReport(ReportType type)
        {
            StringBuilder report = new StringBuilder();

            using (var context = DbContextFactory.GetDbContext())
            {
                var showInfos = context.Shows.Select(s =>
                    new
                    {
                        Show = s,
                        EpisodeCount = s.Episodes.Count,
                        ReleasedEpisodeCount = s.Episodes.Count(e => e.AirDate > DateTime.Today),
                        Seasons = s.Episodes.Where(e => e.Season.HasValue).Max(n => n.Season),
                        s.Network,
                        s.Genres,
                    });

                if (type == ReportType.CSV)
                    report.AppendLine("SHOW_NAME;NETWORK;GENRES;EPISODE_COUNT;RELEASED_EPISODE_COUNT");

                foreach (var info in showInfos)
                {
                    var genres = GetGenreText(info.Genres);

                    switch (type)
                    {
                        case ReportType.CSV:
                            report.AppendLine(ShowReportCsv(info.Show, info.EpisodeCount, info.ReleasedEpisodeCount, info.Seasons, genres));
                            break;
                        case ReportType.Text:
                            report.AppendLine(ShowReportText(info.Show, info.EpisodeCount, info.ReleasedEpisodeCount, info.Seasons, genres));
                            break;
                    }
                }

                return report.ToString();
            }
        }

        private static string ShowReportCsv(Show show, int episodeCount, int releasedEpisodeCount, int? seasons, string genres)
        {
            return $"{show.Title};{show.Network?.Name ?? "Unkown"};{genres};{seasons?.ToString() ?? "Unkown"};{episodeCount}{releasedEpisodeCount}";
        }

        private static string ShowReportText(Show show, int episodeCount, int releasedEpisodeCount, int? seasons, string genres)
        {
            return $"{show.Title} ({show.Network?.Name ?? "Unkown"}) Genres: {genres} Seasons: {seasons?.ToString() ?? "Unkown"} Episodes: {episodeCount} (Relased: {releasedEpisodeCount})";
        }

        public static string RecommendedShowsReport(Genre[] genres, ReportType type)
        {
            StringBuilder report = new StringBuilder();
            var filterGenres = genres.Select(s => s.Description).ToList();
            using (var context = DbContextFactory.GetDbContext())
            {
                var shows = context.Shows.OrderByDescending(s => s.Rating)
                    .Select(s =>
                    new
                    {
                        s.Title,
                        s.Rating,
                        s.Genres,
                        s.Summary,
                        s.ImbdId,
                    });

                if (type == ReportType.CSV)
                    report.AppendLine("SHOW_NAME;RATING;GENRES;SUMMARY;IMDB_LINK");

                int showsRecommended = 0;
                int totalShowsToRecommend = 15;
                foreach (var show in shows)
                {
                    var hasGenres = true;
                    foreach (var filterGenre in filterGenres)
                    {
                        if (show.Genres.Any(s => s.Description == filterGenre))
                            continue;

                        hasGenres = false;
                        break;
                    }

                    if (!hasGenres)
                        continue;

                    var genreText = GetGenreText(show.Genres);

                    var imbdUrl = $@"http://www.imdb.com/title/{show.ImbdId}";
                    switch (type)
                    {
                        case ReportType.CSV:
                            report.AppendLine($"{show.Title};{Math.Round(show.Rating.Value, 1).ToString()};{genreText};{show.Summary};{imbdUrl}");
                            break;
                        case ReportType.Text:
                            report.AppendLine($"{show.Title} ({Math.Round(show.Rating.Value, 1).ToString()}) - {genreText} - {imbdUrl}{Environment.NewLine}{show.Summary}");
                            break;
                    }

                    showsRecommended++;
                    if (showsRecommended > totalShowsToRecommend)
                        break;
                }
            }

            return report.ToString();
        }

        private static string GetGenreText(IEnumerable<Genre> genres)
        {
            var genreText = "Unkown";
            if (genres != null && genres.Count() > 0)
                genreText = string.Join(", ", genres.Select(s => s.Description));
            return genreText;
        }
    }
}
