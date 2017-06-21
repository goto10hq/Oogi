using System;
using System.IO;
using Newtonsoft.Json;
using Oogi.Tokens;

namespace Oogi.Tests
{
    public static class Connection
    {
        public static Oogi.Connection Instance { get; }

        static Connection()
        {            
            var config = new FileInfo("connection.json");            

            if (!config.Exists)
            {
                throw new Exception($"Connection configuration not found in {config}");
            }

            var connectionString = JsonConvert.DeserializeObject<ConnectionString>(File.ReadAllText(config.FullName));

            Instance = new Oogi.Connection(connectionString);
        }
    }
}
