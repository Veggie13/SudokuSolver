using System.Collections.Generic;
using System;
using System.Linq;

namespace SudokuSolver
{
    /// <summary>
    /// BacktrackSudokuSolver attempts to solve the puzzle using an improved backtracking algorithm.
    /// This is essentially a brute force attack on the puzzle using recursion to fill in nodes and attempt every valid "move".
    /// The algorithm selects the best blank candidate cell as the cell with the fewest possibilities, utilizing the SudokuGrid.GetPossibilities() feature.
    /// In this way, it recursively attempts every possibility and keeps track of all complete solutions.
    /// For sake of performance, this algorithm is done in-place on the root grid, so an initial clone is made.
    /// </summary>
    public class BacktrackSudokuSolver : ISudokuSolver
    {
        public SudokuGrid FindFirstSolution(SudokuGrid original)
        {
            List<SudokuGrid> solutions = new List<SudokuGrid>();
            Solve(original.Clone(), 0, 0, solutions, 1);
            return solutions.FirstOrDefault();
        }

        public List<SudokuGrid> FindAllSolutions(SudokuGrid original)
        {
            List<SudokuGrid> solutions = new List<SudokuGrid>();
            Solve(original.Clone(), 0, 0, solutions, -1);
            return solutions;
        }

        public static void Solve(SudokuGrid grid, uint row, uint col, IList<SudokuGrid> solutions, int maxSolutions)
        {
            // This case identifies a solved puzzle.
            if (row >= grid.Size)
            {
                solutions.Add(grid.Clone());
                return;
            }

            // Only attempt possibilities on empty cells.
            if (grid[row, col] >= 0)
                Next(grid, row, col, solutions, maxSolutions);
            else
            {
                foreach (int val in grid.GetPossibilities(row,col))
                {
                    grid[row, col] = val;
                    Next(grid, row, col, solutions, maxSolutions);
                }

                // Reset to empty on our way out.
                grid[row, col] = -1;
            }
        }

        public static void Next(SudokuGrid grid, uint row, uint col, IList<SudokuGrid> solutions, int maxSolutions)
        {
            if (maxSolutions >= 0 && solutions.Count >= maxSolutions)
                return;

            uint curBestRow = grid.Size, curBestCol = 0;
            int curBest = int.MaxValue;

            // Search for the cell with fewest options / most constraints.
            IList<int>[,] possibilities = grid.GetPossibilities();
            for (uint i = 0; i < grid.Size; i++)
            for (uint j = 0; j < grid.Size; j++)
            {
                if (grid[i, j] < 0 && possibilities[i, j].Count < curBest)
                {
                    curBestRow = i;
                    curBestCol = j;
                    curBest = possibilities[i, j].Count;
                }
            }
            
            Solve(grid, curBestRow, curBestCol, solutions, maxSolutions);
        }
    }
}