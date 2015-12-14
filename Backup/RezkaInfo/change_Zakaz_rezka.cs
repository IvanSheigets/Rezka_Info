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
    public partial class change_Zakaz_rezka : Form
    {

        private int m_iDialogResult = -1;

        private int m_iZakazID = 0;

        string m_strPartiya = "";
        int m_iDlinaEtiketki = 0;
        int m_iSmena = 0;
        int m_iCar = 0;
        int m_iMaterialID = 0;
        int m_iTolsID = 0;
        DateTime m_date;

        int m_iTypeRegim = -1;


        Dictionary<string, int> m_dicMaterial = null;
        Dictionary<int, int> m_dicTols = null;

        ////mysql
        string strMSSQLQuery = "";
        public SqlConnection m_MSSQLConnection;
        SqlCommand m_MSSQLCommand;
        SqlDataReader m_MSSQLReader;
        ///////////////////////////



        public change_Zakaz_rezka()
        {
            InitializeComponent();
        }


        public change_Zakaz_rezka(int iZakazID, int iTypeRegim)
        {
            if (iZakazID > 0)
                m_iZakazID = iZakazID;

            if (iTypeRegim > 0)
                m_iTypeRegim = iTypeRegim;

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

        private void change_Zakaz_rezka_Load(object sender, EventArgs e)
        {
            if (CheckConnect() == true)
            {
                m_dicMaterial = new Dictionary<string, int>();
                m_dicTols = new Dictionary<int, int>();

                m_MSSQLCommand = m_MSSQLConnection.CreateCommand();

                if (m_iTypeRegim == 3)  //rezka_master
                    dateTimePicker1.Enabled = true;
                

                /*SelectMaterial(0);*/
                //SelectTols();

                SelectZakaz(m_iZakazID);
            }
        }

        private void SelectMaterial(int iMaterialID)
        {
            if (CheckConnect())
            {
                m_dicMaterial.Clear();
                comboBox_material.Items.Clear();

                strMSSQLQuery = "select id, product_material from itak_etiketka.dbo.itak_productmaterial where material_state=1 order by product_material asc";

                try
                {
                    m_MSSQLCommand.CommandText = strMSSQLQuery;
                    m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
                    if (m_MSSQLReader.HasRows)
                    {
                        while (m_MSSQLReader.Read())
                        {
                            if (m_MSSQLReader["id"] != DBNull.Value || m_MSSQLReader["product_material"] != DBNull.Value)
                            {
                                m_dicMaterial.Add(m_MSSQLReader["product_material"].ToString().Trim(), Convert.ToInt32(m_MSSQLReader["id"]));
                                comboBox_material.Items.Add(m_MSSQLReader["product_material"].ToString().Trim());
                            }
                        }
                    }
                    m_MSSQLReader.Close();

                    if (iMaterialID!=0)
                    {
                        string strKey = "";
                        foreach (var kvp in m_dicMaterial)
                        {
                            if (kvp.Value==iMaterialID)
                            {
                                strKey = kvp.Key;
                                break;
                            }
                        }

                        for (int i=0;i<comboBox_material.Items.Count;i++)
                        {
                            if (comboBox_material.Items[i].ToString()==strKey)
                            {
                                comboBox_material.SelectedIndex = i;
                                break; 
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    WriteLog("SelectMaterial() - change_Zakaz_rezka - получение списка материалов", ex);
                    m_MSSQLReader.Close();
                }
            }
        }


        private void SelectTols(int iTols)
        {
            if (CheckConnect())
            {
                m_dicTols.Clear();
                comboBox_tols.Items.Clear();

                strMSSQLQuery = "select id, product_tols from itak_etiketka.dbo.itak_producttols order by product_tols asc";

                try
                {
                    m_MSSQLCommand.CommandText = strMSSQLQuery;
                    m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
                    if (m_MSSQLReader.HasRows)
                    {
                        while (m_MSSQLReader.Read())
                        {
                            if (m_MSSQLReader["id"] != DBNull.Value || m_MSSQLReader["product_tols"] != DBNull.Value)
                            {
                                m_dicTols.Add(Convert.ToInt32(m_MSSQLReader["product_tols"]), Convert.ToInt32(m_MSSQLReader["id"]));
                                comboBox_tols.Items.Add(m_MSSQLReader["product_tols"].ToString().Trim());
                            }
                        }
                    }
                    m_MSSQLReader.Close();


                    if (iTols != 0)
                    {
                        int iKey = -1;
                        foreach (var kvp in m_dicTols)
                        {
                            if (kvp.Value == iTols)
                            {
                                iKey = kvp.Key;
                                break;
                            }
                        }

                        for (int i = 0; i < comboBox_material.Items.Count; i++)
                        {
                            if (Convert.ToInt32(comboBox_tols.Items[i].ToString()) == iKey)
                            {
                                comboBox_tols.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    WriteLog("SelectTols() - change_Zakaz_rezka - получение списка толщин", ex);
                    m_MSSQLReader.Close();
                }
            }
        }

        private void SelectZakaz(int iZakazID)
        {
            if (CheckConnect())
            {
                strMSSQLQuery = "select partiya, productmaterial_id, producttols_id, dlinaetiketki, smena, machine,datezakaz from itak_etiketka.dbo.itak_zakaz where id=" + iZakazID;
                try
                {
                    m_MSSQLCommand.CommandText = strMSSQLQuery;
                    m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
                    if (m_MSSQLReader.HasRows)
                    {
                        m_MSSQLReader.Read();

                        if (m_MSSQLReader["partiya"] != DBNull.Value)
                        {
                            m_strPartiya = m_MSSQLReader["partiya"].ToString().Trim();
                            textBox_partiya.Text = m_strPartiya;
                        }

                        if (m_MSSQLReader["dlinaetiketki"] != DBNull.Value)
                        {
                            m_iDlinaEtiketki = Convert.ToInt32(m_MSSQLReader["dlinaetiketki"]);
                            textBox_dlinaEtik.Text = m_iDlinaEtiketki.ToString();
                        }

                        if (m_MSSQLReader["smena"] != DBNull.Value)
                        {
                            m_iSmena = Convert.ToInt32(m_MSSQLReader["smena"]);
                            textBox_smena.Text = m_iSmena.ToString();
                        }

                        if (m_MSSQLReader["machine"] != DBNull.Value)
                        {
                            m_iCar = Convert.ToInt32(m_MSSQLReader["machine"]);
                            textBox_Car.Text = m_iCar.ToString();
                        }

                        if (m_MSSQLReader["productmaterial_id"] != DBNull.Value)
                            m_iMaterialID = Convert.ToInt32(m_MSSQLReader["productmaterial_id"]);

                        if (m_MSSQLReader["producttols_id"] != DBNull.Value)
                            m_iTolsID = Convert.ToInt32(m_MSSQLReader["producttols_id"]);

                        if (m_MSSQLReader["datezakaz"] != DBNull.Value)
                            m_date = Convert.ToDateTime(m_MSSQLReader["datezakaz"]);


                    }
                    m_MSSQLReader.Close();

                    if (m_date != null)
                        dateTimePicker1.Value = m_date;

                   // MessageBox.Show(dateTimePicker1.Value.ToShortDateString());

                    SelectMaterial(m_iMaterialID);
                    SelectTols(m_iTolsID);
                }
                catch (System.Exception ex)
                {
                    WriteLog("SelectZakaz() - change_Zakaz_rezka - получение информации по заказу", ex);
                    m_MSSQLReader.Close();
                }

            }

        }

        private void button_change_Click(object sender, EventArgs e)
        {
            if (textBox_partiya.Text.Length != 0)
            {
                m_strPartiya = textBox_partiya.Text.Trim();

                if (comboBox_material.SelectedIndex!=-1)
                {
                    m_iMaterialID = m_dicMaterial[comboBox_material.Items[comboBox_material.SelectedIndex].ToString()];

                    if (comboBox_tols.SelectedIndex!=-1)
                    {
                        m_iTolsID = m_dicTols[Convert.ToInt32(comboBox_tols.Items[comboBox_tols.SelectedIndex].ToString())];

                        if (textBox_dlinaEtik.Text.Length!=0)
                        {
                            m_iDlinaEtiketki = Convert.ToInt32(textBox_dlinaEtik.Text);

                            if (textBox_smena.Text.Length!=0)
                            {
                                m_iSmena = Convert.ToInt32(textBox_smena.Text);

                                if (textBox_Car.Text.Length!=0)
                                {
                                    m_iCar = Convert.ToInt32(textBox_Car.Text);

                                    try
                                    {
                                        strMSSQLQuery = "update itak_etiketka.dbo.itak_zakaz set partiya='" + m_strPartiya + "' " +
                                                                                                 ", productmaterial_id=" + m_iMaterialID +
                                                                                                 ", producttols_id=" + m_iTolsID +
                                                                                                 ", dlinaetiketki=" + m_iDlinaEtiketki +
                                                                                                 ", smena=" + m_iSmena +
                                                                                                 ", machine=" + m_iCar +
                                                                                                 ", datezakaz='" + dateTimePicker1.Value.ToShortDateString()+"' " +
                                                                                                 " where id=" + m_iZakazID;

                                        m_MSSQLCommand.CommandText = strMSSQLQuery;
                                        m_MSSQLCommand.ExecuteNonQuery();

                                        MessageBox.Show("Заказ успешно изменен");
                                        m_iDialogResult = 1;
                                        this.Close();
                                    }
                                    catch (System.Exception ex)
                                    {
                                        WriteLog("button_change_Click() - change_Zakaz_rezka - обновление заказа",ex);
                                    }
                                }                       
                                else
                                {
                                    MessageBox.Show("Введите номер машины");
                                    textBox_Car.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Введите смену");
                                textBox_smena.Focus();
                            }
                        }
                        else 
                        {
                            MessageBox.Show("Введите длину этикетки");
                            textBox_dlinaEtik.Focus();
                        }                        
                    }
                    else
                    {
                        MessageBox.Show("Выберите толщину материала");
                        comboBox_tols.Focus();
                    }                    
                }
                else
                {
                    MessageBox.Show("Выберите материал");
                    comboBox_material.Focus();
                }
            }
            else
            {
                MessageBox.Show("Введите номер партии");
                textBox_partiya.Focus();
            }

        }

        private void button_AddMaterial_Click(object sender, EventArgs e)
        {
            AddOneForm AddForm = new AddOneForm("Материала", 0, 4);
            AddForm.m_MSSQLConnection = m_MSSQLConnection;

            AddForm.ShowDialog();
            if (AddForm.GetDialogResult() == 1)
                SelectMaterial(AddForm.GetAddID());

            AddForm = null;
        }

        private void button_AddTols_Click(object sender, EventArgs e)
        {
            AddOneForm AddForm = new AddOneForm("Толщины", 0, 5);
            AddForm.m_MSSQLConnection = m_MSSQLConnection;

            AddForm.ShowDialog();
            if (AddForm.GetDialogResult() == 1)
                SelectTols(AddForm.GetAddID());

            AddForm = null;
        }

        
    }
}
