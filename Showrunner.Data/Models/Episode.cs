using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Showrunner.Data.Models
{
    public class Episode
    {
        public int Oid { get; set; }
        public Show Show { get; set; }
        public int ShowId { get; set; }
        public int? ApiId { get; set; }
        public string Title { get; set; }
        public int? Season { get; set; }
        public DateTime? AirDate { get; set; }
        public string Summary { get; set; }
        public int? Number { get; set; }

        public string GetSeasonAndEpisodeText()
        {
            string seasonNumber = "-";
            string episodeNumber = "-";

            if (Season.HasValue)
                seasonNumber = Season < 10 ? $"0{Season}" : Season.ToString();

            if (Number.HasValue)
                episodeNumber = Number < 10 ? $"0{Number}" : Number.ToString();

            return $"S{seasonNumber}E{episodeNumber}";
        }
    }
}
