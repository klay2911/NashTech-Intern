using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CsFun3;
class Program
{
        static async Task Main(string[] args)
        {
                //return the total prime numbers and process time
                var sw = new Stopwatch();
                sw.Start();
                var primes = await GetPrimesAsync(2, 10000000);
                sw.Stop();
                Console.WriteLine("Total prime numbers: {0}\nProcess time: {1}", primes.Count, sw.Elapsed.TotalSeconds);
                
                //return prime numbers for a range (e.g. from 0 to 100)
                
                /*
                int start = 0;
                int end = 100;

                List<int> result = await GetPrimesAsync(start, end);
                
                Console.WriteLine("Prime numbers between {0} and {1}:", start, end );
                foreach (int prime in result)
                {
                        Console.Write(prime + " ");
                }
                */

        }

        static async Task<List<int>> GetPrimesAsync(int start, int end)
        {
                var result = new List<int>();

                //List<int> result = new List<int>();

                await Task.Run(() =>
                {
                        for (int i = start; i <= end; i++)
                        {
                                if (IsPrime(i))
                                {
                                        result.Add(i);
                                }
                        }
                });

                return result;
        }

        static bool IsPrime(int number)
        {
                if (number % 2 == 0)
                {
                        return number == 2;
                }
                else
                {
                        var topLimit = (int)Math.Sqrt(number);
                        for (int i = 3; i <= topLimit; i += 2)
                        {
                                if (number % i == 0) return false;
                        }

                        return true;
                }
        }
        /*
         static bool IsPrime(int number)
        {
                if (number < 2) return false;
                if (number == 2) return true;
                if (number % 2 == 0) return false;

                var boundary = (int)Math.Floor(Math.Sqrt(number));

                for (int i = 3; i <= boundary; i += 2)
                        if (number % i == 0)
                                return false;

                return true;
        }
        */
}

