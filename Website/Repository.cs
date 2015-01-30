using System;
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
            _psSelect = _session.Prepare("SELECT id, val from tbl1 LIMIT 10");
        }

        public static void TearDown()
        {
            _cluster.Shutdown(2000);
        }

        public void Insert()
        {
            _session.Execute(_psInsert.Bind(Guid.NewGuid(), DateTime.Now.ToLongTimeString()));
        }

        public RowSet Select()
        {
            return _session.Execute(_psSelect.Bind());
        }
    }
}