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
        private static ShowrunnerDbContext context;
        static void Main(string[] args)
        {
            Data.DatabaseConnection.DbContextFactory.SetConnectionProvider(new SqlConnectionProvider());
            Data.Helpers.ImportFileHelper.Import(@"E:\PayEx\shows.csv");

            context = DbContextFactory.GetDbContext();
            ApiHelper.UpdateShows(context, context.Shows.ToArray(), new TvmazeApi(), CancellationToken.None, new ProgressReport());

            Console.ReadLine();

            context.Dispose();
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
