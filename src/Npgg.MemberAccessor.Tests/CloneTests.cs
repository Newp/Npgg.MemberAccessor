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
            var overwrited = new Sample()
            {
                Description = "NOT DEFAULT VALUE"
            };
            pool.Overwrite(source, overwrited, true);

            Assert.Equal(source.Name, overwrited.Name);
            Assert.Equal(source.Age, overwrited.Age); //private member
            Assert.Equal(source.Description, overwrited.Description); //overwrited existed Description
        }


        [Fact]
        public void OverwriteNoDefaultByFlagTests()
        {
            string desciption = "abc";
            var overwrited = new Sample()
            {
                Description = desciption
            };
            pool.Overwrite(source, overwrited, false);

            Assert.Equal(source.Name, overwrited.Name);
            Assert.Equal(source.Age, overwrited.Age); //private member
            Assert.Equal(desciption, overwrited.Description);
        }


        [Fact]
        public void OverwriteNoDefaultByDefaultOptionTests()
        {
            string desciption = "abc";
            var overwrited = new Sample()
            {
                Description = desciption
            };
            pool.Overwrite(source, overwrited);

            Assert.Equal(source.Name, overwrited.Name);
            Assert.Equal(source.Age, overwrited.Age); //private member
            Assert.Equal(desciption, overwrited.Description);
        }
    }
}
