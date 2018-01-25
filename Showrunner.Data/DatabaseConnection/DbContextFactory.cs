using Showrunner.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Showrunner.Data.DatabaseConnection
{
    public static class DbContextFactory
    {
        private static IDbConnectionProvider _provider;

        public static void SetConnectionProvider(IDbConnectionProvider provider)
        {
            _provider = provider;
        }

        public static ShowrunnerDbContext GetDbContext()
        {
            if (_provider == null)
                throw new NullReferenceException("Connection provider not set");
            return new ShowrunnerDbContext(_provider.GetDbConnection());
        }
    }
}
