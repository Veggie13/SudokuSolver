using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SudokuSolver;

namespace SudokuSolverApp
{
    /// <summary>
    /// This control interfaces with a SudokuGrid to display and allow editing.
    /// </summary>
    public partial class SudokuControl : UserControl
    {
        private TextBox _editor = new TextBox();

        public SudokuControl()
        {
            InitializeComponent();

            _editor.KeyPress += new KeyPressEventHandler(_editor_KeyPress);
            _editor.KeyDown += new KeyEventHandler(_editor_KeyDown);
        }

        /// <summary>
        /// Catch this event to handle special key presses like delete and escape.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _editor_KeyDown(object sender, KeyEventArgs e)
        {
            Label src = _editor.Parent as Label;
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                // Delete/Backspace will clear the current cell.
                for (uint row = 0; row < _grid.Size; row++)
                for (uint col = 0; col < _grid.Size; col++)
                {
                    if (src == _cellLabels[row, col])
                    {
                        _grid[row, col] = -1;
                        _editor.Parent.Controls.Remove(_editor);
                        UpdateUI();
                        return;
                    }
                }
            }
            else if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                // Escape/Enter will stop editing.
                _editor.Parent.Controls.Remove(_editor);
                return;
            }
        }

        /// <summary>
        /// Catch this event to handle regular key presses. Only react to valid alphabet keys.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _editor_KeyPress(object sender, KeyPressEventArgs e)
        {
            Label src = _editor.Parent as Label;
            if (Alphabet.Contains(e.KeyChar))
            {
                int val = Alphabet.IndexOf(e.KeyChar);
                for (uint row = 0; row < _grid.Size; row++)
                for (uint col = 0; col < _grid.Size; col++)
                {
                    if (src == _cellLabels[row, col])
                    {
                        _grid[row, col] = val;
                        _editor.Parent.Controls.Remove(_editor);
                        UpdateUI();
                        return;
                    }
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private string _alphabet;
        public string Alphabet
        {
            get { return _alphabet; }
            set { _alphabet = value; }
        }
        
        private SudokuGrid _grid;
        public SudokuGrid Grid
        {
            get { return _grid; }
            set
            {
                _grid = value;
                if (value != null)
                    SetupUI();
            }
        }

        Label[,] _cellLabels;
        
        private void SetupUI()
        {
            // The grid is constructed using a table layout of table layouts to achieve the visual style.

            // Initialize block grid.
            _tableLayout.Controls.Clear();
            _tableLayout.RowCount = (int)_grid.Dimensionality;
            _tableLayout.ColumnCount = (int)_grid.Dimensionality;
            _tableLayout.RowStyles.Clear();
            _tableLayout.ColumnStyles.Clear();

            // Set block sizes.
            int blockWidth = Width / (int)_grid.Dimensionality - 4;
            int blockHeight = Height / (int)_grid.Dimensionality - 4;
            for (uint i = 0; i < _grid.Dimensionality; i++)
            {
                _tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, blockWidth));
                _tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, blockHeight));
            }

            _cellLabels = new Label[_grid.Size, _grid.Size]; // Matrix of cells for easy access.

            // Each block.
            int colWidth = Width / (int)_grid.Size - 2;
            int rowHeight = Height / (int)_grid.Size - 2;
            for (uint blockRow = 0; blockRow < _grid.Dimensionality; blockRow++)
            for (uint blockCol = 0; blockCol < _grid.Dimensionality; blockCol++)
            {
                // Initialize block contents.
                TableLayoutPanel blockPanel = new TableLayoutPanel();
                blockPanel.Size = new Size(blockWidth, blockHeight);
                blockPanel.Margin = new Padding(0);
                blockPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                blockPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                blockPanel.RowCount = (int)_grid.Dimensionality;
                blockPanel.ColumnCount = (int)_grid.Dimensionality;
                blockPanel.RowStyles.Clear();
                blockPanel.ColumnStyles.Clear();

                // Set cell sizes.
                for (uint i = 0; i < _grid.Dimensionality; i++)
                {
                    blockPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, colWidth));
                    blockPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
                }

                // Populate cell controls.
                for (uint innerRow = 0; innerRow < _grid.Dimensionality; innerRow++)
                for (uint innerCol = 0; innerCol < _grid.Dimensionality; innerCol++)
                {
                    uint row = _grid.Dimensionality * blockRow + innerRow;
                    uint col = _grid.Dimensionality * blockCol + innerCol;

                    _cellLabels[row, col] = new Label();
                    _cellLabels[row, col].TextAlign = ContentAlignment.MiddleCenter;
                    blockPanel.Controls.Add(_cellLabels[row, col]);
                    _cellLabels[row, col].Width = colWidth;
                    _cellLabels[row, col].Height = rowHeight;
                    _cellLabels[row, col].Click += new EventHandler(CellClicked);
                    Font f = new Font(_cellLabels[row, col].Font, FontStyle.Bold);
                    _cellLabels[row, col].Font = f;
                }

                _tableLayout.Controls.Add(blockPanel);
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            // Set contents of each cell.
            for (uint row = 0; row < _grid.Size; row++)
            for (uint col = 0; col < _grid.Size; col++)
            {
                _cellLabels[row, col].Text = (_grid[row, col] < 0) ? "" : _alphabet.Substring(_grid[row, col], 1);
                _cellLabels[row, col].ForeColor = Color.Black;
            }

            // Format for errors.
            foreach (SudokuGridError err in _grid.GetErrors())
            {
                switch (err.Type)
                {
                    case SudokuGridError.ErrorType.Row:
                        for (uint col = 0; col < _grid.Size; col++)
                        {
                            if (_grid[err.Row, col] == err.Value)
                                _cellLabels[err.Row, col].ForeColor = Color.Red;
                        }
                        break;
                    case SudokuGridError.ErrorType.Col:
                        for (uint row = 0; row < _grid.Size; row++)
                        {
                            if (_grid[row, err.Col] == err.Value)
                                _cellLabels[row, err.Col].ForeColor = Color.Red;
                        }
                        break;
                    case SudokuGridError.ErrorType.Block:
                        {
                            uint rowBase = _grid.Dimensionality * err.Row;
                            uint colBase = _grid.Dimensionality * err.Col;
                            for (uint innerRow = 0; innerRow < _grid.Dimensionality; innerRow++)
                            for (uint innerCol = 0; innerCol < _grid.Dimensionality; innerCol++)
                            {
                                uint row = rowBase + innerRow;
                                uint col = colBase + innerCol;
                                if (_grid[row,col] == err.Value)
                                    _cellLabels[row, col].ForeColor = Color.Red;
                            }
                        }
                        break;
                    case SudokuGridError.ErrorType.Value:
                        _cellLabels[err.Row, err.Col].ForeColor = Color.Red;
                        break;
                }
            }
        }

        private void CellClicked(object sender, EventArgs e)
        {
            // Use a floating TextBox control to allow editing of the cells.
            // A little ugly, but grid controls are all terrible in .NET...
            Label src = sender as Label;
            src.Controls.Add(_editor);
            _editor.Size = src.Size;
            _editor.Show();
            _editor.BringToFront();
            _editor.Text = src.Text;
            _editor.SelectAll();
            _editor.Focus();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _tableLayout.Size = Size;
        }
    }
}
