namespace SudokuSolverApp
{
    partial class SudokuControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // _tableLayout
            // 
            this._tableLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tableLayout.BackColor = System.Drawing.SystemColors.Window;
            this._tableLayout.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this._tableLayout.ColumnCount = 1;
            this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayout.Location = new System.Drawing.Point(0, 0);
            this._tableLayout.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayout.Name = "_tableLayout";
            this._tableLayout.RowCount = 1;
            this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayout.Size = new System.Drawing.Size(303, 254);
            this._tableLayout.TabIndex = 0;
            // 
            // SudokuControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tableLayout);
            this.Name = "SudokuControl";
            this.Size = new System.Drawing.Size(306, 257);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _tableLayout;
    }
}
