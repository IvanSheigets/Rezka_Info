using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Drawing.Printing;

namespace RezkaInfo
{
    public partial class settings_form : Form
    {
        int m_iRegim = -1;

        public settings_form()
        {
            InitializeComponent();
        }

        private void settings_form_Load(object sender, EventArgs e)
        {
            string loadPrinter="";
            RegistryKey readKey = Registry.LocalMachine.OpenSubKey("software\\rezka_info");
            if (readKey != null)
            {
                string loadString = (string)readKey.GetValue("Type_app");
                loadPrinter = (string)readKey.GetValue("PrinterEtiketki");
                readKey.Close();
                m_iRegim = Convert.ToInt32(loadString);
                if (m_iRegim == 1)
                    checkBox_Change.Checked = true;
                else if (m_iRegim == 0)
                    checkBox_See.Checked = true;
                else if (m_iRegim == 2)
                    checkBox_Rezka.Checked = true;
            }
            else
            {
                m_iRegim = -1;
                checkBox_See.Checked = false;
                checkBox_Change.Checked = false;
                checkBox_Rezka.Checked = false;
            }

            foreach (string strPrinter in PrinterSettings.InstalledPrinters)
               comboBox_printerEtiketok.Items.Add(strPrinter);

            for (int i=0;i<comboBox_printerEtiketok.Items.Count;i++)
                if (comboBox_printerEtiketok.Items[i].ToString()==loadPrinter)
                {
                    comboBox_printerEtiketok.SelectedIndex = i;
                    break;
                }
        }

        private void checkBox_See_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_See.Checked == true)
            {
                checkBox_Change.Checked = false;
                checkBox_Rezka.Checked = false;

                RegistryKey saveKey = Registry.LocalMachine.CreateSubKey("software\\rezka_info");
                saveKey.SetValue("Type_app", "0"); // 0 - просмотр 1-редактирование  2-резка
                saveKey.Close(); 
            }
            
        }

        private void checkBox_Change_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Change.Checked == true)
            {
                checkBox_See.Checked = false;
                checkBox_Rezka.Checked = false;
                RegistryKey saveKey = Registry.LocalMachine.CreateSubKey("software\\rezka_info");
                saveKey.SetValue("Type_app", "1"); // 0 - просмотр 1-редактирование  2-резка
                saveKey.Close(); 
            }
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox_printerEtiketok_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_printerEtiketok.SelectedIndex!=-1)
            {
                RegistryKey saveKey = Registry.LocalMachine.CreateSubKey("software\\rezka_info");
                saveKey.SetValue("PrinterEtiketki", comboBox_printerEtiketok.Items[comboBox_printerEtiketok.SelectedIndex].ToString());
                saveKey.Close(); 
            }
        }

        private void checkBox_Rezka_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Rezka.Checked == true)
            {
                checkBox_See.Checked = false;
                checkBox_Change.Checked = false;
                
                RegistryKey saveKey = Registry.LocalMachine.CreateSubKey("software\\rezka_info");
                saveKey.SetValue("Type_app", "2"); // 0 - просмотр 1-редактирование  2-резка
                saveKey.Close();
            }
        }
    }
}
