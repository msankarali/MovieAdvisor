using Application.Common.Interfaces;
using Application.Common.Models.Cache;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace Infrastructure.Services;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _database;
    private readonly RedisSettings _cacheSettings;
    private readonly ConnectionMultiplexer _connectionMultiplexer;

    public RedisCacheService(IConfiguration configuration)
    {
        _cacheSettings = configuration.GetSection(nameof(RedisSettings)).Get<RedisSettings>() ?? throw new NotImplementedException();
        _connectionMultiplexer = ConnectionMultiplexer.Connect($"{_cacheSettings.Host}:{_cacheSettings.Port},allowAdmin=true");
        _database = _connectionMultiplexer.GetDatabase();
    }

    public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> action, TimeSpan? duration) where T : class
    {
        var result = await _database.StringGetAsync(key);
        if (result.IsNull)
        {
            var data = await action();
            result = JsonConvert.SerializeObject(data);
            await SetValueAsync<T>(key, data, duration ?? TimeSpan.FromMinutes(1));
        }
        
        return JsonConvert.DeserializeObject<T>(result.ToString());
    }

    public async Task<string> GetValueAsync(string key)
    {
        return await _database.StringGetAsync(key);
    }

    public T GetOrAdd<T>(string key, Func<T> action, TimeSpan? duration) where T : class
    {
        var result = _database.StringGet(key);
        if (result.IsNull)
        {
            result = JsonConvert.SerializeObject(action());
            _database.StringSet(key, result, duration ?? TimeSpan.FromMinutes(30));
        }
        return JsonConvert.DeserializeObject<T>(result);
    }

    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }

    public void RemoveAll()
    {
        var endpoints = _connectionMultiplexer.GetEndPoints(true);
        foreach (var endpoint in endpoints)
        {
            var server = _connectionMultiplexer.GetServer(endpoint);
            server.FlushAllDatabases();
        }
    }

    public void RemoveByPattern(string pattern)
    {
        var keys = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First())
            .Keys(database: _database.Database, pattern: pattern + "*")
            .ToArray();
        _database.KeyDelete(keys);
    }

    public async Task<T> GetValueAsync<T>(string key, T value)
    {
        var result = await _database.StringGetAsync(key);
        return JsonConvert.DeserializeObject<T>(result);
    }

    public async Task SetValueAsync<T>(string key, T value, TimeSpan? duration = null)
    {
        var data = JsonConvert.SerializeObject(value);
        await _database.StringSetAsync(key, data, duration ?? TimeSpan.FromMinutes(30));
    }

    public async Task SetValueAsync(string key, string value, TimeSpan? duration = null)
    {
        await _database.StringSetAsync(key, value, duration ?? TimeSpan.FromMinutes(30));
    }

    public bool TryGet<T>(string key, out T value) where T : class
    {
        var result = _database.StringGet(key);

        if (!result.HasValue)
        {
            value = null;
            return false;
        }

        value = JsonConvert.DeserializeObject<T>(result);
        return true;
    }
}
