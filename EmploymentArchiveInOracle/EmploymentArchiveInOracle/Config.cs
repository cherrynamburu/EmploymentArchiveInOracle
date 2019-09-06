
using System.Configuration;

namespace EmploymentArchiveInOracle
{
    internal static class Config
    {
        internal static string ConnectionString;

        static Config()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnectionString"].ConnectionString;
        }
    }
}
