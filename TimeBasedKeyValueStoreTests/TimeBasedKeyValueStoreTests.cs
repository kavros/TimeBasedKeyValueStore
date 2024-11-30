

using timebased_key_value_store;

namespace timebased_key_value_store_tests
{
    public class TimeBasedKeyValueStoreTests
    {
        [Fact]
        public void Get_WhenTimeIsExpired_ReturnNull()
        {
            var data = new TimeBasedKeyValueStore();
            data.Put("A", 10, 10);
            
            var res = data.Get("A", 70);
            
            Assert.Null(res);
        }

        [Fact]
        public void Get_WhenTimeIsInsideWindow_ReturnValue()
        {
            var data = new TimeBasedKeyValueStore();
            data.Put("A", 10, 10);

            var res = data.Get("A", 20);

            Assert.Equal(10, res!.Value);
        }
        [Fact]
        public void Average_WhenAllDataIsExpired_ReturnNull()
        {
            var data = new TimeBasedKeyValueStore();
            data.Put("A", 10, 10);

            Assert.Null(data.Average(71));
        }

        [Fact]
        public void Average_CalledMultipleTimes()
        {
            var data = new TimeBasedKeyValueStore();
            data.Put("A", 10, 0);
            data.Put("B", 20, 10);

            Assert.Equal(15, data.Average(30));//haven't expired

            Assert.Equal(20, data.Average(65));//A has expired

            Assert.Equal(20, data.Get("B",67));//B hasn't expired

            data.Put("A", 30, 68);

            Assert.Equal(25, data.Average(69)); //A and B havn't expired
            Assert.Equal(30, data.Average(110));//B has expired
        }

        [Fact]
        public void Average_WhenSameKey_UseLatestKey()
        {
            var data = new TimeBasedKeyValueStore();
            data.Put("A", 10, 0);
            data.Put("B", 20, 10);
            data.Put("A", 20, 11);

            Assert.Equal(20, data.Average(10));
        }
    }
}