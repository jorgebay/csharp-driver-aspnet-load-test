using System;
using System.Linq;
using System.Configuration;
using Cassandra;

namespace Website
{
    public class Repository
    {
        private static ISession _session;
        private static ICluster _cluster;

        private static PreparedStatement _psInsert;
        private static PreparedStatement _psSelect;

        private static readonly Random _rnd = new Random();

        private static string[] _stringValues = { String.Join("", Enumerable.Repeat("a", 100)), String.Join("", Enumerable.Repeat("c", 2500)), String.Join("", Enumerable.Repeat("b", 1200)) };

        public static void Setup()
        {
            _cluster = Cluster.Builder()
                .AddContactPoint(ConfigurationManager.AppSettings["ContactPoint"] ?? "127.0.0.1")
                .Build();

            _session = _cluster.Connect();
            _session.Execute("DROP KEYSPACE IF EXISTS web_load_testing_1");
            _session.Execute("CREATE KEYSPACE web_load_testing_1 WITH replication = {'class': 'SimpleStrategy', 'replication_factor' : 3};");
            _session.ChangeKeyspace("web_load_testing_1");
            _session.Execute("CREATE TABLE tbl1 (id uuid primary key, val text)");
            _psInsert = _session.Prepare("INSERT INTO tbl1 (id, val) VALUES (?, ?)");
            _psSelect = _session.Prepare("SELECT id, val from tbl1 LIMIT 1");
        }

        public static void TearDown()
        {
            _cluster.Shutdown(2000);
        }

        public void Insert()
        {
            _session.Execute(_psInsert.Bind(Guid.NewGuid(), _stringValues[_rnd.Next(3)]));
        }

        public string Select()
        {
            string value = null;
            var row = _session.Execute(_psSelect.Bind()).FirstOrDefault();
            if (row != null)
            {
                value = row.GetValue<string>("val");
            }
            return value;
        }
    }
}