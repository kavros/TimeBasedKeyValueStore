# Time-Based Key-Value Store
The Time-Based Key-Value Store is a simple implementation of a key-value store where each value has an associated expiry time. If a value is queried after its expiry time, it is automatically removed from the store.


# Here’s how you can use the TimeBasedKeyValueStore:
```
using System;

namespace timebased_key_value_store
{
    class Program
    {
        static void Main()
        {
            var store = new TimeBasedKeyValueStore();

            // Add a value with a key, data, and expiry time in minutes
            store.Put("myKey", 42, 30);

            // Retrieve a value by key (within the specified expiry time)
            var retrievedValue = store.Get("myKey", 20);
            if (retrievedValue.HasValue)
            {
                Console.WriteLine($"Value for 'myKey': {retrievedValue}");
            }
            else
            {
                Console.WriteLine("Value not found or expired.");
            }

            // Calculate the average of non-expired values within a time window
            var average = store.Average(60);
            if (average.HasValue)
            {
                Console.WriteLine($"Average value: {average}");
            }
            else
            {
                Console.WriteLine("No valid values in the specified time window.");
            }
        }
    }
}
```