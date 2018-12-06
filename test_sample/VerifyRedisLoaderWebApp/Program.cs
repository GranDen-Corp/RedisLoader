using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GranDen.RedisLoader;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace VerifyRedisLoaderWebApp
{
    public class Program
    {
        private const string conn =
            @"localhost:6379";

        private const string redisHashKey = "yaml_config";

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    /*
                     * Note: be sure to prepare a development redis server and load test data from the script file "db.test_content.redis"
                     */
                    builder.AddRedis(conn, redisHashKey, true, "aligala_curDbNum");
                })
                .UseStartup<Startup>();
    }
}
