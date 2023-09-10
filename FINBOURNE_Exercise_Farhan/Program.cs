using FINBOURNE_Exercise_Farhan;

class Program
{
    static void Main(string[] args)
    {
        var cache = new Cache<int>(10, Cache<int>.EvictionPolicy.LeastRecentlyUsed);

        cache.Add(1, "One");
        cache.Add(2, "Two");
        cache.Add(3, "Three");

        if (cache.TryGet("One", out int value))
        {
            Console.WriteLine("The value of key 'One' is: {0}", value);
        }
        else
        {
            Console.WriteLine("Key 'One' not found in the cache.");
        }

        Console.WriteLine("The cache is full: {0}", cache.IsFull());

        cache.Remove("One");


        if (cache.TryGet("One", out int removedValue))
        {
            Console.WriteLine("The value of key 'One' is: {0}", removedValue);
        }
        else
        {
            Console.WriteLine("Key 'One' not found in the cache.");
        }

        Console.ReadLine();
    }
}