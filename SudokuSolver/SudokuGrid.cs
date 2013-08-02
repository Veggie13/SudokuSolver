using System;
using System.Collections.Generic;

namespace SudokuSolver
{
    /// <summary>
    /// SudokuGrid is the central data structure for the SudokuSolver library.
    /// It stores the entire grid of any Dimensionality, but also provides some caching and incremental updating of computed data properties.
    /// </summary>
    public class SudokuGrid
    {
        /// <param name="dimensionality">
        /// This defining property of the data structure is the fundamental character of the grid.
        /// All grids have a size of (D^2) x (D^2), with an alphabet of D^2 characters and subdivided into DxD blocks of DxD.
        /// </param>
        public SudokuGrid(uint dimensionality)
        {
            _grid = new int[dimensionality, dimensionality, dimensionality, dimensionality];
            for (uint blockRow = 0; blockRow < dimensionality; blockRow++)
            for (uint blockCol = 0; blockCol < dimensionality; blockCol++)
            for (uint innerRow = 0; innerRow < dimensionality; innerRow++)
            for (uint innerCol = 0; innerCol < dimensionality; innerCol++)
                _grid[blockRow,blockCol,innerRow,innerCol] = -1;

            RecomputeErrors();
            RecomputePossibilities();
        }

        private SudokuGrid(int[, , ,] grid)
        {
            _grid = (int[,,,])grid.Clone();
            RecomputeErrors();
            RecomputePossibilities();
        }

        public SudokuGrid Clone()
        {
            return new SudokuGrid(_grid);
        }

        public uint Dimensionality
        {
            get { return (uint)_grid.GetLength(0); }
        }

        public uint Size
        {
            get { return Dimensionality * Dimensionality; }
        }

        private void ConvertCoord(uint coord, out uint blockCoord, out uint innerCoord)
        {
            blockCoord = coord / Dimensionality;
            innerCoord = coord % Dimensionality;
        }

        /// <summary>
        /// Index the grid by absolute coordinates.
        /// </summary>
        public int this[uint row, uint col]
        {
            get
            {
                uint blockRow, blockCol, innerRow, innerCol;
                ConvertCoord(row, out blockRow, out innerRow);
                ConvertCoord(col, out blockCol, out innerCol);
                return _grid[blockRow, blockCol, innerRow, innerCol];
            }
            set
            {
                uint blockRow, blockCol, innerRow, innerCol;
                ConvertCoord(row, out blockRow, out innerRow);
                ConvertCoord(col, out blockCol, out innerCol);
                this[blockRow, blockCol, innerRow, innerCol] = value;
            }
        }

        /// <summary>
        /// Index the grid by position within a block, for convenience.
        /// When setting a value, the possibilities matrix is updated for all affected cells.
        /// </summary>
        public int this[uint blockRow, uint blockCol, uint innerRow, uint innerCol]
        {
            get { return _grid[blockRow, blockCol, innerRow, innerCol]; }
            set
            {
                int oldValue = _grid[blockRow, blockCol, innerRow, innerCol];
                _grid[blockRow, blockCol, innerRow, innerCol] = value;

                uint baseRow = Dimensionality * blockRow;
                uint baseCol = Dimensionality * blockCol;
                uint row = baseRow + innerRow;
                uint col = baseCol + innerCol;

                // This cell needs updating if the value is being removed.
                if (value < 0)
                    RecomputeCellPossibilities(row, col);
                else
                {
                    _possibilities[row, col].Clear();
                    _possibilities[row, col].Add(value);
                }

                // For each row/column, update the possibilities for each empty cell.
                for (uint i = 0; i < Size; i++)
                {
                    if (i != col && this[row, i] < 0)
                    {
                        _possibilities[row, i].Remove(value);
                        if (oldValue >= 0 && !CellBordersValue(row, i, oldValue))
                            _possibilities[row, i].Add(oldValue);
                    }
                    if (i != row && this[i, col] < 0)
                    {
                        _possibilities[i, col].Remove(value);
                        if (oldValue >= 0 && !CellBordersValue(i, col, oldValue))
                            _possibilities[i, col].Add(oldValue);
                    }
                }

                // For each empty cell in this block, update the possibilities.
                for (uint i = 0; i < Dimensionality; i++)
                for (uint j = 0; j < Dimensionality; j++)
                {

                    if (i != innerRow && j != innerCol && this[blockRow, blockCol, i, j] < 0)
                    {
                        _possibilities[baseRow + i, baseCol + j].Remove(value);
                        if (oldValue >= 0 && !CellBordersValue(baseRow + i, baseCol + j, oldValue))
                            _possibilities[baseRow + i, baseCol + j].Add(oldValue);
                    }
                }
            }
        }

        private int[, , ,] _grid;
        public int[,] GetGrid()
        {
            int[,] output = new int[Size, Size];
            for (uint row = 0; row < Size; row++)
            for (uint col = 0; col < Size; col++)
            {
                output[row, col] = this[row, col];
            }
            return output;
        }

        List<SudokuGridError> _errors;
        public IList<SudokuGridError> GetErrors()
        {
            RecomputeErrors();
            return new List<SudokuGridError>(_errors);
        }

        private void RecomputeErrors()
        {
            _errors = new List<SudokuGridError>();

            int[] curLine = new int[Size];
            for (uint i = 0; i < Size; i++)
            {
                for (uint col = 0; col < Size; col++)
                {
                    curLine[col] = this[i, col];
                    if (curLine[col] >= Size)
                        _errors.Add(new SudokuGridError(SudokuGridError.ErrorType.Value, i, col, curLine[col]));
                }

                for (int val = 0; val < Size; val++)
                {
                    if (Array.FindAll(curLine, delegate(int a) { return a == val; }).Length > 1)
                        _errors.Add(new SudokuGridError(SudokuGridError.ErrorType.Row, i, 0, val));
                }

                for (uint row = 0; row < Size; row++)
                    curLine[row] = this[row, i];

                for (int val = 0; val < Size; val++)
                {
                    if (Array.FindAll(curLine, delegate(int a) { return a == val; }).Length > 1)
                        _errors.Add(new SudokuGridError(SudokuGridError.ErrorType.Col, 0, i, val));
                }
            }

            for (uint i = 0; i < Dimensionality; i++)
            for (uint j = 0; j < Dimensionality; j++)
            {
                for (uint row = 0, n = 0; row < Dimensionality; row++)
                for (uint col = 0; col < Dimensionality; col++, n++)
                {
                    curLine[n] = _grid[i, j, row, col];
                }

                for (int val = 0; val < Size; val++)
                {
                    if (Array.FindAll(curLine, delegate(int a) { return a == val; }).Length > 1)
                        _errors.Add(new SudokuGridError(SudokuGridError.ErrorType.Block, i, j, val));
                }
            }
        }

        /// <summary>
        /// Returns whether a particular cell shares row, column, or block with a cell of value "val".
        /// </summary>
        private bool CellBordersValue(uint row, uint col, int val)
        {
            for (uint i = 0; i < Size; i++)
            {
                if (i != col && this[row, i] == val)
                    return true;
                if (i != row && this[i, col] == val)
                    return true;
            }

            uint blockRow, blockCol, innerRow, innerCol;
            ConvertCoord(row, out blockRow, out innerRow);
            ConvertCoord(col, out blockCol, out innerCol);
            for (uint i = 0; i < Dimensionality; i++)
            for (uint j = 0; j < Dimensionality; j++)
            {
                if ((i != innerRow || j != innerCol) && this[blockRow, blockCol, i, j] == val)
                    return true;
            }

            return false;
        }

        private IList<int>[,] _possibilities;
        /// <summary>
        /// Retrieve a matrix of possibilities for each cell. Cells with definite values have possibility lists of size 1.
        /// </summary>
        public IList<int>[,] GetPossibilities()
        {
            IList<int>[,] result = new IList<int>[_possibilities.GetLength(0), _possibilities.GetLength(1)];
            for (int row = 0; row < _possibilities.GetLength(0); row++)
            for (int col = 0; col < _possibilities.GetLength(1); col++)
                result[row, col] = new List<int>(_possibilities[row, col]);
                
            return result;
        }
        
        public IList<int> GetPossibilities(uint row, uint col)
        {
            return new List<int>(_possibilities[row, col]);
        }

        private void RecomputeCellPossibilities(uint row, uint col)
        {
            List<int> valid = new List<int>();
            if (this[row, col] >= 0)
            {
                valid.Add(this[row, col]);
                _possibilities[row, col] = valid;
                return;
            }

            for (int val = 0; val < Size; val++)
            {
                if (!CellBordersValue(row, col, val))
                    valid.Add(val);
            }

            _possibilities[row, col] = valid;
        }
        
        private void RecomputePossibilities()
        {
            _possibilities = new List<int>[Size, Size];

            for (uint row = 0; row < Size; row++)
            for (uint col = 0; col < Size; col++)
            {
                RecomputeCellPossibilities(row, col);
            }
        }
    }
}