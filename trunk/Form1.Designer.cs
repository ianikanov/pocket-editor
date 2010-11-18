namespace JSEditor
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnVerify = new System.Windows.Forms.Button();
            this.btnSnippet = new System.Windows.Forms.Button();
            this.btnSaveAll = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnUncomment = new System.Windows.Forms.Button();
            this.btnComment = new System.Windows.Forms.Button();
            this.miNew = new System.Windows.Forms.MenuItem();
            this.miOpen = new System.Windows.Forms.MenuItem();
            this.miSave = new System.Windows.Forms.MenuItem();
            this.miSaveAs = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.miSaveAll = new System.Windows.Forms.MenuItem();
            this.miClose = new System.Windows.Forms.MenuItem();
            this.miPhp = new System.Windows.Forms.MenuItem();
            this.miFlash = new System.Windows.Forms.MenuItem();
            this.miHtml = new System.Windows.Forms.MenuItem();
            this.miCSharp = new System.Windows.Forms.MenuItem();
            this.miCPlus = new System.Windows.Forms.MenuItem();
            this.menuLanguage = new System.Windows.Forms.MenuItem();
            this.miSql = new System.Windows.Forms.MenuItem();
            this.miSettings = new System.Windows.Forms.MenuItem();
            this.miAbout = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.miSearch = new System.Windows.Forms.MenuItem();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.miTabRight = new System.Windows.Forms.MenuItem();
            this.miTabLeft = new System.Windows.Forms.MenuItem();
            this.pFirst = new System.Windows.Forms.TabPage();
            this.rtbMain = new JSEditor.cRichTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.panel1.SuspendLayout();
            this.pFirst.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputPanel1
            // 
            this.inputPanel1.EnabledChanged += new System.EventHandler(this.inputPanel1_EnabledChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnVerify);
            this.panel1.Controls.Add(this.btnSnippet);
            this.panel1.Controls.Add(this.btnSaveAll);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnUncomment);
            this.panel1.Controls.Add(this.btnComment);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(263, 30);
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(161, 3);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(20, 20);
            this.btnVerify.TabIndex = 8;
            this.btnVerify.Text = "V";
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // btnSnippet
            // 
            this.btnSnippet.Location = new System.Drawing.Point(130, 3);
            this.btnSnippet.Name = "btnSnippet";
            this.btnSnippet.Size = new System.Drawing.Size(24, 20);
            this.btnSnippet.TabIndex = 7;
            this.btnSnippet.Text = "Sp";
            this.btnSnippet.Click += new System.EventHandler(this.btnSnippet_Click);
            // 
            // btnSaveAll
            // 
            this.btnSaveAll.Location = new System.Drawing.Point(86, 3);
            this.btnSaveAll.Name = "btnSaveAll";
            this.btnSaveAll.Size = new System.Drawing.Size(23, 20);
            this.btnSaveAll.TabIndex = 6;
            this.btnSaveAll.Text = "SA";
            this.btnSaveAll.Click += new System.EventHandler(this.btnSaveAll_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(56, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(24, 20);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "S";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnUncomment
            // 
            this.btnUncomment.Location = new System.Drawing.Point(29, 3);
            this.btnUncomment.Name = "btnUncomment";
            this.btnUncomment.Size = new System.Drawing.Size(20, 20);
            this.btnUncomment.TabIndex = 4;
            this.btnUncomment.Text = "U";
            this.btnUncomment.Click += new System.EventHandler(this.btnUncomment_Click);
            // 
            // btnComment
            // 
            this.btnComment.Location = new System.Drawing.Point(3, 3);
            this.btnComment.Name = "btnComment";
            this.btnComment.Size = new System.Drawing.Size(20, 20);
            this.btnComment.TabIndex = 3;
            this.btnComment.Text = "C";
            this.btnComment.Click += new System.EventHandler(this.btnComment_Click);
            // 
            // miNew
            // 
            this.miNew.Text = "New";
            this.miNew.Click += new System.EventHandler(this.miNew_Click);
            // 
            // miOpen
            // 
            this.miOpen.Text = "Open...";
            this.miOpen.Click += new System.EventHandler(this.miOpen_Click);
            // 
            // miSave
            // 
            this.miSave.Text = "Save";
            this.miSave.Click += new System.EventHandler(this.miSave_Click);
            // 
            // miSaveAs
            // 
            this.miSaveAs.Text = "Save as...";
            this.miSaveAs.Click += new System.EventHandler(this.miSaveAs_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this.miNew);
            this.menuItem1.MenuItems.Add(this.miOpen);
            this.menuItem1.MenuItems.Add(this.miSave);
            this.menuItem1.MenuItems.Add(this.miSaveAs);
            this.menuItem1.MenuItems.Add(this.miSaveAll);
            this.menuItem1.MenuItems.Add(this.miClose);
            this.menuItem1.Text = "File";
            // 
            // miSaveAll
            // 
            this.miSaveAll.Text = "Save all";
            this.miSaveAll.Click += new System.EventHandler(this.miSaveAll_Click);
            // 
            // miClose
            // 
            this.miClose.Text = "Close";
            this.miClose.Click += new System.EventHandler(this.miClose_Click);
            // 
            // miPhp
            // 
            this.miPhp.Checked = true;
            this.miPhp.Text = "PHP";
            this.miPhp.Click += new System.EventHandler(this.miLang_Click);
            // 
            // miFlash
            // 
            this.miFlash.Text = "Flash AS";
            this.miFlash.Click += new System.EventHandler(this.miLang_Click);
            // 
            // miHtml
            // 
            this.miHtml.Text = "HTML + JS";
            this.miHtml.Click += new System.EventHandler(this.miLang_Click);
            // 
            // miCSharp
            // 
            this.miCSharp.Text = "C#";
            this.miCSharp.Click += new System.EventHandler(this.miLang_Click);
            // 
            // miCPlus
            // 
            this.miCPlus.Text = "C++";
            this.miCPlus.Click += new System.EventHandler(this.miLang_Click);
            // 
            // menuLanguage
            // 
            this.menuLanguage.MenuItems.Add(this.miPhp);
            this.menuLanguage.MenuItems.Add(this.miFlash);
            this.menuLanguage.MenuItems.Add(this.miHtml);
            this.menuLanguage.MenuItems.Add(this.miCSharp);
            this.menuLanguage.MenuItems.Add(this.miCPlus);
            this.menuLanguage.MenuItems.Add(this.miSql);
            this.menuLanguage.Text = "Language";
            // 
            // miSql
            // 
            this.miSql.Text = "SQL";
            this.miSql.Click += new System.EventHandler(this.miLang_Click);
            // 
            // miSettings
            // 
            this.miSettings.Text = "Settings...";
            this.miSettings.Click += new System.EventHandler(this.miSettings_Click);
            // 
            // miAbout
            // 
            this.miAbout.Text = "About";
            this.miAbout.Click += new System.EventHandler(this.miAbout_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.MenuItems.Add(this.miSettings);
            this.menuItem12.MenuItems.Add(this.miAbout);
            this.menuItem12.MenuItems.Add(this.miSearch);
            this.menuItem12.Text = "System";
            // 
            // miSearch
            // 
            this.miSearch.Text = "Search...";
            this.miSearch.Click += new System.EventHandler(this.miSearch_Click);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuLanguage);
            this.mainMenu1.MenuItems.Add(this.menuItem12);
            this.mainMenu1.MenuItems.Add(this.miTabRight);
            this.mainMenu1.MenuItems.Add(this.miTabLeft);
            // 
            // miTabRight
            // 
            this.miTabRight.Text = ">>";
            this.miTabRight.Click += new System.EventHandler(this.miTabRight_Click);
            // 
            // miTabLeft
            // 
            this.miTabLeft.Text = "<<";
            this.miTabLeft.Click += new System.EventHandler(this.miTabLeft_Click);
            // 
            // pFirst
            // 
            this.pFirst.Controls.Add(this.rtbMain);
            this.pFirst.Location = new System.Drawing.Point(4, 25);
            this.pFirst.Name = "pFirst";
            this.pFirst.Size = new System.Drawing.Size(255, 258);
            this.pFirst.Text = "Untitled";
            // 
            // rtbMain
            // 
            this.rtbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbMain.Location = new System.Drawing.Point(0, 0);
            this.rtbMain.Name = "rtbMain";
            this.rtbMain.Size = new System.Drawing.Size(255, 258);
            this.rtbMain.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.pFirst);
            this.tabControl1.Location = new System.Drawing.Point(0, 32);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(263, 287);
            this.tabControl1.TabIndex = 5;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(263, 322);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Menu = this.mainMenu1;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "JSEditor";
            this.GotFocus += new System.EventHandler(this.Form1_GotFocus);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.panel1.ResumeLayout(false);
            this.pFirst.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.WindowsCE.Forms.InputPanel inputPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuItem miNew;
        private System.Windows.Forms.MenuItem miOpen;
        private System.Windows.Forms.MenuItem miSave;
        private System.Windows.Forms.MenuItem miSaveAs;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem miPhp;
        private System.Windows.Forms.MenuItem miFlash;
        private System.Windows.Forms.MenuItem miHtml;
        private System.Windows.Forms.MenuItem miCSharp;
        private System.Windows.Forms.MenuItem miCPlus;
        private System.Windows.Forms.MenuItem menuLanguage;
        private System.Windows.Forms.MenuItem miSettings;
        private System.Windows.Forms.MenuItem miAbout;
        private System.Windows.Forms.MenuItem menuItem12;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.Button btnUncomment;
        private System.Windows.Forms.Button btnComment;
        private System.Windows.Forms.TabPage pFirst;
        private cRichTextBox rtbMain;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.MenuItem miSaveAll;
        private System.Windows.Forms.MenuItem miClose;
        private System.Windows.Forms.Button btnSaveAll;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button btnSnippet;
        private System.Windows.Forms.MenuItem miSql;
        private System.Windows.Forms.MenuItem miTabRight;
        private System.Windows.Forms.MenuItem miTabLeft;
        private System.Windows.Forms.MenuItem miSearch;
    }
}

