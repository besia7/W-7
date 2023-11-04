using System;
using System.Collections.Generic;

public class FunctionCache<TKey, TResult>
{
    private Dictionary<TKey, CacheItem> cache = new Dictionary<TKey, CacheItem>();
    private TimeSpan cacheDuration;

    public FunctionCache(TimeSpan cacheDuration)
    {
        this.cacheDuration = cacheDuration;
    }

    public TResult GetOrAdd(TKey key, Func<TKey, TResult> func)
    {
        if (cache.TryGetValue(key, out CacheItem item) && !IsCacheItemExpired(item))
        {
            return item.Result;
        }
        else
        {
            TResult result = func(key);
            CacheItem newCacheItem = new CacheItem(result, DateTime.Now);
            cache[key] = newCacheItem;
            return result;
        }
    }

    private bool IsCacheItemExpired(CacheItem item)
    {
        return (DateTime.Now - item.Timestamp) > cacheDuration;
    }

    private class CacheItem
    {
        public TResult Result { get; }
        public DateTime Timestamp { get; }

        public CacheItem(TResult result, DateTime timestamp)
        {
            Result = result;
            Timestamp = timestamp;
        }
    }
}

class Program
{
    static void Main()
    {
        // Приклад використання дженеричного кешу з викликами користувацьких функцій

        FunctionCache<string, int> cache = new FunctionCache<string, int>(TimeSpan.FromSeconds(5));

        // Функція для обчислення довжини рядка (приклад користувацької функції)
        Func<string, int> stringLengthFunc = s => s.Length;

        string input = "Hello, World!";
        int length = cache.GetOrAdd(input, stringLengthFunc);
        Console.WriteLine($"Довжина рядка: {length}");

        // Знову викликаємо функцію з тим же ключем
        // Оскільки минуло менше 5 секунд, результат береться з кешу
        int cachedLength = cache.GetOrAdd(input, stringLengthFunc);
        Console.WriteLine($"Довжина рядка (з кешу): {cachedLength}");

        // Зачекайте 5 секунд, щоб закешований результат протух
        System.Threading.Thread.Sleep(5000);

        // Знову викликаємо функцію з тим же ключем
        // Оскільки пройшло більше 5 секунд, функція виконається знову
        int newLength = cache.GetOrAdd(input, stringLengthFunc);
        Console.WriteLine($"Довжина рядка (новий запит): {newLength}");
    }
}
