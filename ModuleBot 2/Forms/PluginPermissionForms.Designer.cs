namespace ModuleBot_2.Forms
{
    partial class PluginPermissionForms
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
            this.permissionsList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // permissionsList
            // 
            this.permissionsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.permissionsList.FormattingEnabled = true;
            this.permissionsList.Location = new System.Drawing.Point(0, 0);
            this.permissionsList.Name = "permissionsList";
            this.permissionsList.Size = new System.Drawing.Size(308, 201);
            this.permissionsList.TabIndex = 0;
            // 
            // PluginPermissionForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 201);
            this.Controls.Add(this.permissionsList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "PluginPermissionForms";
            this.Text = "-";
            this.Load += new System.EventHandler(this.PluginPermissionForms_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox permissionsList;
    }
}