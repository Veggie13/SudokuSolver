using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SudokuSolver;
using AlgorithmX;

namespace SudokuSolverTest
{
    static class SudokuSolverTest
    {
        static void Main()
        {
            const string Alphabet = "123456789";
            const string Unknowns = "0.";

            SudokuParser parser = new SudokuParser();
            SudokuGrid grid = parser.ParseString(
                "030605000600090002070100006090000000810050069000000080400003020900020005000908030",
                Alphabet, Unknowns);
            ISudokuSolver solver = new AlgXSudokuSolver(new NaiveAlgorithmX());
            IList<SudokuGrid> solutions = solver.FindAllSolutions(grid);
            Console.WriteLine("{0} solution(s).", solutions.Count);
            if (solutions.Count > 0)
            {
                string render = parser.Render(Alphabet, solutions[0]);
                Console.Write(render);
            }
            Console.Read();
        }
    }
}
