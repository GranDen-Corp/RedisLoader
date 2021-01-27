using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace GranDen.Configuration.RedisLoader
{
    /// <inheritdoc />
    /// <summary>
    /// A Redis based <see cref="T:Microsoft.Extensions.Configuration.ConfigurationProvider" />.
    /// </summary>
    public class RedisConfigurationProvider : ConfigurationProvider
    {
        private readonly Func<IConnectionMultiplexer> _connectionFactory;
        private readonly string _key;

        private IConnectionMultiplexer _connection;
        private IDatabase _database;
        private int _dbNum;
        private readonly string _currentRedisDbNumKey;


        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public RedisConfigurationProvider(Func<IConnectionMultiplexer> connectionFactory, string key, bool reloadOnChange, string currentRedisDbNumKey = null)
        {
            _currentRedisDbNumKey = currentRedisDbNumKey;
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _key = key ?? throw new ArgumentNullException(nameof(key));

            if (reloadOnChange)
            {
                Connect();

                var subscriber = _connection.GetSubscriber();
                var topic = $"__keyspace@{_database.Database}__:{_key}";
                subscriber.Subscribe(topic, (channel, value) =>
                {
                    Reload();
                });
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Loads the configuration data from the redis store.
        /// </summary>
        public override void Load()
        {
            Connect();

            var data = GetTableData();

            Data = data;
        }

        private Dictionary<string, string> GetTableData()
        {
            var hash = _database.HashGetAll(_key);
            var data = new Dictionary<string, string>(hash.Length, StringComparer.OrdinalIgnoreCase);
            foreach (var entry in hash)
            {
                data.Add(entry.Name, entry.Value);
            }

            return data;
        }

        private void Reload()
        {
            Load();
            OnReload();
        }

        private void Connect()
        {
            if (_connection == null)
            {
                _connection = _connectionFactory();

                if (!string.IsNullOrEmpty(_currentRedisDbNumKey))
                {
                    var keyDb = _connection.GetDatabase();
                    if (keyDb.KeyExists(_currentRedisDbNumKey))
                    {
                        _dbNum = int.Parse(keyDb.StringGet(_currentRedisDbNumKey));
                    }
                }

                _database = _connection.GetDatabase(_dbNum);
            }
        }
    }
}