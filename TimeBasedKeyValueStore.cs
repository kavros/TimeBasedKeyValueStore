using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timebased_key_value_store
{
    public class Value
    {
        public DateTime ExpiryTime { get; set; }
        public ExpiryTimeListItem PointerToList {  get; set; }
        public int Data { get; set; }
    }
    public class ExpiryTimeListItem
    {
        public string Key { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
    public class TimeBasedKeyValueStore
    {
        private readonly int _windowInMins = 60;
        private readonly DateTime _intialTime = DateTime.UtcNow;
        private readonly Dictionary<string, Value> _dict = new();
        private int _sum = 0;
        private int _count = 0;
        private LinkedList<ExpiryTimeListItem> _list = new ();

        public TimeBasedKeyValueStore() { }

        public void Put(string key, int value, int mins)
        {
            var expiryTime = _intialTime.AddMinutes(_windowInMins + mins);
            var expiryTimeListItem = new ExpiryTimeListItem() 
            { 
                ExpiryTime = expiryTime,
                Key = key
            };
            var hashTableValue = new Value() { 
                Data = value,
                PointerToList = expiryTimeListItem,
                ExpiryTime = expiryTime
            };

            if (_dict.ContainsKey(key))
            {
                _sum -= _dict[key].Data;
                _list.Remove(_dict[key].PointerToList);
            }
            else
            {                
                _count++;
            }
            // add expiry time to list           
            _list.AddLast(expiryTimeListItem);

            // add key and the value to dictionary
            _dict[key] = hashTableValue;
            _sum += value;
        }

        public int? Get(string key, int mins) 
        {
            
            if(_dict.ContainsKey(key) )
            {
                if (!HasExpired(key, mins))
                {
                    return _dict[key].Data;
                }
                else
                {
                    _list.Remove(_dict[key].PointerToList);
                    _dict.Remove(key);
                }
            }
            return null;            
        }

        private bool HasExpired(string key, int mins)
        {
            var currTime = _intialTime.AddMinutes(mins);
            return currTime >= _dict[key].ExpiryTime;
        }

        public int? Average(int mins)
        {
            if (_count == 0)
            {
                return null;
            }

            var currTime = _intialTime.AddMinutes(mins);
            for (int i=0; i < _list.Count; i++)
            {
                var item = _list.ElementAt(i);
                if (currTime >= item.ExpiryTime)//item is expired
                {
                    _count--;
                    _sum -= _dict[item.Key].Data;
                    
                    // remove expired items from hashTable and the list
                    _list.Remove(item);
                    _dict.Remove(item.Key);
                }
                else
                {
                    break;
                }
            }
            if (_count == 0)
            {
                return null;
            }

            return _sum/_count;
        }
    }
}
