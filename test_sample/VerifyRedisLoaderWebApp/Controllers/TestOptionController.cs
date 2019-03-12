using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GranDen.RedisLoader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VerifyRedisLoaderWebApp.TypedOptions;

namespace VerifyRedisLoaderWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestOptionController : ControllerBase
    {
        private ServiceProvider _container;

        private readonly MissionRarityOption _missionRarity;
        private readonly MissionTimeOption _missionTimeOption;

        public TestOptionController(
            IOptionsSnapshot<MissionRarityOption> missionRarityOptionAccessor,
            IOptionsSnapshot<MissionTimeOption> missionTimeOptionAccessor
            )
        {
            _container = CreateTypedOptionContainer(CreateRedisConfiguration());


            _missionRarity = missionRarityOptionAccessor.Value;
            _missionTimeOption = missionTimeOptionAccessor.Value;
        }

        private static IConfiguration CreateRedisConfiguration()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddRedis("localhost:6379", "yaml_config", true, "aligala_curDbNum");
            return configBuilder.Build();
        }

        private ServiceProvider CreateTypedOptionContainer(IConfiguration configuration)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.Configure<MissionMainDataOption>(configuration.GetSection("Mission_MainData"));
            return serviceCollection.BuildServiceProvider();
        }

        [Route("tableData")]
        public IActionResult GetTableData()
        {
            var option = _container.GetService<IOptionsMonitor<MissionMainDataOption>>();
            var mData = option.CurrentValue;

            return Ok(new
            {
                missionMainData = mData,
                missionRarity = _missionRarity,
                missionTimeOption = _missionTimeOption
            });
        }


    }
}