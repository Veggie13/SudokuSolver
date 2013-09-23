using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlgorithmX;

namespace AlgorithmXTest
{
    class Program
    {
        static void Main(string[] args)
        {
            bool[,] matrix = new bool[,] {
                {true, false, false, true, false, false, true},
                {true, false, false, true, false, false, false},
                {false, false, false, true, true, false, true},
                {false, false, true, false, true, true, false},
                {false, true, true, false, false, true, true},
                {false, true, false, false, false, false, true}
            };

            IAlgorithmX alg = new NaiveAlgorithmX();
            var solutions = alg.AlgorithmX(matrix);

            Console.ReadLine();
        }
    }
}
