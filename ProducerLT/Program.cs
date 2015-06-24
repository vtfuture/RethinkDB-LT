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

namespace ProducerLT
{
    class Program
    {
        static bool saveJsonToDisk = false;

        static void Main(string[] args)
        {
            Console.WriteLine("ProducerLT started");

            DatabaseFactory.InitDatabase();
            Console.WriteLine("Initialize database done");

            Run10KThreads(8);

            Console.ReadKey();
        }

        static void Run10KThreads(int n)
        {
            var threads = new List<Thread>();
            for (int i = 0; i < n; i++)
            {
                threads.Add(new Thread(InsertOrgWithUsers10K));
            }

            threads.ForEach(t =>
            {
                t.IsBackground = true;
                t.Start();
            });
        }

        static void InsertOrgWithUsers10K()
        {
            var gen = new ContentGenerator();

            var batch = Guid.NewGuid().ToString();

            var org = new Organization
            {
                AcceptedDomainList = gen.GetAddress(),
                AuthenticationData = gen.GetTwitter(),
                Description = gen.GetArticle(),
                DocumentsTotalSize = 1024 * 1024 * 12,
                Email = gen.GetEmail(),
                Enabled = true,
                Guid = Guid.NewGuid().ToString(),
                ImagesTotalSize = 1024 * 1024 * 24,
                ImportData = gen.GetTwitter(),
                IsOwner = true,
                IsSuspended = false,
                Name = gen.GetName(),
                SuspendedMessage = gen.GetArticle(),
                Timestamp = DateTime.UtcNow,
                VatId = gen.GetPhone(),
                VideosTotalSize = 1024 * 1024 * 1024,
            };

            using(var rcon = DatabaseFactory.Connect())
            {
                rcon.QueryTimeout = TimeSpan.FromMinutes(10);

                // insert org
                var orgResp = rcon.Run(DatabaseFactory.Organizations.Insert(org));
                var idOrg = orgResp.GeneratedKeys[0];

                Console.WriteLine("{0} Organization saved, id {1}", batch, idOrg);

                // gen users
                var newUsers = new List<User>();
                for (int i = 0; i < 10000; i++)
                {
                    newUsers.Add(new User
                    {
                        IdOrganization = idOrg,
                        Biography = gen.GetArticle(),
                        Birthdate = DateTime.UtcNow.AddYears(-32),
                        Birthyear = DateTime.UtcNow.AddYears(-32),
                        DepartmentName = gen.GetAddress(),
                        DisplayEmail = gen.GetEmail(),
                        Email = gen.GetEmail(),
                        Enabled = true,
                        IdentityToken = Guid.NewGuid().ToString(),
                        ImportData = gen.GetPhone(),
                        IsOwner = false,
                        LastLoginDate = DateTime.UtcNow,
                        Name = gen.GetName(),
                        PhoneNumber = gen.GetPhone(),
                        Position = gen.GetEmail(),
                        Responsibilities = gen.GetAddress(),
                        Timestamp = DateTime.UtcNow,
                        Timezone = gen.GetAddress(),
                        Workplace = gen.GetAddress(),
                    });
                }

                // save to disk
                if (saveJsonToDisk)
                {
                    var dir = AppDomain.CurrentDomain.BaseDirectory;
                    var jsonFile = Path.Combine(dir, "users-" + batch + ".json");
                    File.WriteAllText(jsonFile, JsonConvert.SerializeObject(newUsers));
                    Console.WriteLine("{0} 10K batch saved to disk, size {1}kb", batch, new FileInfo(jsonFile).Length / 1024);
                }

                var st = Stopwatch.StartNew();

                // batch insert 200 records
                newUsers.Split(200).ForEach(l =>
                {
                    rcon.Run(DatabaseFactory.Users.Insert(l));
                });
                
                st.Stop();

                Console.WriteLine("{0} 10K batch saved to db, time {1}ms", batch, st.Elapsed.TotalMilliseconds);
            }
        }
    }
}
