using Sum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TestGatherPairConsole
{
    class Program
    {
        static void RunTest(int count, int value)
        {
            Console.WriteLine($"items: {count}");

            var watch = new Stopwatch();
            watch.Start();
            var pairs = GatherPairs.CollectPairs(Enumerable.Range(1, count).ToList(), value);
            watch.Stop();
            Console.WriteLine($"fast struct collection. total: {pairs.Count} time: {watch.Elapsed}");

            watch.Restart();
            var pairs2 = GatherPairs.CollectPairsClass(Enumerable.Range(1, count).ToList(), value);
            watch.Stop();
            Console.WriteLine($"fast class collection. total: {pairs2.Count} time: {watch.Elapsed}");

            watch.Restart();
            pairs = GatherPairs.CollectPairsNaive(Enumerable.Range(1, count).ToList(), value);
            watch.Stop();
            Console.WriteLine($"naive solution struct collection. total: {pairs.Count} time: {watch.Elapsed}");
        }

        static void Main(string[] args)
        {
            for (int i = 500; i < 1000000; i *= 10)
            {
                RunTest(i, i / 3);
            }          

            Console.ReadKey();
        }
    }
}
