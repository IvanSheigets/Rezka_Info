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
    public partial class change_Product_rezka : Form
    {
        private int m_iZakazId = 0;
        private int m_iProductId = 0;
        private int m_iWidth = 0;
        private int m_iZakazchikId = 0;

        private int m_iDialogResult = -1;

        //private int m_iVesTaryIdDB = 0;
        private double m_dVesTary = 0;

        string m_strProductDB = "";
        int m_iProductIdDB = 0;

        Dictionary<string, int> m_dicProduct;
        Dictionary<int, int> m_dicWidth;
        Dictionary<double, int> m_dicVesTary;

        ////mysql
        string strMSSQLQuery = "";
        public SqlConnection m_MSSQLConnection;
        SqlCommand m_MSSQLCommand;
        SqlDataReader m_MSSQLReader;
        ///////////////////////////

        public change_Product_rezka()
        {
            InitializeComponent();
        }

        public change_Product_rezka(int iZakazId, int iProductId, int iWidth)
        {
            if (iZakazId > 0)
                m_iZakazId = iZakazId;

            if (iProductId > 0)
                m_iProductId = iProductId;

            if (iWidth > 0)
                m_iWidth = iWidth;

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

        private void button_cancel_Click(object sender, EventArgs e)
        {
            m_iDialogResult = 0;
            this.Close();
        }

        private void change_Product_rezka_Load(object sender, EventArgs e)
        {
            if (CheckConnect() == true)
            {
                m_dicProduct = new Dictionary<string, int>();
                m_dicWidth = new Dictionary<int, int>();
                m_dicVesTary = new Dictionary<double, int>();

                m_MSSQLCommand = m_MSSQLConnection.CreateCommand();
                SelectProduct(m_iProductId);
                SelectWidth(m_iWidth);
                SelectVesTary("");
            }

        }


        private void SelectProduct(int iProductID)
        {
            if (CheckConnect())
            {
                strMSSQLQuery = "select pr.id,pr.product_name,pr.id_zakazchik from itak_etiketka.dbo.itak_product pr , itak_etiketka.dbo.itak_zakaz z " +
                                "where z.zakazchik_id=pr.id_zakazchik and z.id=" + m_iZakazId+" order by pr.product_name asc";

                m_dicProduct.Clear();
                comboBox_Product.Items.Clear();
                try
                {
                    m_MSSQLCommand.CommandText = strMSSQLQuery;
                    m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
                    if (m_MSSQLReader.HasRows)
                    {
                        while (m_MSSQLReader.Read())
                        {
                            if (m_MSSQLReader["id"] != DBNull.Value || m_MSSQLReader["product_name"] != DBNull.Value)
                            {
                                m_dicProduct.Add(m_MSSQLReader["product_name"].ToString().Trim(), Convert.ToInt32(m_MSSQLReader["id"]));
                                comboBox_Product.Items.Add(m_MSSQLReader["product_name"].ToString().Trim());
                                
                            }
                        }
                    }
                    m_MSSQLReader.Close();
                }
                catch (System.Exception ex)
                {
                    WriteLog("select product() - change_Product_rezka - получение списка продуктов по заказчику", ex);
                    m_MSSQLReader.Close();
                }

                strMSSQLQuery = "select pr.id , pr.product_name, pr.id_zakazchik from itak_etiketka.dbo.itak_product pr " +
                               "where pr.id=" + iProductID;

                 try
                 {
                     m_MSSQLCommand.CommandText = strMSSQLQuery;
                     m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
                     if (m_MSSQLReader.HasRows)
                     {
                         m_MSSQLReader.Read();

                         if (m_MSSQLReader["id"] != DBNull.Value || m_MSSQLReader["product_name"] != DBNull.Value)
                         {
                             m_iProductIdDB = Convert.ToInt32(m_MSSQLReader["id"]);
                             m_strProductDB = m_MSSQLReader["product_name"].ToString().Trim();
                             m_iZakazchikId = Convert.ToInt32(m_MSSQLReader["id_zakazchik"]);
                         }

                     }
                     m_MSSQLReader.Close();
                 }

                 catch (System.Exception ex)
                 {
                     WriteLog("select product() - change_Product_rezka - получение продукта по нашему заказу", ex);
                     m_MSSQLReader.Close();
                 }

                 for (int i = 0; i < comboBox_Product.Items.Count;i++ )
                 {
                     if (comboBox_Product.Items[i].ToString() == m_strProductDB)
                     {
                         comboBox_Product.SelectedIndex = i;
                         break;
                     }
                 }
                
            }
        }

        private void SelectVesTary(string strVes)
        {
            if (CheckConnect())
            {
                strMSSQLQuery = "select id, vaga from itak_etiketka.dbo.itak_vagatary order by vaga";
                m_dicVesTary.Clear();
                comboBox_vesTary.Items.Clear();
                try
                {
                    m_MSSQLCommand.CommandText = strMSSQLQuery;
                    m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
                    if (m_MSSQLReader.HasRows)
                    {
                        while (m_MSSQLReader.Read())
                        {
                            if (m_MSSQLReader["id"] != DBNull.Value || m_MSSQLReader["vaga"] != DBNull.Value)
                            {
                                m_dicVesTary.Add(Convert.ToDouble(m_MSSQLReader["vaga"]), Convert.ToInt32(m_MSSQLReader["id"]));
                                comboBox_vesTary.Items.Add(m_MSSQLReader["vaga"].ToString().Trim());
                            }
                        }
                    }
                    m_MSSQLReader.Close();
                }
                catch (System.Exception ex)
                {
                    WriteLog("SelectVesTary() - change_Product_rezka - получение списка vaga", ex);
                    m_MSSQLReader.Close();
                }

                if (strVes.Length == 0)
                {
                    strMSSQLQuery = "select distinct vagatary from itak_etiketka.dbo.itak_vihidrylon where zakaz_id=" + m_iZakazId + " and product_id=" + m_iProductId;
                    try
                    {
                        m_MSSQLCommand.CommandText = strMSSQLQuery;
                        m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
                        if (m_MSSQLReader.HasRows)
                        {
                            m_MSSQLReader.Read();
                            m_dVesTary = Convert.ToDouble(m_MSSQLReader[0]);
                            m_MSSQLReader.Close();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        WriteLog("SelectVesTary() - change_Product_rezka - получение веса тару в продукте", ex);
                        m_MSSQLReader.Close();
                    }
                }
                else
                    m_dVesTary = Convert.ToDouble(strVes.Replace('.',','));

                for (int i = 0; i < comboBox_vesTary.Items.Count; i++)
                {
                    if (comboBox_vesTary.Items[i].ToString() == m_dVesTary.ToString())
                    {
                        comboBox_vesTary.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void SelectWidth(int iWidth)
        {
            if (CheckConnect())
            {
                strMSSQLQuery = "select pw.id, pw.product_width from itak_etiketka.dbo.itak_productwidth pw order by product_width";
                m_dicWidth.Clear();
                comboBox_Width.Items.Clear();
                try
                {
                    m_MSSQLCommand.CommandText = strMSSQLQuery;
                    m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
                    if (m_MSSQLReader.HasRows)
                    {
                        while (m_MSSQLReader.Read())
                        {
                            if (m_MSSQLReader["id"] != DBNull.Value || m_MSSQLReader["product_width"] != DBNull.Value)
                            {
                                m_dicWidth.Add(Convert.ToInt32(m_MSSQLReader["product_width"]), Convert.ToInt32(m_MSSQLReader["id"]));
                                comboBox_Width.Items.Add(m_MSSQLReader["product_width"].ToString().Trim());
                            }
                        }
                    }
                    m_MSSQLReader.Close();
                }
                catch (System.Exception ex)
                {
                    WriteLog("SelectWidth() - change_Product_rezka - получение списка width", ex);
                    m_MSSQLReader.Close();
                }

                for (int i = 0; i < comboBox_Width.Items.Count; i++)
                {
                    if (comboBox_Width.Items[i].ToString() == iWidth.ToString())
                    {
                        comboBox_Width.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void button_AddProduct_Click(object sender, EventArgs e)
        {
            
            AddOneForm AddForm = new AddOneForm("Продукта", m_iZakazchikId,1);
            AddForm.m_MSSQLConnection = m_MSSQLConnection;

            AddForm.ShowDialog();

            if (AddForm.GetDialogResult()==1)
                SelectProduct(AddForm.GetAddID());

            AddForm = null;
            
        }

        private void button_change_Click(object sender, EventArgs e)
        {
            if (CheckConnect())
            {
                bool bFlag = false;
                 
                if (comboBox_Product.SelectedIndex != -1 && comboBox_Width.SelectedIndex != -1 && comboBox_vesTary.SelectedIndex!=-1)
                {
                 //   MessageBox.Show(m_dicProduct[comboBox_Product.Items[comboBox_Product.SelectedIndex].ToString()].ToString() +" - "+m_iZakazId);
                    int iProductId = m_dicProduct[comboBox_Product.Items[comboBox_Product.SelectedIndex].ToString()];
                    int iWidth = Convert.ToInt32(comboBox_Width.Items[comboBox_Width.SelectedIndex]);
                    double dVesTary = Convert.ToDouble(comboBox_vesTary.Items[comboBox_vesTary.SelectedIndex]);
                    try
                    {
                        strMSSQLQuery = "update itak_etiketka.dbo.itak_vihidrylon set product_id=" + iProductId + ", width=" + iWidth + ", vagatary="+dVesTary.ToString("0.00").Replace(',','.')+" where product_id=" + m_iProductIdDB + " and zakaz_id=" + m_iZakazId + " and width="+m_iWidth+" and vagatary="+m_dVesTary.ToString("0.00").Replace(',','.');
                        m_MSSQLCommand.CommandText = strMSSQLQuery;
                        m_MSSQLCommand.ExecuteNonQuery();

                        bFlag = true;
                    }
                    catch(System.Exception ex)
                    {
                        WriteLog("button_change_Click() - change_Product_rezka - изменение продукта, ширины, вес тары в заказе",ex);
                        bFlag = false;
                    }
                }

                if (bFlag)
                {
                    MessageBox.Show("Изменение прошли успешно");
                    m_iDialogResult = 1;
                    this.Close();
                }
                else 
                {
                    MessageBox.Show("Ошибка изменений");
                    m_iDialogResult = 0;
                    this.Close();
                }
            }
        }

        private void button_AddWidth_Click(object sender, EventArgs e)
        {
            AddOneForm AddForm = new AddOneForm("Ширины",m_iZakazchikId, 2);
            AddForm.m_MSSQLConnection = m_MSSQLConnection;

            AddForm.ShowDialog();

            if (AddForm.GetDialogResult() == 1)
                SelectWidth(Convert.ToInt32(AddForm.GetAddSting()));

            AddForm = null;

        }

        private void button_AddVesTary_Click(object sender, EventArgs e)
        {
            AddOneForm AddForm = new AddOneForm("Вес Тары", m_iZakazchikId, 3);
            AddForm.m_MSSQLConnection = m_MSSQLConnection;

            AddForm.ShowDialog();
            if (AddForm.GetDialogResult() == 1)
                SelectVesTary(AddForm.GetAddSting());

            AddForm = null;
        }
    }
}
