namespace RezkaInfo
{
    partial class repairRylon_form
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
            this.button_Repair = new System.Windows.Forms.Button();
            this.button_exit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_Repair
            // 
            this.button_Repair.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_Repair.Location = new System.Drawing.Point(22, 26);
            this.button_Repair.Name = "button_Repair";
            this.button_Repair.Size = new System.Drawing.Size(128, 59);
            this.button_Repair.TabIndex = 5;
            this.button_Repair.Text = "ВОССТАНОВИТЬ";
            this.button_Repair.UseVisualStyleBackColor = true;
            this.button_Repair.Click += new System.EventHandler(this.button_Repair_Click);
            // 
            // button_exit
            // 
            this.button_exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_exit.Location = new System.Drawing.Point(174, 26);
            this.button_exit.Name = "button_exit";
            this.button_exit.Size = new System.Drawing.Size(88, 59);
            this.button_exit.TabIndex = 4;
            this.button_exit.Text = "ОТМЕНА";
            this.button_exit.UseVisualStyleBackColor = true;
            this.button_exit.Click += new System.EventHandler(this.button_exit_Click);
            // 
            // repairRylon_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 110);
            this.ControlBox = false;
            this.Controls.Add(this.button_Repair);
            this.Controls.Add(this.button_exit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "repairRylon_form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ТОВ \"ИТАК\"";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Repair;
        private System.Windows.Forms.Button button_exit;
    }
}