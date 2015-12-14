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
    public partial class selectUser_form : Form
    {
        int m_iDialogResut = -1;
        int m_iCheckMen = -1;
        int m_iDialogType = -1;//1 выход    0-отмена


        public selectUser_form()
        {
            InitializeComponent();
        }

        public int GetDialogResult()
        {   return m_iDialogResut;  }

        public int GetCheckMen()
        {   return m_iCheckMen; }

        public void SetCheckMen(int iCheckMen)
        {   m_iCheckMen = iCheckMen;    }

        public void SetDialogType(int iDialogType)
        { m_iDialogType = iDialogType; }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (comboBox_checkMen.SelectedIndex != -1)
            {
                m_iCheckMen = Convert.ToInt32(comboBox_checkMen.Items[comboBox_checkMen.SelectedIndex]);
                m_iDialogResut = 1;
                this.Close();
            }
            else
                MessageBox.Show("Выберите проверяющего");
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            if (m_iDialogType == 1)
                m_iDialogResut = 0;
            else if (m_iDialogType == 0)
                m_iDialogResut = 2;
            this.Close();
        }

        private void selectUser_form_Load(object sender, EventArgs e)
        {
            if (m_iCheckMen!=-1)
                for (int i=0;i<comboBox_checkMen.Items.Count;i++)
                    if (Convert.ToInt32(comboBox_checkMen.Items[i])==m_iCheckMen)
                    {
                        comboBox_checkMen.SelectedIndex = i;
                        break;
                    }

            if (m_iDialogType == 1)
                button_exit.Text = "Выход";
            else if (m_iDialogType == 0)
                button_exit.Text = "Отмена";
        }


    }
}
