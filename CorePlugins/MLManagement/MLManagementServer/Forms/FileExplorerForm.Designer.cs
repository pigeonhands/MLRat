namespace MLManagementServer.Forms
{
    partial class FileExplorerForm
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
            this.components = new System.ComponentModel.Container();
            this.lvFileView = new System.Windows.Forms.ListView();
            this.chFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmFileExplorer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmFileExplorer.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvFileView
            // 
            this.lvFileView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFile,
            this.chSize});
            this.lvFileView.ContextMenuStrip = this.cmFileExplorer;
            this.lvFileView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFileView.FullRowSelect = true;
            this.lvFileView.GridLines = true;
            this.lvFileView.Location = new System.Drawing.Point(0, 0);
            this.lvFileView.MultiSelect = false;
            this.lvFileView.Name = "lvFileView";
            this.lvFileView.Size = new System.Drawing.Size(553, 326);
            this.lvFileView.TabIndex = 3;
            this.lvFileView.UseCompatibleStateImageBehavior = false;
            this.lvFileView.View = System.Windows.Forms.View.Details;
            this.lvFileView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvFileView_MouseDoubleClick);
            // 
            // chFile
            // 
            this.chFile.Text = "Name";
            this.chFile.Width = 248;
            // 
            // chSize
            // 
            this.chSize.Text = "Size";
            this.chSize.Width = 194;
            // 
            // cmFileExplorer
            // 
            this.cmFileExplorer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadFileToolStripMenuItem,
            this.refreshToolStripMenuItem});
            this.cmFileExplorer.Name = "cmFileExplorer";
            this.cmFileExplorer.Size = new System.Drawing.Size(214, 48);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // downloadFileToolStripMenuItem
            // 
            this.downloadFileToolStripMenuItem.Name = "downloadFileToolStripMenuItem";
            this.downloadFileToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.downloadFileToolStripMenuItem.Text = "Download File (10 kb max)";
            this.downloadFileToolStripMenuItem.Click += new System.EventHandler(this.downloadFileToolStripMenuItem_Click);
            // 
            // FileExplorerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 326);
            this.Controls.Add(this.lvFileView);
            this.Name = "FileExplorerForm";
            this.Text = "FileExplorerForm";
            this.Load += new System.EventHandler(this.FileExplorerForm_Load);
            this.cmFileExplorer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView lvFileView;
        private System.Windows.Forms.ColumnHeader chFile;
        private System.Windows.Forms.ColumnHeader chSize;
        private System.Windows.Forms.ContextMenuStrip cmFileExplorer;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadFileToolStripMenuItem;
    }
}