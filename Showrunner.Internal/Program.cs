using Showrunner.Data.DatabaseConnection;
using Showrunner.Data.Helpers;
using Showrunner.Data.Models;
using Showrunner.Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Showrunner.Internal
{
    class Program
    {
        static void Main(string[] args)
        {
            Data.DatabaseConnection.DbContextFactory.SetConnectionProvider(new SqlConnectionProvider());
            Data.Helpers.ImportFileHelper.Import(@"E:\PayEx\shows.csv");

            using (var context = DbContextFactory.GetDbContext())
            {
                ApiHelper.UpdateShows(context.Shows.ToArray(), new TvmazeApi(), CancellationToken.None, new ProgressReport());
            }

            Console.ReadLine();
        }

        public class ProgressReport : IProgress<int>
        {
            public void Report(int value)
            {
                Console.WriteLine($"{value}%");
            }
        }
    }
}
