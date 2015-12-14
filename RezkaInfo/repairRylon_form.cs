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
    public partial class repairRylon_form : Form
    {
        int m_iDialogResult = -1;
        public repairRylon_form()
        {
            InitializeComponent();
        }

        private void button_Repair_Click(object sender, EventArgs e)
        {
            m_iDialogResult = 1;
            this.Close();
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            m_iDialogResult = 0;
            this.Close();
        }

        public int GetDialogResult()
        {
            return m_iDialogResult;
        }
    }
}
