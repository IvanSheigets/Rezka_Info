namespace RezkaInfo
{
    partial class rylon_form
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.dataGridView_rylon = new System.Windows.Forms.DataGridView();
            this.checkBox_selectAll = new System.Windows.Forms.CheckBox();
            this.button_print = new System.Windows.Forms.Button();
            this.button_all_label = new System.Windows.Forms.Button();
            this.checkbox_logo = new System.Windows.Forms.CheckBox();
            this.Num_rylon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Brytto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Netto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dlina = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Square = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CountEtik = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Check_men = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Check_Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Check_rylon = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_rylon)).BeginInit();
            this.SuspendLayout();
            // 
            // button_ok
            // 
            this.button_ok.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_ok.Location = new System.Drawing.Point(414, 424);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(98, 40);
            this.button_ok.TabIndex = 0;
            this.button_ok.Text = "Сохранить";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_cancel.Location = new System.Drawing.Point(569, 424);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(98, 40);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "Отмена";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // dataGridView_rylon
            // 
            this.dataGridView_rylon.AllowUserToAddRows = false;
            this.dataGridView_rylon.AllowUserToDeleteRows = false;
            this.dataGridView_rylon.AllowUserToResizeColumns = false;
            this.dataGridView_rylon.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_rylon.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_rylon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_rylon.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Num_rylon,
            this.Brytto,
            this.Netto,
            this.Dlina,
            this.Square,
            this.CountEtik,
            this.Check_men,
            this.Check_Time,
            this.Check_rylon});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_rylon.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_rylon.Location = new System.Drawing.Point(12, 3);
            this.dataGridView_rylon.MultiSelect = false;
            this.dataGridView_rylon.Name = "dataGridView_rylon";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_rylon.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView_rylon.RowHeadersVisible = false;
            this.dataGridView_rylon.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView_rylon.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView_rylon.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_rylon.Size = new System.Drawing.Size(996, 384);
            this.dataGridView_rylon.TabIndex = 3;
            this.dataGridView_rylon.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_rylon_CellContentClick);
            this.dataGridView_rylon.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView_rylon_CurrentCellDirtyStateChanged);
            this.dataGridView_rylon.DoubleClick += new System.EventHandler(this.dataGridView_rylon_DoubleClick);
            // 
            // checkBox_selectAll
            // 
            this.checkBox_selectAll.AutoSize = true;
            this.checkBox_selectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox_selectAll.Location = new System.Drawing.Point(579, 393);
            this.checkBox_selectAll.Name = "checkBox_selectAll";
            this.checkBox_selectAll.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox_selectAll.Size = new System.Drawing.Size(102, 17);
            this.checkBox_selectAll.TabIndex = 4;
            this.checkBox_selectAll.Text = "Выбрать все";
            this.checkBox_selectAll.UseVisualStyleBackColor = true;
            this.checkBox_selectAll.Visible = false;
            this.checkBox_selectAll.CheckedChanged += new System.EventHandler(this.checkBox_selectAll_CheckedChanged);
            // 
            // button_print
            // 
            this.button_print.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_print.Location = new System.Drawing.Point(259, 424);
            this.button_print.Name = "button_print";
            this.button_print.Size = new System.Drawing.Size(98, 40);
            this.button_print.TabIndex = 5;
            this.button_print.Text = "Печать";
            this.button_print.UseVisualStyleBackColor = true;
            this.button_print.Click += new System.EventHandler(this.button_print_Click);
            // 
            // button_all_label
            // 
            this.button_all_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_all_label.Location = new System.Drawing.Point(12, 393);
            this.button_all_label.Name = "button_all_label";
            this.button_all_label.Size = new System.Drawing.Size(190, 40);
            this.button_all_label.TabIndex = 6;
            this.button_all_label.Text = "Печать всех этикеток";
            this.button_all_label.UseVisualStyleBackColor = true;
            this.button_all_label.Visible = false;
            this.button_all_label.Click += new System.EventHandler(this.button_all_label_Click);
            // 
            // checkbox_logo
            // 
            this.checkbox_logo.AutoSize = true;
            this.checkbox_logo.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkbox_logo.Checked = true;
            this.checkbox_logo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkbox_logo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkbox_logo.Location = new System.Drawing.Point(12, 439);
            this.checkbox_logo.Name = "checkbox_logo";
            this.checkbox_logo.Size = new System.Drawing.Size(192, 20);
            this.checkbox_logo.TabIndex = 13;
            this.checkbox_logo.Text = "Этикетка с логотипом";
            this.checkbox_logo.UseVisualStyleBackColor = true;
            this.checkbox_logo.Visible = false;
            // 
            // Num_rylon
            // 
            this.Num_rylon.HeaderText = "№ рулона";
            this.Num_rylon.Name = "Num_rylon";
            this.Num_rylon.ReadOnly = true;
            // 
            // Brytto
            // 
            this.Brytto.HeaderText = "Брутто, кг";
            this.Brytto.Name = "Brytto";
            this.Brytto.ReadOnly = true;
            // 
            // Netto
            // 
            this.Netto.HeaderText = "Нетто, кг";
            this.Netto.Name = "Netto";
            this.Netto.ReadOnly = true;
            // 
            // Dlina
            // 
            this.Dlina.HeaderText = "Длина м.п.";
            this.Dlina.Name = "Dlina";
            this.Dlina.ReadOnly = true;
            this.Dlina.Width = 110;
            // 
            // Square
            // 
            this.Square.HeaderText = "К-во м.кв.";
            this.Square.Name = "Square";
            // 
            // CountEtik
            // 
            this.CountEtik.HeaderText = "К-во етикеток";
            this.CountEtik.Name = "CountEtik";
            this.CountEtik.ReadOnly = true;
            this.CountEtik.Width = 130;
            // 
            // Check_men
            // 
            this.Check_men.HeaderText = "№ провер.";
            this.Check_men.Name = "Check_men";
            this.Check_men.Width = 110;
            // 
            // Check_Time
            // 
            this.Check_Time.HeaderText = "Дата проверки";
            this.Check_Time.Name = "Check_Time";
            this.Check_Time.Width = 135;
            // 
            // Check_rylon
            // 
            this.Check_rylon.HeaderText = "Проверка";
            this.Check_rylon.Name = "Check_rylon";
            this.Check_rylon.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Check_rylon.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // rylon_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 476);
            this.ControlBox = false;
            this.Controls.Add(this.checkbox_logo);
            this.Controls.Add(this.button_all_label);
            this.Controls.Add(this.button_print);
            this.Controls.Add(this.checkBox_selectAll);
            this.Controls.Add(this.dataGridView_rylon);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "rylon_form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Рулоны";
            this.Load += new System.EventHandler(this.rylon_form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_rylon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.DataGridView dataGridView_rylon;
        private System.Windows.Forms.CheckBox checkBox_selectAll;
        private System.Windows.Forms.Button button_print;
        private System.Windows.Forms.Button button_all_label;
        private System.Windows.Forms.CheckBox checkbox_logo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Num_rylon;
        private System.Windows.Forms.DataGridViewTextBoxColumn Brytto;
        private System.Windows.Forms.DataGridViewTextBoxColumn Netto;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dlina;
        private System.Windows.Forms.DataGridViewTextBoxColumn Square;
        private System.Windows.Forms.DataGridViewTextBoxColumn CountEtik;
        private System.Windows.Forms.DataGridViewTextBoxColumn Check_men;
        private System.Windows.Forms.DataGridViewTextBoxColumn Check_Time;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Check_rylon;
    }
}