using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace HotelLibrary.Databases
{
    public class SqliteDataAccess : ISqliteDataAccess 
    {
        private readonly IConfiguration config;

        public SqliteDataAccess(IConfiguration config)
        {
            this.config = config;
        }

        public List<T> LoadData<T, U>(string sqliteStatement,
                                      U parameters,
                                      string connectionStringName)
        {
            string connectionString = config.GetConnectionString(connectionStringName);


            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqliteStatement, parameters).ToList();
                return rows; 
            }
        }

        public void SaveData<T>(string sqliteStatement,
                                T parameters,
                                string connectionStringName)
        {
            string connectionString = config.GetConnectionString(connectionStringName);


            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Execute(sqliteStatement, parameters);
            }
        }
    }
}
