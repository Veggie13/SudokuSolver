using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SudokuSolver;
using System.IO;
using AlgorithmX;

namespace SudokuSolverApp
{
    public partial class MainForm : Form
    {
        const string ALPHABET = "123456789";
        const string UNKNOWNS = ".0";

        SudokuParser _parser = new SudokuParser();
        ISudokuSolver _solver = new BacktrackSudokuSolver();

        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _sudokuControl.Alphabet = ALPHABET;

            _cboSolver.SelectedIndex = 0;
        }

        private void _btnNew_Click(object sender, EventArgs e)
        {
            _sudokuControl.Grid = new SudokuGrid(3);
        }

        private void _btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = "txt";
            dlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string path = dlg.FileName;
                StreamReader reader = new StreamReader(path);
                try
                {
                    string content = reader.ReadToEnd();
                    _sudokuControl.Grid = _parser.ParseString(content, ALPHABET, UNKNOWNS);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    reader.Close();
                }
            }
        }

        private void _btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = "txt";
            dlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string path = dlg.FileName;
                StreamWriter writer = new StreamWriter(path);
                try
                {
                    string render = _parser.Render(ALPHABET, _sudokuControl.Grid);
                    writer.Write(render);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    writer.Flush();
                    writer.Close();
                }
            }
        }

        private void _btnSolve_Click(object sender, EventArgs e)
        {
            SudokuGrid result = _solver.FindFirstSolution(_sudokuControl.Grid);
            if (result == null)
                MessageBox.Show("No solutions were found.");
            else
            {
                _sudokuControl.Grid = result;
            }
        }

        private void _cboSolver_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (_cboSolver.SelectedIndex)
            {
                case 0:
                    _solver = new BacktrackSudokuSolver();
                    break;
                case 1:
                    _solver = new AlgXSudokuSolver(new NaiveAlgorithmX());
                    break;
                case 2:
                    _solver = new AlgXSudokuSolver(new DLX());
                    break;
                default:
                    break;
            }
        }
    }
}
