using Npgg.Reflection;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Npgg.MemberAccessorTests
{

    public class Sample
    {
        public string Name { get; set; }
        public int Age { get; private set; }
    }

    public class BaseTest : BaseFixture<Sample>
    {
        Sample item = new Sample()
        {
            Name = "test name"
        };

        [Fact]
        public void GetTest()
        {
            var assigner = this.GetAssigner(nameof(Sample.Name));

            Assert.True(assigner.CheckType(typeof(string)));
            Assert.Equal(item.Name, assigner.GetValue<string>(item));
        }



        [Fact]
        public void GetAccessorViaExpression()
        {
            var accessor = MemberAccessor.GetAccessor<Sample>(sample => sample.Name);

            var member = typeof(Sample).GetMember(nameof(Sample.Name)).First() as PropertyInfo;

            Assert.Equal(member.Name, accessor.Name);
            Assert.Equal(typeof(Sample), accessor.DeclaringType);
            Assert.Equal(member.PropertyType, accessor.ValueType);
        }


        [Fact]
        public void SetTest()
        {
            var assigner = this.GetAssigner(nameof(Sample.Name));

            Assert.True(assigner.CheckType(typeof(string)));
            Assert.Equal(item.Name, assigner.GetValue<string>(item));

            string newValue = "chagned name";

            assigner.SetValue(item, newValue);

            Assert.Equal(newValue, item.Name);
        }



        [Fact]
        public void PrivateSetTest()
        {
            var assigner = this.GetAssigner(nameof(Sample.Age));

            Assert.True(assigner.CheckType(typeof(int)));

            Assert.Equal(default, item.Age);

            int newValue = 3939;
            assigner.SetValue(item, newValue);

            Assert.Equal(newValue, item.Age);
        }
    }
}
