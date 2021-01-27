using System;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace GranDen.Configuration.RedisLoader
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a Redis database as an <see cref="T:Microsoft.Extensions.Configuration.IConfigurationSource" />.
    /// </summary>
    public class RedisConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// The redis instance.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Determines whether the source will be loaded if the underlying configuration changes.
        /// </summary>
        public bool ReloadOnChange { get; set; }

        /// <summary>
        /// The factory of <see cref="IConnectionMultiplexer"/> for database access.
        /// </summary>
        public Func<IConnectionMultiplexer> ConnectionFactory { get; set; }
        
        /// <summary>
        /// For specified the Redis key-value string which store the current DB number that has valid Game Table Data
        /// </summary>
        public string CurrentRedisDbNumKey { get; set; }

        /// <summary>
        /// Builds the <see cref="RedisConfigurationSource"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>A <see cref="RedisConfigurationProvider"/></returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new RedisConfigurationProvider(ConnectionFactory, Key, ReloadOnChange, CurrentRedisDbNumKey);
        }
    }
}