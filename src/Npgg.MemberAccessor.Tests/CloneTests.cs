using Npgg.Reflection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Npgg.MemberAccessorTests 
{
    public class CopyTests 
    {
        readonly Sample source = new Sample("test_test", 239);

        readonly MemberAccessorPool pool = new MemberAccessorPool();

        [Fact]
        public void CloneTests()
        {
            var cloned = pool.CreateClone(source);

            Assert.Equal(source.Name, cloned.Name);
            Assert.Equal(source.Age, cloned.Age); //private member
        }

        [Fact]
        public void OverwriteTests()
        {
            var overwrited = new Sample();
            pool.Overwrite(source, overwrited);

            Assert.Equal(source.Name, overwrited.Name);
            Assert.Equal(source.Age, overwrited.Age); //private member
        }
    }
}
