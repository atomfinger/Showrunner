﻿using Showrunner.Data.Classes;
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
            switch(type)
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
                        case DayOfWeek.Saturday:report.AppendLine($"{episode.Show.Title};;;;;;{info};"); break;
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

                    switch(type)
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
    }
}
