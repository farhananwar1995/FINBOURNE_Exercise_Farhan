
namespace FINBOURNE_Exercise_Farhan;

public class CacheItem<T>
{
    public DateTime DateTime { get; set; }
    public T Item { get; set; }
    public string Key { get; set; }
}

public class Cache<T> : IDisposable
{
    private readonly Dictionary<string, CacheItem<T>> _items = new Dictionary<string, CacheItem<T>>();
    private readonly int _maxItems;
    private readonly EvictionPolicy _evictionPolicy;
    private readonly Dictionary<string, CacheItem<T>> _evictedItems = new Dictionary<string, CacheItem<T>>();

    public Cache(int maxItems, EvictionPolicy evictionPolicy)
    {
        _maxItems = maxItems;
        _evictionPolicy = evictionPolicy;
    }

    public void Add(T item, string key)
    {
        if (_items.Count >= _maxItems)
        {
            EvictItem();
        }

        var cacheItem = new CacheItem<T>();
        cacheItem.DateTime = DateTime.Now;
        cacheItem.Item = item;
        cacheItem.Key = key;
        _items[key] = cacheItem;
    }

    public bool TryGet(string key, out T value)
    {
        value = default(T); 

        if (_items.ContainsKey(key))
        {
            value = _items[key].Item;
            return true; // Key found
        }
        else
        {
            return false; // Key not found
        }
    }

    public void Remove(string key)
    {
        _items.Remove(key);
    }

    public bool IsFull()
    {
        return _items.Count >= _maxItems;
    }

    public event EventHandler<ItemEvictedEventArgs> ItemEvicted;

    protected virtual void OnItemEvicted(ItemEvictedEventArgs e)
    {
        ItemEvicted?.Invoke(this, e);
    }

    private void EvictItem()
    {
        var leastRecentlyUsedItem = _items.Values.OrderBy(x => x.DateTime).First();
        _evictedItems[leastRecentlyUsedItem.Key] = leastRecentlyUsedItem;
        _items.Remove(leastRecentlyUsedItem.Key);
        OnItemEvicted(new ItemEvictedEventArgs(leastRecentlyUsedItem));
    }

    public void Dispose()
    {
        _items.Clear();
        _evictedItems.Clear();
    }

    public enum EvictionPolicy
    {
        LeastRecentlyUsed,
        FirstInFirstOut
    }

    public class ItemEvictedEventArgs : EventArgs
    {
        public readonly CacheItem<T> Item;

        public ItemEvictedEventArgs(CacheItem<T> item)
        {
            Item = item;
        }
    }
}