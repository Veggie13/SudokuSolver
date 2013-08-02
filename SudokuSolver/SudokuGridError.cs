namespace SudokuSolver
{
    /// <summary>
    /// SudokuGridError objects describe a type of inconsistency in a SudokuGrid.
    /// ErrorType.Row errors indicate more than one of the same value in a row.
    /// ErrorType.Col errors indicate more than one of the same value in a column.
    /// ErrorType.Block errors indicate more than one of the same value in a block.
    /// ErrorType.Value errors indicate an invalid value in a cell.
    /// </summary>
    public class SudokuGridError
    {
        public enum ErrorType {Row, Col, Block, Value};

        public SudokuGridError(ErrorType type, uint row, uint col, int value)
        {
            _type = type;
            _row = row;
            _col = col;
            _value = value;
        }

        private ErrorType _type;
        public ErrorType Type
        {
            get { return _type; }
        }

        private uint _row;
        public uint Row
        {
            get { return _row; }
        }

        private uint _col;
        public uint Col
        {
            get { return _col; }
        }

        private int _value;
        public int Value
        {
            get { return _value; }
        }
    }
}