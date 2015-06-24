using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLT;
using RethinkDb;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading;

namespace ConsumerLT
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseFactory.InitDatabase();

            ScanForChanges(8);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static void ScanForChanges(int n)
        {
            var threads = new List<Thread>();
            for (int i = 0; i < n; i++)
            {
                threads.Add(new Thread(ScanOrganizations));
            }

            threads.ForEach(t =>
            {
                t.IsBackground = true;
                t.Start();
            });
        }

        static void ScanOrganizations()
        {
            while (true)
            {
                using (var rcon = DatabaseFactory.Connect())
                {
                    rcon.QueryTimeout = TimeSpan.FromMinutes(10);

                    if (rcon.Run(DatabaseFactory.Organizations.Select(o => o.Id).Take(1).Count()) > 0)
                    {
                        foreach (var org in rcon.Run(DatabaseFactory.Organizations.Take(1000)))
                        {
                            var st = Stopwatch.StartNew();
                            var latestUsers = rcon.Run(DatabaseFactory.Users.GetAll(org.Id, User.IdxOrg).Count());
                            st.Stop();

                            //Console.WriteLine("Org {0} total users {1}, query time: {2}ms", org.Id, latestUsers, st.Elapsed.TotalMilliseconds);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No organizations found");
                    }
                }

                Thread.Sleep(1000);
            }
        }
    }
}
