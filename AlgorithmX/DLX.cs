using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.ToolsAndTests;

namespace AlgorithmX
{
    public class DLX : IAlgorithmX
    {
        public List<List<int>> AlgorithmX(bool[,] matrix, int maxSolutions)
        {
            var solver = new ExactCover((bool[,])matrix.Clone());
            List<List<int>> results = new List<List<int>>();
            if (!solver.Solve())
                return results;

            results.Add(solver.MatrixState.ToList());
            return results;
        }
    }
}
