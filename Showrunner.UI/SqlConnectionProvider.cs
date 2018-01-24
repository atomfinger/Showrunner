using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Showrunner.UI
{
    public class SqlConnectionProvider : Data.Interfaces.IDbConnectionProvider
    {
        public DbConnection GetDbConnection()
        {
            var connectionString = $@"Data Source=DESKTOP-GV01MCK; Initial Catalog=ShowRunner;Integrated Security=SSPI;";
            return new SqlConnection(connectionString);
        }
    }
}
