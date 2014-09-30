namespace Twitch_Downloader
{
    partial class mainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.vodURL = new System.Windows.Forms.TextBox();
            this.viewButton = new System.Windows.Forms.Button();
            this.status = new System.Windows.Forms.TextBox();
            this.downloadButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar3 = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar4 = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.rate1 = new System.Windows.Forms.Label();
            this.rate2 = new System.Windows.Forms.Label();
            this.rate4 = new System.Windows.Forms.Label();
            this.rate3 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.containerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mKVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mP4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter VOD URL:";
            // 
            // vodURL
            // 
            this.vodURL.Location = new System.Drawing.Point(104, 31);
            this.vodURL.Name = "vodURL";
            this.vodURL.Size = new System.Drawing.Size(448, 20);
            this.vodURL.TabIndex = 1;
            // 
            // viewButton
            // 
            this.viewButton.Location = new System.Drawing.Point(558, 29);
            this.viewButton.Name = "viewButton";
            this.viewButton.Size = new System.Drawing.Size(75, 23);
            this.viewButton.TabIndex = 2;
            this.viewButton.Text = "&View Files";
            this.viewButton.UseVisualStyleBackColor = true;
            this.viewButton.Click += new System.EventHandler(this.viewButton_Click);
            // 
            // status
            // 
            this.status.Location = new System.Drawing.Point(15, 58);
            this.status.Multiline = true;
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.status.Size = new System.Drawing.Size(618, 252);
            this.status.TabIndex = 3;
            // 
            // downloadButton
            // 
            this.downloadButton.Location = new System.Drawing.Point(15, 429);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(618, 50);
            this.downloadButton.TabIndex = 4;
            this.downloadButton.Text = "&Download Videos";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 320);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Download Thread #1";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(129, 316);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(349, 23);
            this.progressBar1.TabIndex = 7;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(129, 344);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(349, 23);
            this.progressBar2.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 348);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Download Thread #2";
            // 
            // progressBar3
            // 
            this.progressBar3.Location = new System.Drawing.Point(129, 372);
            this.progressBar3.Name = "progressBar3";
            this.progressBar3.Size = new System.Drawing.Size(349, 23);
            this.progressBar3.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 376);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Download Thread #3";
            // 
            // progressBar4
            // 
            this.progressBar4.Location = new System.Drawing.Point(129, 400);
            this.progressBar4.Name = "progressBar4";
            this.progressBar4.Size = new System.Drawing.Size(349, 23);
            this.progressBar4.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 404);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Download Thread #4";
            // 
            // rate1
            // 
            this.rate1.AutoSize = true;
            this.rate1.Location = new System.Drawing.Point(485, 319);
            this.rate1.Name = "rate1";
            this.rate1.Size = new System.Drawing.Size(0, 13);
            this.rate1.TabIndex = 14;
            // 
            // rate2
            // 
            this.rate2.AutoSize = true;
            this.rate2.Location = new System.Drawing.Point(485, 348);
            this.rate2.Name = "rate2";
            this.rate2.Size = new System.Drawing.Size(0, 13);
            this.rate2.TabIndex = 15;
            // 
            // rate4
            // 
            this.rate4.AutoSize = true;
            this.rate4.Location = new System.Drawing.Point(485, 405);
            this.rate4.Name = "rate4";
            this.rate4.Size = new System.Drawing.Size(0, 13);
            this.rate4.TabIndex = 17;
            // 
            // rate3
            // 
            this.rate3.AutoSize = true;
            this.rate3.Location = new System.Drawing.Point(485, 376);
            this.rate3.Name = "rate3";
            this.rate3.Size = new System.Drawing.Size(0, 13);
            this.rate3.TabIndex = 16;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(648, 24);
            this.menuStrip1.TabIndex = 18;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.containerToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "&Settings";
            // 
            // containerToolStripMenuItem
            // 
            this.containerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mKVToolStripMenuItem,
            this.mP4ToolStripMenuItem});
            this.containerToolStripMenuItem.Name = "containerToolStripMenuItem";
            this.containerToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.containerToolStripMenuItem.Text = "&Video Container";
            // 
            // mKVToolStripMenuItem
            // 
            this.mKVToolStripMenuItem.Checked = true;
            this.mKVToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mKVToolStripMenuItem.Name = "mKVToolStripMenuItem";
            this.mKVToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.mKVToolStripMenuItem.Text = "MKV";
            this.mKVToolStripMenuItem.Click += new System.EventHandler(this.mKVToolStripMenuItem_Click);
            // 
            // mP4ToolStripMenuItem
            // 
            this.mP4ToolStripMenuItem.Name = "mP4ToolStripMenuItem";
            this.mP4ToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.mP4ToolStripMenuItem.Text = "MP4";
            this.mP4ToolStripMenuItem.Click += new System.EventHandler(this.mP4ToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem1.Text = "&About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem1_Click);
            // 
            // mainForm
            // 
            this.AcceptButton = this.viewButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 497);
            this.Controls.Add(this.rate4);
            this.Controls.Add(this.rate3);
            this.Controls.Add(this.rate2);
            this.Controls.Add(this.rate1);
            this.Controls.Add(this.progressBar4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.progressBar3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.downloadButton);
            this.Controls.Add(this.status);
            this.Controls.Add(this.viewButton);
            this.Controls.Add(this.vodURL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "mainForm";
            this.Text = "Twitch Downloader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox vodURL;
        private System.Windows.Forms.Button viewButton;
        private System.Windows.Forms.Button downloadButton;
        public System.Windows.Forms.TextBox status;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar progressBar4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label rate1;
        private System.Windows.Forms.Label rate2;
        private System.Windows.Forms.Label rate4;
        private System.Windows.Forms.Label rate3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem containerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mKVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mP4ToolStripMenuItem;
    }
}

