using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using PrintEtiketkaMy;
using Microsoft.Win32;

namespace RezkaInfo
{
    public partial class change_rylon : Form
    {
        int m_iDialogResult = -1;
        
        int m_iRylonID = -1;

        double m_dVagaTary = -1;
        double m_dNetto = -1;
        double m_dBrytto = -1;
        int m_iDlinaRylona = -1;
        int m_iCountEtiketki = -1;

        bool m_bFlagLoad = false;

        ////mysql
        string strMSSQLQuery = "";
        public SqlConnection m_MSSQLConnection;
        SqlCommand m_MSSQLCommand;
        SqlDataReader m_MSSQLReader;
        ///////////////////////////

        public change_rylon()
        {
            InitializeComponent();
        }

        public int GetDialogResult()
        { return m_iDialogResult; }

        public void SetRylonID(int iRylonID)
        {
            if (iRylonID >= 0)
                m_iRylonID = iRylonID;
            else m_iRylonID = 0;
        }

        private bool CheckConnect()
        {
            if (m_MSSQLConnection.State.ToString() == "Closed")
            {
                try
                {
                    m_MSSQLConnection.Open();
                    return true;
                }
                catch (System.Exception ex)
                {
                    WriteLog("Ошибка подключения к бд", ex);
                    return false;
                }
            }
            else
                return true;
        }

        private void WriteLog(string err, System.Exception ex)
        {
            ///////log file
            string strError = "";
            String current_time_str;
            StreamWriter logFile = null;
            ///////////////////////////

            try
            {
                FileInfo fi = new FileInfo("log.txt");
                logFile = fi.AppendText();
                current_time_str = DateTime.Now.ToString("[dd.MM:yyyy - HH:mm:ss]");
                strError = current_time_str + "- " + err + "- " + ex.Message;
                logFile.WriteLine(strError);
                logFile.Close();
            }
            catch (System.Exception ex1)
            {
                string s = ex1.Message;
                logFile.Close();
            }
        }

        private void button_change_Click(object sender, EventArgs e)
        {
            if (textBox_netto.Text.Length!=0)
            {
                m_dNetto = Convert.ToDouble(textBox_netto.Text);

                if (textBox_brytto.Text.Length != 0)
                {
                    m_dBrytto = Convert.ToDouble(textBox_brytto.Text);

                    if(textBox_dlina.Text.Length!=0)
                    {
                        m_iDlinaRylona = Convert.ToInt32(textBox_dlina.Text);
                        if (textBox_countEtik.Text.Length!=0)
                        {
                            m_iCountEtiketki = Convert.ToInt32(textBox_countEtik.Text);

                            if (CheckConnect())
                            {
                                if (m_dBrytto.ToString().IndexOf(',')!=-1)
                                    strMSSQLQuery = "update itak_etiketka.dbo.itak_vihidrylon set brytto=" + m_dBrytto.ToString().Substring(0, m_dBrytto.ToString().IndexOf(',')) +"."+ m_dBrytto.ToString().Substring(m_dBrytto.ToString().IndexOf(',')+1) +", dlinarylona=" + m_iDlinaRylona.ToString() +", koletiketki=" + m_iCountEtiketki.ToString() +", state_rylon=0 where id=" + m_iRylonID;
                                else strMSSQLQuery = "update itak_etiketka.dbo.itak_vihidrylon set brytto=" + m_dBrytto.ToString() + ", dlinarylona=" + m_iDlinaRylona.ToString() + ", koletiketki=" + m_iCountEtiketki.ToString() + ", state_rylon=0 where id=" + m_iRylonID;
                                
                                m_MSSQLCommand.CommandText = strMSSQLQuery;
                                try
                                {
                                    m_MSSQLCommand.ExecuteNonQuery();

                                    try
                                    {
                                        string strPrinter = "";
                                        RegistryKey readKey = Registry.LocalMachine.OpenSubKey("software\\rezka_info");
                                        if (readKey != null)
                                        {
                                            strPrinter = (string)readKey.GetValue("PrinterEtiketki");
                                            readKey.Close();
                                        }
                                        if (strPrinter != null)
                                        {
                                            PrintEtiketka m_print = new PrintEtiketka();

                                            strMSSQLQuery = "select vh.brytto-vh.vagatary,convert(date,z.datezakaz), zk.zakazchik_name, pr.product_name, z.partiya, z.machine, z.smena, pm.product_material, pt.product_tols, " +
                                                            "vh.width, vh.dlinarylona, vh.koletiketki, vh.num_rylon,  vh.brytto, vh.id " +
                                                            "from itak_etiketka.dbo.itak_zakazchik zk, itak_etiketka.dbo.itak_product pr, itak_etiketka.dbo.itak_zakaz z, " +
                                                            "itak_etiketka.dbo.itak_productmaterial pm, itak_etiketka.dbo.itak_producttols pt, itak_etiketka.dbo.itak_vihidrylon vh " +
                                                            "where z.zakazchik_id=zk.id and z.productmaterial_id = pm.id and z.producttols_id=pt.id " +
                                                            "and vh.zakaz_id=z.id and vh.product_id=pr.id and vh.id=" + m_iRylonID;
                                            m_MSSQLCommand.CommandText = strMSSQLQuery;
                                            m_MSSQLReader = m_MSSQLCommand.ExecuteReader();

                                            while (m_MSSQLReader.Read())
                                            {
                                                m_print.SetParametrs(m_MSSQLReader["zakazchik_name"].ToString().Trim(),
                                                                     m_MSSQLReader["product_name"].ToString().Trim(),
                                                                     m_MSSQLReader["partiya"].ToString().Trim(),
                                                                     Convert.ToInt32(m_MSSQLReader["machine"]),
                                                                     Convert.ToInt32(m_MSSQLReader["smena"]),
                                                                     m_MSSQLReader["product_material"].ToString().Trim(),
                                                                     Convert.ToInt32(m_MSSQLReader["product_tols"]),
                                                                     Convert.ToInt32(m_MSSQLReader["width"]),
                                                                     Convert.ToInt32(m_MSSQLReader["dlinarylona"]),
                                                                     Convert.ToInt32(m_MSSQLReader["koletiketki"]),
                                                                     m_MSSQLReader[1].ToString().Trim(),
                                                                     Convert.ToInt32(m_MSSQLReader["num_rylon"]),
                                                                     Convert.ToDouble(m_MSSQLReader[0]),
                                                                     Convert.ToDouble(m_MSSQLReader["brytto"]),
                                                                     Convert.ToInt32(m_MSSQLReader["id"]));

                                                bool bLogo = true;
                                                if (checkbox_logo.CheckState == CheckState.Checked)
                                                    bLogo = true;
                                                else if (checkbox_logo.CheckState == CheckState.Unchecked)
                                                    bLogo = false;
                                                
                                                if (!m_print.SetPrinter(strPrinter, 60, 70, false,false,bLogo))
                                                    MessageBox.Show("Ошибка печати етикетки");
                                            }

                                            m_MSSQLReader.Close();
                                            m_print = null;

                                        }
                                    }
                                    catch(System.Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                    m_iDialogResult = 1;
                                    this.Close();
                                }
                                catch (System.Exception ex)
                                {
                                    MessageBox.Show("Ошибка записи в базу данных - "+ex.Message);
                                }
                                
                            }   else MessageBox.Show("Ошибка записи в базу данных");
                            
                        }    else MessageBox.Show("Введите длину рулона");
                    }   else MessageBox.Show("Введите длину рулона");
                }   else MessageBox.Show("Введите вес брутто");
            }   else MessageBox.Show("Введите вес нетто"); 

            if (textBox_brytto.Text.Length != 0)
                m_dBrytto = Convert.ToDouble(textBox_brytto.Text);
            else 
                MessageBox.Show("Введите брутто");
            
            
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            m_iDialogResult = 0;
            this.Close();
        }

        private void change_rylon_Load(object sender, EventArgs e)
        {
            if (CheckConnect() == true)
            {
                m_MSSQLCommand = m_MSSQLConnection.CreateCommand();
                SelectRylon(m_iRylonID);
            }


        }

        private void SelectRylon(int iRylonID)
        {
            strMSSQLQuery = "select brytto, brytto-vagatary, dlinarylona, koletiketki, vagatary from itak_etiketka.dbo.itak_vihidrylon where id=" + iRylonID;
            m_MSSQLCommand.CommandText = strMSSQLQuery;
            m_MSSQLReader = m_MSSQLCommand.ExecuteReader();

            while (m_MSSQLReader.Read())
            {
                try
                {
                    textBox_brytto.Text = m_MSSQLReader[0].ToString();
                    m_dBrytto = Convert.ToDouble(m_MSSQLReader[0]);

                    textBox_netto.Text = m_MSSQLReader[1].ToString();
                    m_dNetto = Convert.ToDouble(m_MSSQLReader[1]);

                    textBox_dlina.Text = m_MSSQLReader[2].ToString();
                    m_iDlinaRylona = Convert.ToInt32(m_MSSQLReader[2]);

                    textBox_countEtik.Text = m_MSSQLReader[3].ToString();
                    m_iCountEtiketki = Convert.ToInt32(m_MSSQLReader[3]);

                    textBox_vagaTary.Text = m_MSSQLReader[4].ToString();
                    m_dVagaTary = Convert.ToDouble(m_MSSQLReader[4]);

                    m_bFlagLoad = true;

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            m_MSSQLReader.Close();

        }

        private void textBox_netto_TextChanged(object sender, EventArgs e)
        {
            if (m_bFlagLoad==true)
            {
                if (textBox_netto.Text.Length != 0)
                    textBox_brytto.Text = (Convert.ToDouble(textBox_netto.Text) + m_dVagaTary).ToString();
                else textBox_brytto.Text = m_dVagaTary.ToString();
            }
        }

        private void textBox_brytto_TextChanged(object sender, EventArgs e)
        {
            if(m_bFlagLoad==true)
                if (textBox_brytto.Text.Length != 0)
                    textBox_netto.Text = (Convert.ToDouble(textBox_brytto.Text) - m_dVagaTary).ToString();
        }

        private void textBox_netto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && !((e.KeyChar == ',') && (textBox_netto.Text.IndexOf(",") == -1) && (textBox_netto.Text.Length != 0)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }
        }

        private void textBox_brytto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && !((e.KeyChar == ',') && (textBox_brytto.Text.IndexOf(",") == -1) && (textBox_brytto.Text.Length != 0)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }

        }

        private void textBox_dlina_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }   
        }

        private void textBox_countEtik_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }
        }
    }
}
