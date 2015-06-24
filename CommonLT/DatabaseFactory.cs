using RethinkDb;
using RethinkDb.ConnectionFactories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLT
{
    public static class DatabaseFactory
    {
        public static string ClusterName = "rdb";
        public static string DatabaseName = "test";

        public static IConnectionFactory ConnectionFactory = RethinkDb.Newtonsoft
                               .Configuration.ConfigurationAssembler
                               .CreateConnectionFactory(ClusterName);

        public static IDatabaseQuery Database = Query.Db(DatabaseName);

        public static ITableQuery<User> Users = Database.Table<User>(User.GetTableName());

        public static ITableQuery<Organization> Organizations = Database.Table<Organization>(Organization.GetTableName());

        public static IConnection Connect()
        {
            return new ReliableConnectionFactory(ConnectionFactory).Get();
        }

        public static void InitDatabase()
        {
            using (var rcon = Connect())
            {
                // db
                if (!rcon.Run(Query.DbList()).Contains(DatabaseName))
                {
                    rcon.Run(Query.DbCreate(DatabaseName));
                }

                // Users
                if (!rcon.Run(Database.TableList()).Contains(User.GetTableName()))
                {
                    rcon.Run(Database.TableCreate(User.GetTableName()));                 
                }

                // Users indexes
                var userIdxs = rcon.Run(Users.IndexList());
                if (!userIdxs.Contains(User.IdxOrg))
                {
                    rcon.Run(Users.IndexCreate(User.IdxOrg, u => u.IdOrganization));
                    rcon.Run(Users.IndexWait(User.IdxOrg));
                }

                // Organization
                if (!rcon.Run(Database.TableList()).Contains(Organization.GetTableName()))
                {
                    rcon.Run(Database.TableCreate(Organization.GetTableName()));
                }

                // Org indexes
                if (!rcon.Run(Organizations.IndexList()).Contains(Organization.IdxTimestamp))
                {
                    rcon.Run(Organizations.IndexCreate(Organization.IdxTimestamp, u => u.Timestamp));
                    rcon.Run(Organizations.IndexWait(Organization.IdxTimestamp));
                }
            }

            Console.WriteLine("Database has been initialized");
        }
    }
}
