namespace RezkaInfo
{
    partial class change_rylon
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
            this.button_change = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.label_brytto = new System.Windows.Forms.Label();
            this.label_netto = new System.Windows.Forms.Label();
            this.label_dlinarylona = new System.Windows.Forms.Label();
            this.label_countEtiket = new System.Windows.Forms.Label();
            this.textBox_brytto = new System.Windows.Forms.TextBox();
            this.textBox_netto = new System.Windows.Forms.TextBox();
            this.textBox_dlina = new System.Windows.Forms.TextBox();
            this.textBox_countEtik = new System.Windows.Forms.TextBox();
            this.textBox_vagaTary = new System.Windows.Forms.TextBox();
            this.label_vagatary = new System.Windows.Forms.Label();
            this.checkbox_logo = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button_change
            // 
            this.button_change.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_change.Location = new System.Drawing.Point(69, 209);
            this.button_change.Name = "button_change";
            this.button_change.Size = new System.Drawing.Size(89, 37);
            this.button_change.TabIndex = 10;
            this.button_change.Text = "Изменить";
            this.button_change.UseVisualStyleBackColor = true;
            this.button_change.Click += new System.EventHandler(this.button_change_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_cancel.Location = new System.Drawing.Point(193, 210);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(89, 37);
            this.button_cancel.TabIndex = 11;
            this.button_cancel.Text = "Отмена";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // label_brytto
            // 
            this.label_brytto.AutoSize = true;
            this.label_brytto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_brytto.Location = new System.Drawing.Point(22, 50);
            this.label_brytto.Name = "label_brytto";
            this.label_brytto.Size = new System.Drawing.Size(84, 16);
            this.label_brytto.TabIndex = 2;
            this.label_brytto.Text = "Брутто, кг";
            // 
            // label_netto
            // 
            this.label_netto.AutoSize = true;
            this.label_netto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_netto.Location = new System.Drawing.Point(22, 18);
            this.label_netto.Name = "label_netto";
            this.label_netto.Size = new System.Drawing.Size(76, 16);
            this.label_netto.TabIndex = 0;
            this.label_netto.Text = "Нетто, кг";
            // 
            // label_dlinarylona
            // 
            this.label_dlinarylona.AutoSize = true;
            this.label_dlinarylona.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_dlinarylona.Location = new System.Drawing.Point(22, 82);
            this.label_dlinarylona.Name = "label_dlinarylona";
            this.label_dlinarylona.Size = new System.Drawing.Size(147, 16);
            this.label_dlinarylona.TabIndex = 4;
            this.label_dlinarylona.Text = "Длина рулона, м.п.";
            // 
            // label_countEtiket
            // 
            this.label_countEtiket.AutoSize = true;
            this.label_countEtiket.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_countEtiket.Location = new System.Drawing.Point(22, 114);
            this.label_countEtiket.Name = "label_countEtiket";
            this.label_countEtiket.Size = new System.Drawing.Size(138, 16);
            this.label_countEtiket.TabIndex = 6;
            this.label_countEtiket.Text = "К-во этикеток, шт";
            // 
            // textBox_brytto
            // 
            this.textBox_brytto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_brytto.Location = new System.Drawing.Point(182, 44);
            this.textBox_brytto.Name = "textBox_brytto";
            this.textBox_brytto.Size = new System.Drawing.Size(147, 22);
            this.textBox_brytto.TabIndex = 3;
            this.textBox_brytto.TextChanged += new System.EventHandler(this.textBox_brytto_TextChanged);
            this.textBox_brytto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_brytto_KeyPress);
            // 
            // textBox_netto
            // 
            this.textBox_netto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_netto.Location = new System.Drawing.Point(182, 12);
            this.textBox_netto.Name = "textBox_netto";
            this.textBox_netto.Size = new System.Drawing.Size(147, 22);
            this.textBox_netto.TabIndex = 1;
            this.textBox_netto.TextChanged += new System.EventHandler(this.textBox_netto_TextChanged);
            this.textBox_netto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_netto_KeyPress);
            // 
            // textBox_dlina
            // 
            this.textBox_dlina.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_dlina.Location = new System.Drawing.Point(182, 76);
            this.textBox_dlina.Name = "textBox_dlina";
            this.textBox_dlina.Size = new System.Drawing.Size(147, 22);
            this.textBox_dlina.TabIndex = 5;
            this.textBox_dlina.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_dlina_KeyPress);
            // 
            // textBox_countEtik
            // 
            this.textBox_countEtik.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_countEtik.Location = new System.Drawing.Point(182, 108);
            this.textBox_countEtik.Name = "textBox_countEtik";
            this.textBox_countEtik.Size = new System.Drawing.Size(147, 22);
            this.textBox_countEtik.TabIndex = 7;
            this.textBox_countEtik.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_countEtik_KeyPress);
            // 
            // textBox_vagaTary
            // 
            this.textBox_vagaTary.Enabled = false;
            this.textBox_vagaTary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox_vagaTary.Location = new System.Drawing.Point(182, 140);
            this.textBox_vagaTary.Name = "textBox_vagaTary";
            this.textBox_vagaTary.Size = new System.Drawing.Size(147, 22);
            this.textBox_vagaTary.TabIndex = 9;
            // 
            // label_vagatary
            // 
            this.label_vagatary.AutoSize = true;
            this.label_vagatary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_vagatary.Location = new System.Drawing.Point(22, 146);
            this.label_vagatary.Name = "label_vagatary";
            this.label_vagatary.Size = new System.Drawing.Size(98, 16);
            this.label_vagatary.TabIndex = 8;
            this.label_vagatary.Text = "Вес тары, кг";
            // 
            // checkbox_logo
            // 
            this.checkbox_logo.AutoSize = true;
            this.checkbox_logo.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkbox_logo.Checked = true;
            this.checkbox_logo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkbox_logo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkbox_logo.Location = new System.Drawing.Point(21, 175);
            this.checkbox_logo.Name = "checkbox_logo";
            this.checkbox_logo.Size = new System.Drawing.Size(192, 20);
            this.checkbox_logo.TabIndex = 12;
            this.checkbox_logo.Text = "Этикетка с логотипом";
            this.checkbox_logo.UseVisualStyleBackColor = true;
            // 
            // change_rylon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 259);
            this.ControlBox = false;
            this.Controls.Add(this.checkbox_logo);
            this.Controls.Add(this.textBox_vagaTary);
            this.Controls.Add(this.label_vagatary);
            this.Controls.Add(this.textBox_countEtik);
            this.Controls.Add(this.textBox_dlina);
            this.Controls.Add(this.textBox_netto);
            this.Controls.Add(this.textBox_brytto);
            this.Controls.Add(this.label_countEtiket);
            this.Controls.Add(this.label_dlinarylona);
            this.Controls.Add(this.label_netto);
            this.Controls.Add(this.label_brytto);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_change);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "change_rylon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Изменение рулона";
            this.Load += new System.EventHandler(this.change_rylon_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_change;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Label label_brytto;
        private System.Windows.Forms.Label label_netto;
        private System.Windows.Forms.Label label_dlinarylona;
        private System.Windows.Forms.Label label_countEtiket;
        private System.Windows.Forms.TextBox textBox_brytto;
        private System.Windows.Forms.TextBox textBox_netto;
        private System.Windows.Forms.TextBox textBox_dlina;
        private System.Windows.Forms.TextBox textBox_countEtik;
        private System.Windows.Forms.TextBox textBox_vagaTary;
        private System.Windows.Forms.Label label_vagatary;
        private System.Windows.Forms.CheckBox checkbox_logo;
    }
}