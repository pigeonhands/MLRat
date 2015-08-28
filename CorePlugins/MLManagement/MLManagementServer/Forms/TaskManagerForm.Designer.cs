namespace MLManagementServer.Forms
{
    partial class TaskManagerForm
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
            this.lvProcessList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvProcessList
            // 
            this.lvProcessList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvProcessList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvProcessList.FullRowSelect = true;
            this.lvProcessList.GridLines = true;
            this.lvProcessList.Location = new System.Drawing.Point(0, 0);
            this.lvProcessList.Name = "lvProcessList";
            this.lvProcessList.Size = new System.Drawing.Size(345, 354);
            this.lvProcessList.TabIndex = 0;
            this.lvProcessList.UseCompatibleStateImageBehavior = false;
            this.lvProcessList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 129;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "ID";
            this.columnHeader2.Width = 133;
            // 
            // TaskManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 354);
            this.Controls.Add(this.lvProcessList);
            this.Name = "TaskManagerForm";
            this.Text = "TaskManagerForm";
            this.Load += new System.EventHandler(this.TaskManagerForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvProcessList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}