using Npgg;
using System;
using System.Diagnostics;

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
            var assignerPool = new MemberAssignerPool();

            int count = 10000;

            Console.WriteLine($"without cache { Check(count, () => MemberAssigner.GetAssigners(type)) } ms elapsed");

            Console.WriteLine($"with cache { Check(count, () => assignerPool.GetAssigners(type)) } ms elapsed");
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
