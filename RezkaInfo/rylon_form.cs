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
using System.Drawing.Printing;
using System.Drawing.Drawing2D;
using PrintEtiketkaMy;
using Microsoft.Win32;


namespace RezkaInfo
{
    public partial class rylon_form : Form
    {
        int m_iDialogResult = -1;
        int m_iTypeDialog = -1;
        int m_iTypeRegim = -1;
        int m_iWidth = -1;

        int m_iProductID = -1;
        int m_iZakazID = -1;

        int m_iCheckMen = -1;        

        int m_iStatusZakaz = -1;//1 - в работе порезки  0-порезан

        int m_iPrintPage = 0;
        int m_iCountRows = 0;
        int m_iCounter = 0;

        


        bool flag = false;
        ////mysql
        string strMSSQLQuery = "";
        public SqlConnection m_MSSQLConnection;
        SqlCommand m_MSSQLCommand;
        SqlDataReader m_MSSQLReader;
        ///////////////////////////

        Dictionary<string, int> m_dicRylon;
        Dictionary<int, int> m_dicRylonChecked;
        Dictionary<int, int> m_dicRylonColorCheck;
        

        delChange_form m_delChange = null;
        change_rylon m_changeRylon = null;



        string m_strZakazchik = "";
        string m_strProductName = "";
        string m_strPartiya = "";
        string m_strDateZakaz = "";
        string m_strDlinaEtiketki = "";
        string m_strProductTols = "";
        string m_strProductMaterial = "";
        string m_strVagaTary = "";
        string m_strWidth = "";
        string m_strSmena = "";

               
        public rylon_form()
        {
            InitializeComponent();
        }

        public rylon_form(int iTypeDialog, int iTypeRegim)//1- odin vid //2 - neskolko vidov
        {
            m_iTypeDialog = iTypeDialog;
            InitializeComponent();
        }


        public int GetDialogResult()
        {return m_iDialogResult;}

        public void SetCheckMen(int iCheckMen)
        {   m_iCheckMen = iCheckMen;    }

        public void SetWidth(int iWidth)
        {    m_iWidth = iWidth;     }

        public void SetProductID(int iProductID)
        {m_iProductID = iProductID;}

        public void SetZakazID(int iZakazID)
        { m_iZakazID = iZakazID; }

        public void SetStatusZakaz(int iStatus)
        {m_iStatusZakaz = iStatus;}

        public void SetRegim(int iRegim)
        { m_iTypeRegim = iRegim; }


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

        private void button_ok_Click(object sender, EventArgs e)
        {
            /*string strKey = "";
            int iCheckRylon = 0;
            string strCheckTime = "";
            int iCheckMen = -1;

            for (int i = 0; i < dataGridView_rylon.Rows.Count;i++)
            {
                strKey = dataGridView_rylon.Rows[i].Cells[0].Value.ToString() + " - "
                    + dataGridView_rylon.Rows[i].Cells[1].Value.ToString() + " - "
                    + dataGridView_rylon.Rows[i].Cells[2].Value.ToString() + " - "
                    + dataGridView_rylon.Rows[i].Cells[3].Value.ToString() + " - "
                    + dataGridView_rylon.Rows[i].Cells[4].Value.ToString();

                if (Convert.ToBoolean(dataGridView_rylon.Rows[i].Cells["Check_rylon"].Value) == true)
                    iCheckRylon = 1;
                else if (Convert.ToBoolean(dataGridView_rylon.Rows[i].Cells["Check_rylon"].Value) == false)
                    iCheckRylon = 0;

                if (dataGridView_rylon.Rows[i].Cells["Check_men"].Value != null)
                    iCheckMen = Convert.ToInt32(dataGridView_rylon.Rows[i].Cells["Check_men"].Value.ToString());
                else iCheckMen = 0;
                
                strCheckTime = dataGridView_rylon.Rows[i].Cells["Check_Time"].Value.ToString();

                if (CheckConnect())
                {
                    strMSSQLQuery = "update itak_etiketka.dbo.itak_vihidrylon set check_men=" + iCheckMen + ", state_rylon=" + iCheckRylon + ", check_time=CONVERT (SMALLDATETIME, CONVERT (DATETIME, CONVERT (VARCHAR, '" + strCheckTime + "',103),103)) where id=" + m_dicRylon[strKey];
                    
                    m_MSSQLCommand.CommandText = strMSSQLQuery;
                    m_MSSQLCommand.ExecuteNonQuery();
                }
            }*/
            
            m_iDialogResult = 1;
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            m_iDialogResult = 0;
            this.Close();
        }

        private void rylon_form_Load(object sender, EventArgs e)
        {
            m_dicRylon = new Dictionary<string, int>();
            m_dicRylonChecked = new Dictionary<int,int>();
            m_dicRylonColorCheck = new Dictionary<int, int>();



            if (CheckConnect() == true)
            {
                m_MSSQLCommand = m_MSSQLConnection.CreateCommand();
                SelectRylon();
            }

            if (m_iTypeRegim == 2)
            {
                button_all_label.Visible = true;
                checkbox_logo.Visible = true;
            }

            if (m_iTypeRegim == 0 || m_iTypeRegim==2)
                checkBox_selectAll.Enabled = false;

            flag = true;
        }

        private void SelectRylon()
        {
            if (CheckConnect())
            {
                strMSSQLQuery = "select brytto, brytto-vagatary, dlinarylona, koletiketki, num_rylon, state_rylon, id, check_men, check_time  from itak_etiketka.dbo.itak_vihidrylon where zakaz_id=" + m_iZakazID.ToString() + " and product_id=" + m_iProductID + " and width=" + m_iWidth.ToString() + " order by num_rylon ASC";
                m_MSSQLCommand.CommandText = strMSSQLQuery;
                m_MSSQLReader = m_MSSQLCommand.ExecuteReader();

                string strChecked = "";
                m_dicRylon.Clear();
                m_dicRylonChecked.Clear();
                m_dicRylonColorCheck.Clear();
                dataGridView_rylon.Rows.Clear();
                int iCheckRylon = 0;
                string strKey = "";
                int iCountCheck = 0;
                int iCountUnCheck = 0;
                int iCheckMen = -1;

                string strCheckTime = "";

                int i = 0;
                while (m_MSSQLReader.Read())
                {
                    try
                    {
                        dataGridView_rylon.Rows.Add(m_MSSQLReader["num_rylon"].ToString().Trim(),
                                                    m_MSSQLReader["brytto"].ToString().Trim(),
                                                    m_MSSQLReader[1].ToString().Trim(),
                                                    m_MSSQLReader["dlinarylona"].ToString().Trim(),
                                                    m_MSSQLReader["koletiketki"].ToString().Trim());

                        iCheckMen = Convert.ToInt32(m_MSSQLReader["check_men"].ToString().Trim());

                        if (iCheckMen != 0)
                            dataGridView_rylon.Rows[i].Cells["Check_men"].Value = iCheckMen.ToString();
                      //  MessageBox.Show("1");

                        if (m_MSSQLReader["check_time"].ToString().Trim() == "01.01.1900 0:00:00")
                            strCheckTime = "";
                        else strCheckTime = m_MSSQLReader["check_time"].ToString().Trim();
                        dataGridView_rylon.Rows[i].Cells["Check_Time"].Value = strCheckTime;

                        strChecked = m_MSSQLReader["state_rylon"].ToString().Trim();
                        if (strChecked == "0")
                        {
                            iCheckRylon = 0;
                            iCountUnCheck++;
                        }
                        else if (strChecked == "1")
                        {
                            iCountCheck++;
                            iCheckRylon = 1;
                        }
                        else if (strChecked == "2")
                        {
                            iCheckRylon = 2;
                            iCountUnCheck++;
                        }

                        if (iCheckRylon==0  )
                            dataGridView_rylon.Rows[i].Cells["check_rylon"].Value = false;
                        else if (iCheckRylon == 1)
                            dataGridView_rylon.Rows[i].Cells["check_rylon"].Value = true;
                        

                        if (iCheckRylon==1)
                            dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                        else if (iCheckRylon == 0)
                            dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        else if (iCheckRylon == 2)
                        {
                            dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor = Color.Brown;
                            dataGridView_rylon.Rows[i].Cells["Check_rylon"].ReadOnly = true;
                            dataGridView_rylon.Rows[i].Cells["Check_rylon"].Value= false;
                        }

                        if ((m_iTypeRegim == 0 || m_iTypeRegim==2) && iCheckRylon != 2)
                            dataGridView_rylon.Rows[i].Cells["Check_rylon"].ReadOnly = true;

                        m_dicRylonColorCheck.Add(i, iCheckRylon);

                        i++;

                        strKey = m_MSSQLReader["num_rylon"].ToString().Trim()+" - "
                            + m_MSSQLReader["brytto"].ToString().Trim()+" - "
                            + m_MSSQLReader[1].ToString().Trim() + " - "
                            + m_MSSQLReader["dlinarylona"].ToString().Trim()+" - "
                            + m_MSSQLReader["koletiketki"].ToString().Trim();

                        m_dicRylon.Add(strKey, Convert.ToInt32(m_MSSQLReader["id"].ToString().Trim()));
                        m_dicRylonChecked.Add(Convert.ToInt32(m_MSSQLReader["id"].ToString().Trim()), iCheckRylon);

                        if (dataGridView_rylon.Rows.Count!=0)
                            dataGridView_rylon.Rows[0].Selected = false;
                        
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
                }
                m_MSSQLReader.Close();
               
                if (i == iCountCheck)
                    checkBox_selectAll.Checked = true;
                else checkBox_selectAll.Checked = false;


               /* for (i=0;i<m_dicRylonColorCheck.Count;i++)
                {
                    if (m_dicRylonColorCheck[i] == 0)
                    {
                        dataGridView_rylon.Rows[i].Cells["Check_rylon"].Value = false;
                        dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                    else if (m_dicRylonColorCheck[i] == 1)
                    {
                        dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                        dataGridView_rylon.Rows[i].Cells["Check_rylon"].Value = true;
                    }
                    else if (m_dicRylonColorCheck[i] == 2)
                    {
                        dataGridView_rylon.Rows[i].Cells["Check_rylon"].Value = false;
                        dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor = Color.Brown;
                    }

                }*/

            }
        }

        private void checkBox_selectAll_CheckedChanged(object sender, EventArgs e)
        {
           /* if (flag == true)
            {
                int iStateRylon = 0;
                if (checkBox_selectAll.Checked == true)
                    for (int i = 0; i < dataGridView_rylon.Rows.Count; i++)
                    {
                        dataGridView_rylon.Rows[i].Cells["Check_rylon"].Value = true;
                        dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    }
                else
                    for (int i = 0; i < dataGridView_rylon.Rows.Count; i++)
                    {
                        dataGridView_rylon.Rows[i].Cells["Check_rylon"].Value = false;
                        dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }

                
                for (int i = 0; i < dataGridView_rylon.Rows.Count; i++)
                {
                    DateTime dt = System.DateTime.Now;

                    string strCheckTime = dt.Year + "." + dt.Month + "." + dt.Day + " " + dt.Hour + ":" + dt.Minute + ":" + dt.Second;

                    dataGridView_rylon.Rows[i].Cells["Check_Time"].Value = strCheckTime;
                    dataGridView_rylon.Rows[i].Cells["Check_men"].Value = m_iCheckMen;

                    if (Convert.ToBoolean(dataGridView_rylon.Rows[i].Cells["Check_rylon"].Value) == true)
                        iStateRylon = 1;
                    else if (Convert.ToBoolean(dataGridView_rylon.Rows[i].Cells["Check_rylon"].Value) == false)
                        iStateRylon = 0;
                    
                
                    string strKey = dataGridView_rylon.Rows[i].Cells[0].Value.ToString() + " - "
                       + dataGridView_rylon.Rows[i].Cells[1].Value.ToString() + " - "
                       + dataGridView_rylon.Rows[i].Cells[2].Value.ToString() + " - "
                       + dataGridView_rylon.Rows[i].Cells[3].Value.ToString() + " - "
                       + dataGridView_rylon.Rows[i].Cells[4].Value.ToString();


                    if (CheckConnect())
                    {
                        strMSSQLQuery = "update itak_etiketka.dbo.itak_vihidrylon set state_rylon=" + iStateRylon + ", check_men=" + m_iCheckMen + ", check_time=convert(smalldatetime,'" + strCheckTime + "',101) where id=" + m_dicRylon[strKey];
                        m_MSSQLCommand.CommandText = strMSSQLQuery;
                        m_MSSQLCommand.ExecuteNonQuery();
                    }
                }
            }*/

        }

        private void dataGridView_rylon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView_rylon.Columns["Check_rylon"].Index && m_iTypeRegim==1)
            {
                DataGridViewCheckBoxCell curCell = (DataGridViewCheckBoxCell)dataGridView_rylon.Rows[e.RowIndex].Cells["Check_rylon"];//, e.RowIndex];

                if (curCell.ReadOnly == false)
                {
                    int iStateRylon = 0;
                    if (Convert.ToBoolean(curCell.Value) == true)
                    {
                        dataGridView_rylon.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                        iStateRylon = 1;
                    }
                    else if (Convert.ToBoolean(curCell.Value) == false)
                    {
                        dataGridView_rylon.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                        iStateRylon = 0;
                    }

                    DateTime dt = System.DateTime.Now;

                    string strCheckTime = dt.Year + "." + dt.Month + "." + dt.Day + " " + dt.Hour + ":" + dt.Minute + ":" + dt.Second;
                    dataGridView_rylon.Rows[e.RowIndex].Cells["Check_Time"].Value = strCheckTime;
                    dataGridView_rylon.Rows[e.RowIndex].Cells["Check_men"].Value = m_iCheckMen.ToString();

                    string strKey = dataGridView_rylon.Rows[e.RowIndex].Cells[0].Value.ToString() + " - "
                        + dataGridView_rylon.Rows[e.RowIndex].Cells[1].Value.ToString() + " - "
                        + dataGridView_rylon.Rows[e.RowIndex].Cells[2].Value.ToString() + " - "
                        + dataGridView_rylon.Rows[e.RowIndex].Cells[3].Value.ToString() + " - "
                        + dataGridView_rylon.Rows[e.RowIndex].Cells[4].Value.ToString();


                    if (CheckConnect())
                    {
                        strMSSQLQuery = "update itak_etiketka.dbo.itak_vihidrylon set state_rylon=" + iStateRylon + ", check_men=" + m_iCheckMen + ", check_time=convert(smalldatetime,'" + strCheckTime + "',101) where id=" + m_dicRylon[strKey];
                        m_MSSQLCommand.CommandText = strMSSQLQuery;
                        m_MSSQLCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        private void dataGridView_rylon_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView_rylon.IsCurrentCellDirty)
            {
                dataGridView_rylon.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView_rylon_DoubleClick(object sender, EventArgs e)
        {
            if (/*m_iTypeRegim == 1 ||*/ m_iTypeRegim == 2)
            {
                int iRowSelect = -1;
                for (int i = 0; i < dataGridView_rylon.Rows.Count; i++)
                    if (dataGridView_rylon.Rows[i].Selected == true)
                    {
                        iRowSelect = i;
                        break;
                    }

                if (iRowSelect != -1)
                {
                    if (dataGridView_rylon.Rows[iRowSelect].DefaultCellStyle.BackColor != Color.Brown)
                    {
                        m_delChange = new delChange_form();
                        m_delChange.ShowDialog();

                        string strKey = dataGridView_rylon.Rows[iRowSelect].Cells["Num_rylon"].Value.ToString() + " - " +
                                               dataGridView_rylon.Rows[iRowSelect].Cells["Brytto"].Value.ToString() + " - " +
                                               dataGridView_rylon.Rows[iRowSelect].Cells["Netto"].Value.ToString() + " - " +
                                               dataGridView_rylon.Rows[iRowSelect].Cells["Dlina"].Value.ToString() + " - " +
                                               dataGridView_rylon.Rows[iRowSelect].Cells["CountEtik"].Value.ToString();

                        int iRylonID = Convert.ToInt32(m_dicRylon[strKey]);

                        if (m_delChange.GetDialogResult() == 1)
                        {
                            m_changeRylon = new change_rylon();
                            m_changeRylon.m_MSSQLConnection = m_MSSQLConnection;
                            m_changeRylon.SetRylonID(iRylonID);

                            m_changeRylon.ShowDialog();

                            if (m_changeRylon.GetDialogResult() == 1)
                            {
                                SelectRylon();
                               // SelectRylon();
                                dataGridView_rylon.Rows[iRowSelect].Selected = true;
                            }

                        }
                        else if (m_delChange.GetDialogResult() == 2)
                        {
                            if (MessageBox.Show(this, "Вы действительно хотите удалить рулон №" + dataGridView_rylon.Rows[iRowSelect].Cells["Num_rylon"].Value.ToString() + "??", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                if (CheckConnect())
                                {
                                    DateTime dt = System.DateTime.Now;
                                    string strCheckTime = dt.Year + "." + dt.Month + "." + dt.Day + " " + dt.Hour + ":" + dt.Minute + ":" + dt.Second;
                                    if(m_iTypeRegim==1)
                                        strMSSQLQuery = "update itak_etiketka.dbo.itak_vihidrylon set  state_rylon=2, check_men=" + m_iCheckMen + ", check_time='" + strCheckTime + "' where id=" + iRylonID;
                                    else if (m_iTypeRegim == 2)
                                        strMSSQLQuery = "update itak_etiketka.dbo.itak_vihidrylon set state_rylon=2, check_time='" + strCheckTime + "' where id=" + iRylonID;

                                    m_MSSQLCommand.CommandText = strMSSQLQuery;
                                    m_MSSQLCommand.ExecuteNonQuery();
                                    
                                    //SelectRylon();
                                   // MessageBox.Show(dataGridView_rylon.Rows[0].Cells["Check_rylon"].Value.ToString());
                                }
                            }
                            SelectRylon();
                        }

                        m_delChange = null;
                    }
                }

            }

        }

        private void button_print_Click(object sender, EventArgs e)
        {
            if (dataGridView_rylon.Rows.Count > 0)
            {
                strMSSQLQuery = "select zk.zakazchik_name,  pr.product_name, z.partiya, z.datezakaz, z.smena,z.machine, z.dlinaetiketki, pt.product_tols, pm.product_material, vh.vagatary, vh.width " +
                                "from itak_etiketka.dbo.itak_zakaz z, itak_etiketka.dbo.itak_zakazchik zk, itak_etiketka.dbo.itak_vihidrylon vh, itak_etiketka.dbo.itak_product pr, "+
                                "itak_etiketka.dbo.itak_producttols pt, itak_etiketka.dbo.itak_productmaterial pm "+
                                "where z.zakazchik_id=zk.id and vh.zakaz_id=z.id and vh.product_id=pr.id and z.producttols_id=pt.id and z.productmaterial_id=pm.id and z.id="+m_iZakazID;
                m_MSSQLCommand.CommandText = strMSSQLQuery;
                m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
                while (m_MSSQLReader.Read())
                {
                    try
                    {
                        m_strZakazchik = m_MSSQLReader["zakazchik_name"].ToString().Trim();
                        m_strProductName = m_MSSQLReader["product_name"].ToString().Trim();
                        m_strPartiya = m_MSSQLReader["partiya"].ToString().Trim();
                        m_strDateZakaz = m_MSSQLReader["datezakaz"].ToString().Trim();
                        m_strDlinaEtiketki = m_MSSQLReader["dlinaetiketki"].ToString().Trim();
                        m_strProductTols = m_MSSQLReader["product_tols"].ToString().Trim();
                        m_strProductMaterial = m_MSSQLReader["product_material"].ToString().Trim();
                        m_strVagaTary = m_MSSQLReader["vagatary"].ToString().Trim();
                        m_strWidth = m_MSSQLReader["width"].ToString().Trim();
                        m_strSmena = m_MSSQLReader["machine"].ToString().Trim() + "/" + m_MSSQLReader["smena"].ToString().Trim(); 

                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                m_MSSQLReader.Close();



                PrintDocument docPrint = new PrintDocument();
                if (docPrint.PrinterSettings.IsValid)
                {
                    docPrint.DefaultPageSettings.Margins.Top = 10;
                    docPrint.DefaultPageSettings.Margins.Bottom = 10;
                    docPrint.DefaultPageSettings.Margins.Right = 10;
                    docPrint.DefaultPageSettings.Margins.Left = 10;

                    docPrint.DefaultPageSettings.Landscape = false;


                    docPrint.PrintPage += new PrintPageEventHandler
                           (this.docPrint_PrintPage);

                    docPrint.PrinterSettings.Copies = 1;

                    PrintPreviewDialog dlgPrint = new PrintPreviewDialog();
                    //dlgPrint.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height);
                    dlgPrint.Document = docPrint;
                     //dlgPrint.ShowDialog();
                    docPrint.Print();
                }
            }
        }

        
        private void docPrint_PrintPage(object sender, PrintPageEventArgs e)
        {
            System.Drawing.Font fnt10Bold = new Font("Times New Roman", 10, FontStyle.Bold);
            System.Drawing.Font fnt10 = new Font("Times New Roman", 10, FontStyle.Regular);

            System.Drawing.Font fnt8 = new Font("Times New Roman", 8, FontStyle.Regular);

            System.Drawing.Font fnt12Bold = new Font("Times New Roman", 12, FontStyle.Bold);
            System.Drawing.Font fnt12 = new Font("Times New Roman", 12, FontStyle.Regular);
            System.Drawing.Font fnt12BoldUnder = new Font("Times New Roman", 12, FontStyle.Bold | FontStyle.Underline);
            System.Drawing.Font fnt14Bold = new Font("Times New Roman", 14, FontStyle.Bold);
            System.Drawing.Font fnt14 = new Font("Times New Roman", 14, FontStyle.Regular);
            System.Drawing.Font fnt14BoldUnder = new Font("Times New Roman", 14, FontStyle.Bold | FontStyle.Underline);
            System.Drawing.Font fnt16Bold = new Font("Times New Roman", 16, FontStyle.Bold);
            System.Drawing.Font fnt16BoldUnder = new Font("Times New Roman", 16, FontStyle.Bold | FontStyle.Underline);

            int t = 0;

            int iLeft = 40;

            int iTableWidth = iLeft + 620;
            int[] iCoord = { iLeft, iLeft + 60, iLeft + 130, iLeft + 210, iLeft + 280, iLeft + 355, iLeft + 425, iLeft + 555, iLeft + 620 };


            if (m_iPrintPage == 0)
            {
                t += 45;
                e.Graphics.DrawString("Замовник:", fnt12, Brushes.Black, 40, t);
                e.Graphics.DrawString(m_strZakazchik, fnt12Bold, Brushes.Black, 130, t);

                t += 20;
                e.Graphics.DrawString("Продукція:", fnt12, Brushes.Black, 40, t);
                e.Graphics.DrawString(m_strProductName, fnt12Bold, Brushes.Black, 130, t);

                t += 20;
                e.Graphics.DrawString("Партія: ", fnt12, Brushes.Black, 40, t);
                e.Graphics.DrawString(m_strPartiya, fnt12Bold, Brushes.Black, 130, t);

                t += 20;
                e.Graphics.DrawString("Матеріал: ", fnt12, Brushes.Black, 40, t);
                m_strProductMaterial = m_strProductMaterial + " " + m_strProductTols;
                e.Graphics.DrawString(m_strProductMaterial, fnt12Bold, Brushes.Black, 130, t);

                t += 20;
                e.Graphics.DrawString("Крок малюнку, мм:", fnt12, Brushes.Black, 40, t);
                e.Graphics.DrawString(m_strDlinaEtiketki, fnt12Bold, Brushes.Black, 200, t);

                t += 20;
                e.Graphics.DrawString("Ширина бобіни, мм:", fnt12, Brushes.Black, 40, t);
                e.Graphics.DrawString(m_strWidth, fnt12Bold, Brushes.Black, 200, t);

                t += 20;
                e.Graphics.DrawString("Зміна:", fnt12, Brushes.Black, 40, t);
                e.Graphics.DrawString(m_strSmena, fnt12Bold, Brushes.Black, 100, t);

                t += 20;
                if (m_strDateZakaz.Length != 0)
                    m_strDateZakaz = m_strDateZakaz.Substring(0, m_strDateZakaz.IndexOf(' '));
                else m_strDateZakaz = "";
                e.Graphics.DrawString("Дата замовлення:", fnt12, Brushes.Black, 40, t);
                e.Graphics.DrawString(m_strDateZakaz, fnt12Bold, Brushes.Black, 200, t);

                t += 30;
                
                int t1 = t;
                int t2 = t;


                m_iCountRows = dataGridView_rylon.Rows.Count;
                int[] iBarCode = new int[m_iCountRows];

                int iCountRows = 0;
                if (m_iCountRows > 58)
                    iCountRows = 58;
                else
                    iCountRows = m_iCountRows;






                e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t1), new Point(iTableWidth, t1));
                e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t1 + 35), new Point(iTableWidth, t1 + 35));
                for (int j = 0; j <= 8; j++)
                    e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iCoord[j], t1 - 1), new Point(iCoord[j], t1 + 36));

                e.Graphics.DrawString("№", fnt10Bold, Brushes.Black, iLeft + 20, t1 + 2);
                e.Graphics.DrawString("рулона", fnt10Bold, Brushes.Black, iLeft + 5, t1 + 15);

                e.Graphics.DrawString("Масса", fnt10Bold, Brushes.Black, iLeft + 75, t1 + 2);
                e.Graphics.DrawString("нетто, кг", fnt10Bold, Brushes.Black, iLeft + 65, t1 + 15);

                e.Graphics.DrawString("Масса", fnt10Bold, Brushes.Black, iLeft + 150, t1 + 2);
                e.Graphics.DrawString("брутто, кг", fnt10Bold, Brushes.Black, iLeft + 135, t1 + 15);

                e.Graphics.DrawString("Довжина", fnt10Bold, Brushes.Black, iLeft + 215, t1 + 2);
                e.Graphics.DrawString("м.п.", fnt10Bold, Brushes.Black, iLeft + 233, t1 + 15);

                e.Graphics.DrawString("Кількість", fnt10Bold, Brushes.Black, iLeft + 285, t1 + 2);
                e.Graphics.DrawString("тис. шт.", fnt10Bold, Brushes.Black, iLeft + 290, t1 + 15);

                e.Graphics.DrawString("№ провер.", fnt10Bold, Brushes.Black, iLeft + 355, t1 + 9);

                e.Graphics.DrawString("Дата", fnt10Bold, Brushes.Black, iLeft + 470, t1 + 2);
                e.Graphics.DrawString("перевірки", fnt10Bold, Brushes.Black, iLeft + 455, t1 + 15);

                e.Graphics.DrawString("Статус", fnt10Bold, Brushes.Black, iLeft + 560, t1 + 2);
                e.Graphics.DrawString("рулона", fnt10Bold, Brushes.Black, iLeft + 560, t1 + 15);

                t1 += 35;

                int i = 0;

                for ( i = 0; i < iCountRows; i++)
                {
                    for (int j = 0; j <= 8; j++)
                        if (j == 0 || j == 8)
                            e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iCoord[j], t1 - 1), new Point(iCoord[j], t1 + 15));
                        else
                            e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[j], t1 - 1), new Point(iCoord[j], t1 + 15));

                    string strTemp1 = "";
                    if (dataGridView_rylon.Rows[i].Cells["Num_rylon"].Value != null)
                        strTemp1 = dataGridView_rylon.Rows[i].Cells["Num_rylon"].Value.ToString();//надо
                    
                    if (Convert.ToInt32(strTemp1) > 9)
                        e.Graphics.DrawString(strTemp1, fnt8, Brushes.Black, iCoord[0] + 18, t1);
                    else
                        e.Graphics.DrawString(strTemp1, fnt8, Brushes.Black, iCoord[0] + 25, t1);

                    if (dataGridView_rylon.Rows[i].Cells["Netto"].Value != null)
                        e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Netto"].Value.ToString(), fnt10, Brushes.Black, iCoord[1] + 5, t1);

                    if (dataGridView_rylon.Rows[i].Cells["Brytto"].Value != null)
                        e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Brytto"].Value.ToString(), fnt10, Brushes.Black, iCoord[2] + 5, t1);

                    if (dataGridView_rylon.Rows[i].Cells["Dlina"].Value != null)
                        e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Dlina"].Value.ToString(), fnt10, Brushes.Black, iCoord[3] + 5, t1);

                    if (dataGridView_rylon.Rows[i].Cells["CountEtik"].Value != null)
                        e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["CountEtik"].Value.ToString(), fnt10, Brushes.Black, iCoord[4] + 5, t1);

                    if (dataGridView_rylon.Rows[i].Cells["Check_men"].Value!=null)
                        e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Check_men"].Value.ToString(), fnt10, Brushes.Black, iCoord[5] + 30, t1);
                    if (dataGridView_rylon.Rows[i].Cells["Check_Time"].Value!=null)
                        e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Check_Time"].Value.ToString(), fnt10, Brushes.Black, iCoord[6] + 5, t1);

                    if (dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor == Color.Red)
                        e.Graphics.DrawString("Не перев.", fnt10, Brushes.Black, iCoord[7] + 5, t1);
                    else if (dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor == Color.LightGreen)
                        e.Graphics.DrawString("Перев.", fnt10, Brushes.Black, iCoord[7] + 5, t1);
                    else if (dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor == Color.Brown)
                        e.Graphics.DrawString("Видален.", fnt10, Brushes.Black, iCoord[7] + 5, t1);


                    e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t1 + 15), new Point(iTableWidth, t1 + 15));

                    t1 += 15;
                }

                if (m_iCountRows <= 58)
                {
                    e.HasMorePages = false;
                    m_iPrintPage = 0;
                }
                else
                {
                    e.Graphics.DrawString("-  1  -", fnt14Bold, Brushes.Black, 375, 1130);
                    e.HasMorePages = true;
                    m_iCounter = i;
                    m_iPrintPage = 1;
                }
            }
            else if (m_iPrintPage==1)
            {
               
                int t1 = t+40;
                int i = 0;

                if (m_iCountRows > 58)
                {
                    int iCountPageRows = 0;

                    if (m_iCountRows > 130)
                        iCountPageRows = 130;
                    else iCountPageRows = m_iCountRows;

                    int iCountRows = iCountPageRows - m_iCounter;

                    e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t1), new Point(iTableWidth, t1));

                    for (i = m_iCounter; i < iCountPageRows; i++)
                    {
                        for (int j = 0; j <= 8; j++)
                            if (j == 0 || j == 8)
                                e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iCoord[j], t1 - 1), new Point(iCoord[j], t1 + 15));
                            else
                                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[j], t1 - 1), new Point(iCoord[j], t1 + 15));

                        string strTemp1 = "";
                        if (dataGridView_rylon.Rows[i].Cells["Num_rylon"].Value != null)
                            strTemp1 = dataGridView_rylon.Rows[i].Cells["Num_rylon"].Value.ToString();//надо

                        if (Convert.ToInt32(strTemp1) > 9)
                            e.Graphics.DrawString(strTemp1, fnt8, Brushes.Black, iCoord[0] + 18, t1);
                        else
                            e.Graphics.DrawString(strTemp1, fnt8, Brushes.Black, iCoord[0] + 25, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Netto"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Netto"].Value.ToString(), fnt10, Brushes.Black, iCoord[1] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Brytto"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Brytto"].Value.ToString(), fnt10, Brushes.Black, iCoord[2] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Dlina"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Dlina"].Value.ToString(), fnt10, Brushes.Black, iCoord[3] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["CountEtik"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["CountEtik"].Value.ToString(), fnt10, Brushes.Black, iCoord[4] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Check_men"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Check_men"].Value.ToString(), fnt10, Brushes.Black, iCoord[5] + 30, t1);
                        if (dataGridView_rylon.Rows[i].Cells["Check_Time"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Check_Time"].Value.ToString(), fnt10, Brushes.Black, iCoord[6] + 5, t1);

                        if (dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor == Color.Red)
                            e.Graphics.DrawString("Не перев.", fnt10, Brushes.Black, iCoord[7] + 5, t1);
                        else if (dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor == Color.LightGreen)
                            e.Graphics.DrawString("Перев.", fnt10, Brushes.Black, iCoord[7] + 5, t1);
                        else if (dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor == Color.Brown)
                            e.Graphics.DrawString("Видален.", fnt10, Brushes.Black, iCoord[7] + 5, t1);


                        e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t1 + 15), new Point(iTableWidth, t1 + 15));

                        t1 += 15;
                    }
                    
                }

                if (m_iCountRows <= 130)
                {
                    e.Graphics.DrawString("-  2  -", fnt14Bold, Brushes.Black, 375, 1130);
                    e.HasMorePages = false;
                    m_iPrintPage = 0;
                }
                else
                {
                    e.Graphics.DrawString("-  2  -", fnt14Bold, Brushes.Black, 375, 1130);
                    e.HasMorePages = true;
                    m_iCounter = i;
                    m_iPrintPage = 2;

                }
            }
            else if (m_iPrintPage==2)
            {

                int t1 = t + 40;
                int i = 0;

                if (m_iCountRows > 130)
                {
                    int iCountPageRows = 0;

                    if (m_iCountRows > 202)
                        iCountPageRows = 202;
                    else iCountPageRows = m_iCountRows;

                    int iCountRows = iCountPageRows - m_iCounter;

                    e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t1), new Point(iTableWidth, t1));

                    for (i = m_iCounter; i < iCountPageRows; i++)
                    {
                        for (int j = 0; j <= 8; j++)
                            if (j == 0 || j == 8)
                                e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iCoord[j], t1 - 1), new Point(iCoord[j], t1 + 15));
                            else
                                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[j], t1 - 1), new Point(iCoord[j], t1 + 15));

                        string strTemp1 = "";
                        if (dataGridView_rylon.Rows[i].Cells["Num_rylon"].Value != null)
                            strTemp1 = dataGridView_rylon.Rows[i].Cells["Num_rylon"].Value.ToString();//надо

                        if (Convert.ToInt32(strTemp1) > 9)
                            e.Graphics.DrawString(strTemp1, fnt8, Brushes.Black, iCoord[0] + 18, t1);
                        else
                            e.Graphics.DrawString(strTemp1, fnt8, Brushes.Black, iCoord[0] + 25, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Netto"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Netto"].Value.ToString(), fnt10, Brushes.Black, iCoord[1] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Brytto"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Brytto"].Value.ToString(), fnt10, Brushes.Black, iCoord[2] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Dlina"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Dlina"].Value.ToString(), fnt10, Brushes.Black, iCoord[3] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["CountEtik"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["CountEtik"].Value.ToString(), fnt10, Brushes.Black, iCoord[4] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Check_men"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Check_men"].Value.ToString(), fnt10, Brushes.Black, iCoord[5] + 30, t1);
                        if (dataGridView_rylon.Rows[i].Cells["Check_Time"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Check_Time"].Value.ToString(), fnt10, Brushes.Black, iCoord[6] + 5, t1);

                        if (dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor == Color.Red)
                            e.Graphics.DrawString("Не перев.", fnt10, Brushes.Black, iCoord[7] + 5, t1);
                        else if (dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor == Color.LightGreen)
                            e.Graphics.DrawString("Перев.", fnt10, Brushes.Black, iCoord[7] + 5, t1);
                        else if (dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor == Color.Brown)
                            e.Graphics.DrawString("Видален.", fnt10, Brushes.Black, iCoord[7] + 5, t1);


                        e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t1 + 15), new Point(iTableWidth, t1 + 15));

                        t1 += 15;
                    }
                }

                if (m_iCountRows <= 202)
                {
                    e.Graphics.DrawString("-  3  -", fnt14Bold, Brushes.Black, 375, 1130);
                    e.HasMorePages = false;
                    m_iPrintPage = 0;
                }
                else
                {
                    e.Graphics.DrawString("-  3  -", fnt14Bold, Brushes.Black, 375, 1130);
                    e.HasMorePages = true;
                    m_iCounter = i;
                    m_iPrintPage = 3;

                }
               
            }
            else if (m_iPrintPage==3)
            {

                int t1 = t + 40;
                int i = 0;

                if (m_iCountRows > 202)
                {
                    int iCountPageRows = 0;

                    if (m_iCountRows > 274)
                        iCountPageRows = 274;
                    else iCountPageRows = m_iCountRows;

                    int iCountRows = iCountPageRows - m_iCounter;

                    e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t1), new Point(iTableWidth, t1));

                    for (i = m_iCounter; i < iCountPageRows; i++)
                    {
                        for (int j = 0; j <= 8; j++)
                            if (j == 0 || j == 8)
                                e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iCoord[j], t1 - 1), new Point(iCoord[j], t1 + 15));
                            else
                                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[j], t1 - 1), new Point(iCoord[j], t1 + 15));

                        string strTemp1 = "";
                        if (dataGridView_rylon.Rows[i].Cells["Num_rylon"].Value != null)
                            strTemp1 = dataGridView_rylon.Rows[i].Cells["Num_rylon"].Value.ToString();//надо

                        if (Convert.ToInt32(strTemp1) > 9)
                            e.Graphics.DrawString(strTemp1, fnt8, Brushes.Black, iCoord[0] + 18, t1);
                        else
                            e.Graphics.DrawString(strTemp1, fnt8, Brushes.Black, iCoord[0] + 25, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Netto"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Netto"].Value.ToString(), fnt10, Brushes.Black, iCoord[1] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Brytto"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Brytto"].Value.ToString(), fnt10, Brushes.Black, iCoord[2] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Dlina"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Dlina"].Value.ToString(), fnt10, Brushes.Black, iCoord[3] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["CountEtik"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["CountEtik"].Value.ToString(), fnt10, Brushes.Black, iCoord[4] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Check_men"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Check_men"].Value.ToString(), fnt10, Brushes.Black, iCoord[5] + 30, t1);
                        if (dataGridView_rylon.Rows[i].Cells["Check_Time"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Check_Time"].Value.ToString(), fnt10, Brushes.Black, iCoord[6] + 5, t1);

                        if (dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor == Color.Red)
                            e.Graphics.DrawString("Не перев.", fnt10, Brushes.Black, iCoord[7] + 5, t1);
                        else if (dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor == Color.LightGreen)
                            e.Graphics.DrawString("Перев.", fnt10, Brushes.Black, iCoord[7] + 5, t1);
                        else if (dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor == Color.Brown)
                            e.Graphics.DrawString("Видален.", fnt10, Brushes.Black, iCoord[7] + 5, t1);


                        e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t1 + 15), new Point(iTableWidth, t1 + 15));

                        t1 += 15;
                    }
                }

                if (m_iCountRows <= 274)
                {
                    e.Graphics.DrawString("-  4  -", fnt14Bold, Brushes.Black, 375, 1130);
                    e.HasMorePages = false;
                    m_iPrintPage = 0;
                }
                else
                {
                    e.Graphics.DrawString("-  4  -", fnt14Bold, Brushes.Black, 375, 1130);
                    e.HasMorePages = true;
                    m_iCounter = i;
                    m_iPrintPage = 4;
                }

            }
            else if(m_iPrintPage==4)
            {
                int t1 = t + 40;
                int i = 0;

                if (m_iCountRows > 274)
                {
                    int iCountPageRows = 0;

                    if (m_iCountRows > 346)
                        iCountPageRows = 346;
                    else iCountPageRows = m_iCountRows;

                    int iCountRows = iCountPageRows - m_iCounter;

                    e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t1), new Point(iTableWidth, t1));

                    for (i = m_iCounter; i < iCountPageRows; i++)
                    {
                        for (int j = 0; j <= 8; j++)
                            if (j == 0 || j == 8)
                                e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iCoord[j], t1 - 1), new Point(iCoord[j], t1 + 15));
                            else
                                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[j], t1 - 1), new Point(iCoord[j], t1 + 15));

                        string strTemp1 = "";
                        if (dataGridView_rylon.Rows[i].Cells["Num_rylon"].Value != null)
                            strTemp1 = dataGridView_rylon.Rows[i].Cells["Num_rylon"].Value.ToString();//надо

                        if (Convert.ToInt32(strTemp1) > 9)
                            e.Graphics.DrawString(strTemp1, fnt8, Brushes.Black, iCoord[0] + 18, t1);
                        else
                            e.Graphics.DrawString(strTemp1, fnt8, Brushes.Black, iCoord[0] + 25, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Netto"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Netto"].Value.ToString(), fnt10, Brushes.Black, iCoord[1] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Brytto"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Brytto"].Value.ToString(), fnt10, Brushes.Black, iCoord[2] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Dlina"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Dlina"].Value.ToString(), fnt10, Brushes.Black, iCoord[3] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["CountEtik"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["CountEtik"].Value.ToString(), fnt10, Brushes.Black, iCoord[4] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Check_men"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Check_men"].Value.ToString(), fnt10, Brushes.Black, iCoord[5] + 30, t1);
                        if (dataGridView_rylon.Rows[i].Cells["Check_Time"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Check_Time"].Value.ToString(), fnt10, Brushes.Black, iCoord[6] + 5, t1);

                        if (dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor == Color.Red)
                            e.Graphics.DrawString("Не перев.", fnt10, Brushes.Black, iCoord[7] + 5, t1);
                        else if (dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor == Color.LightGreen)
                            e.Graphics.DrawString("Перев.", fnt10, Brushes.Black, iCoord[7] + 5, t1);
                        else if (dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor == Color.Brown)
                            e.Graphics.DrawString("Видален.", fnt10, Brushes.Black, iCoord[7] + 5, t1);


                        e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t1 + 15), new Point(iTableWidth, t1 + 15));

                        t1 += 15;
                    }
                }

                if (m_iCountRows <= 346)
                {
                    e.Graphics.DrawString("-  5  -", fnt14Bold, Brushes.Black, 375, 1130);
                    e.HasMorePages = false;
                    m_iPrintPage = 0;
                }
                else
                {
                    e.Graphics.DrawString("-  5  -", fnt14Bold, Brushes.Black, 375, 1130);
                    e.HasMorePages = true;
                    m_iCounter = i;
                    m_iPrintPage = 5;
                }
            }
            else if (m_iPrintPage==5)
            {
                int t1 = t + 40;
                int i = 0;

                if (m_iCountRows > 346)
                {
                    int iCountPageRows = 0;

                    if (m_iCountRows > 418)
                        iCountPageRows = 418;
                    else iCountPageRows = m_iCountRows;

                    int iCountRows = iCountPageRows - m_iCounter;

                    e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t1), new Point(iTableWidth, t1));

                    for (i = m_iCounter; i < iCountPageRows; i++)
                    {
                        for (int j = 0; j <= 8; j++)
                            if (j == 0 || j == 8)
                                e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iCoord[j], t1 - 1), new Point(iCoord[j], t1 + 15));
                            else
                                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[j], t1 - 1), new Point(iCoord[j], t1 + 15));

                        string strTemp1 = "";
                        if (dataGridView_rylon.Rows[i].Cells["Num_rylon"].Value != null)
                            strTemp1 = dataGridView_rylon.Rows[i].Cells["Num_rylon"].Value.ToString();//надо

                        if (Convert.ToInt32(strTemp1) > 9)
                            e.Graphics.DrawString(strTemp1, fnt8, Brushes.Black, iCoord[0] + 18, t1);
                        else
                            e.Graphics.DrawString(strTemp1, fnt8, Brushes.Black, iCoord[0] + 25, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Netto"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Netto"].Value.ToString(), fnt10, Brushes.Black, iCoord[1] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Brytto"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Brytto"].Value.ToString(), fnt10, Brushes.Black, iCoord[2] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Dlina"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Dlina"].Value.ToString(), fnt10, Brushes.Black, iCoord[3] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["CountEtik"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["CountEtik"].Value.ToString(), fnt10, Brushes.Black, iCoord[4] + 5, t1);

                        if (dataGridView_rylon.Rows[i].Cells["Check_men"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Check_men"].Value.ToString(), fnt10, Brushes.Black, iCoord[5] + 30, t1);
                        if (dataGridView_rylon.Rows[i].Cells["Check_Time"].Value != null)
                            e.Graphics.DrawString(dataGridView_rylon.Rows[i].Cells["Check_Time"].Value.ToString(), fnt10, Brushes.Black, iCoord[6] + 5, t1);

                        if (dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor == Color.Red)
                            e.Graphics.DrawString("Не перев.", fnt10, Brushes.Black, iCoord[7] + 5, t1);
                        else if (dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor == Color.LightGreen)
                            e.Graphics.DrawString("Перев.", fnt10, Brushes.Black, iCoord[7] + 5, t1);
                        else if (dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor == Color.Brown)
                            e.Graphics.DrawString("Видален.", fnt10, Brushes.Black, iCoord[7] + 5, t1);


                        e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t1 + 15), new Point(iTableWidth, t1 + 15));

                        t1 += 15;
                    }
                }
                e.Graphics.DrawString("-  6  -", fnt14Bold, Brushes.Black, 375, 1130);
                e.HasMorePages = false;
                m_iPrintPage = 0;
                /*if (m_iCountRows <= 346)
                {
                    e.Graphics.DrawString("-  5  -", fnt14Bold, Brushes.Black, 375, 1130);
                    e.HasMorePages = false;
                    m_iPrintPage = 0;
                }
                else
                {
                    e.Graphics.DrawString("-  5  -", fnt14Bold, Brushes.Black, 375, 1130);
                    e.HasMorePages = true;
                    m_iCounter = i;
                    m_iPrintPage = 5;
                }*/

            }


            
        }

        private void button_all_label_Click(object sender, EventArgs e)
        {
            if (m_iTypeRegim == 2)
            {
               /* int iRowSelect = -1;
                for (int i = 0; i < dataGridView_rylon.Rows.Count; i++)
                    if (dataGridView_rylon.Rows[i].Selected == true)
                    {
                        iRowSelect = i;
                        break;
                    }

                if (iRowSelect != -1)
                {
                    if (dataGridView_rylon.Rows[iRowSelect].DefaultCellStyle.BackColor != Color.Brown)
                    {
                       // m_delChange = new delChange_form();
                        //m_delChange.ShowDialog();

                        string strKey = dataGridView_rylon.Rows[iRowSelect].Cells["Num_rylon"].Value.ToString() + " - " +
                                               dataGridView_rylon.Rows[iRowSelect].Cells["Brytto"].Value.ToString() + " - " +
                                               dataGridView_rylon.Rows[iRowSelect].Cells["Netto"].Value.ToString() + " - " +
                                               dataGridView_rylon.Rows[iRowSelect].Cells["Dlina"].Value.ToString() + " - " +
                                               dataGridView_rylon.Rows[iRowSelect].Cells["CountEtik"].Value.ToString();

                        int iRylonID = Convert.ToInt32(m_dicRylon[strKey]);

                        MessageBox.Show(iRylonID.ToString());

                    }
                }*/

                int iCounter = 0; 
                string strRylon="";

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

                    for (int i = 0; i < dataGridView_rylon.Rows.Count; i++)
                    {
                        if (dataGridView_rylon.Rows[i].DefaultCellStyle.BackColor != Color.Brown)
                        {
                            string strKey = dataGridView_rylon.Rows[i].Cells["Num_rylon"].Value.ToString() + " - " +
                                                   dataGridView_rylon.Rows[i].Cells["Brytto"].Value.ToString() + " - " +
                                                   dataGridView_rylon.Rows[i].Cells["Netto"].Value.ToString() + " - " +
                                                   dataGridView_rylon.Rows[i].Cells["Dlina"].Value.ToString() + " - " +
                                                   dataGridView_rylon.Rows[i].Cells["CountEtik"].Value.ToString();

                            int iRylonID = Convert.ToInt32(m_dicRylon[strKey]);

                            strMSSQLQuery = "select vh.brytto-vh.vagatary,convert(date,z.datezakaz), zk.zakazchik_name, pr.product_name, z.partiya, z.machine, z.smena, pm.product_material, pt.product_tols, " +
                                        "vh.width, vh.dlinarylona, vh.koletiketki, vh.num_rylon,  vh.brytto, vh.id " +
                                        "from itak_etiketka.dbo.itak_zakazchik zk, itak_etiketka.dbo.itak_product pr, itak_etiketka.dbo.itak_zakaz z, " +
                                        "itak_etiketka.dbo.itak_productmaterial pm, itak_etiketka.dbo.itak_producttols pt, itak_etiketka.dbo.itak_vihidrylon vh " +
                                        "where z.zakazchik_id=zk.id and z.productmaterial_id = pm.id and z.producttols_id=pt.id " +
                                        "and vh.zakaz_id=z.id and vh.product_id=pr.id and vh.id=" + iRylonID;
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

                                if (!m_print.SetPrinter(strPrinter, 60, 70, false, false, bLogo))
                                    MessageBox.Show("Ошибка печати етикетки");
                            }

                            m_MSSQLReader.Close();
                            
                       }

                    }
                    m_print = null;
                }
            }
        }




    }
}
