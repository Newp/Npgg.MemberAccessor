using Npgg;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Npgg.Reflection;

namespace Example
{
    class Program
    {

        class Sample
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        static void Main(string[] args)
        {

            var type = typeof(Sample);
            var assignerPool = new MemberAccessorPool();
            var member = type.GetMember(nameof(Sample.Name)).First() as PropertyInfo;
            var item = new Sample();

            Console.WriteLine("cache single member accessor performance test");
            Console.WriteLine($"without cache { Check(10000, () => MemberAccessor.GetAccessor<Sample>(sample => sample.Name)) } ms elapsed");
            Console.WriteLine($"with cache { Check(10000, () => assignerPool.GetAccessor<Sample>(sample => sample.Name)) } ms elapsed");

            Console.WriteLine("cache all member accessor performance test");
            Console.WriteLine($"without cache { Check(10000, () => MemberAccessor.GetAssigners(type)) } ms elapsed");
            Console.WriteLine($"with cache { Check(10000, () => assignerPool.GetAccessors(type)) } ms elapsed");


            Console.WriteLine("benchmark with reflection");

            string sampleName = "test";
            Console.WriteLine($"without assigner { Check(500000, () => member.SetValue(item, sampleName)) } ms elapsed");

            var assigner = assignerPool.GetAccessors(type)[nameof(Sample.Name)];
            Console.WriteLine($"with assigner { Check(500000, () => assigner.SetValue(item, sampleName)) } ms elapsed");

        }

        static long Check(int count, Action action)
        {
            Stopwatch watch = Stopwatch.StartNew();

            for (int i = 0; i < count; i++)
            {
                action();
            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
    }
}
