using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlgorithmX;

namespace SudokuSolver
{
    public class AlgXSudokuSolver : ISudokuSolver
    {
        private IAlgorithmX _algorithm;

        public AlgXSudokuSolver(IAlgorithmX alg)
        {
            _algorithm = alg;
        }

        public SudokuGrid FindFirstSolution(SudokuGrid original)
        {
            throw new NotImplementedException();
        }

        public List<SudokuGrid> FindAllSolutions(SudokuGrid original)
        {
            bool[,] matrix = constructMatrix(original);
            var solutions = _algorithm.AlgorithmX(matrix);

            List<SudokuGrid> results = solutions.Select(s => decodeGrid(matrix, s)).ToList();

            return results;
        }

        private bool[,] constructMatrix(SudokuGrid grid)
        {
            bool[,] constraintMatrix = new bool[729, 324];
            var possibilities = grid.GetPossibilities();

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    int coord = 9 * row + col;
                    for (int v = 0; v < 9; v++)
                    {
                        // R<row>C<col>#<v>
                        int pos1 = 9 * coord + v;

                        // row-column R<row>C<col>
                        if (possibilities[row, col].Contains(v))
                        {
                            constraintMatrix[pos1, coord] = true;
                        }

                        // row-number R<row>#<v>
                        constraintMatrix[pos1, 81 + 9 * row + v] = true;

                        // column-number C<col>#<v>
                        constraintMatrix[pos1, 162 + 9 * col + v] = true;

                        // box-number B<row/col>#<v>
                        int box = 3 * (row / 3) + (col / 3);
                        constraintMatrix[pos1, 243 + 9 * box + v] = true;
                    }
                }
            }

            return constraintMatrix;
        }

        private SudokuGrid decodeGrid(bool[,] matrix, IEnumerable<int> selectedRows)
        {
            SudokuGrid grid = new SudokuGrid(3);

            foreach (int matrixRow in selectedRows)
            {
                uint row = (uint)matrixRow / 81;
                uint col = ((uint)matrixRow % 81) / 9;
                int val = matrixRow % 9;

                grid[row, col] = val;
            }

            return grid;
        }
    }
}
