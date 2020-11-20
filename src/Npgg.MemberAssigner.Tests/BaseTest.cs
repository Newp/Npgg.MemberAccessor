using System;
using System.Linq;
using Xunit;

namespace Npgg.MemberAssignerTests
{

    public class Sample
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class BaseTest : BaseFixture<Sample>
    {
        Sample item = new Sample()
        {
            Name = "test name",
            Age = 3939
        };

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

            Assert.Equal(newValue, item.Name);
        }
    }
}
