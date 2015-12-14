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

namespace RezkaInfo
{
    public partial class AddOneForm : Form
    {
        ////mysql
        string strMSSQLQuery = "";
        public SqlConnection m_MSSQLConnection;
        SqlCommand m_MSSQLCommand;
        SqlDataReader m_MSSQLReader;
        ///////////////////////////

        private int m_iZakazchikId = 0;
        private int m_iDialogResult = 0;
        private string strAddString = "";
        private int m_iAddID = 0;
        private int m_iAddType = 0; //1 - add product   2 - add width   3 - add vaga_tary   4 - add material
        private string m_strCaption = "";  

        public AddOneForm(string strCaption, int iZakazchikId , int iAddType)
        {
            if (iZakazchikId > 0)
                m_iZakazchikId = iZakazchikId;

            if (iAddType > 0)
                m_iAddType = iAddType;

            m_strCaption = strCaption;

            InitializeComponent();
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

        public int GetDialogResult()
        {
            return m_iDialogResult;
        }

        public string GetAddSting()
        {
            return strAddString;
        }

        public int GetAddID()
        {
            return m_iAddID;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddOneForm_Load(object sender, EventArgs e)
        {
            if (CheckConnect() == true)
            {
                m_MSSQLCommand = m_MSSQLConnection.CreateCommand();
                this.Text += " " + m_strCaption;
                button_add.Enabled = true;
                textBox.Focus();
            }
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            Add();
        }

        private void Add()
        {
            if (CheckConnect())
            {
                strAddString = textBox.Text.ToUpper().Trim();
                int iCount = 0;
                bool bFlag = false;
                if (strAddString.Length != 0)
                {
                    if (m_iAddType == 1)
                        strMSSQLQuery = "select count(id) from itak_etiketka.dbo.itak_product where product_name='" + strAddString + "' and id_zakazchik=" + m_iZakazchikId;
                    else if (m_iAddType == 2)
                        strMSSQLQuery = "select count(id) from itak_etiketka.dbo.itak_productwidth where product_width=" + strAddString;
                    else if (m_iAddType == 3)
                    {
                        strAddString = strAddString.Replace(',', '.');
                        strMSSQLQuery = "select count(id) from itak_etiketka.dbo.itak_vagatary where vaga=" + strAddString;
                    }
                    else if (m_iAddType == 4)
                        strMSSQLQuery = "select count(id) from itak_etiketka.dbo.itak_productmaterial where product_material='" + strAddString + "'";
                    else if (m_iAddType == 5)
                        strMSSQLQuery = "select count(id) from itak_etiketka.dbo.itak_producttols where product_tols=" + strAddString;

                    if (strMSSQLQuery.Length != 0)
                    {
                        try
                        {
                            m_MSSQLCommand.CommandText = strMSSQLQuery;
                            m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
                            if (m_MSSQLReader.HasRows)
                            {
                                m_MSSQLReader.Read();
                                iCount = Convert.ToInt32(m_MSSQLReader[0]);
                            }
                            m_MSSQLReader.Close();
                            bFlag = true;
                        }
                        catch (System.Exception ex)
                        {
                            WriteLog("ADD() - change_Product_rezka - получение списка продуктов, width,vaga, material, tols ", ex);
                            m_MSSQLReader.Close();
                            bFlag = false;
                        }


                        if (bFlag && iCount == 0)
                        {
                            try
                            {
                                if (m_iAddType == 1)
                                    strMSSQLQuery = "insert into itak_etiketka.dbo.itak_product (product_name, id_zakazchik) values ('" + strAddString + "'," + m_iZakazchikId + ")";
                                else if (m_iAddType == 2)
                                    strMSSQLQuery = "insert into itak_etiketka.dbo.itak_productwidth (product_width) values (" + strAddString + ")";
                                else if (m_iAddType == 3)
                                    strMSSQLQuery = "insert into itak_etiketka.dbo.itak_vagatary (vaga) values (" + strAddString + ")";
                                else if (m_iAddType == 4)
                                    strMSSQLQuery = "insert into itak_etiketka.dbo.itak_productmaterial (product_material) values ('" + strAddString + "')";
                                else if (m_iAddType == 5)
                                    strMSSQLQuery = "insert into itak_etiketka.dbo.itak_producttols (product_tols) values (" + strAddString + ")";

                                m_MSSQLCommand.CommandText = strMSSQLQuery;
                                m_MSSQLCommand.ExecuteNonQuery();

                                strMSSQLQuery = "select @@IDENTITY AS 'Identity'";
                                m_MSSQLCommand.CommandText = strMSSQLQuery;
                                m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
                                try
                                {
                                    m_MSSQLReader.Read();
                                    if (m_MSSQLReader.HasRows)
                                        m_iAddID = Convert.ToInt32(m_MSSQLReader[0]);
                                    m_MSSQLReader.Close();
                                }
                                catch (System.Exception ex)
                                {
                                    WriteLog("ADD() - change_Product_rezka  - получение ID вставленной записи продуктов, width,vaga, material, tols", ex);
                                    m_MSSQLReader.Close();
                                }


                                m_iDialogResult = 1;
                                this.Close();
                            }
                            catch (System.Exception ex)
                            {
                                WriteLog("ADD() - change_Product_rezka - вставка продуктов, width,vaga, material в базу", ex);
                                m_iDialogResult = 0;
                                this.Close();
                            }
                        }
                        else if (bFlag && iCount != 0)
                        {
                            MessageBox.Show("Данная запись уже есть в базе данных");
                            textBox.Text = "";
                            textBox.Focus();
                        }
                    }
                }
                else 
                {
                    MessageBox.Show("Введите значение");
                    textBox.Focus();
                }
            }

        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                Add();
        }
    }
}
