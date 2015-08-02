namespace ModuleBot_2.Forms
{
    partial class AddCommandForm
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
            this.addButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.Isregex = new System.Windows.Forms.CheckBox();
            this.modOnly = new System.Windows.Forms.CheckBox();
            this.CaseSensitive = new System.Windows.Forms.CheckBox();
            this.FlagTextbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CommandName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(6, 85);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(269, 23);
            this.addButton.TabIndex = 18;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(229, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(46, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "Select";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Isregex
            // 
            this.Isregex.AutoSize = true;
            this.Isregex.Location = new System.Drawing.Point(179, 62);
            this.Isregex.Name = "Isregex";
            this.Isregex.Size = new System.Drawing.Size(57, 17);
            this.Isregex.TabIndex = 16;
            this.Isregex.Text = "Regex";
            this.Isregex.UseVisualStyleBackColor = true;
            // 
            // modOnly
            // 
            this.modOnly.AutoSize = true;
            this.modOnly.Location = new System.Drawing.Point(6, 62);
            this.modOnly.Name = "modOnly";
            this.modOnly.Size = new System.Drawing.Size(68, 17);
            this.modOnly.TabIndex = 15;
            this.modOnly.Text = "ModOnly";
            this.modOnly.UseVisualStyleBackColor = true;
            // 
            // CaseSensitive
            // 
            this.CaseSensitive.AutoSize = true;
            this.CaseSensitive.Location = new System.Drawing.Point(80, 62);
            this.CaseSensitive.Name = "CaseSensitive";
            this.CaseSensitive.Size = new System.Drawing.Size(96, 17);
            this.CaseSensitive.TabIndex = 14;
            this.CaseSensitive.Text = "Case Sensitive";
            this.CaseSensitive.UseVisualStyleBackColor = true;
            // 
            // FlagTextbox
            // 
            this.FlagTextbox.Location = new System.Drawing.Point(93, 30);
            this.FlagTextbox.Name = "FlagTextbox";
            this.FlagTextbox.Size = new System.Drawing.Size(182, 20);
            this.FlagTextbox.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 16);
            this.label3.TabIndex = 12;
            this.label3.Text = "Flag:";
            // 
            // CommandName
            // 
            this.CommandName.AutoSize = true;
            this.CommandName.Location = new System.Drawing.Point(90, 5);
            this.CommandName.Name = "CommandName";
            this.CommandName.Size = new System.Drawing.Size(78, 13);
            this.CommandName.TabIndex = 11;
            this.CommandName.Text = "None Selected";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "Command:";
            // 
            // AddCommandForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 110);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Isregex);
            this.Controls.Add(this.modOnly);
            this.Controls.Add(this.CaseSensitive);
            this.Controls.Add(this.FlagTextbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CommandName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AddCommandForm";
            this.Text = "Add Command";
            this.Load += new System.EventHandler(this.AddCommandForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox Isregex;
        private System.Windows.Forms.CheckBox modOnly;
        private System.Windows.Forms.CheckBox CaseSensitive;
        private System.Windows.Forms.TextBox FlagTextbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label CommandName;
        private System.Windows.Forms.Label label1;
    }
}