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
        ReadonlySample item = new ReadonlySample(initstring);


        [Fact]
        public void GetTest()
        {
            var assigner = this.GetAssigner(nameof(Sample.Name));

            Assert.True(assigner.CheckType(typeof(string)));
            Assert.Equal(item.Name, assigner.GetValue<string>(item));
        }


        [Fact]
        public void SetTest()
        {
            var assigner = this.GetAssigner(nameof(Sample.Name));

            Assert.True(assigner.CheckType(typeof(string)));
            Assert.Equal(item.Name, assigner.GetValue<string>(item));

            string newValue = "chagned name";

            assigner.SetValue(item, newValue);

            Assert.NotEqual(newValue, item.Name);
            Assert.Equal(initstring, item.Name);
        }

    }
}
