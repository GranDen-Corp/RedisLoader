using System;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace GranDen.Configuration.RedisLoader
{
    /// <summary>
    /// Extension methods for adding <see cref="RedisConfigurationProvider"/>.
    /// </summary>
    public static class RedisConfigurationExtensions
    {
        /// <summary>
        /// Adds the Redis configuration provider to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="configuration">The configuration used to connect to Redis.</param>
        /// <param name="key">The Redis key used for configuration.</param>
        /// <param name="reloadOnChange">Determines whether the source will be loaded if the underlying configuration changes.
        /// Requires the Redis Keyspace Notifications feature enabled on the Redis server. 
        /// <code>CONFIG SET notify-keyspace-events Kh</code>. 
        /// See http://redis.io/topics/notifications for details. 
        /// </param>
        /// <param name="currentRedisDbNumKey">Specified the key that store Redis Db Number that current valid Game Table Data resided.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddRedis(this IConfigurationBuilder builder, string configuration, string key, bool reloadOnChange = false, string currentRedisDbNumKey = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (string.IsNullOrEmpty(configuration))
            {
                throw new ArgumentException("Redis configuration must be a non-empty string.", nameof(configuration));
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Redis hash key must be a non-empty string.", nameof(key));
            }

            return builder.AddRedis(() => ConnectionMultiplexer.Connect(configuration), key, reloadOnChange, currentRedisDbNumKey);
        }

        private static IConfigurationBuilder AddRedis(this IConfigurationBuilder builder, Func<IConnectionMultiplexer> connectionFactory, string key, bool reloadOnChange, string currentRedisDbNumKey)
        {
            var source = new RedisConfigurationSource()
            {
                Key = key,
                ReloadOnChange = reloadOnChange,
                ConnectionFactory = connectionFactory,
                CurrentRedisDbNumKey = currentRedisDbNumKey
            };

            builder.Add(source);
            return builder;
        }
    }
}