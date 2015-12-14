using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RezkaInfo
{
    public partial class password_form : Form
    {
        int m_iDialogResult = -1;

        public password_form()
        {
            InitializeComponent();
        }

        public int GetDialogResult()
        {return m_iDialogResult;}

        private void button_ok_Click(object sender, EventArgs e)
        {
            OnOK();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            m_iDialogResult = 0;
            this.Close();
        }

        private void OnOK()
        {
            if (textBox_password.Text.Length == 0)
            {
                MessageBox.Show("Введите парооль");
                this.ActiveControl = textBox_password;
            }
            else if (textBox_password.Text != "coolworld")
            {
                MessageBox.Show("Не верный пароль");
                textBox_password.Text = "";
                this.ActiveControl = textBox_password;
            }
            else
            {
                m_iDialogResult = 1;
                this.Close();
            }
        }

        private void password_form_Load(object sender, EventArgs e)
        {
            this.ActiveControl = textBox_password;
        }

        private void textBox_password_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                OnOK();
        }
    }
}
