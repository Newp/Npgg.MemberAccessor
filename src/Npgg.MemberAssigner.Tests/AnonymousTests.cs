using System;
using System.Linq;
using Xunit;

namespace Npgg.MemberAssignerTests
{
    public class AnonymousTests
    {

        [Fact]
        public void AnonymouseReadonlyTest()
        {
            var item = new { name = "anon" };
            var assigner = MemberAssigner.GetAssigners(item.GetType()).Values.First(); ;

            Assert.True(assigner.IsReadonly);

            Assert.Equal(assigner.GetValue<string>(item), item.name);
        }

        [Fact]
        public void CacheTest()
        {
            var item = new { name = "anon" };
            for (int i =0;i<100;i++)
            {
                MemberAssigner.GetAssigners(item.GetType());
            }

            Assert.Single(MemberAssigner.Cached);
        }
    }
}
