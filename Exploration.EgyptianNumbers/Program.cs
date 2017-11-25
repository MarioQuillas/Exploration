using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exploration.EgyptianNumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            //var a = new List<int>() { 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29,31,33,35,37,39,41,43,45,47,49,51,53,55,57,59,61,63,65,67,69,71,73,75,77,79,81,83,85,87,89,91,93,95,97,99,101,103,105,107,109,111,113,115,117,119,121,123};//,125,127,129,131,133,135,137,139,141,143,145,147,149,151,153,155,157,159,161,163,165,167,169,171,173,175,177,179,181,183,185,187,189,191,193,195,197,199,201 };

            var a = new List<int>() { 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31, 33, 35, 37, 39, 41, 43, 45, 47, 49, 51, 53, 55, 57, 59, 61, 63, 65, 67, 69, 71, 73, 75, 77, 79, 81, 83, 85, 87, 89, 91, 93, 95, 97, 99, 101, 103, 105, 107, 109, 111, 113, 115, 117, 119, 121, 123 ,125,127,129,131,133,135,137,139,141,143,145,147,149,151,153,155,157,159,161,163,165,167,169,171,173,175,177,179,181,183,185,187,189,191,193,195,197,199,201 };

            //var a0 = new List<int>() { 3, 5, 7, 9, 11, 13, 15, 17, 19, 21};
            //var a1 = new List<int>() { 23, 25, 27, 29, 31, 33, 35, 37, 39};//
            //var a2 = new List<int>() { 41, 43, 45, 47, 49, 51, 53, 55, 57 };//, 59, 61, 63, 65, 67, 69, 71, 73, 75, 77, 79, 81, 83, 85, 87, 89, 91, 93, 95, 97, 99, 101, 103, 105, 107, 109, 111, 113, 115, 117, 119, 121, 123 };//,125,127,129,131,133,135,137,139,141,143,145,147,149,151,153,155,157,159,161,163,165,167,169,171,173,175,177,179,181,183,185,187,189,191,193,195,197,199,201 };

            //var a = new List<int>() { 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31, 33, 35, 37, 39, 41, 43, 45, 47, 49, 51, 53, 55};
            //var b = new List<int>() { 57, 59, 61, 63, 65, 67, 69, 71, 73, 75, 77, 79, 81, 83, 85, 87, 89, 91, 93, 95, 97, 99, 101, 103, 105, 107, 109, 111, 113, 115, 117, 119, 121, 123 };

            //var aa = new List<int>() { 1, 3, 5, 7, 9, 11 };

            //var qg = SubSets(aa, 3).ToList();

            //var test = pgcdForN(30, 462, 390, 798);

            var test = ppcm(3, 5, 7, 66);

            var seeds = new List<int>(){3, 5, 7, 11};

            var n = 9;

            var found = false;

            var filtered = a.Where(x =>
            {
                //var result = true;
                return seeds.Any(seed => x % seed == 0);
                //return result;
            }).ToList();

            while (!found && n < 50)
            {
                Console.WriteLine(n);
                var g = SubSets(filtered, n);

                long i = 1;

                foreach (var item in g)
                {
                    Console.Write(i + ": ");
                    PrintList(item);
                    i = i + 1;
                    if (SumInverse(item) == 1)
                    {
                        Console.Write("BINGO : ");
                        PrintList(item);
                        found = true;
                        break;

                    }
                }

                n = n + 2;
            }

            Console.ReadLine();
        }

        static void PrintList(List<int> a)
        {
            Console.Write("[ ");
            foreach (var i in a)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine("]");
        }

        static decimal SumInverse(List<int> aa) => aa.Sum(i => ((decimal) 1.0) / i);

        static IEnumerable<List<int>> SubSets(List<int> a, int k)
        {
            if (a.Count == k)
            {
                yield return a;
                yield break;
            }

            if (k == 1)
            {
                foreach (var x in a)
                {
                    yield return new List<int>() {x};
                }
                yield break;
            }

            var list = new List<int>();

            for (int i = 1; i < a.Count; ++i)
            {
                list.Add(a[i]);
            }

            var s = SubSets(list, k - 1);
            var t = SubSets(list, k);
            
            foreach (var item in s)
            {
                var rr = new List<int>();

                rr.Add(a[0]);
                rr.AddRange(item);

                yield return rr;
            }

            foreach (var item in t)
            {
                yield return item;
            }
        }

        static long pgcd(long x, long y)
        {
            long r = 0;
            while (y != 0)
            {
                r = x % y;
                x = y;
                y = r;
            }
            return x;
        }

        static long pgcdForN(params long[] x)
        {
            if (x.Length == 2) return pgcd(x[0], x[1]);

            var p = pgcd(x[0], x[1]);

            for (var i = 2; i < x.Length; ++i)
            {
                p = pgcd(p, x[i]);
            }

            return p;
        }

        static long ppcm(params long[] x)
        {
            if (x.Length == 2) return x[0] * x[1] / pgcd(x[0], x[1]);

            var p = x[0] * x[1] / pgcd(x[0], x[1]);

            for (var i = 2; i < x.Length; ++i)
            {
                p =  p*x[i]/pgcd(p, x[i]);
            }

            return p;
        }
    }
}
