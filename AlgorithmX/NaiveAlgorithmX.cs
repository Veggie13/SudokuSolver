using System.Linq;
using System.Collections.Generic;

namespace AlgorithmX
{
    public class NaiveAlgorithmX : IAlgorithmX
    {
        private class Matrix
        {
            private bool[,] _matrix;
            private bool[] _rowAllow;
            private bool[] _colAllow;
            private int[] _rowCount;
            private int[] _colCount;

            public Matrix(bool[,] matrix)
            {
                _matrix = (bool[,])matrix.Clone();

                _rowAllow = Enumerable.Repeat<bool>(true, _matrix.GetLength(0)).ToArray();
                _colAllow = Enumerable.Repeat<bool>(true, _matrix.GetLength(1)).ToArray();

                init();
            }

            public Matrix(Matrix old, IEnumerable<int> badRows, IEnumerable<int> badCols)
            {
                _matrix = old._matrix;

                _rowAllow = (bool[])old._rowAllow.Clone();
                foreach (int row in badRows)
                    _rowAllow[row] = false;

                _colAllow = (bool[])old._colAllow.Clone();
                foreach (int col in badCols)
                    _colAllow[col] = false;

                init();
            }

            public bool this[int row, int col]
            {
                get { return _matrix[row, col]; }
            }

            public IEnumerable<int> Rows()
            {
                for (int row = 0; row < _rowAllow.Length; row++)
                    if (_rowAllow[row])
                        yield return row;
            }

            public IEnumerable<int> Columns()
            {
                for (int col = 0; col < _colAllow.Length; col++)
                    if (_colAllow[col])
                        yield return col;
            }

            public int RowTotal(int row)
            {
                return _rowCount[row];
            }

            public int ColTotal(int col)
            {
                return _colCount[col];
            }

            private void init()
            {
                _rowCount = new int[_matrix.GetLength(0)];
                foreach (int row in Rows())
                    _rowCount[row] = Columns().Where(c => _matrix[row, c]).Count();

                _colCount = new int[_matrix.GetLength(1)];
                foreach (int col in Columns())
                    _colCount[col] = Rows().Where(r => _matrix[r, col]).Count();
            }
        }

        public List<List<int>> AlgorithmX(bool[,] matrix)
        {
            List<List<int>> solutions = new List<List<int>>();
            Matrix A = new Matrix(matrix);
            AlgorithmX(A, solutions);
            return solutions;
        }

        private static bool AlgorithmX(Matrix A, List<List<int>> solutions)
        {
            return AlgorithmX(A, new List<int>(), solutions);
        }

        private static bool AlgorithmX(Matrix A, IEnumerable<int> partial, List<List<int>> solutions)
        {
            // Step 1 - If A is empty, problem solved, terminate successfully.
            if (A.Rows().Count() == 0 || A.Columns().Count() == 0)
            {
                solutions.Add(partial.ToList());
                return true;
            }

            // Step 2 - Choose first column with fewest markers.
            int fewestInColumns = int.MaxValue;
            int col = -1;
            foreach (int c in A.Columns())
            {
                int total = A.ColTotal(c);
                if (total < fewestInColumns)
                {
                    fewestInColumns = total;
                    col = c;
                }
            }
            if (fewestInColumns == 0)
            {
                return false;
            }

            foreach (int row in A.Rows())
            {
                // Step 3 - Select rows with markers in the chosen column.
                if (A[row, col])
                {
                    // Step 4 - Include A row in the partial solutions.
                    List<int> newPartial = partial.ToList();
                    newPartial.Add(row);

                    // Step 5 - For each column in A row with markers ...
                    List<int> badCols = new List<int>();
                    foreach (int c in A.Columns())
                    {
                        // 5c - ... finally, delete column from matrix
                        if (A[row, c])
                            badCols.Add(c);
                    }

                    // 5a - ... for each row with a marker in each of those columns ...
                    List<int> badRows = new List<int>();
                    foreach (int r in A.Rows())
                    {
                        // 5b - ... delete row from matrix ...
                        if (badCols.Select(c => A[r, c]).Contains(true))
                            badRows.Add(r);
                    }

                    // Step 6 - Repeat algorithm recursively on reduced matrix
                    var B = new Matrix(A, badRows, badCols);
                    AlgorithmX(B, newPartial, solutions);
                }
            }

            return false;
        }
    }
}
