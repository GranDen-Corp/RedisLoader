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
                       HSET "tutorialspoint" "name" "redis tutorial"
                       HSET "tutorialspoint" "description" "redis basic commands for caching"
                       HSET "tutorialspoint" "likes" "20"
                       HSET "tutorialspoint" "visitors" "23000"
                       HSET "tutorialspoint" "test" "abc123"
                     */

                    builder.AddRedis("localhost:6379", "tutorialspoint", true);
                })
                .UseStartup<Startup>();
    }
}
