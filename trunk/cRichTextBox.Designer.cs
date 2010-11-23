namespace JSEditor
{
    partial class cRichTextBox
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
            this.lblLines = new System.Windows.Forms.Label();
            this.txtMain = new System.Windows.Forms.TextBox();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.miCut = new System.Windows.Forms.MenuItem();
            this.miCopy = new System.Windows.Forms.MenuItem();
            this.miPaste = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // lblLines
            // 
            this.lblLines.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblLines.Location = new System.Drawing.Point(0, 0);
            this.lblLines.Name = "lblLines";
            this.lblLines.Size = new System.Drawing.Size(10, 139);
            this.lblLines.Text = "123";
            // 
            // txtMain
            // 
            this.txtMain.AcceptsReturn = true;
            this.txtMain.AcceptsTab = true;
            this.txtMain.ContextMenu = this.contextMenu1;
            this.txtMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMain.Location = new System.Drawing.Point(10, 0);
            this.txtMain.Multiline = true;
            this.txtMain.Name = "txtMain";
            this.txtMain.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMain.Size = new System.Drawing.Size(139, 139);
            this.txtMain.TabIndex = 1;
            this.txtMain.WordWrap = false;
            this.txtMain.TextChanged += new System.EventHandler(this.txtMain_TextChanged);
            this.txtMain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMain_KeyPress);
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.Add(this.miCut);
            this.contextMenu1.MenuItems.Add(this.miCopy);
            this.contextMenu1.MenuItems.Add(this.miPaste);
            // 
            // miCut
            // 
            this.miCut.Text = "Cut";
            this.miCut.Click += new System.EventHandler(this.miCut_Click);
            // 
            // miCopy
            // 
            this.miCopy.Text = "Copy";
            this.miCopy.Click += new System.EventHandler(this.miCopy_Click);
            // 
            // miPaste
            // 
            this.miPaste.Text = "Paste";
            this.miPaste.Click += new System.EventHandler(this.miPaste_Click);
            // 
            // cRichTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.txtMain);
            this.Controls.Add(this.lblLines);
            this.Name = "cRichTextBox";
            this.Size = new System.Drawing.Size(149, 139);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblLines;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem miCopy;
        private System.Windows.Forms.MenuItem miPaste;
        private System.Windows.Forms.MenuItem miCut;
        internal System.Windows.Forms.TextBox txtMain;
    }
}
