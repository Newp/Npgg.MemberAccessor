using Npgg.Reflection;
using System;
using System.Linq;
using Xunit;

namespace Npgg.MemberAccessorTests
{

    public class ReadonlySample
    {
        public readonly string Name;

        public ReadonlySample(string name)
        {
            Name = name;
        }
    }

    public class ReadonlyTests : BaseFixture<ReadonlySample>
    {
        const string initstring = "aabbccc";
        readonly ReadonlySample item = new ReadonlySample(initstring);


        [Fact]
        public void GetTest()
        {
            var accessor = this.GetAccessor(nameof(Sample.Name));

            Assert.True(accessor.CheckType(typeof(string)));
            Assert.Equal(item.Name, accessor.GetValue<string>(item));
        }


        [Fact]
        public void SetTest()
        {
            var accessor = this.GetAccessor(nameof(Sample.Name));

            Assert.True(accessor.CheckType(typeof(string)));
            Assert.Equal(item.Name, accessor.GetValue<string>(item));

            string newValue = "chagned name";

            accessor.SetValue(item, newValue);

            Assert.NotEqual(newValue, item.Name);
            Assert.Equal(initstring, item.Name);
        }

    }
}
