using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Npgsql;
using BryceFamily.Repo.PG.Config;
using System.Data;

namespace BryceFamily.Repo.PG
{
    public class ConnectionResolver
    {
        private readonly DataEnvironment _dataEnvironment;

        public ConnectionResolver(DataEnvironment dataEnvironment)
        {
            _dataEnvironment = dataEnvironment;
        }

        public IDbConnection GetConnection()
        {
            var connectionString = _dataEnvironment.Value == "local" ?
                "Host=192.168.99.100;User Id=brycefamily;Password=Yv8%_]x!P`q}H4P?;Database=brycefamily" : "";
            return new NpgsqlConnection(connectionString);

        }
    }
}
