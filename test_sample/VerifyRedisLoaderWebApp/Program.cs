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
                     The example redis has following hash content:
                       HSET "yaml_config" "name" "redis tutorial"
                       HSET "yaml_config" "description" "redis basic commands for caching"
                       HSET "yaml_config" "likes" "20"
                       HSET "yaml_config" "visitors" "23000"
                       HSET "yaml_config" "test" "abc123"
                     */

                    builder.AddRedis(conn, redisHashKey, true, "aligala_curDbNum");
                })
                .UseStartup<Startup>();
    }
}
