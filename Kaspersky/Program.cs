using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kaspersky
{
    class Program
    {
        private class TestData
        {
            private byte[] _data;

            TestData()
            {
                var rand = new Random();
                this._data = new byte[rand.Next(1024)];
            }
        }

        private static List<string> log = new List<string>();
        private static IList<List<Tuple<DateTime, string>>> logs = new List<List<Tuple<DateTime, string>>>();
        private static readonly object locker = new object();

        private static void AddLog(List<Tuple<DateTime, string>> l)
        {
            lock (locker)
            {
                logs.Add(l);
            }
        }

        private static void MergeLogs()
        {
            lock (locker)
            {
                log.AddRange(logs.SelectMany(l => l.Select(s => s)).OrderBy(s => s.Item1).Select(s => s.Item2));
            }
        }

        private static async Task Check(ParallelQueque.ParallelQueue<int> queue, int id)
        {
            var list = new List<Tuple<DateTime, string>>();

            for (int i = 0; i < 10000; i++)
            {
                queue.Push(id);
                var date = DateTime.Now;
                list.Add(Tuple.Create(date, $"#{id} [{date.Ticks}]"));
                if (i % id == 0)
                {
                    await Task.Delay(1).ConfigureAwait(false);
                }
                var value = queue.Pop();
                date = DateTime.Now;
                list.Add(Tuple.Create(date, $"#{id} [{date.Ticks}] value: {value}"));
            }

            AddLog(list);
        }

        static void Main(string[] args)
        {
            var queue = new ParallelQueque.ParallelQueue<int>();
            var tasks = Enumerable.Range(1, 10).Select(i => Check(queue, i)).ToArray();
            Task.WaitAll(tasks);
            MergeLogs();
            File.WriteAllLines("log.txt", log);

        }
    }
}
