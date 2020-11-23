using System;
using System.Linq;
using Xunit;
using Npgg.Reflection;

namespace Npgg.MemberAccessorTests
{
    public class AnonymousTests
    {

        [Fact]
        public void AnonymouseReadonlyTest()
        {
            var item = new { name = "anon" };
            var accessor = MemberAccessor.GetAccessors(item.GetType()).Values.First(); ;

            Assert.True(accessor.IsReadonly);

            Assert.Equal(accessor.GetValue<string>(item), item.name);
        }

        [Fact]
        public void CacheTest()
        {
            var item = new { name = "anon" };
            for (int i =0;i<100;i++)
            {
                MemberAccessor.GetAccessors(item.GetType());
            }

        }
    }
}
