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
            IList<SudokuGrid> results = _solver.FindAllSolutions(_sudokuControl.Grid);
            if (results.Count < 1)
                MessageBox.Show("No solutions were found.");
            else
            {
                if (results.Count > 1)
                    MessageBox.Show("More than one solution was found. Returning first.");
                _sudokuControl.Grid = results[0];
            }
        }
    }
}
