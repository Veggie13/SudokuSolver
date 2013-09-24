namespace SudokuSolverApp
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._btnNew = new System.Windows.Forms.Button();
            this._btnLoad = new System.Windows.Forms.Button();
            this._btnSave = new System.Windows.Forms.Button();
            this._btnSolve = new System.Windows.Forms.Button();
            this._cboSolver = new System.Windows.Forms.ComboBox();
            this._sudokuControl = new SudokuSolverApp.SudokuControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this._sudokuControl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._btnNew, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._btnLoad, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._btnSave, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this._btnSolve, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this._cboSolver, 3, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(506, 535);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // _btnNew
            // 
            this._btnNew.Location = new System.Drawing.Point(3, 509);
            this._btnNew.Name = "_btnNew";
            this._btnNew.Size = new System.Drawing.Size(75, 23);
            this._btnNew.TabIndex = 2;
            this._btnNew.Text = "New";
            this._btnNew.UseVisualStyleBackColor = true;
            this._btnNew.Click += new System.EventHandler(this._btnNew_Click);
            // 
            // _btnLoad
            // 
            this._btnLoad.Location = new System.Drawing.Point(84, 509);
            this._btnLoad.Name = "_btnLoad";
            this._btnLoad.Size = new System.Drawing.Size(75, 23);
            this._btnLoad.TabIndex = 2;
            this._btnLoad.Text = "Load";
            this._btnLoad.UseVisualStyleBackColor = true;
            this._btnLoad.Click += new System.EventHandler(this._btnLoad_Click);
            // 
            // _btnSave
            // 
            this._btnSave.Location = new System.Drawing.Point(165, 509);
            this._btnSave.Name = "_btnSave";
            this._btnSave.Size = new System.Drawing.Size(75, 23);
            this._btnSave.TabIndex = 2;
            this._btnSave.Text = "Save";
            this._btnSave.UseVisualStyleBackColor = true;
            this._btnSave.Click += new System.EventHandler(this._btnSave_Click);
            // 
            // _btnSolve
            // 
            this._btnSolve.Location = new System.Drawing.Point(428, 509);
            this._btnSolve.Name = "_btnSolve";
            this._btnSolve.Size = new System.Drawing.Size(75, 23);
            this._btnSolve.TabIndex = 2;
            this._btnSolve.Text = "SOLVE!";
            this._btnSolve.UseVisualStyleBackColor = true;
            this._btnSolve.Click += new System.EventHandler(this._btnSolve_Click);
            // 
            // _cboSolver
            // 
            this._cboSolver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._cboSolver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboSolver.FormattingEnabled = true;
            this._cboSolver.Items.AddRange(new object[] {
            "Backtrack",
            "Naive AlgorithmX",
            "Dancing Links"});
            this._cboSolver.Location = new System.Drawing.Point(246, 510);
            this._cboSolver.Name = "_cboSolver";
            this._cboSolver.Size = new System.Drawing.Size(176, 21);
            this._cboSolver.TabIndex = 3;
            this._cboSolver.SelectedIndexChanged += new System.EventHandler(this._cboSolver_SelectedIndexChanged);
            // 
            // _sudokuControl
            // 
            this._sudokuControl.Alphabet = null;
            this._sudokuControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._sudokuControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.SetColumnSpan(this._sudokuControl, 5);
            this._sudokuControl.Grid = null;
            this._sudokuControl.Location = new System.Drawing.Point(3, 3);
            this._sudokuControl.Name = "_sudokuControl";
            this._sudokuControl.Size = new System.Drawing.Size(500, 500);
            this._sudokuControl.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 545);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Sudoku Solver!";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SudokuControl _sudokuControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button _btnLoad;
        private System.Windows.Forms.Button _btnSolve;
        private System.Windows.Forms.Button _btnNew;
        private System.Windows.Forms.Button _btnSave;
        private System.Windows.Forms.ComboBox _cboSolver;
    }
}

