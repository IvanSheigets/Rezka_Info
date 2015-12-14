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
    public partial class delChange_form : Form
    {
        int m_iDialogResult = -1;   //0-exit    1 - change  2 - del 
        int m_iTypeRegim = -1; //2 - rezka   3 - master
        public delChange_form()
        {
            InitializeComponent();
        }

        public int GetDialogResult()
        {   return m_iDialogResult; }

        private void button_exit_Click(object sender, EventArgs e)
        {
            m_iDialogResult = 0;
            this.Close();
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            m_iDialogResult = 2;
            this.Close();
        }

        private void button_Change_Click(object sender, EventArgs e)
        {
            m_iDialogResult = 1;
            this.Close();            
        }

        private void button_Repair_Click(object sender, EventArgs e)
        {
            m_iDialogResult = 3;
            this.Close();
        }

        





    }

      
}
