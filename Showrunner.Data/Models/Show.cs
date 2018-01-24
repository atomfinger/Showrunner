using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Showrunner.Data.Models
{
    public class Show
    {
        public int Oid { get; set; }
        public string Title { get; set; }
        public int? ApiId { get; set; }
        public decimal? Rating { get; set; }
        public DateTime? Premiered { get; set; }

        public void CopyValues(Show toCopy)
        {
            ApiId = toCopy.ApiId;
            Premiered = toCopy.Premiered;
            Rating = toCopy.Rating;
            Title = toCopy.Title;
        }
    }
}
