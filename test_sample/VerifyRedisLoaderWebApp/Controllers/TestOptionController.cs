using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GranDen.Configuration.RedisLoader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VerifyRedisLoaderWebApp.TypedOptions;
using webapi.Models.PocketHiGameDb.YamlDb;

namespace VerifyRedisLoaderWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestOptionController : ControllerBase
    {
        private ServiceProvider _container;
        private IConfigurationSection _stickerRewardSection;
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
            
            //configBuilder.AddRedis("aligala-gametable-stage.redis.cache.windows.net:6380,password=4cRFQqlw38TZ0oz7ABDBk9lfmkgWieZKWeu5jchSzkY=,ssl=True,abortConnect=False",
                //"aligala_game_table", true, "aligala_curDbNum");

            return configBuilder.Build();
        }

        private ServiceProvider CreateTypedOptionContainer(IConfiguration configuration)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.Configure<MissionMainDataOption>(configuration.GetSection("Mission_MainData"));
            //serviceCollection.Configure<List<StickerRewardDataYamlEntity>>(configuration.GetSection("Sticker_Reward_Data"));

            _stickerRewardSection = configuration.GetSection("Sticker_Reward_Data");

            return serviceCollection.BuildServiceProvider();
        }

        [Route("tableData")]
        public IActionResult GetTableData()
        {
            //var missionDataOption = _container.GetService<IOptionsMonitor<MissionMainDataOption>>();
            //var mData = missionDataOption.CurrentValue;

            //var stickerRewardDataOption = _container.GetService<IOptionsMonitor<List<StickerRewardDataYamlEntity>>>();
            //var stickerData = stickerRewardDataOption.CurrentValue;

            var reward004Section = _stickerRewardSection.GetSection("Sticker_Page_Reward_004");

            var stickerData = new StickerRewardDataYamlEntity();

            reward004Section.Bind(stickerData);

            return Ok(new
            {
                //missionMainData = mData,
                //missionRarity = _missionRarity,
                //missionTimeOption = _missionTimeOption,
                stickerRewardData = stickerData
            });
        }


    }
}