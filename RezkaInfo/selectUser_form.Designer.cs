namespace RezkaInfo
{
    partial class selectUser_form
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
            this.label_checkMen = new System.Windows.Forms.Label();
            this.comboBox_checkMen = new System.Windows.Forms.ComboBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_exit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_checkMen
            // 
            this.label_checkMen.AutoSize = true;
            this.label_checkMen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_checkMen.Location = new System.Drawing.Point(21, 9);
            this.label_checkMen.Name = "label_checkMen";
            this.label_checkMen.Size = new System.Drawing.Size(111, 16);
            this.label_checkMen.TabIndex = 0;
            this.label_checkMen.Text = "Проверяющий";
            // 
            // comboBox_checkMen
            // 
            this.comboBox_checkMen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_checkMen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox_checkMen.FormattingEnabled = true;
            this.comboBox_checkMen.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.comboBox_checkMen.Location = new System.Drawing.Point(17, 35);
            this.comboBox_checkMen.Name = "comboBox_checkMen";
            this.comboBox_checkMen.Size = new System.Drawing.Size(121, 24);
            this.comboBox_checkMen.TabIndex = 1;
            // 
            // button_ok
            // 
            this.button_ok.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_ok.Location = new System.Drawing.Point(9, 88);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(66, 35);
            this.button_ok.TabIndex = 2;
            this.button_ok.Text = "ОК";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_exit
            // 
            this.button_exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_exit.Location = new System.Drawing.Point(81, 88);
            this.button_exit.Name = "button_exit";
            this.button_exit.Size = new System.Drawing.Size(63, 35);
            this.button_exit.TabIndex = 3;
            this.button_exit.Text = "Выход";
            this.button_exit.UseVisualStyleBackColor = true;
            this.button_exit.Click += new System.EventHandler(this.button_exit_Click);
            // 
            // selectUser_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(152, 134);
            this.ControlBox = false;
            this.Controls.Add(this.button_exit);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.comboBox_checkMen);
            this.Controls.Add(this.label_checkMen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "selectUser_form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Выбор проверяющего";
            this.Load += new System.EventHandler(this.selectUser_form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_checkMen;
        private System.Windows.Forms.ComboBox comboBox_checkMen;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_exit;
    }
}