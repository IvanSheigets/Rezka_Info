namespace RezkaInfo
{
    partial class settings_form
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
            this.checkBox_Change = new System.Windows.Forms.CheckBox();
            this.checkBox_See = new System.Windows.Forms.CheckBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.label_printerEtiketok = new System.Windows.Forms.Label();
            this.comboBox_printerEtiketok = new System.Windows.Forms.ComboBox();
            this.checkBox_Rezka = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBox_Change
            // 
            this.checkBox_Change.AutoSize = true;
            this.checkBox_Change.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox_Change.Location = new System.Drawing.Point(12, 38);
            this.checkBox_Change.Name = "checkBox_Change";
            this.checkBox_Change.Size = new System.Drawing.Size(152, 20);
            this.checkBox_Change.TabIndex = 0;
            this.checkBox_Change.Text = "Редактирование";
            this.checkBox_Change.UseVisualStyleBackColor = true;
            this.checkBox_Change.CheckedChanged += new System.EventHandler(this.checkBox_Change_CheckedChanged);
            // 
            // checkBox_See
            // 
            this.checkBox_See.AutoSize = true;
            this.checkBox_See.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox_See.Location = new System.Drawing.Point(12, 12);
            this.checkBox_See.Name = "checkBox_See";
            this.checkBox_See.Size = new System.Drawing.Size(100, 20);
            this.checkBox_See.TabIndex = 1;
            this.checkBox_See.Text = "Просмотр";
            this.checkBox_See.UseVisualStyleBackColor = true;
            this.checkBox_See.CheckedChanged += new System.EventHandler(this.checkBox_See_CheckedChanged);
            // 
            // button_ok
            // 
            this.button_ok.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_ok.Location = new System.Drawing.Point(71, 179);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 2;
            this.button_ok.Text = "ОК";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // label_printerEtiketok
            // 
            this.label_printerEtiketok.AutoSize = true;
            this.label_printerEtiketok.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_printerEtiketok.Location = new System.Drawing.Point(12, 107);
            this.label_printerEtiketok.Name = "label_printerEtiketok";
            this.label_printerEtiketok.Size = new System.Drawing.Size(134, 16);
            this.label_printerEtiketok.TabIndex = 3;
            this.label_printerEtiketok.Text = "Печать этикеток";
            // 
            // comboBox_printerEtiketok
            // 
            this.comboBox_printerEtiketok.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_printerEtiketok.FormattingEnabled = true;
            this.comboBox_printerEtiketok.Location = new System.Drawing.Point(15, 126);
            this.comboBox_printerEtiketok.Name = "comboBox_printerEtiketok";
            this.comboBox_printerEtiketok.Size = new System.Drawing.Size(199, 21);
            this.comboBox_printerEtiketok.TabIndex = 4;
            this.comboBox_printerEtiketok.SelectedIndexChanged += new System.EventHandler(this.comboBox_printerEtiketok_SelectedIndexChanged);
            // 
            // checkBox_Rezka
            // 
            this.checkBox_Rezka.AutoSize = true;
            this.checkBox_Rezka.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox_Rezka.Location = new System.Drawing.Point(12, 64);
            this.checkBox_Rezka.Name = "checkBox_Rezka";
            this.checkBox_Rezka.Size = new System.Drawing.Size(72, 20);
            this.checkBox_Rezka.TabIndex = 5;
            this.checkBox_Rezka.Text = "Резка";
            this.checkBox_Rezka.UseVisualStyleBackColor = true;
            this.checkBox_Rezka.CheckedChanged += new System.EventHandler(this.checkBox_Rezka_CheckedChanged);
            // 
            // settings_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(226, 215);
            this.Controls.Add(this.checkBox_Rezka);
            this.Controls.Add(this.comboBox_printerEtiketok);
            this.Controls.Add(this.label_printerEtiketok);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.checkBox_See);
            this.Controls.Add(this.checkBox_Change);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "settings_form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.settings_form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_Change;
        private System.Windows.Forms.CheckBox checkBox_See;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Label label_printerEtiketok;
        private System.Windows.Forms.ComboBox comboBox_printerEtiketok;
        private System.Windows.Forms.CheckBox checkBox_Rezka;
    }
}