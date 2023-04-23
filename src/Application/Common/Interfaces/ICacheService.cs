namespace Application.Common.Interfaces;

public interface ICacheService
{
    Task<string> GetValueAsync(string key);
    Task<T> GetValueAsync<T>(string key, T value);
    Task SetValueAsync(string key, string value, TimeSpan? duration = null);
    Task SetValueAsync<T>(string key, T value, TimeSpan? duration = null);
    Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> action, TimeSpan? duration = null) where T : class;
    T GetOrAdd<T>(string key, Func<T> action, TimeSpan? duration = null) where T : class;
    bool TryGet<T>(string key, out T value) where T : class;
    Task RemoveAsync(string key);
    void RemoveAll();
    void RemoveByPattern(string pattern);
}
