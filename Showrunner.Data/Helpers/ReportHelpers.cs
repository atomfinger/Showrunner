using Showrunner.Data.Classes;
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
    }
}
