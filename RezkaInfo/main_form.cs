using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;
using System.IO;


namespace RezkaInfo
{
    struct Zakazchik
    {
        public int iZakazchikId;
        public string strZakazchikName;
        public int iProductId;
        public string strProductName;
        public string strPartiya;
        public string strManager;
    }

    struct ProductInfo
    {
        public int iZakazchikId;
        public int iProductId;
        public int iCountRylon;
        public double dNetto;
        public double dBrytto;
        public int iCountEtik;
        public int iDlinaRylona;
        public string strPartiya;
    }

    public struct stProductInfo
    {
        public int iProductID;
        public string strProductName;
        public double dNetto;
        public double dBrytto;
        public int iCountRylon;
        public int iCountEtiket;
        public int iWidth;
    };

    public struct stProductInfoSmall
    {
        public double dNetto;
        public double dBrytto;
        public int iCountRylon;
        public int iCountEtiket;
    }

    public struct stProductInfoObschee
    {
        public int iProductID;
        public string strProductName;
        
        public stProductInfoSmall stPrSmallRezka;
        public stProductInfoSmall stPrSmallPrinyatoRezka;
        public stProductInfoSmall stPrSmallOtgryjeno;
        public stProductInfoSmall stPrSmallOstatki;
    }

    public partial class main_form : Form
    {
        string strMSSQLConnectionString = "";
        SqlConnection m_MSSQLConnection;
        SqlCommand m_MSSQLCommand;
        SqlDataReader m_MSSQLReader;
        string strMSSQLQuery = "";
        bool m_bConnect = false;


        double m_dNetto = 0;
        double m_dBrytto = 0;
        double m_dSquare = 0;
        int m_iCountRylon = 0;
        int m_iKolEtiketki = 0;
        int m_iDlinaRylona = 0;
        string m_strMaterial = "";
        string m_strDlinaEtiketki = "";
        int m_iWidth = 0;

        int m_iZakazID = -1;
        int m_iProductID = -1;

        int m_iTypeRegim = -1;
        int m_iCheckMen = 1;

        bool flag = false;

        Dictionary<string, int> m_dicZakaz;
        Dictionary<string, int> m_dicProduct;

        Dictionary<string, string> m_dicDlinaEtiketki;
        Dictionary<string, string> m_dicMaterial;
        Dictionary<string, string> m_dicWidth;
        Dictionary<string, string> m_dicTols;
        Dictionary<string, string> m_dicManager;


        

        rylon_form m_rylon = null;
        password_form m_password = null;
        settings_form m_settings = null;
        selectUser_form m_selectUser = null;
        delChange_form m_delChangeForm = null;

        int m_iAddSkladCountRows = 0;
        int m_iAddSkladPrintPage = 0;
        int m_iAddSkladCounter = 0;
        int m_iAddSkladRecord = 0;
        int m_iAddSkladI = 0;
        int m_iAddSkladTop = 0;
        int m_iTypeFunc = -1;



        /////
        string m_strRezkaZakazchiName = "";
        string m_strRezkaPartiya = "";
        string m_strRezkaMaterial = "";
        string m_strRezkaTols = "";
        string m_strRezkaDateZakaz = "";
        string m_strRezkaDlinaEtiketki = "";
        string m_strRezkaManager = "";
        int m_iRezkaAllCountRylon = 0;
        int m_iRezkaAllCountEtik = 0;
        double m_dRezkaAllNetto = 0;
        double m_dRezkaAllBrytto = 0;
        //string m_strRezkaManager = "";

        Dictionary<string, int> m_dicRezkaProduct;
        Dictionary<string, stProductInfo> m_dicRezkaProductInfo;


        int m_iCountCheckedZakaz = 0;

        stProductInfoObschee prObscheeInfo ;
        stProductInfoSmall stRezka; 
        stProductInfoSmall stPrinyato; 
        stProductInfoSmall stOtgryjeno;
        stProductInfoSmall stOstatki;



        ////////////////////////////////////sklad

        DataGridViewTextBoxColumn[] cColumns;


        Dictionary<string, int> m_dicZakazchikID;
        Dictionary<int, string> m_dicProductID;
        Dictionary<int, string> m_dicZakazID;

        Dictionary<string, int> m_dicSkladManager;

        List<Zakazchik> m_lsZakazchik;
        List<ProductInfo> m_lsProductInfo;

        List<Zakazchik> m_lsAddSkladZakazchik;
        List<ProductInfo> m_lsAddSkladProductInfo;

        List<Zakazchik> m_lsOtgryzSkladZakazchik;
        List<ProductInfo> m_lsOtgryzSkladProductInfo;

        List<Zakazchik> m_lsOstatkiSkladZakazchik;
        List<ProductInfo> m_lsOstatkiSkladProductInfo;

        Zakazchik zk;
        ProductInfo pr;

        int iCountNaSklade = 0;
        int iCountOtgryz = 0;

        int m_iColumnType = -1;

        ////////////////////////////////////



        /////////////Obschee
        Dictionary<string, int> m_dicObscheeZakaz;
        Dictionary<string, int> m_dicObscheeProduct;

        Dictionary<string, stProductInfoObschee> m_dicObscheeProductInfo;
        
        string m_strObscheeZakazchik = "";
        string m_strObscheePartiya = "";
        string m_strObscheeMaterial = "";
        string m_strObscheeTols = "";
        string m_strObscheeDlinaEtiketki = "";
        string m_strObscheeManager = "";
        string m_strObscheeDataPorezki = "";

        int m_iAllCountRylonRezka = 0;
        int m_iAllCountRylonPrinyato = 0;
        int m_iAllCountRylonOtgryjeno = 0;
        int m_iAllCountRylonOstatki = 0;

        double m_dAllNettoRezka = 0;
        double m_dAllNettoPrinyato = 0;
        double m_dAllNettoOtgryjeno = 0;
        double m_dAllNettoOstatki = 0;

        double m_dAllBryttoRezka = 0;
        double m_dAllBryttoPrinyato = 0;
        double m_dAllBryttoOtgryjeno = 0;
        double m_dAllBryttoOstatki = 0;

        bool m_bObscheeSelectZakaz = false;
        ///////
        

        public main_form()
        {
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

        private void Form1_Load(object sender, EventArgs e)
        {
            //strMSSQLConnectionString = "Database=itak_etiketka;Data Source=127.0.0.1;User Id=ivan;Password=coolworld";
            strMSSQLConnectionString = "Database=itak_etiketka;Data Source=192.168.0.14;User Id=Ivan;Password=coolworld";
            m_MSSQLConnection = new SqlConnection(strMSSQLConnectionString);

            m_dicZakaz = new Dictionary<string, int>();
            m_dicProduct = new Dictionary<string, int>();
            m_dicDlinaEtiketki = new Dictionary<string, string>();
            m_dicMaterial = new Dictionary<string,string>();
            m_dicManager = new Dictionary<string, string>();
            m_dicWidth = new Dictionary<string, string>();
            m_dicTols = new Dictionary<string, string>();

            m_dicRezkaProduct = new Dictionary<string, int>();
            m_dicRezkaProductInfo = new Dictionary<string, stProductInfo>();


            m_dicObscheeZakaz = new Dictionary<string,int>();
            m_dicObscheeProduct = new Dictionary<string, int>();
            m_dicObscheeProductInfo = new Dictionary<string, stProductInfoObschee>();

            //m_dicAddSkladProduct = new Dictionary<string, int>();

            //tabControl1.Controls.Remove(tabPage2);
            try
            {
                m_MSSQLConnection.Open();
                toolStripStatusLabel1.Text = "Подключение успешно к MSSQL server";
                m_MSSQLCommand = m_MSSQLConnection.CreateCommand();
                m_bConnect = true;
            }
            catch (System.Exception)
            {
                toolStripStatusLabel1.Text = "Ошибка подключения к MSSQL server";
                m_bConnect = false;            	
            }


            if (m_bConnect)
            {

                //string strDate = monthCalendar1.SelectionRange.Start.ToString();
                //strDate = strDate.Remove(strDate.IndexOf(' '));
                string strDate = monthCalendar1.SelectionRange.Start.Year.ToString() + "." + monthCalendar1.SelectionRange.Start.Month.ToString() + "." + monthCalendar1.SelectionRange.Start.Day.ToString();
                SelectZakaz(strDate, "");

                for (int i = 0; i < dataGridView_rezkaZakaz.Rows.Count; i++)
                    if (dataGridView_rezkaZakaz.Rows[i].Selected == true)
                        dataGridView_rezkaZakaz.Rows[0].Selected = false;

                RegistryKey readKey = Registry.LocalMachine.OpenSubKey("software\\rezka_info");
                if (readKey != null)
                {
                    string loadString = (string)readKey.GetValue("Type_app");
                    readKey.Close();
                    m_iTypeRegim = Convert.ToInt32(loadString);
                }
                else
                    OnSettings();

                SetTypeRegim();
                flag = true;

                m_dicZakazchikID = new Dictionary<string, int>();
                m_dicProductID = new Dictionary<int, string>();
                m_dicZakazID = new Dictionary<int, string>();
                m_dicSkladManager = new Dictionary<string, int>();

                zk = new Zakazchik();
                pr = new ProductInfo();
                m_lsZakazchik = new List<Zakazchik>();
                m_lsProductInfo = new List<ProductInfo>();

                m_lsAddSkladZakazchik = new List<Zakazchik>();
                m_lsAddSkladProductInfo = new List<ProductInfo>();

                m_lsOtgryzSkladZakazchik = new List<Zakazchik>();
                m_lsOtgryzSkladProductInfo = new List<ProductInfo>();

                m_lsOstatkiSkladZakazchik = new List<Zakazchik>();
                m_lsOstatkiSkladProductInfo = new List<ProductInfo>();

                comboBoxSkladOtgryz_Zakazchik.Items.Clear();
                comboBoxSkladAdd_Zakazchik.Items.Clear();

                comboBoxSkladAdd_Manager.Items.Clear();
                comboBoxSkladOtgryz_Manager.Items.Clear();
                comboBoxSkladOstatki_Manager.Items.Clear();


                strMSSQLQuery = "select id, zakazchik_name from itak_etiketka.dbo.itak_zakazchik order by zakazchik_name asc";
                m_MSSQLCommand.CommandText = strMSSQLQuery;
                m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
                
                while (m_MSSQLReader.Read())
                {
                    m_dicZakazchikID.Add(m_MSSQLReader["zakazchik_name"].ToString().Trim(), Convert.ToInt32(m_MSSQLReader["id"]));
                    comboBoxSkladAdd_Zakazchik.Items.Add(m_MSSQLReader["zakazchik_name"].ToString().Trim());
                    comboBoxSkladOtgryz_Zakazchik.Items.Add(m_MSSQLReader["zakazchik_name"].ToString().Trim());
                    comboBoxSkladOstatki_Zakazchik.Items.Add(m_MSSQLReader["zakazchik_name"].ToString().Trim());
                }
                m_MSSQLReader.Close();


                strMSSQLQuery = "select id, manager_name from itak_etiketka.dbo.itak_manager order by manager_name asc";
                m_MSSQLCommand.CommandText = strMSSQLQuery;
                m_MSSQLReader = m_MSSQLCommand.ExecuteReader();

                while (m_MSSQLReader.Read())
                {
                    m_dicSkladManager.Add(m_MSSQLReader["manager_name"].ToString().Trim(), Convert.ToInt32(m_MSSQLReader["id"]));
                    comboBoxSkladAdd_Manager.Items.Add(m_MSSQLReader["manager_name"].ToString().Trim());
                    comboBoxSkladOtgryz_Manager.Items.Add(m_MSSQLReader["manager_name"].ToString().Trim());
                    comboBoxSkladOstatki_Manager.Items.Add(m_MSSQLReader["manager_name"].ToString().Trim());
                }
                m_MSSQLReader.Close();
                               

                CreateColumnNeOtgryj(ref dataGridViewSklad_AddProduct);
                

                string strDateStart = dateTimePickerAddSklad_Start.Value.Year + "-" + dateTimePickerAddSklad_Start.Value.Month + "-" + dateTimePickerAddSklad_Start.Value.Day;
                string strDateEnd = dateTimePickerAddSklad_End.Value.Year + "-" + dateTimePickerAddSklad_End.Value.Month + "-" + (dateTimePickerAddSklad_End.Value.Day);
                //SelectSearchProduct(1, "", "", 0, strDateStart, strDateEnd);

                SelectAddSkladProduct("", "", strDateStart, strDateEnd, ref dataGridViewSklad_AddProduct, 0, ref m_lsAddSkladZakazchik, ref m_lsAddSkladProductInfo,"");


                CreateDataGridOdscheeProduct();
                ChangeSize();
                
                
                //tabPage2.Enabled = false;
                //tabPage3.Enabled = false;
            }
        }


        private void CreateColumnNeOtgryj(ref DataGridView data)
        {
            data.Columns.Clear();
            cColumns = new DataGridViewTextBoxColumn[9];
            for (int i = 0; i < 9; i++)
                cColumns[i] = null;

            for (int i = 0; i < 9; i++)
                cColumns[i] = new DataGridViewTextBoxColumn();

            cColumns[0].HeaderText = "№";
            cColumns[0].Name = "Number";
            //cColumns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; 

            cColumns[1].HeaderText = "Заказчик";
            cColumns[1].Name = "Zakazchik";
            //cColumns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.NotSet; 

            cColumns[2].HeaderText = "Продукция";
            cColumns[2].Name = "Product";
            //cColumns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.NotSet; 
          //  cColumns[2].Visible = false;

            /* cColumns[3].HeaderText = "Дата поступления";
             cColumns[3].Name = "DateCome";

             cColumns[4].HeaderText = "Дата отгрузки";
             cColumns[4].Name = "DateOut";*/
            cColumns[3].HeaderText = "Партия";
            cColumns[3].Name = "Partiya";

            cColumns[4].HeaderText = "К-во рулонов";
            cColumns[4].Name = "CountRylon";

            cColumns[5].HeaderText = "Нетто, кг";
            cColumns[5].Name = "Netto";

            cColumns[6].HeaderText = "Брутто, кг";
            cColumns[6].Name = "Brytto";

            cColumns[7].HeaderText = "К-во этикетки";
            cColumns[7].Name = "CountEtiketki";
            
            cColumns[8].HeaderText = "Менеджер";
            cColumns[8].Name = "SkladAddManager";

            ChangeColumnSize(ref data);

            for (int i = 0; i < 9; i++)
                data.Columns.Add(cColumns[i]);

            m_iColumnType = 1;

        }

        private void ChangeColumnSize(ref DataGridView data)
        {
            int iWidth = data.ClientSize.Width;
            //int iWidth = data.Size.Width;
            iWidth -= 40;
            for (int i = 0; i < 9; i++)
            {
                if (cColumns[2].Visible == true)
                {
                    if (i == 0) cColumns[i].Width = 40;
                    else if (i >= 1 && i < 3) cColumns[i].Width = ((iWidth - (iWidth / 2)) / 2) - 13;
                    else if (i >= 3) cColumns[i].Width = (iWidth - (iWidth / 2)) / 6;
                }
               /* else if (cColumns[2].Visible == false)
                {
                    if (i == 0) cColumns[i].Width = 40;
                    else if (i == 1) cColumns[i].Width = ((iWidth - (iWidth / 2)));
                    else if (i >= 2) cColumns[i].Width = ((iWidth - (iWidth / 2)) / 4) - 8;
                }*/
            }

           
        }

        public int maxDays(int year, int month)
        {
            if (month == 2)
                return (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0)) ? 29 : 28;
            else
                return (month == 4 || month == 6 || month == 9 || month == 11) ? 30 : 31;
        }

        //sklad

        private void SelectAddSkladProduct(string strZakazchik, string strPartiya, string strDateStart, 
                                            string strDateEnd, ref DataGridView dataGrid, int iTypeFunc,
                                            ref List<Zakazchik> lsZakazchik, ref List<ProductInfo> lsProductInfo, string strManager) //0 - Add 1 - Otgryz  2-ostatki
        {
            if (CheckConnect())
            {
                //m_dicAddSkladProduct.Clear();
                dataGrid.Rows.Clear();

                string strZakazchikID = "";
                string strYear = "";
                string strManagerID = " ";


                
                if (strZakazchik.Length != 0)
                {
                    strZakazchikID = " and pr.id_zakazchik=" + m_dicZakazchikID[strZakazchik] + " ";
                    cColumns[2].Visible = true;
                    ChangeColumnSize(ref dataGrid);
                }
                else
                {
                    strZakazchikID = " ";
                    //cColumns[2].Visible = false;
                    ChangeColumnSize(ref dataGrid);
                }

                if (strManager.Length != 0)
                {
                    strManagerID = " and m.id="+m_dicSkladManager[strManager];
                }

                if (strPartiya.Length != 0)
                {
                    strPartiya = " and z.partiya like '%" + strPartiya + "%'";
                    cColumns[2].Visible = true;
                    ChangeColumnSize(ref dataGrid);
                }
                else
                {
                    strPartiya = "";
                    //cColumns[2].Visible = false;
                    ChangeColumnSize(ref dataGrid);
                }

                if (iTypeFunc == 0)
                {
                    if (strPartiya.Length != 0 && checkBoxAddSklad_EnableDate.Checked == true)
                        strYear = " and (convert(date,date_postyp,101) >= '" + strDateStart + "' and convert(date,date_postyp,101)<='" + strDateEnd + "') ";
                    else if (strPartiya.Length != 0 && checkBoxAddSklad_EnableDate.Checked == false)
                        strYear = " ";
                    else if (strPartiya.Length == 0 && checkBoxAddSklad_EnableDate.Checked == false)
                    {
                        strDateStart = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                        strDateEnd = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                        strYear = " and (convert(date,date_postyp,101) >= '" + strDateStart + "' and convert(date,date_postyp,101)<='" + strDateEnd + "') ";
                    }
                    else
                        strYear = " and (convert(date,date_postyp,101) >= '" + strDateStart + "' and convert(date,date_postyp,101)<='" + strDateEnd + "') ";


                    strMSSQLQuery = "select distinct zk.id,zk.zakazchik_name, pr.id, pr.product_name, z.partiya,  m.manager_name  " +
                                  " from itak_etiketka.dbo.itak_sklad s, itak_etiketka.dbo.itak_zakaz z, itak_etiketka.dbo.itak_vihidrylon vh, itak_etiketka.dbo.itak_product pr, itak_etiketka.dbo.itak_zakazchik zk, itak_etiketka.dbo.itak_manager m " +
                                  " where z.zakazchik_id=zk.id and m.id=z.manager_id and z.id=vh.zakaz_id and vh.product_id=pr.id and z.id=s.zakaz_id and vh.id=s.rylon_id " + strYear + strZakazchikID + strPartiya + strManagerID + " order by zk.zakazchik_name asc";
                }
                else if (iTypeFunc==1)
                {
                    if (strPartiya.Length != 0 && checkBoxOtgryzSklad_EnableDate.Checked == true)
                        strYear = " and (convert(date,date_otgryz,101) >= '" + strDateStart + "' and convert(date,date_otgryz,101)<='" + strDateEnd + "') ";
                    else if (strPartiya.Length != 0 && checkBoxOtgryzSklad_EnableDate.Checked == false)
                        strYear = " ";
                    else if (strPartiya.Length == 0 && checkBoxOtgryzSklad_EnableDate.Checked == false)
                    {
                        strDateStart = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                        strDateEnd = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                        strYear = " and (convert(date,date_otgryz,101) >= '" + strDateStart + "' and convert(date,date_otgryz,101)<='" + strDateEnd + "') ";
                    }
                    else
                        strYear = " and (convert(date,date_otgryz,101) >= '" + strDateStart + "' and convert(date,date_otgryz,101)<='" + strDateEnd + "') ";

                    strMSSQLQuery = "select distinct zk.id,zk.zakazchik_name, pr.id, pr.product_name, z.partiya, m.manager_name  " +
                                  " from itak_etiketka.dbo.itak_sklad s, itak_etiketka.dbo.itak_zakaz z, itak_etiketka.dbo.itak_vihidrylon vh, itak_etiketka.dbo.itak_product pr, itak_etiketka.dbo.itak_zakazchik zk, itak_etiketka.dbo.itak_manager m " +
                                  " where z.zakazchik_id=zk.id and m.id=z.manager_id and z.id=vh.zakaz_id and vh.product_id=pr.id and z.id=s.zakaz_id and vh.id=s.rylon_id and s.rylon_state=2 " + strYear + strZakazchikID + strPartiya + strManagerID+" order by zk.zakazchik_name asc";
                }
                else if (iTypeFunc==2)
                {
                    strMSSQLQuery = "select distinct zk.id,zk.zakazchik_name, pr.id, pr.product_name, z.partiya,  m.manager_name  " +
                                  " from itak_etiketka.dbo.itak_sklad s, itak_etiketka.dbo.itak_zakaz z, itak_etiketka.dbo.itak_vihidrylon vh, itak_etiketka.dbo.itak_product pr, itak_etiketka.dbo.itak_zakazchik zk, itak_etiketka.dbo.itak_manager m " +
                                  " where z.zakazchik_id=zk.id and m.id=z.manager_id and z.id=vh.zakaz_id and vh.product_id=pr.id and z.id=s.zakaz_id and vh.id=s.rylon_id and s.rylon_state=1 " + strZakazchikID + strPartiya +strManagerID+ " order by zk.zakazchik_name asc";

                }


                try
                {
                    lsZakazchik.Clear();

                    m_MSSQLCommand.CommandText = strMSSQLQuery;
                    m_MSSQLReader = m_MSSQLCommand.ExecuteReader();

                    while (m_MSSQLReader.Read())
                    {
                        zk.iZakazchikId = Convert.ToInt32(m_MSSQLReader[0]);
                        zk.strZakazchikName = m_MSSQLReader[1].ToString().Trim();
                        zk.iProductId = Convert.ToInt32(m_MSSQLReader[2]);
                        zk.strProductName = m_MSSQLReader[3].ToString().Trim();
                        zk.strPartiya = m_MSSQLReader[4].ToString().Trim();
                        zk.strManager = m_MSSQLReader[5].ToString().Trim();
                        
                        lsZakazchik.Add(zk);

                    }
                    m_MSSQLReader.Close();
                }
                catch (System.Exception ex)
                {
                    WriteLog("Ошибка выборки елементов с базы полученых на склад", ex);
                    m_MSSQLReader.Close();
                }

       
                lsProductInfo.Clear();
                int iProgres = 0;
                if (lsZakazchik.Count!=0)
                    iProgres = (100/lsZakazchik.Count);
                

                toolStripProgressBar1.Value = 0;
                for (int i = 0; i < lsZakazchik.Count; i++)
                {
                    /*strMSSQLQuery = "select COUNT(vh.id) , ROUND(sum(vh.brytto-vh.vagatary),3), ROUND(sum(vh.brytto),3), SUM(vh.koletiketki),SUM(vh.dlinarylona) " +
                                    "from itak_etiketka.dbo.itak_vihidrylon vh, itak_etiketka.dbo.itak_zakaz z, itak_etiketka.dbo.itak_sklad s " +
                                    "where s.rylon_id=vh.id and vh.zakaz_id=z.id and vh.product_id=" + lsZakazchik[i].iProductId + " and z.partiya='" + lsZakazchik[i].strPartiya + "' " + strYear;*/

                    if (iTypeFunc==2)
                        strMSSQLQuery = "select COUNT(vh.id) , ROUND(sum(vh.brytto-vh.vagatary),3), ROUND(sum(vh.brytto),3), SUM(vh.koletiketki),SUM(vh.dlinarylona) " +
                                    "from itak_etiketka.dbo.itak_vihidrylon vh, itak_etiketka.dbo.itak_sklad s, itak_etiketka.dbo.itak_zakaz z  " +
                                    "where s.rylon_id=vh.id and s.rylon_state=1 and vh.zakaz_id=z.id and vh.product_id=" + lsZakazchik[i].iProductId + " and z.partiya='" + lsZakazchik[i].strPartiya + "' " + strYear;
                    else if (iTypeFunc==1)
                        strMSSQLQuery = "select COUNT(vh.id) , ROUND(sum(vh.brytto-vh.vagatary),3), ROUND(sum(vh.brytto),3), SUM(vh.koletiketki),SUM(vh.dlinarylona) " +
                                    "from itak_etiketka.dbo.itak_vihidrylon vh, itak_etiketka.dbo.itak_sklad s, itak_etiketka.dbo.itak_zakaz z " +
                                    "where s.rylon_id=vh.id and s.rylon_state=2 and vh.zakaz_id=z.id and vh.product_id=" + lsZakazchik[i].iProductId + " and z.partiya='" + lsZakazchik[i].strPartiya+ "' " + strYear;
                    else
                        strMSSQLQuery = "select COUNT(vh.id) , ROUND(sum(vh.brytto-vh.vagatary),3), ROUND(sum(vh.brytto),3), SUM(vh.koletiketki),SUM(vh.dlinarylona) " +
                                    "from itak_etiketka.dbo.itak_vihidrylon vh, itak_etiketka.dbo.itak_sklad s, itak_etiketka.dbo.itak_zakaz z " +
                                     "where s.rylon_id=vh.id and vh.zakaz_id=z.id  and vh.product_id=" + lsZakazchik[i].iProductId + " and z.partiya='" + lsZakazchik[i].strPartiya + "' " + strYear;

                    try
                    {
                        m_MSSQLCommand.CommandText = strMSSQLQuery;
                        m_MSSQLReader = m_MSSQLCommand.ExecuteReader();

                        m_MSSQLReader.Read();

                        pr.iZakazchikId = lsZakazchik[i].iZakazchikId;
                        pr.iProductId = lsZakazchik[i].iProductId;

                        if (m_MSSQLReader[0] != DBNull.Value)
                            pr.iCountRylon = Convert.ToInt32(m_MSSQLReader[0]);

                        if (m_MSSQLReader[1] != DBNull.Value)
                            pr.dNetto = Convert.ToDouble(m_MSSQLReader[1]);

                        if (m_MSSQLReader[2] != DBNull.Value)
                            pr.dBrytto = Convert.ToDouble(m_MSSQLReader[2]);

                        if (m_MSSQLReader[3] != DBNull.Value)
                            pr.iCountEtik = Convert.ToInt32(m_MSSQLReader[3]);

                        if (m_MSSQLReader[4] != DBNull.Value)
                            pr.iDlinaRylona = Convert.ToInt32(m_MSSQLReader[4]);

                        pr.strPartiya = lsZakazchik[i].strPartiya;
                        lsProductInfo.Add(pr);

                        m_MSSQLReader.Close();
                    }
                    catch (System.Exception ex)
                    {
                        WriteLog("Ошибка выборки елементов с базы продукции по заказчику", ex);
                        //MessageBox.Show(ex.Message);
                        m_MSSQLReader.Close();
                    }
                    
                }

                int iCountRylon = 0;
                double dNetto = 0;
                double dBrytto = 0;
                int iCountEtiketki = 0;
                int iDlinaRylona = 0;

                int iNumber = 0;
                
                for (int i = 0; i < lsZakazchik.Count; i++)
                {
                    dataGrid.Rows.Add((iNumber + 1).ToString());
                    dataGrid.Rows[iNumber].Cells["Zakazchik"].Value = lsZakazchik[i].strZakazchikName;
                    dataGrid.Rows[i].Cells["Product"].Value = lsZakazchik[i].strProductName;
                    dataGrid.Rows[i].Cells["Partiya"].Value = lsZakazchik[i].strPartiya;

                    dataGrid.Rows[iNumber].Cells["CountRylon"].Value = lsProductInfo[i].iCountRylon;
                    dataGrid.Rows[iNumber].Cells["CountEtiketki"].Value = lsProductInfo[i].iCountEtik;
                    dataGrid.Rows[iNumber].Cells["Netto"].Value = lsProductInfo[i].dNetto; ;
                    dataGrid.Rows[iNumber].Cells["Brytto"].Value = lsProductInfo[i].dBrytto;

                    dataGrid.Rows[i].Cells["SkladAddManager"].Value = lsZakazchik[i].strManager;

                    if (i % 2 == 0)
                        dataGrid.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                    else dataGrid.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;
                    iNumber++;
                    
                    toolStripProgressBar1.Value += iProgres;
                }

                for (int j = 0; j < lsProductInfo.Count; j++)
                {
                    iCountRylon += lsProductInfo[j].iCountRylon;
                    iCountEtiketki += lsProductInfo[j].iCountEtik;
                    dBrytto += lsProductInfo[j].dBrytto;
                    dNetto += lsProductInfo[j].dNetto;
                    iDlinaRylona += lsProductInfo[j].iDlinaRylona;
                }

                if (iTypeFunc == 0)
                {
                    labelAddSklad_ZnachBrytto.Text = dBrytto.ToString("0.00") + " кг";
                    labelAddSklad_ZnachNetto.Text = dNetto.ToString("0.00") + " кг";
                    labelAddSklad_ZnachCountRylon.Text = iCountRylon.ToString() + " шт";
                    labelAddSklad_ZnachCountEtik.Text = iCountEtiketki.ToString() + " шт";
                    labelAddSklad_ZnachMP.Text = iDlinaRylona.ToString() + " м.п.";
                }
                else if (iTypeFunc == 1)
                {
                    labelOtgryzSklad_ZnachBrytto.Text = dBrytto.ToString("0.00") + " кг";
                    labelOtgryzSklad_ZnachNetto.Text = dNetto.ToString("0.00") + " кг";
                    labelOtgryzSklad_ZnachCountRylon.Text = iCountRylon.ToString() + " шт";
                    labelOtgryzSklad_ZnachCountEtik.Text = iCountEtiketki.ToString() + " шт";
                    labelOtgryzSklad_ZnachMP.Text = iDlinaRylona.ToString() + " м.п.";
                }
                else if (iTypeFunc==2)
                {
                    labelOstatkiSklad_ZnachBrytto.Text = dBrytto.ToString("0.00") + " кг";
                    labelOstatkiSklad_ZnachNetto.Text = dNetto.ToString("0.00") + " кг";
                    labelOstatkiSklad_ZnachCountRylon.Text = iCountRylon.ToString() + " шт";
                    labelOstatkiSklad_ZnachCountEtik.Text = iCountEtiketki.ToString() + " шт";
                    labelOstatkiSklad_ZnachMP.Text = iDlinaRylona.ToString() + " м.п.";
                }
            }
           // toolStripProgressBar1.Value = 0;
        }

        private void SelectSearchProduct(int iRegim, string strPartiya, string strZakazchik, int iCheckBoxSatate, string strDateStart, string strDateEnd)//CheckBoxState  0-vse 1-nasklade 3-otgryjen
        {
            //if (CheckConnect())
            {
                int i = 0;
                iCountNaSklade = iCountOtgryz = 0;
                dataGridViewSklad_AddProduct.Rows.Clear();

                string strCheck_rylon = "";
                string strZakazchikID = "";
                string strYear = "";

                double dBryttoNaSklade = 0;
                double dNettoNaSklade = 0;
                int iCountRylonNaSklade = 0;

                double dBryttoOtgryz = 0;
                double dNettoOtgryz = 0;
                int iCountRylonOtgryz = 0;

                if (iCheckBoxSatate == 1)//na sklade
                    strCheck_rylon = " and s.rylon_state=1 ";
                else if (iCheckBoxSatate == 2)//otgryjen
                    strCheck_rylon = " and s.rylon_state=2 ";
                else if (iCheckBoxSatate == 0)//vse
                    strCheck_rylon = " ";

                if (strZakazchik.Length != 0)
                {
                    strZakazchikID = " and pr.id_zakazchik=" + m_dicZakazchikID[strZakazchik] + " ";
                    cColumns[2].Visible = true;
                   // ChangeColumnSize();
                }
                else
                {
                    strZakazchikID = " ";
                    //cColumns[2].Visible = false;
                   // ChangeColumnSize();
                }

                if (strPartiya.Length != 0)
                {
                    strPartiya = " and z.partiya='" + strPartiya + "'";
                    cColumns[2].Visible = true;
                    //ChangeColumnSize();
                }
                else
                {
                    strPartiya = "";
                    //cColumns[2].Visible = false;
                    //ChangeColumnSize();
                }

                //if (checkBoxEnableDate.Checked == true)
               
                if (textBoxSkladAdd_NumberTZ.Text.Length==0)
                {
                    
                    if (iCheckBoxSatate == 1)
                        strYear = " and convert(date,date_postyp,101) >= '" + strDateStart + "' and convert(date,date_postyp,101)<='" + strDateEnd + "'";
                    else if (iCheckBoxSatate == 2)
                        strYear = " and convert(date,date_otgryz,101) >= '" + strDateStart + "' and convert(date,date_otgryz,101)<='" + strDateEnd + "'";
                    else if (iCheckBoxSatate == 0)
                        strYear = " and (convert(date,date_postyp,101) >= '" + strDateStart + "' and convert(date,date_postyp,101)<='" + strDateEnd + "'" + " or " + " convert(date,date_otgryz,101) >= '" + strDateStart + "' and convert(date,date_otgryz,101)<='" + strDateEnd + "')";
                }

                                
                //strYear = " and (convert(date,date_postyp,101) >= '" + strDateStart + "' and convert(date,date_postyp,101)<='" + strDateEnd + "'" + " or " + " convert(date,date_otgryz,101) >= '" + strDateStart + "' and convert(date,date_otgryz,101)<='" + strDateEnd + "')";
                

               // int iState = 0;
                if (iRegim == 1)
                {
//                     strMSSQLQuery = "select  distinct zk.id, zk.zakazchik_name, pr.id, pr.product_name " +
//                                     "from itak_etiketka.dbo.itak_zakazchik zk, itak_etiketka.dbo.itak_vihidrylon vh, itak_etiketka.dbo.itak_product pr, itak_etiketka.dbo.itak_zakaz z " +
//                                     "where zk.id=pr.id_zakazchik and vh.product_id=pr.id and vh.zakaz_id=z.id " + strCheck_rylon + strYear + strZakazchikID + strPartiya + " order by zk.zakazchik_name asc";

                   /* strMSSQLQuery = "select distinct zk.id,zk.zakazchik_name, pr.id, pr.product_name  " +
                                  " from itak_etiketka.dbo.itak_sklad s, itak_etiketka.dbo.itak_zakaz z, itak_etiketka.dbo.itak_vihidrylon vh, itak_etiketka.dbo.itak_product pr, itak_etiketka.dbo.itak_zakazchik zk " +
                                  " where z.id=s.zakaz_id and vh.id=s.rylon_id and pr.id=vh.product_id and pr.id_zakazchik=zk.id " + strCheck_rylon+ strYear + strZakazchikID + strPartiya + " order by zk.zakazchik_name asc";*/

                    strMSSQLQuery = "select distinct zk.id,zk.zakazchik_name, pr.id, pr.product_name, z.partiya  " +
                                  " from itak_etiketka.dbo.itak_sklad s, itak_etiketka.dbo.itak_zakaz z, itak_etiketka.dbo.itak_vihidrylon vh, itak_etiketka.dbo.itak_product pr, itak_etiketka.dbo.itak_zakazchik zk " +
                                  " where z.zakazchik_id=zk.id and z.id=vh.zakaz_id and vh.product_id=pr.id and z.id=s.zakaz_id and vh.id=s.rylon_id " + strCheck_rylon + strYear + strZakazchikID + strPartiya + " order by zk.zakazchik_name asc";

                    try
                    {
                        m_lsZakazchik.Clear();

                        m_MSSQLCommand.CommandText = strMSSQLQuery;
                        m_MSSQLReader = m_MSSQLCommand.ExecuteReader();

                        while (m_MSSQLReader.Read())
                        {
                            zk.iZakazchikId = Convert.ToInt32(m_MSSQLReader[0]);
                            zk.strZakazchikName = m_MSSQLReader[1].ToString().Trim();
                            zk.iProductId = Convert.ToInt32(m_MSSQLReader[2]);
                            zk.strProductName = m_MSSQLReader[3].ToString().Trim();
                            zk.strPartiya = m_MSSQLReader[4].ToString().Trim();
                            /*
                                iState = Convert.ToInt32(m_MSSQLReader[4]);
                                                            if (iState == 1)
                                                                iCountNaSklade++;
                                                            else if (iState == 2)
                                                                iCountOtgryz++;*/

                            m_lsZakazchik.Add(zk);
                        }
                        m_MSSQLReader.Close();
                    }
                    catch (System.Exception)
                    {
                        //WriteLog("Ошибка выборки елементов с базы", ex);
                        m_MSSQLReader.Close();
                    }

                    //   toolStripStatusLabel_mainForm.Text = "Выбор элементов";

                    m_lsProductInfo.Clear();
                   // toolStripProgressBar_LOAD.Maximum = m_lsZakazchik.Count;

                    for (i = 0; i < m_lsZakazchik.Count; i++)
                    {
                        strMSSQLQuery = "select COUNT(vh.id) , ROUND(sum(vh.brytto-vh.vagatary),3), ROUND(sum(vh.brytto),3), SUM(vh.koletiketki),SUM(vh.dlinarylona) " +
                                    "from itak_etiketka.dbo.itak_vihidrylon vh, itak_etiketka.dbo.itak_zakaz z, itak_etiketka.dbo.itak_sklad s "+
                                    "where s.rylon_id=vh.id and vh.zakaz_id=z.id and vh.product_id="+m_lsZakazchik[i].iProductId+" and z.partiya='"+m_lsZakazchik[i].strPartiya+"'"+strCheck_rylon+" "+strYear; 
                                    
                        
                        try
                        {
                            m_MSSQLCommand.CommandText = strMSSQLQuery;
                            m_MSSQLReader = m_MSSQLCommand.ExecuteReader();

                            m_MSSQLReader.Read();

                            pr.iZakazchikId = m_lsZakazchik[i].iZakazchikId;
                            pr.iProductId = m_lsZakazchik[i].iProductId;
                            if (m_MSSQLReader[0]!=DBNull.Value)
                                pr.iCountRylon = Convert.ToInt32(m_MSSQLReader[0]);

                            if (m_MSSQLReader[1] != DBNull.Value)
                                pr.dNetto = Convert.ToDouble(m_MSSQLReader[1]);
                            
                            if (m_MSSQLReader[2] != DBNull.Value)
                                pr.dBrytto = Convert.ToDouble(m_MSSQLReader[2]);

                            if (m_MSSQLReader[3] != DBNull.Value)
                                pr.iCountEtik = Convert.ToInt32(m_MSSQLReader[3]);

                            if (m_MSSQLReader[4] != DBNull.Value)
                                pr.iDlinaRylona = Convert.ToInt32(m_MSSQLReader[4]);

                            pr.strPartiya = m_lsZakazchik[i].strPartiya;
                            m_lsProductInfo.Add(pr);

                            m_MSSQLReader.Close();
                        }
                        catch (System.Exception ex)
                        {
                            //WriteLog("Ошибка выборки елементов с базы", ex);
                            MessageBox.Show(ex.Message);
                            m_MSSQLReader.Close();
                        }
                       // toolStripProgressBar_LOAD.Value = i;


                        /*strMSSQLQuery = "select s.rylon_state from itak_etiketka.dbo.itak_sklad s , itak_etiketka.dbo.itak_vihidrylon vh " +
                                        "where s.rylon_id=vh.id and vh.product_id=" + m_lsZakazchik[i].iProductId + " " + strCheck_rylon + " " + strYear;
                        try
                        {
                            m_MSSQLCommand.CommandText = strMSSQLQuery;
                            m_MSSQLReader = m_MSSQLCommand.ExecuteReader();

                            while (m_MSSQLReader.Read())
                            {
                                iState = Convert.ToInt32(m_MSSQLReader[0]);
                                if (iState == 1)
                                    iCountNaSklade++;
                                else if (iState == 2)
                                    iCountOtgryz++;
                            }

                            m_MSSQLReader.Close();
                        }
                        catch(System.Exception ex)
                        {
                            m_MSSQLReader.Close();
                        }*/

                        if (iCheckBoxSatate==0)
                        {
                            strMSSQLQuery = "select COUNT(vh.id) , ROUND(sum(vh.brytto-vh.vagatary),3), ROUND(sum(vh.brytto),3) " +
                                       "from itak_etiketka.dbo.itak_vihidrylon vh , itak_etiketka.dbo.itak_sklad s, itak_etiketka.dbo.itak_zakaz z " +
                                       "where s.rylon_id=vh.id and s.rylon_state=1 and vh.product_id=" + m_lsZakazchik[i].iProductId + " " + strYear + "and vh.zakaz_id=z.id and z.partiya='"+m_lsZakazchik[i].strPartiya+"'";
                            try
                            {
                                m_MSSQLCommand.CommandText = strMSSQLQuery;
                                m_MSSQLReader = m_MSSQLCommand.ExecuteReader();

                                m_MSSQLReader.Read();

                                iCountRylonNaSklade += Convert.ToInt32(m_MSSQLReader[0]);
                                dNettoNaSklade += Convert.ToDouble(m_MSSQLReader[1]);
                                dBryttoNaSklade += Convert.ToDouble(m_MSSQLReader[2]);


                                m_MSSQLReader.Close();
                            }
                            catch (System.Exception)
                            {
                                m_MSSQLReader.Close();
                            }


                            strMSSQLQuery = "select COUNT(vh.id) , ROUND(sum(vh.brytto-vh.vagatary),3), ROUND(sum(vh.brytto),3) " +
                                       "from itak_etiketka.dbo.itak_vihidrylon vh , itak_etiketka.dbo.itak_sklad s , itak_etiketka.dbo.itak_zakaz z " +
                                       "where s.rylon_id=vh.id and s.rylon_state=2 and vh.product_id=" + m_lsZakazchik[i].iProductId + " " + strYear + "and vh.zakaz_id=z.id and z.partiya='" + m_lsZakazchik[i].strPartiya + "'"; ;
                            try
                            {
                                m_MSSQLCommand.CommandText = strMSSQLQuery;
                                m_MSSQLReader = m_MSSQLCommand.ExecuteReader();

                                m_MSSQLReader.Read();

                                iCountRylonOtgryz += Convert.ToInt32(m_MSSQLReader[0]);
                                dNettoOtgryz += Convert.ToDouble(m_MSSQLReader[1]);
                                dBryttoOtgryz += Convert.ToDouble(m_MSSQLReader[2]);

                                m_MSSQLReader.Close();
                            }
                            catch (System.Exception)
                            {
                                m_MSSQLReader.Close();
                            }
                        }


                    }

                    //  toolStripStatusLabel_mainForm.Text = "Выборка прошла успешно";
                    //toolStripProgressBar_LOAD.Value = 0;

                    //int iZakazchikKey = 0;
                    int iCountRylon = 0;
                    double dNetto = 0;
                    double dBrytto = 0;
                    int iCountEtiketki = 0;
                    int iDlinaRylona = 0;

                    double dAllBrytto = 0;
                    double dAllNetto = 0;
                    int iAllCountRylon = 0;
                    int iAllCountEtiketki = 0;
                    int iAllDlinaRylona=0;

                    int iNumber = 0;
                    /*if (strZakazchik.Length == 0 && strPartiya.Length == 0)*/
                    {
                        for (i = 0; i < m_lsZakazchik.Count; i++)
                        {
                            //if (iZakazchikKey != m_lsZakazchik[i].iZakazchikId)
                            {
                               
                                    dataGridViewSklad_AddProduct.Rows.Add((iNumber + 1).ToString());
                                    dataGridViewSklad_AddProduct.Rows[iNumber].Cells["Zakazchik"].Value = m_lsZakazchik[i].strZakazchikName;
                                    dataGridViewSklad_AddProduct.Rows[i].Cells["Product"].Value = m_lsZakazchik[i].strProductName;
                                    dataGridViewSklad_AddProduct.Rows[i].Cells["Partiya"].Value = m_lsZakazchik[i].strPartiya;

                                    /*
                                        for (int j = 0; j < m_lsProductInfo.Count; j++)
                                                                            {
                                                                                if (m_lsZakazchik[i].iZakazchikId == m_lsProductInfo[j].iZakazchikId 
                                                                                    && m_lsZakazchik[i].iProductId == m_lsProductInfo[j].iProductId
                                                                                    && m_lsZakazchik[i].strPartiya == m_lsProductInfo[j].strPartiya)
                                                                                {*/
                                        
                                            dataGridViewSklad_AddProduct.Rows[iNumber].Cells["CountRylon"].Value = m_lsProductInfo[i].iCountRylon;
                                            dataGridViewSklad_AddProduct.Rows[iNumber].Cells["CountEtiketki"].Value = m_lsProductInfo[i].iCountEtik;
                                            dataGridViewSklad_AddProduct.Rows[iNumber].Cells["Netto"].Value = m_lsProductInfo[i].dNetto; ;
                                            dataGridViewSklad_AddProduct.Rows[iNumber].Cells["Brytto"].Value = m_lsProductInfo[i].dBrytto;
                                      /*
                                      }
                                                                        }*/
                                    
                                   

                                    if (i % 2 == 0)
                                        dataGridViewSklad_AddProduct.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                                    else dataGridViewSklad_AddProduct.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;


                                    //iCountRylon = iCountEtiketki = 0;
                                    //dNetto = dBrytto = 0;


                                    // iZakazchikKey = m_lsZakazchik[i].iZakazchikId;
                                    iNumber++;
                                
                            }
                        }
                    }

                    for (int j = 0; j < m_lsProductInfo.Count; j++)
                    {
                       // if (m_lsProductInfo[j].iZakazchikId == m_lsZakazchik[i].iZakazchikId)
                        {
                            iCountRylon += m_lsProductInfo[j].iCountRylon;
                            iCountEtiketki += m_lsProductInfo[j].iCountEtik;
                            dBrytto += m_lsProductInfo[j].dBrytto;
                            dNetto += m_lsProductInfo[j].dNetto;
                            iDlinaRylona += m_lsProductInfo[j].iDlinaRylona;
                        }
                    }





                    dAllBrytto += dBrytto;
                    dAllNetto += dNetto;
                    iAllCountRylon += iCountRylon;
                    iAllCountEtiketki += iCountEtiketki;
                    iAllDlinaRylona += iDlinaRylona;
                    /*
                    else if (strZakazchik.Length != 0 || strPartiya.Length != 0)
                                        {
                                            for (int j = 0; j < m_lsProductInfo.Count; j++)
                                            {
                                                dataGridView_result.Rows.Add((j + 1).ToString());
                                                dataGridView_result.Rows[j].Cells["Zakazchik"].Value = m_lsZakazchik[j].strZakazchikName;
                                                dataGridView_result.Rows[j].Cells["Product"].Value = m_lsZakazchik[j].strProductName;
                    
                                                iCountRylon += m_lsProductInfo[j].iCountRylon;
                                                iCountEtiketki += m_lsProductInfo[j].iCountEtik;
                                                dBrytto += m_lsProductInfo[j].dBrytto;
                                                dNetto += m_lsProductInfo[j].dNetto;
                    
                                                dataGridView_result.Rows[j].Cells["CountRylon"].Value = iCountRylon;
                                                dataGridView_result.Rows[j].Cells["CountEtiketki"].Value = iCountEtiketki;
                                                dataGridView_result.Rows[j].Cells["Netto"].Value = dNetto;
                                                dataGridView_result.Rows[j].Cells["Brytto"].Value = dBrytto;
                    
                                                dAllBrytto += dBrytto;
                                                dAllNetto += dNetto;
                                                iAllCountRylon += iCountRylon;
                                                iAllCountEtiketki += iCountEtiketki;
                                                iAllDlinaRylona += iDlinaRylona;
                    
                                                iCountRylon = iCountEtiketki = 0;
                                                dNetto = dBrytto = 0;
                                            }
                                        }*/
                    

                    //MessageBox.Show("Na - " + iCountNaSklade.ToString() + " Otgr - " + iCountOtgryz.ToString());
                    labelAddSklad_ZnachBrytto.Text = dBrytto.ToString("0.00") + " кг";
                    labelAddSklad_ZnachNetto.Text = dNetto.ToString("0.00") + " кг";
                    labelAddSklad_ZnachCountRylon.Text = iCountRylon.ToString() + " шт";
                    labelAddSklad_ZnachCountEtik.Text = iCountEtiketki.ToString() + " шт";
                    labelAddSklad_ZnachMP.Text = iDlinaRylona.ToString() + " м.п.";

                    /*label_nettoNaSklade.Text = dNettoNaSklade.ToString("0.00") + " кг";
                    label_bryttoNaSklade.Text = dBryttoNaSklade.ToString("0.00") + " кг";
                    label_countRylonNaSklade.Text = iCountRylonNaSklade.ToString() + " шт";

                    label_nettoOtgryz.Text = dNettoOtgryz.ToString("0.00") + " кг";
                    label_bryttoOtgryz.Text = dBryttoOtgryz.ToString("0.00") + " кг";
                    label_countRylonOtgryz.Text = iCountRylonOtgryz.ToString() + " шт";*/

                }
                else if (iRegim == 5)
                {

                }


            }
        }

        private void SetTypeRegim()
        {
            if (m_iTypeRegim == 1)
            {
                SelectCheckMen();
                label_checkMenLab.Visible = true;
                label_checkMenValue.Visible = true;
                button_changeCheckMen.Visible = true;
            }
            else if (m_iTypeRegim == 0 || m_iTypeRegim==2 || m_iTypeRegim==3)
            {
                label_checkMenLab.Visible = false;
                label_checkMenValue.Visible = false;
                button_changeCheckMen.Visible = false;
            }
        }

        private void SelectCheckMen()
        {
            m_selectUser = new selectUser_form();
            m_selectUser.SetCheckMen(m_iCheckMen);
                       
            if (flag == false)
                m_selectUser.SetDialogType(1);
            else m_selectUser.SetDialogType(0);

            m_selectUser.ShowDialog();

            if (m_selectUser.GetDialogResult() == 1)
            {
                m_iCheckMen = m_selectUser.GetCheckMen();
                label_checkMenValue.Text = m_iCheckMen.ToString();
            }
            else if (m_selectUser.GetDialogResult()==0)
                this.Close();
            m_selectUser = null;
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            //string strDate = monthCalendar1.SelectionRange.Start.ToString();
            string strDate = monthCalendar1.SelectionRange.Start.Year.ToString() + "." + monthCalendar1.SelectionRange.Start.Month.ToString() + "." + monthCalendar1.SelectionRange.Start.Day.ToString();
           // strDate = strDate.Remove(strDate.IndexOf(' '));
            SelectZakaz(strDate,"");

            dataGridView_product.Rows.Clear();
            m_dicProduct.Clear();

        }


        private void SelectZakazInfo(string strQuery)
        {
            m_MSSQLCommand.CommandText = strQuery;
            m_MSSQLReader = m_MSSQLCommand.ExecuteReader();


            string strListInfo = "";
            string strStatus = "";
            string strStatusOTK = "";
            string strManager = "";

            int i=1;
            while (m_MSSQLReader.Read())
            {
                string strDate1 = m_MSSQLReader["datezakaz"].ToString().Trim();
                strDate1 = strDate1.Remove(strDate1.IndexOf(' '));
                strStatus = m_MSSQLReader["timeendzakaz"].ToString().Trim();
                strStatusOTK = m_MSSQLReader["check_zakaz"].ToString().Trim();
                strManager = m_MSSQLReader["manager_name"].ToString().Trim();


                
                dataGridView_rezkaZakaz.Rows.Add(i.ToString(), 
                                        m_MSSQLReader["zakazchik_name"].ToString().Trim(),
                                        m_MSSQLReader["partiya"].ToString().Trim(),
                                        strDate1,
                                        m_MSSQLReader["timestartzakaz"].ToString().Trim(),
                                        "",
                                        strManager
                                        );

                strListInfo = m_MSSQLReader["zakazchik_name"].ToString().Trim() +
                                m_MSSQLReader["partiya"].ToString().Trim() +
                                strDate1 + m_MSSQLReader["timestartzakaz"].ToString().Trim();

                m_dicZakaz.Add(strListInfo, Convert.ToInt32(m_MSSQLReader["id"].ToString().Trim()));
                m_dicDlinaEtiketki.Add(m_MSSQLReader["id"].ToString().Trim(), m_MSSQLReader["dlinaetiketki"].ToString().Trim());
                m_dicMaterial.Add(m_MSSQLReader["id"].ToString().Trim(), m_MSSQLReader["product_material"].ToString().Trim());
                m_dicManager.Add(m_MSSQLReader["id"].ToString().Trim(), m_MSSQLReader["manager_name"].ToString().Trim());

                m_dicTols.Add(m_MSSQLReader["id"].ToString().Trim(), m_MSSQLReader["product_tols"].ToString().Trim());

                if (strStatus.Length == 0)
                {
                    dataGridView_rezkaZakaz.Rows[i - 1].Cells["Status"].Value = "В ПОРЕЗКЕ";
                    dataGridView_rezkaZakaz.Rows[i - 1].DefaultCellStyle.BackColor = Color.Yellow;

                }
                else
                {
                    dataGridView_rezkaZakaz.Rows[i - 1].Cells["Status"].Value = "НЕ ПРОВЕРЕН ВЕС";
                    dataGridView_rezkaZakaz.Rows[i-1].DefaultCellStyle.BackColor = Color.Red;
                }

                i++;
            }
            m_MSSQLReader.Close();
            
            string strKey;
            double dBrytto = 0;
            double dNetto = 0;
            int iCountRylon = 0;
            int iCountCheckRylon = 0;
            int iCountUnCheckRylon = 0;
            int iCountDelRylon = 0;
            int iKolEtiketki = 0;
            int iDlinaRylona = 0;
            double dSquare = 0;

            
            for (i=0;i<dataGridView_rezkaZakaz.Rows.Count;i++)
            {
                strKey = dataGridView_rezkaZakaz.Rows[i].Cells[1].Value.ToString()
                    +dataGridView_rezkaZakaz.Rows[i].Cells[2].Value.ToString()
                    +dataGridView_rezkaZakaz.Rows[i].Cells[3].Value.ToString()
                    + dataGridView_rezkaZakaz.Rows[i].Cells[4].Value.ToString();
                
                strQuery = "select state_rylon from itak_etiketka.dbo.itak_vihidrylon where zakaz_id=" + m_dicZakaz[strKey];
                m_MSSQLCommand.CommandText = strQuery;
                m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
                
                while (m_MSSQLReader.Read())
                {
                    try
                    {
                        if (Convert.ToInt32(m_MSSQLReader["state_rylon"]) == 1)
                            iCountCheckRylon++;
                        else if (Convert.ToInt32(m_MSSQLReader["state_rylon"]) == 0)
                            iCountUnCheckRylon++;
                        else if (Convert.ToInt32(m_MSSQLReader["state_rylon"]) == 2)
                            iCountDelRylon++;
                       // MessageBox.Show()

                        iCountRylon++;
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                m_MSSQLReader.Close();

                if (iCountRylon == (iCountCheckRylon-iCountDelRylon))
                {
                    dataGridView_rezkaZakaz.Rows[i].Cells["Status"].Value = "ПРОВЕРЕНО";
                    dataGridView_rezkaZakaz.Rows[i].DefaultCellStyle.BackColor= Color.LightGreen;
                }


                iCountRylon = iCountCheckRylon = iCountUnCheckRylon=iCountDelRylon = 0;

            }

            

            m_dNetto = 0;
            m_dBrytto = 0;
            m_dSquare = 0;
            m_iCountRylon = 0;
            m_iKolEtiketki = 0;
            m_iDlinaRylona = 0;
          //  m_strMaterial = "";


            
            foreach (KeyValuePair<string, int> pair in m_dicZakaz)
            {
                SelectInfo(pair.Value);
                dBrytto += m_dBrytto;
                dNetto += m_dNetto;
                iCountRylon += m_iCountRylon;
                iKolEtiketki += m_iKolEtiketki;
                iDlinaRylona += m_iDlinaRylona;
                dSquare += m_dSquare;
            }

            label_brytto.Text = dBrytto.ToString("0.00") + " кг";
            label_netto.Text = dNetto.ToString("0.00") + " кг";
            label_countRylon.Text = iCountRylon.ToString() + " шт";
            label_koletiket.Text = iKolEtiketki.ToString() + " шт";
            label_mp.Text = iDlinaRylona.ToString() + " м";
            label_square.Text = dSquare.ToString("0.0") + " м.кв";
            //label_Material.Text = m_strMaterial;

        }


        private void SelectZakaz(string strDate, string strPartiya)
        {
            dataGridView_rezkaZakaz.Rows.Clear();
            m_dicZakaz.Clear();
            m_dicDlinaEtiketki.Clear();
            m_dicMaterial.Clear();
            m_dicManager.Clear();
            m_dicTols.Clear();

            int iSelectProduct = -1;
            for (int j = 0; j < dataGridView_product.Rows.Count; j++)
                if (dataGridView_product.Rows[j].Selected == true)
                    iSelectProduct = j;


            if (iSelectProduct != -1)
            {
                if (button_PrintEasy.Enabled == true)
                    button_PrintEasy.Enabled = false;
            }


            if (strDate.Length!=0 && strPartiya.Length==0)
            {
                strMSSQLQuery = "select z.id,zn.zakazchik_name, z.partiya, z.datezakaz, z.timestartzakaz, z.timeendzakaz, z.check_zakaz, z.dlinaetiketki, pm.product_material, pt.product_tols,m.manager_name " +
                           "from itak_etiketka.dbo.itak_zakazchik zn, itak_etiketka.dbo.itak_zakaz z, itak_etiketka.dbo.itak_productmaterial pm, itak_etiketka.dbo.itak_producttols pt, itak_etiketka.dbo.itak_manager m " +
                           "where z.zakazchik_id=zn.id and pm.id=z.productmaterial_id and z.producttols_id=pt.id and m.id=z.manager_id and z.state=1 and z.datezakaz='" + strDate + "' order by z.timestartzakaz ASC";
                SelectZakazInfo(strMSSQLQuery);

            }
            else if (strPartiya.Length!=0 && strDate.Length==0)
            {
                strMSSQLQuery = "select z.id,zn.zakazchik_name, z.partiya, z.datezakaz, z.timestartzakaz, z.timeendzakaz, z.check_zakaz, z.dlinaetiketki, pm.product_material, pt.product_tols,m.manager_name  " +
                          "from itak_etiketka.dbo.itak_zakazchik zn, itak_etiketka.dbo.itak_zakaz z, itak_etiketka.dbo.itak_productmaterial pm, itak_etiketka.dbo.itak_producttols pt, itak_etiketka.dbo.itak_manager m " +
                          "where z.zakazchik_id=zn.id and pm.id=z.productmaterial_id and z.producttols_id=pt.id and m.id=z.manager_id and z.state=1 and z.partiya like'%" + strPartiya + "%' order by z.timestartzakaz ASC";
                SelectZakazInfo(strMSSQLQuery);
            }
            else if(strDate.Length!=0 && strPartiya.Length!=0)
            {
                strMSSQLQuery = "select z.id,zn.zakazchik_name, z.partiya, z.datezakaz, z.timestartzakaz, z.timeendzakaz, z.check_zakaz, z.dlinaetiketki, pm.product_material, pt.product_tols,m.manager_name  " +
                          "from itak_etiketka.dbo.itak_zakazchik zn, itak_etiketka.dbo.itak_zakaz z, itak_etiketka.dbo.itak_productmaterial pm, itak_etiketka.dbo.itak_producttols pt, itak_etiketka.dbo.itak_manager m " +
                          "where z.zakazchik_id=zn.id and pm.id=z.productmaterial_id and z.producttols_id=pt.id and m.id=z.manager_id and z.state=1 and z.partiya like '%" + strPartiya + "%' and z.datezakaz='" + strDate + "' order by z.timestartzakaz ASC";
                SelectZakazInfo(strMSSQLQuery);
            }          
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void DataGrid1_Click()
        {
            
            int iSelectedZakaz = -1;
            if (dataGridView_rezkaZakaz.CurrentCell.RowIndex != -1)
            {
                for (int i = 0; i < dataGridView_rezkaZakaz.Rows.Count; i++)
                    if (dataGridView_rezkaZakaz.Rows[i].Selected == true)
                        iSelectedZakaz = i;
            }

            if (/*dataGridView1.CurrentCell.RowIndex */iSelectedZakaz != -1)
            {
                int iSelectProduct = -1;
                for (int j = 0; j < dataGridView_product.Rows.Count; j++)
                    if (dataGridView_product.Rows[j].Selected == true)
                        iSelectProduct = j;


                if (iSelectProduct != -1)
                {
                    if (button_PrintEasy.Enabled == true)
                        button_PrintEasy.Enabled = false;
                }

                int iSelRow = /*dataGridView1.CurrentCell.RowIndex;*/iSelectedZakaz;
                string strListInfo = dataGridView_rezkaZakaz.Rows[iSelRow].Cells[1].Value.ToString() +
                                     dataGridView_rezkaZakaz.Rows[iSelRow].Cells[2].Value.ToString() +
                                     dataGridView_rezkaZakaz.Rows[iSelRow].Cells[3].Value.ToString() +
                                     dataGridView_rezkaZakaz.Rows[iSelRow].Cells[4].Value.ToString();


                m_dNetto = 0;
                m_dBrytto = 0;
                m_iCountRylon = 0;
                m_iKolEtiketki = 0;
                m_iDlinaRylona = 0;

                m_iZakazID = m_dicZakaz[strListInfo];
                SelectInfo(m_iZakazID);

                label_brytto.Text = m_dBrytto.ToString("0.00") + " кг";
                label_netto.Text = m_dNetto.ToString("0.00") + " кг";
                label_countRylon.Text = m_iCountRylon.ToString() + " шт";
                label_koletiket.Text = m_iKolEtiketki.ToString() + " шт";
                label_mp.Text = m_iDlinaRylona.ToString() + " м";
                label_Material.Text = m_strMaterial;
                label_rezkaManagerValue.Text = m_dicManager[m_iZakazID.ToString()];
                label_DlinaEtiketkiValue.Text = m_strDlinaEtiketki + " мм";

                SelectProduct(m_dicZakaz[strListInfo]);

                for (int i = 0; i < dataGridView_product.Rows.Count; i++)
                    dataGridView_product.Rows[i].Selected = false;
            }
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (dataGridView_rezkaZakaz.Rows.Count!=0)
                DataGrid1_Click();
            
        }

        private void SelectProduct(int iZakazID)
        {
            m_dicProduct.Clear();
            m_dicWidth.Clear();
            dataGridView_product.Rows.Clear();

            strMSSQLQuery = "select distinct  pr.product_name, vh.width, pr.id from itak_etiketka.dbo.itak_product pr, itak_etiketka.dbo.itak_vihidrylon vh where vh.product_id=pr.id and vh.zakaz_id=" + iZakazID.ToString();
            m_MSSQLCommand.CommandText = strMSSQLQuery;
            m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
            int i=1;
            while (m_MSSQLReader.Read())
            {
                try
                {
                    dataGridView_product.Rows.Add((i++).ToString(), m_MSSQLReader["product_name"].ToString().Trim(), m_MSSQLReader["width"].ToString().Trim());
                    m_dicProduct.Add(m_MSSQLReader["product_name"].ToString().Trim()+"-"+m_MSSQLReader["width"].ToString().Trim(), Convert.ToInt32(m_MSSQLReader["id"].ToString().Trim()));
                    m_dicWidth.Add(m_MSSQLReader["product_name"].ToString().Trim()+"-"+m_MSSQLReader["width"].ToString().Trim(), m_MSSQLReader["width"].ToString().Trim());
                }
                catch (System.Exception)
                {
                   // MessageBox.Show(ex.Message);
                }
            }

            m_MSSQLReader.Close();

            i = 0;
            foreach (var items in m_dicProduct)
            {
                int iCountCheck = 0;
                int iCountUnCheck = 0;
                int iCountRylon =0;
                int iDelRylon = 0;
                strMSSQLQuery = "select state_rylon from itak_etiketka.dbo.itak_vihidrylon where zakaz_id=" + iZakazID.ToString() + " and product_id=" + items.Value + " and width=" + m_dicWidth[items.Key].ToString();
                
                m_MSSQLCommand.CommandText = strMSSQLQuery;
                m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
                while (m_MSSQLReader.Read())
                {
                    try
                    {
                        if (Convert.ToInt32(m_MSSQLReader["state_rylon"]) == 1)
                            iCountCheck++;
                        else if (Convert.ToInt32(m_MSSQLReader["state_rylon"]) == 0)
                            iCountUnCheck++;
                        else if (Convert.ToInt32(m_MSSQLReader["state_rylon"]) == 2)
                            iDelRylon++;
                        iCountRylon++;
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                m_MSSQLReader.Close();

                dataGridView_product.Rows[i].Cells["RylonCount"].Value = iCountRylon;
                dataGridView_product.Rows[i].Cells["Check"].Value = iCountCheck;
                dataGridView_product.Rows[i].Cells["UnCheck"].Value = iCountUnCheck;


                if (iCountCheck == iCountRylon-iDelRylon)
                    dataGridView_product.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                else dataGridView_product.Rows[i].DefaultCellStyle.BackColor = Color.Red;

                i++;


               
                iCountRylon = iCountCheck = iCountUnCheck = iDelRylon = 0;

               // MessageBox.Show(items.Value+" - "+items.Key);
            }

            int iCountCheckProduct = 0;
            for (i=0;i<dataGridView_product.Rows.Count;i++)
            {
                if(dataGridView_product.Rows[i].DefaultCellStyle.BackColor==Color.LightGreen)
                {
                    iCountCheckProduct++;
                }
            }
            
            if (iCountCheckProduct == i)
            {
                if (dataGridView_rezkaZakaz.Rows[dataGridView_rezkaZakaz.CurrentRow.Index].DefaultCellStyle.BackColor == Color.Red)
                {
                    dataGridView_rezkaZakaz.Rows[dataGridView_rezkaZakaz.CurrentRow.Index].DefaultCellStyle.BackColor = Color.LightGreen;
                    dataGridView_rezkaZakaz.Rows[dataGridView_rezkaZakaz.CurrentRow.Index].Cells["Status"].Value = "ПРОВЕРЕНО";
                }

            }
            else if (dataGridView_rezkaZakaz.Rows[dataGridView_rezkaZakaz.CurrentRow.Index].DefaultCellStyle.BackColor == Color.LightGreen)
            {
                dataGridView_rezkaZakaz.Rows[dataGridView_rezkaZakaz.CurrentRow.Index].DefaultCellStyle.BackColor = Color.Red;
                dataGridView_rezkaZakaz.Rows[dataGridView_rezkaZakaz.CurrentRow.Index].Cells["Status"].Value = "НЕ ПРОВЕРЕН ВЕС";
            }

        }

        private void SelectInfo(int iZakazID)
        {
            strMSSQLQuery = "select sum(brytto), sum(brytto-vagatary), count(id), sum(dlinarylona), sum(koletiketki)  from itak_etiketka.dbo.itak_vihidrylon where (state_rylon=1 or state_rylon=0) and zakaz_id=" + iZakazID.ToString();
            m_MSSQLCommand.CommandText = strMSSQLQuery;
            m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
            
            while (m_MSSQLReader.Read())
            {
                try
                {
                    m_dBrytto = Convert.ToDouble(m_MSSQLReader[0]);
                    m_dNetto = Convert.ToDouble(m_MSSQLReader[1]);
                    m_iCountRylon = Convert.ToInt32(m_MSSQLReader[2]);
                    m_iDlinaRylona = Convert.ToInt32(m_MSSQLReader[3]);
                    m_iKolEtiketki = Convert.ToInt32(m_MSSQLReader[4]);
                    m_strMaterial = m_dicMaterial[iZakazID.ToString()].ToString()+" "+m_dicTols[iZakazID.ToString()].ToString();
                    m_strDlinaEtiketki = m_dicDlinaEtiketki[iZakazID.ToString()].ToString();
                    
                }
                catch (System.Exception)
                {
                                    	
                }
                
            }
            m_MSSQLReader.Close();
        }


        private void ButtonSearchClick()
        {
            dataGridView_product.Rows.Clear();
            m_dicProduct.Clear();

            if (textBox_tz.Text.Length != 0)
            {
                SelectZakaz("", textBox_tz.Text);
            }

            if (checkBox1.Checked == true)
            {
                if (textBox_tz.Text.Length != 0)
                {
                    //string strDate = monthCalendar1.SelectionRange.Start.ToString();
                    //strDate = strDate.Remove(strDate.IndexOf(' '));
                    string strDate = monthCalendar1.SelectionRange.Start.Year.ToString() + "." + monthCalendar1.SelectionRange.Start.Month.ToString() + "." + monthCalendar1.SelectionRange.Start.Day.ToString();

                    SelectZakaz(strDate, textBox_tz.Text);
                }
            }

            for (int i = 0; i < dataGridView_rezkaZakaz.Rows.Count; i++)
                dataGridView_rezkaZakaz.Rows[i].Selected = false;
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            ButtonSearchClick();
        }

        private void SelectProductInfo(int iZakazID, int iProductID)
        {
            m_iZakazID = iZakazID;
            m_iProductID = iProductID;

            strMSSQLQuery = "select sum(brytto), sum(brytto-vagatary), count(id), sum(dlinarylona), sum(koletiketki)  from itak_etiketka.dbo.itak_vihidrylon where (state_rylon=1 or state_rylon=0) and zakaz_id=" + iZakazID.ToString() + " and product_id=" + iProductID.ToString();
            m_MSSQLCommand.CommandText = strMSSQLQuery;
            m_MSSQLReader = m_MSSQLCommand.ExecuteReader();

            while (m_MSSQLReader.Read())
            {
                try
                {
                    m_dBrytto = Convert.ToDouble(m_MSSQLReader[0]);
                    m_dNetto = Convert.ToDouble(m_MSSQLReader[1]);
                    m_iCountRylon = Convert.ToInt32(m_MSSQLReader[2]);
                    m_iDlinaRylona = Convert.ToInt32(m_MSSQLReader[3]);
                    m_iKolEtiketki = Convert.ToInt32(m_MSSQLReader[4]);
                    m_strMaterial = m_dicMaterial[iZakazID.ToString()].ToString() + " " + m_dicTols[iZakazID.ToString()].ToString();
                    m_strDlinaEtiketki = m_dicDlinaEtiketki[iZakazID.ToString()].ToString();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            m_MSSQLReader.Close();
        }

        private void GetProductWidth(int iZakazID, int iProductID)
        {
            strMSSQLQuery = "select width from itak_etiketka.dbo.itak_vihidrylon where (state_rylon=1 or state_rylon=0) and zakaz_id=" 
                + iZakazID.ToString() + " and product_id=" + iProductID.ToString();
            m_MSSQLCommand.CommandText = strMSSQLQuery;
            m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
            m_MSSQLReader.Read();
            if (m_MSSQLReader.HasRows)
            {
                m_iWidth = Convert.ToInt32(m_MSSQLReader["width"]);
            }
            m_MSSQLReader.Close();

        }

        private void RefreshBut()
        {
            int iSelectedZakaz = -1;
            int iSelectProduct = -1;

            for (int i = 0; i < dataGridView_rezkaZakaz.Rows.Count; i++)
                if (dataGridView_rezkaZakaz.Rows[i].Selected == true)
                    iSelectedZakaz = i;

            for (int i = 0; i < dataGridView_product.Rows.Count; i++)
                if (dataGridView_product.Rows[i].Selected == true)
                    iSelectProduct = i;

            if (textBox_tz.Text.Length != 0)
            {
                SelectZakaz("", textBox_tz.Text);

                if (checkBox1.Checked == true)
                {
                    if (textBox_tz.Text.Length != 0)
                    {
                        //string strDate = monthCalendar1.SelectionRange.Start.ToString();
                        //strDate = strDate.Remove(strDate.IndexOf(' '));
                        string strDate = monthCalendar1.SelectionRange.Start.Year.ToString() + "." + monthCalendar1.SelectionRange.Start.Month.ToString() + "." + monthCalendar1.SelectionRange.Start.Day.ToString();
                        SelectZakaz(strDate, textBox_tz.Text);
                    }
                }
            }
            else
            {
                //string strDate = monthCalendar1.SelectionRange.Start.ToString();
                //strDate = strDate.Remove(strDate.IndexOf(' '));
                string strDate = monthCalendar1.SelectionRange.Start.Year.ToString() + "." + monthCalendar1.SelectionRange.Start.Month.ToString() + "." + monthCalendar1.SelectionRange.Start.Day.ToString();

                SelectZakaz(strDate, "");

                //listBox1.Items.Clear();
                //dataGridView_product.Rows.Clear();
                //m_dicProduct.Clear();
            }

            // dataGridView1.Rows[0].Selected = false;
            if (iSelectedZakaz != -1)
            {
                dataGridView_rezkaZakaz.Rows[iSelectedZakaz].Selected = true;
                DataGrid1_Click();
            }

            if (iSelectProduct != -1)
            {
                dataGridView_product.Rows[iSelectProduct].Selected = true;
                DataGridProductClick();
            }
        }

        private void button_refresh_Click(object sender, EventArgs e)
        {
            RefreshBut();
        }

        private void DataGridProductClick()
        {
            int iSelectProduct = -1;
            int iSelectedZakaz = -1;
           
            for (int i = 0; i < dataGridView_rezkaZakaz.Rows.Count; i++)
                if (dataGridView_rezkaZakaz.Rows[i].Selected == true)
                    iSelectedZakaz = i;
        
            for (int i = 0; i < dataGridView_product.Rows.Count; i++)
                if (dataGridView_product.Rows[i].Selected == true)
                    iSelectProduct = i;
            

            if (/*dataGridView_product.CurrentCell.RowIndex*/iSelectProduct != -1)
            {
                if (/*dataGridView1.CurrentCell.RowIndex*/iSelectedZakaz != -1)
                {
                    if (button_PrintEasy.Enabled == false)
                        button_PrintEasy.Enabled = true;

                    int iSelRow = /*dataGridView1.CurrentCell.RowIndex*/iSelectedZakaz;
                    string strListInfo = dataGridView_rezkaZakaz.Rows[iSelRow].Cells[1].Value.ToString() +
                                         dataGridView_rezkaZakaz.Rows[iSelRow].Cells[2].Value.ToString() +
                                         dataGridView_rezkaZakaz.Rows[iSelRow].Cells[3].Value.ToString() +
                                         dataGridView_rezkaZakaz.Rows[iSelRow].Cells[4].Value.ToString();


                    m_dNetto = 0;
                    m_dBrytto = 0;
                    m_iCountRylon = 0;
                    m_iKolEtiketki = 0;
                    m_iDlinaRylona = 0;
                    m_iWidth = 0;

                    string strKey = dataGridView_product.Rows[iSelectProduct].Cells["Product"].Value.ToString() +"-"+ dataGridView_product.Rows[iSelectProduct].Cells["Width"].Value.ToString(); ;

                    SelectProductInfo(m_dicZakaz[strListInfo], m_dicProduct[strKey]);
                    GetProductWidth(m_dicZakaz[strListInfo], m_dicProduct[strKey]);

                    m_dSquare = (m_iDlinaRylona * m_iWidth) / 1000.0;

                    label_brytto.Text = m_dBrytto.ToString("0.00") + " кг";
                    label_netto.Text = m_dNetto.ToString("0.00") + " кг";
                    label_countRylon.Text = m_iCountRylon.ToString() + " шт";
                    label_koletiket.Text = m_iKolEtiketki.ToString() + " шт";
                    label_mp.Text = m_iDlinaRylona.ToString() + " м";
                    label_Material.Text = m_strMaterial;
                    label_rezkaManagerValue.Text = m_strRezkaManager;
                    label_DlinaEtiketkiValue.Text = m_strDlinaEtiketki+" мм";
                    label_square.Text = m_dSquare.ToString("0.0") + " м.кв";

                }
            }
        }


        private void Menu_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
            /*RegistryKey saveKey = Registry.LocalMachine.CreateSubKey("software\\rezka_info");
            saveKey.SetValue("Type_app", "0"); // 0 - user 1-admin
            saveKey.Close(); */
        }

        private void OnSettings()
        {
            m_password = new password_form();
            m_password.ShowDialog();
            if (m_password.GetDialogResult() == 1)
            {
                m_settings = new settings_form();
                m_settings.ShowDialog();
                m_settings = null;

                RegistryKey readKey = Registry.LocalMachine.OpenSubKey("software\\rezka_info");
                if (readKey != null)
                {
                    string loadString = (string)readKey.GetValue("Type_app");
                    readKey.Close();
                    m_iTypeRegim = Convert.ToInt32(loadString);
                }

                SetTypeRegim();
            }

            m_password = null;
        }
        
        private void Menu_Settings_Click(object sender, EventArgs e)
        {
            OnSettings();
        }

        private void dataGridView_product_DoubleClick(object sender, EventArgs e)
        {

            if (dataGridView_product.Rows.Count!=0)
            if (dataGridView_product.CurrentCell.RowIndex != -1)
            {

                m_rylon = new rylon_form(1,m_iTypeRegim);
                m_rylon.m_MSSQLConnection = m_MSSQLConnection;
                int iListCurSel = dataGridView_product.CurrentCell.RowIndex;

                string strKey = dataGridView_product.Rows[iListCurSel].Cells["Product"].Value.ToString() + "-" + dataGridView_product.Rows[iListCurSel].Cells["Width"].Value.ToString();
                m_iProductID = m_dicProduct[strKey];


                if (dataGridView_rezkaZakaz.Rows[dataGridView_rezkaZakaz.CurrentCell.RowIndex].Cells["Status"].Value.ToString() == "В ПОРЕЗКЕ")
                    m_rylon.SetStatusZakaz(1);
                else
                    m_rylon.SetStatusZakaz(0);

                m_rylon.SetProductID(m_iProductID);
                m_rylon.SetZakazID(m_iZakazID);
                m_rylon.SetRegim(m_iTypeRegim);
                m_rylon.SetCheckMen(m_iCheckMen);
                m_rylon.SetWidth(Convert.ToInt32(m_dicWidth[strKey]));

                m_rylon.ShowDialog();

                if (m_rylon.GetDialogResult() == 1)
                {
                    int iSelectedProduct = dataGridView_product.CurrentCell.RowIndex;

                    DataGrid1_Click();
                    SelectProduct(m_iZakazID);

                    dataGridView_product.Rows[iSelectedProduct].Selected = true;
                }
                m_rylon = null;
            }
        }

        private void dataGridView_product_MouseClick(object sender, MouseEventArgs e)
        {
            DataGridProductClick();
        }

        private void button_changeCheckMen_Click(object sender, EventArgs e)
        {
            SelectCheckMen();
        }

        private void button_PrintEasy_Click(object sender, EventArgs e)
        {
            
            if (dataGridView_product.SelectedRows.Count!=-0)
            {
                PrintDocument docPrint = new PrintDocument();
                if (docPrint.PrinterSettings.IsValid)
                {
                    docPrint.DefaultPageSettings.Margins.Top = 10;
                    docPrint.DefaultPageSettings.Margins.Bottom = 10;
                    docPrint.DefaultPageSettings.Margins.Right = 0;
                    docPrint.DefaultPageSettings.Margins.Left = 0;

                    docPrint.DefaultPageSettings.Landscape = true;
                    docPrint.PrintPage += new PrintPageEventHandler
                           (this.docPrint_PrintPage);


                    //docPrintt.PrinterSettings.Copies = (short)pr.m_iNumCopies;
                    docPrint.PrinterSettings.Copies = 1;

                    PrintPreviewDialog dlgPrint = new PrintPreviewDialog();
                    dlgPrint.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height);
                    dlgPrint.Document = docPrint;
                    //dlgPrint.ShowDialog();
                    docPrint.Print();
                }
                else
                    MessageBox.Show("Принтер не доступен!");
            }
        }
        private void docPrint_PrintPage(object sender, PrintPageEventArgs e)
        {
            System.Drawing.Font fnt10Bold = new Font("Times New Roman", 10, FontStyle.Bold);
            System.Drawing.Font fnt10 = new Font("Times New Roman", 10, FontStyle.Regular);

            System.Drawing.Font fnt12Bold = new Font("Times New Roman", 12, FontStyle.Bold);
            System.Drawing.Font fnt12 = new Font("Times New Roman", 12, FontStyle.Regular);
            System.Drawing.Font fnt12BoldUnder = new Font("Times New Roman", 12, FontStyle.Bold | FontStyle.Underline);
            System.Drawing.Font fnt14Bold = new Font("Times New Roman", 14, FontStyle.Bold);
            System.Drawing.Font fnt14 = new Font("Times New Roman", 14, FontStyle.Regular);
            System.Drawing.Font fnt14BoldUnder = new Font("Times New Roman", 14, FontStyle.Bold | FontStyle.Underline);
            System.Drawing.Font fnt16Bold = new Font("Times New Roman", 16, FontStyle.Bold);
            System.Drawing.Font fnt16BoldUnder = new Font("Times New Roman", 16, FontStyle.Bold | FontStyle.Underline);
            SolidBrush brushGray = new SolidBrush(Color.LightGray);

            /////////////////////
            string strPartiya = "";
            string strZakazchik = "";
            string strProduct = "";
            string strDate = "";
            string strMaterial = "";
            string strWidth = "";
            string strEtiketkaWidth = "";
            /////////////////////

            if (dataGridView_rezkaZakaz.SelectedRows.Count!=0)
            {
                int iSelectedZakazchikRow = -1;
                int iSelectedProductRow = -1;

                for (int i=0;i<dataGridView_rezkaZakaz.Rows.Count;i++)
                    if (dataGridView_rezkaZakaz.Rows[i].Selected==true)
                    {
                        iSelectedZakazchikRow = i;
                        break;
                    }
                
                if (iSelectedZakazchikRow!=-1)
                {
                    strZakazchik = dataGridView_rezkaZakaz.Rows[iSelectedZakazchikRow].Cells[1].Value.ToString();
                    strPartiya = dataGridView_rezkaZakaz.Rows[iSelectedZakazchikRow].Cells[2].Value.ToString();
                    strDate = dataGridView_rezkaZakaz.Rows[iSelectedZakazchikRow].Cells[3].Value.ToString();

                    string strListInfo = strZakazchik + strPartiya + strDate+ dataGridView_rezkaZakaz.Rows[iSelectedZakazchikRow].Cells[4].Value.ToString();
                    strEtiketkaWidth = m_dicDlinaEtiketki[m_dicZakaz[strListInfo].ToString()].ToString();
                    strMaterial = m_dicMaterial[m_dicZakaz[strListInfo].ToString()].ToString() + " " + m_dicTols[m_dicZakaz[strListInfo].ToString()].ToString();
                    m_strDlinaEtiketki = m_dicDlinaEtiketki[m_dicZakaz[strListInfo].ToString()].ToString();

                    strDate += " р.";
                }


                if(dataGridView_product.SelectedRows.Count!=0)
                    for (int i=0;i<dataGridView_product.Rows.Count;i++)
                        if (dataGridView_product.Rows[i].Selected==true)
                        {
                            iSelectedProductRow = i;
                            break;
                        }

                if (iSelectedProductRow!=-1)
                {
                    strProduct = dataGridView_product.Rows[iSelectedProductRow].Cells["Product"].Value.ToString()+"-"+dataGridView_product.Rows[iSelectedProductRow].Cells["Width"].Value.ToString();
                    strWidth = m_dicWidth[strProduct];
                }

            }

            int x1 = e.PageBounds.Left;
            int x2 = e.PageBounds.Right-40;
            int y1 = e.PageBounds.Top;
            int y2 = e.PageBounds.Bottom;

            SolidBrush brush = new SolidBrush(Color.Black);
            Pen mP = new Pen(Color.Black,1);
            mP.DashStyle = DashStyle.Dash;

            int x1_left = x1;
            int x1_right = (x2 / 2)-20;
            
            int x2_left = (x2/2)+10;
            int x2_right = x2 ;
            
            e.Graphics.DrawLine(mP, new Point((int)(x2/2), (int)y1), new Point((int)(x2/2), (int)y2));

            int x_left = x1_left;
            int x_right = x1_right;
            int iTop = y1+15;


            string strText = "ТОВ \"ІТАК\"";
            int xx = ((x_right - x_left) / 2) - ((strText.Length * 5));
            e.Graphics.DrawString(strText, fnt12Bold, Brushes.Black, xx, iTop);
            
            iTop += 20;
            strText = "м.Київ, вул. Червоноткацька, 44";
            xx = ((x_right - x_left) / 2) - ((strText.Length * 4));
            e.Graphics.DrawString(strText, fnt12Bold, Brushes.Black, xx, iTop);

            iTop += 60;
            strText = "СЕРТИФІКАТ ЯКОСТІ №" + strPartiya;
            xx = ((x_right - x_left) / 2) - ((strText.Length * 6));
            e.Graphics.DrawString(strText, fnt14Bold, Brushes.Black, xx, iTop);

            iTop += 40;
            e.Graphics.DrawString("Номер", fnt10, Brushes.Black, x_left + 55, iTop);
            e.Graphics.DrawString("замовлення", fnt10, Brushes.Black, x_left + 40, iTop+10);
            e.Graphics.FillRectangle(brushGray, x_left + 130, iTop , 150, 25);
            e.Graphics.DrawString(strPartiya, fnt12Bold, Brushes.Black, x_left + 135, iTop+2);

            iTop += 35;
            e.Graphics.DrawString("Замовник", fnt10, Brushes.Black, x_left + 45, iTop+3);
            e.Graphics.FillRectangle(brushGray, x_left + 130, iTop-5 , x_right-x_left-140, 25);
            e.Graphics.DrawString(strZakazchik, fnt12Bold, Brushes.Black, x_left + 135, iTop-2);

            iTop += 25;
            e.Graphics.DrawString("Найменування", fnt10, Brushes.Black, x_left + 30, iTop );
            e.Graphics.DrawString("продукції", fnt10, Brushes.Black, x_left + 45, iTop+10);
            
            e.Graphics.FillRectangle(brushGray, x_left + 130, iTop , x_right - x_left - 140, 25);
            e.Graphics.DrawString(strProduct, fnt10Bold, Brushes.Black, x_left + 135, iTop+5);

            iTop += 30;
            e.Graphics.DrawString("Дата", fnt10, Brushes.Black, x_left + 58, iTop);
            e.Graphics.DrawString("виготовлення", fnt10, Brushes.Black, x_left + 33, iTop + 10);

            e.Graphics.FillRectangle(brushGray, x_left + 130, iTop, 140, 25);
            e.Graphics.DrawString(strDate, fnt12Bold, Brushes.Black, x_left + 135, iTop + 2);

            int iTableLeft = x_left + 15;
            int iTableRight = x_right - x_left ;

            int iTabCol1 = iTableLeft+220;
            int iTabCol2 = iTabCol1 + 97;
            int iTabCol3 = iTabCol2 + 106;
                        
    
            iTop+=50;
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop, iTableRight, iTop);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop, iTableLeft, iTop+30);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableRight, iTop, iTableRight, iTop + 30);
            e.Graphics.DrawString("Найменування сировини", fnt10Bold, Brushes.Black, iTableLeft + 35,iTop+7);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTabCol1, iTop, iTabCol1, iTop + 30);

            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTabCol2, iTop, iTabCol2, iTop + 30);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTabCol3, iTop, iTabCol3, iTop + 30);

            e.Graphics.DrawString("Маса, г/м2", fnt10Bold, Brushes.Black, iTabCol1 + 13, iTop + 7);
            e.Graphics.DrawString("Ширина бобіни,", fnt10Bold, Brushes.Black, iTabCol2 + 1, iTop );
            e.Graphics.DrawString("мм", fnt10Bold, Brushes.Black, iTabCol2 + 40, iTop+12);

            e.Graphics.DrawString("Крок малюнку,", fnt10Bold, Brushes.Black, iTabCol3 + 1, iTop);
            e.Graphics.DrawString("мм", fnt10Bold, Brushes.Black, iTabCol3 + 40, iTop + 12);

            iTop += 30;
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop, iTableRight, iTop);

            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop, iTableLeft, iTop + 50);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableRight, iTop, iTableRight, iTop + 50);

            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTabCol1, iTop, iTabCol1, iTop + 50);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTabCol2, iTop, iTabCol2, iTop + 50);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTabCol3, iTop, iTabCol3, iTop + 50);

            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop+50, iTableRight, iTop+50);

            iTop += 15;
            if (strMaterial.Length<=23)
                e.Graphics.DrawString(strMaterial, fnt10Bold, Brushes.Black, iTableLeft + 5, iTop );
            else if (strMaterial.Length >= 23)
            {
                string s1 = strMaterial.Substring(0, 23);
                string s2 = strMaterial.Substring(23);
                e.Graphics.DrawString(s1, fnt10Bold, Brushes.Black, iTableLeft + 5, iTop - 10);
                e.Graphics.DrawString(s2, fnt10Bold, Brushes.Black, iTableLeft + 5, iTop +10);
            }

            e.Graphics.DrawString(strWidth + "±1", fnt10Bold, Brushes.Black, iTabCol2 + 30, iTop);
            e.Graphics.DrawString(strEtiketkaWidth, fnt10Bold, Brushes.Black, iTabCol3 + 35, iTop);


            iTop += 50;
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop, iTableRight, iTop);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop, iTableLeft, iTop + 30);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableRight, iTop, iTableRight, iTop + 30);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTabCol2, iTop, iTabCol2, iTop + 30);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop + 30, iTableRight, iTop + 30);

            e.Graphics.DrawString("Найменування показника", fnt10Bold, Brushes.Black, iTableLeft + 80, iTop + 7);
            e.Graphics.DrawString("Результат вимірювань", fnt10Bold, Brushes.Black, iTabCol2 + 30, iTop + 7);

            iTop += 50;
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop, iTableRight, iTop);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop+20, iTableRight, iTop+20);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop+50, iTableRight, iTop+50);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop-20, iTableLeft, iTop + 50);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableRight, iTop-20, iTableRight, iTop + 50);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTabCol2, iTop-20, iTabCol2, iTop + 50);
            
            e.Graphics.DrawString("Зчитування штрих-коду", fnt10, Brushes.Black, iTableLeft + 82, iTop -18);
            e.Graphics.DrawString("Відповідає", fnt10Bold, Brushes.Black, iTabCol2 + 70, iTop - 18);

            iTop += 20;
            e.Graphics.DrawString("Коефіцієнт тертя", fnt10, Brushes.Black, iTableLeft + 107, iTop - 18);

            iTop += 50;
            e.Graphics.DrawString("Продукція відповідає ТУ У 22.1-16476839-001-2004", fnt10Bold, Brushes.Black, iTableLeft, iTop);

            iTop += 50;
            e.Graphics.DrawString("Зам. директора по виробництву                   ______________________", fnt10, Brushes.Black, iTableLeft+50, iTop);

            iTop += 30;
            e.Graphics.DrawString("Інженер з якості                               ______________________", fnt10, Brushes.Black, iTableLeft + 100, iTop);

            iTop += 30;
            e.Graphics.DrawString("______________________", fnt10, Brushes.Black, iTableLeft + 305, iTop);



            iTop += 70;
            e.Graphics.DrawString("Термін зберігання флексоетикетки - 12 місяців з дати виготвлення. Продукцію зберігати в", fnt10, Brushes.Black, iTableLeft, iTop);
            iTop += 18;
            e.Graphics.DrawString("закритих, чистих, сухих, добре провітрюваних приміщеннях, що не містять сторонніх", fnt10, Brushes.Black, iTableLeft +10, iTop);
            iTop += 18;
            e.Graphics.DrawString("запахів, при темперетутрі від +10 до +20 С та відносній вологості повітря від 50% до 80%.", fnt10, Brushes.Black, iTableLeft , iTop);
            iTop += 18;
            e.Graphics.DrawString("При виявленні браку просимо передати зразки продукції з ярликами бобін з бобін,", fnt10, Brushes.Black, iTableLeft + 15, iTop);
            iTop += 18;
            e.Graphics.DrawString("а також з копією цього Сертифікату на ТОВ \"ІТАК\".", fnt10, Brushes.Black, iTableLeft + 100, iTop);
            iTop += 18;
            e.Graphics.DrawString("Тел./факс: +38 (044) 292-90-01, 573-33-52 - приймальня", fnt10, Brushes.Black, iTableLeft + 85, iTop);

             x_left = x2_left;
             x_right = x2_right;
             iTop = y1 + 15;


            strText = "ТОВ \"ІТАК\"";
            xx = ((x_right - x_left) / 2) - ((strText.Length * 5))+x_left;
            e.Graphics.DrawString(strText, fnt12Bold, Brushes.Black, xx, iTop);

            iTop += 20;
            strText = "м.Київ, вул. Червоноткацька, 44";
            xx = ((x_right - x_left) / 2) - ((strText.Length * 4)) + x_left;
            e.Graphics.DrawString(strText, fnt12Bold, Brushes.Black, xx, iTop);

            iTop += 60;
            strText = "СЕРТИФІКАТ ЯКОСТІ №" + strPartiya;
            xx = ((x_right - x_left) / 2) - ((strText.Length * 6)) + x_left;
            e.Graphics.DrawString(strText, fnt14Bold, Brushes.Black, xx, iTop);

            iTop += 40;
            e.Graphics.DrawString("Номер", fnt10, Brushes.Black, x_left + 55, iTop);
            e.Graphics.DrawString("замовлення", fnt10, Brushes.Black, x_left + 40, iTop + 10);
            e.Graphics.FillRectangle(brushGray, x_left + 130, iTop, 150, 25);
            e.Graphics.DrawString(strPartiya, fnt12Bold, Brushes.Black, x_left + 135, iTop + 2);

            iTop += 35;
            e.Graphics.DrawString("Замовник", fnt10, Brushes.Black, x_left + 45, iTop + 3);
            e.Graphics.FillRectangle(brushGray, x_left + 130, iTop - 5, x_right - x_left - 140, 25);
            e.Graphics.DrawString(strZakazchik, fnt12Bold, Brushes.Black, x_left + 135, iTop - 2);

            iTop += 25;
            e.Graphics.DrawString("Найменування", fnt10, Brushes.Black, x_left + 30, iTop);
            e.Graphics.DrawString("продукції", fnt10, Brushes.Black, x_left + 45, iTop + 10);

            e.Graphics.FillRectangle(brushGray, x_left + 130, iTop, x_right - x_left - 140, 25);
            e.Graphics.DrawString(strProduct, fnt10Bold, Brushes.Black, x_left + 135, iTop + 5);

            iTop += 30;
            e.Graphics.DrawString("Дата", fnt10, Brushes.Black, x_left + 58, iTop);
            e.Graphics.DrawString("виготовлення", fnt10, Brushes.Black, x_left + 33, iTop + 10);

            e.Graphics.FillRectangle(brushGray, x_left + 130, iTop, 140, 25);
            e.Graphics.DrawString(strDate, fnt12Bold, Brushes.Black, x_left + 135, iTop + 2);

            iTableLeft = x_left + 15;
            iTableRight = x_right-10;

            iTabCol1 = iTableLeft + 220;
            iTabCol2 = iTabCol1 + 97;
            iTabCol3 = iTabCol2 + 106;
            
            iTop += 50;
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop, iTableRight, iTop);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop, iTableLeft, iTop + 30);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableRight, iTop, iTableRight, iTop + 30);
            e.Graphics.DrawString("Найменування сировини", fnt10Bold, Brushes.Black, iTableLeft + 35, iTop + 7);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTabCol1, iTop, iTabCol1, iTop + 30);

            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTabCol2, iTop, iTabCol2, iTop + 30);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTabCol3, iTop, iTabCol3, iTop + 30);

            e.Graphics.DrawString("Маса, г/м2", fnt10Bold, Brushes.Black, iTabCol1 + 13, iTop + 7);
            e.Graphics.DrawString("Ширина бобіни,", fnt10Bold, Brushes.Black, iTabCol2 + 1, iTop);
            e.Graphics.DrawString("мм", fnt10Bold, Brushes.Black, iTabCol2 + 40, iTop + 12);

            e.Graphics.DrawString("Крок малюнку,", fnt10Bold, Brushes.Black, iTabCol3 + 1, iTop);
            e.Graphics.DrawString("мм", fnt10Bold, Brushes.Black, iTabCol3 + 40, iTop + 12);

            iTop += 30;
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop, iTableRight, iTop);

            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop, iTableLeft, iTop + 50);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableRight, iTop, iTableRight, iTop + 50);

            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTabCol1, iTop, iTabCol1, iTop + 50);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTabCol2, iTop, iTabCol2, iTop + 50);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTabCol3, iTop, iTabCol3, iTop + 50);

            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop + 50, iTableRight, iTop + 50);

            iTop += 15;
            if (strMaterial.Length <= 23)
                e.Graphics.DrawString(strMaterial, fnt10Bold, Brushes.Black, iTableLeft + 5, iTop);
            else if (strMaterial.Length >= 23)
            {
                string s1 = strMaterial.Substring(0, 23);
                string s2 = strMaterial.Substring(23);
                e.Graphics.DrawString(s1, fnt10Bold, Brushes.Black, iTableLeft + 5, iTop - 10);
                e.Graphics.DrawString(s2, fnt10Bold, Brushes.Black, iTableLeft + 5, iTop + 10);
            }

            e.Graphics.DrawString(strWidth + "±1", fnt10Bold, Brushes.Black, iTabCol2 + 30, iTop);
            e.Graphics.DrawString(strEtiketkaWidth, fnt10Bold, Brushes.Black, iTabCol3 + 35, iTop);


            iTop += 50;
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop, iTableRight, iTop);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop, iTableLeft, iTop + 30);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableRight, iTop, iTableRight, iTop + 30);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTabCol2, iTop, iTabCol2, iTop + 30);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop + 30, iTableRight, iTop + 30);

            e.Graphics.DrawString("Найменування показника", fnt10Bold, Brushes.Black, iTableLeft + 80, iTop + 7);
            e.Graphics.DrawString("Результат вимірювань", fnt10Bold, Brushes.Black, iTabCol2 + 30, iTop + 7);

            iTop += 50;
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop, iTableRight, iTop);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop + 20, iTableRight, iTop + 20);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop + 50, iTableRight, iTop + 50);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableLeft, iTop - 20, iTableLeft, iTop + 50);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTableRight, iTop - 20, iTableRight, iTop + 50);
            e.Graphics.DrawLine(new Pen(Color.Black, 1), iTabCol2, iTop - 20, iTabCol2, iTop + 50);

            e.Graphics.DrawString("Зчитування штрих-коду", fnt10, Brushes.Black, iTableLeft + 82, iTop - 18);
            e.Graphics.DrawString("Відповідає", fnt10Bold, Brushes.Black, iTabCol2 + 70, iTop - 18);

            iTop += 20;
            e.Graphics.DrawString("Коефіцієнт тертя", fnt10, Brushes.Black, iTableLeft + 107, iTop - 18);

            iTop += 50;
            e.Graphics.DrawString("Продукція відповідає ТУ У 22.1-16476839-001-2004", fnt10Bold, Brushes.Black, iTableLeft, iTop);

            iTop += 50;
            e.Graphics.DrawString("Зам. директора по виробництву                   ______________________", fnt10, Brushes.Black, iTableLeft + 50, iTop);

            iTop += 30;
            e.Graphics.DrawString("Інженер з якості                               ______________________", fnt10, Brushes.Black, iTableLeft + 100, iTop);

            iTop += 30;
            e.Graphics.DrawString("______________________", fnt10, Brushes.Black, iTableLeft + 305, iTop);



            iTop += 70;
            e.Graphics.DrawString("Термін зберігання флексоетикетки - 12 місяців з дати виготвлення. Продукцію зберігати в", fnt10, Brushes.Black, iTableLeft, iTop);
            iTop += 18;
            e.Graphics.DrawString("закритих, чистих, сухих, добре провітрюваних приміщеннях, що не містять сторонніх", fnt10, Brushes.Black, iTableLeft + 10, iTop);
            iTop += 18;
            e.Graphics.DrawString("запахів, при темперетутрі від +10 до +20 С та відносній вологості повітря від 50% до 80%.", fnt10, Brushes.Black, iTableLeft, iTop);
            iTop += 18;
            e.Graphics.DrawString("При виявленні браку просимо передати зразки продукції з ярликами бобін з бобін,", fnt10, Brushes.Black, iTableLeft + 15, iTop);
            iTop += 18;
            e.Graphics.DrawString("а також з копією цього Сертифікату на ТОВ \"ІТАК\".", fnt10, Brushes.Black, iTableLeft + 100, iTop);
            iTop += 18;
            e.Graphics.DrawString("Тел./факс: +38 (044) 292-90-01, 573-33-52 - приймальня", fnt10, Brushes.Black, iTableLeft + 85, iTop);

           
        }

        private void checkBox_otgryjen_CheckedChanged(object sender, EventArgs e)
        {
           /* if (checkBox_otgryjen.Checked == true)
            {
                checkBox_all.Checked = false;
                checkBox_Ostatok.Checked = false;
            }*/
        }

        private void checkBox_neOtgryjen_CheckedChanged(object sender, EventArgs e)
        {
            /*if (checkBox_Ostatok.Checked == true)
            {
                checkBox_all.Checked = false;
                checkBox_otgryjen.Checked = false;
                //CreateColumnNeOtgryj();
            }*/
        }

        private void checkBox_all_CheckedChanged(object sender, EventArgs e)
        {
            /*if (checkBox_all.Checked == true)
            {
                checkBox_otgryjen.Checked = false;
                checkBox_Ostatok.Checked = false;
            }*/
        }

        private void dataGridView_result_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (dataGridViewSklad_AddProduct.Rows.Count >= 1)
            {
                dataGridViewSklad_AddProduct.Rows[e.RowIndex].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewSklad_AddProduct.Rows[e.RowIndex].Cells[1].Style.Padding = new Padding(3, 0, 0, 0);
                dataGridViewSklad_AddProduct.Rows[e.RowIndex].Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewSklad_AddProduct.Rows[e.RowIndex].Cells[2].Style.Padding = new Padding(3, 0, 0, 0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //RefreshTable();
            RefreshAddSklad(textBoxSkladAdd_NumberTZ, comboBoxSkladAdd_Zakazchik, dateTimePickerAddSklad_Start, dateTimePickerAddSklad_End,0, comboBoxSkladAdd_Manager);
        }

        private void RefreshAddSklad(TextBox textNumberTZ, ComboBox comboZakazchik, DateTimePicker dateStart, DateTimePicker dateEnd, int iTypeFunc, ComboBox comboManager)    //0-AddSklad    1-OtgryzSklad
        {
            string strPartiya = "";
            string strZakazchik = "";
            string strDateStart = "";
            string strDateEnd = "";
            string strManager = ""; 

            if (textNumberTZ.Text.Length != 0)
                strPartiya = textNumberTZ.Text;

            if (comboZakazchik.SelectedIndex != -1)
                strZakazchik = comboZakazchik.Items[comboZakazchik.SelectedIndex].ToString();

            if (comboManager.SelectedIndex != -1)
                strManager = comboManager.Items[comboManager.SelectedIndex].ToString();


            strDateStart = dateStart.Value.Year + "-" + dateStart.Value.Month + "-" + dateStart.Value.Day;
            strDateEnd = dateEnd.Value.Year + "-" + dateEnd.Value.Month + "-" + (dateEnd.Value.Day);
            int iDay = dateEnd.Value.Day;
            int iMounth = dateEnd.Value.Month;
            int iYear = dateEnd.Value.Year;
            int iMaxDays = maxDays(dateEnd.Value.Year, dateEnd.Value.Month);

            if (iDay == iMaxDays + 1)
            {
                if (iMaxDays == 31 || iMaxDays == 30 || iMaxDays == 29 || iMaxDays == 28)
                {
                    iDay = 1;
                    if (dateEnd.Value.Month != 12)
                        iMounth = dateEnd.Value.Month + 1;
                    else
                    {
                        iMounth = 1;
                        iYear++;
                    }
                }
            }
            strDateEnd = iYear + "-" + iMounth + "-" + iDay;
            if (iTypeFunc==0)
                SelectAddSkladProduct(strZakazchik, strPartiya, strDateStart, strDateEnd, ref dataGridViewSklad_AddProduct,0,
                                    ref m_lsAddSkladZakazchik, ref m_lsAddSkladProductInfo, strManager);
            else if (iTypeFunc==1)
                SelectAddSkladProduct(strZakazchik, strPartiya, strDateStart, strDateEnd, ref dataGridViewSklad_OtgryzProduct, 1,
                                    ref m_lsOtgryzSkladZakazchik, ref m_lsOtgryzSkladProductInfo, strManager);
            else if (iTypeFunc==2)
                SelectAddSkladProduct(strZakazchik, strPartiya, strDateStart, strDateEnd, ref dataGridViewSklad_OstatkiProduct, 2,
                                    ref m_lsOstatkiSkladZakazchik, ref m_lsOstatkiSkladProductInfo, strManager);
            
        }


        private void RefreshTable()
        {
            string strPartiya = "";
            string strZakazchik = "";
            int iCheckBoxState = -1;
            //int iYear = 0;
            string strDateStart = "";
            string strDateEnd = "";


            if (textBoxSkladAdd_NumberTZ.Text.Length != 0)
                strPartiya = textBoxSkladAdd_NumberTZ.Text;

            if (comboBoxSkladAdd_Zakazchik.SelectedIndex != -1)
                strZakazchik = comboBoxSkladAdd_Zakazchik.Items[comboBoxSkladAdd_Zakazchik.SelectedIndex].ToString();

            /*if (checkBox_all.Checked == true)
                iCheckBoxState = 0;//vse
            else if (checkBox_Ostatok.Checked == true)
                iCheckBoxState = 1;//prinat
            else if (checkBox_otgryjen.Checked == true)
                iCheckBoxState = 2;//otgryjen*/


            strDateStart = dateTimePickerAddSklad_Start.Value.Year + "-" + dateTimePickerAddSklad_Start.Value.Month + "-" + dateTimePickerAddSklad_Start.Value.Day;
            strDateEnd = dateTimePickerAddSklad_End.Value.Year + "-" + dateTimePickerAddSklad_End.Value.Month + "-" + (dateTimePickerAddSklad_End.Value.Day);
            int iDay = dateTimePickerAddSklad_End.Value.Day;
            int iMounth = dateTimePickerAddSklad_End.Value.Month;
            int iYear = dateTimePickerAddSklad_End.Value.Year;
            int iMaxDays = maxDays(dateTimePickerAddSklad_End.Value.Year, dateTimePickerAddSklad_End.Value.Month);

            if (iDay == iMaxDays + 1)
            {
                if (iMaxDays == 31 || iMaxDays == 30 || iMaxDays == 29 || iMaxDays == 28)
                {
                    iDay = 1;
                    if (dateTimePickerAddSklad_End.Value.Month != 12)
                        iMounth = dateTimePickerAddSklad_End.Value.Month + 1;
                    else
                    {
                        iMounth = 1;
                        iYear++;
                    }
                }
            }
            strDateEnd = iYear + "-" + iMounth + "-" + iDay;

            SelectSearchProduct(1, strPartiya, strZakazchik, iCheckBoxState, strDateStart, strDateEnd);
        }


        private void checkBoxEnableDate_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAddSklad_EnableDate.Checked == true && groupBoxAddSklad_Date.Enabled == false)
                groupBoxAddSklad_Date.Enabled = true;
            else if (checkBoxAddSklad_EnableDate.Checked == false && groupBoxAddSklad_Date.Enabled == true)
                groupBoxAddSklad_Date.Enabled = false;
        }

        private void textBox_numberTZ_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                RefreshAddSklad(textBoxSkladAdd_NumberTZ, comboBoxSkladAdd_Zakazchik, dateTimePickerAddSklad_Start, dateTimePickerAddSklad_End,0,comboBoxSkladAdd_Manager);
        }


        private void SkladDataGridClick(DataGridView dataView, List<Zakazchik> lsZakazchik, TextBox textNumberTZ, DateTimePicker dateStart, DateTimePicker dateEnd, int iTypeFunc)//0-AddSklad  1-OtgryzSklad
        {
            int iSelectedZakaz = -1;
            if (dataView.CurrentCell.RowIndex != -1)
            {
                for (int i = 0; i < dataView.Rows.Count; i++)
                    if (dataView.Rows[i].Selected == true)
                        iSelectedZakaz = i;
            }

            if (iSelectedZakaz != -1)
            {
                for (int i = 0; i < lsZakazchik.Count; i++)
                {
                    if (lsZakazchik[i].strZakazchikName == dataView.Rows[iSelectedZakaz].Cells["Zakazchik"].Value.ToString()
                        && lsZakazchik[i].strProductName == dataView.Rows[iSelectedZakaz].Cells["Product"].Value.ToString()
                        && lsZakazchik[i].strPartiya == dataView.Rows[iSelectedZakaz].Cells["Partiya"].Value.ToString())
                    {

                        string strDateStart = dateStart.Value.Year + "-" + dateStart.Value.Month + "-" + dateStart.Value.Day;
                        string strDateEnd = dateEnd.Value.Year + "-" + dateEnd.Value.Month + "-" + (dateEnd.Value.Day);
                        string strYear = "";


                        if (textNumberTZ.Text.Length == 0)
                        {
                            if (iTypeFunc == 0)
                                strYear = " and (convert(date,date_postyp,101) >= '" + strDateStart + "' and convert(date,date_postyp,101)<='" + strDateEnd + "') ";
                            else if (iTypeFunc == 1)
                                strYear = " and (convert(date,date_otgryz,101) >= '" + strDateStart + "' and convert(date,date_otgryz,101)<='" + strDateEnd + "') ";
                        }

                        if (iTypeFunc==2)
                            strMSSQLQuery = "select Count(vh.id), ROUND(SUM(vh.brytto),3), ROUND(SUM(vh.brytto-vh.vagatary),3), SUM(vh.dlinarylona), SUM(vh.koletiketki) " +
                                            "from itak_etiketka.dbo.itak_sklad s, itak_etiketka.dbo.itak_vihidrylon vh, itak_etiketka.dbo.itak_zakaz z " +
                                            "where vh.id=s.rylon_id and vh.zakaz_id=z.id and vh.product_id=" + lsZakazchik[i].iProductId + " and z.zakazchik_id=" + lsZakazchik[i].iZakazchikId + "  and z.partiya='" + lsZakazchik[i].strPartiya + "'";
                        else
                            strMSSQLQuery = "select Count(vh.id), ROUND(SUM(vh.brytto),3), ROUND(SUM(vh.brytto-vh.vagatary),3), SUM(vh.dlinarylona), SUM(vh.koletiketki) " +
                                            "from itak_etiketka.dbo.itak_sklad s, itak_etiketka.dbo.itak_vihidrylon vh, itak_etiketka.dbo.itak_zakaz z " +
                                            "where vh.id=s.rylon_id and vh.zakaz_id=z.id and vh.product_id=" + lsZakazchik[i].iProductId + " and z.zakazchik_id=" + lsZakazchik[i].iZakazchikId + "  and z.partiya='" + lsZakazchik[i].strPartiya + "'" + strYear;

                        m_MSSQLCommand.CommandText = strMSSQLQuery;
                        m_MSSQLReader = m_MSSQLCommand.ExecuteReader();

                        if (m_MSSQLReader.HasRows)
                        {
                            while (m_MSSQLReader.Read())
                            {
                                try
                                {
                                    if (iTypeFunc == 0)
                                    {
                                        if (DBNull.Value != m_MSSQLReader[1])
                                            labelAddSklad_ZnachBrytto.Text = m_MSSQLReader[1].ToString() + " кг";
                                        else labelAddSklad_ZnachBrytto.Text = "0 кг";

                                        if (DBNull.Value != m_MSSQLReader[2])
                                            labelAddSklad_ZnachNetto.Text = m_MSSQLReader[2].ToString() + " кг";
                                        else labelAddSklad_ZnachNetto.Text = "0 кг";

                                        if (DBNull.Value != m_MSSQLReader[0])
                                            labelAddSklad_ZnachCountRylon.Text = m_MSSQLReader[0].ToString() + " шт";
                                        else labelAddSklad_ZnachCountRylon.Text = "0 шт";

                                        if (DBNull.Value != m_MSSQLReader[4])
                                            labelAddSklad_ZnachCountEtik.Text = m_MSSQLReader[4].ToString() + " шт";
                                        else labelAddSklad_ZnachCountEtik.Text = "0 шт";

                                        if (DBNull.Value != m_MSSQLReader[3])
                                            labelAddSklad_ZnachMP.Text = m_MSSQLReader[3].ToString() + " шт";
                                        else labelAddSklad_ZnachMP.Text = "0 м";
                                    }
                                    else if (iTypeFunc==1)
                                    {
                                        if (DBNull.Value != m_MSSQLReader[1])
                                            labelOtgryzSklad_ZnachBrytto.Text = m_MSSQLReader[1].ToString() + " кг";
                                        else labelOtgryzSklad_ZnachBrytto.Text = "0 кг";

                                        if (DBNull.Value != m_MSSQLReader[2])
                                            labelOtgryzSklad_ZnachNetto.Text = m_MSSQLReader[2].ToString() + " кг";
                                        else labelOtgryzSklad_ZnachNetto.Text = "0 кг";

                                        if (DBNull.Value != m_MSSQLReader[0])
                                            labelOtgryzSklad_ZnachCountRylon.Text = m_MSSQLReader[0].ToString() + " шт";
                                        else labelOtgryzSklad_ZnachCountRylon.Text = "0 шт";

                                        if (DBNull.Value != m_MSSQLReader[4])
                                            labelOtgryzSklad_ZnachCountEtik.Text = m_MSSQLReader[4].ToString() + " шт";
                                        else labelOtgryzSklad_ZnachCountEtik.Text = "0 шт";

                                        if (DBNull.Value != m_MSSQLReader[3])
                                            labelOtgryzSklad_ZnachMP.Text = m_MSSQLReader[3].ToString() + " шт";
                                        else labelOtgryzSklad_ZnachMP.Text = "0 м";
                                    }
                                    else if (iTypeFunc==2)
                                    {
                                        if (DBNull.Value != m_MSSQLReader[1])
                                            labelOstatkiSklad_ZnachBrytto.Text = m_MSSQLReader[1].ToString() + " кг";
                                        else labelOstatkiSklad_ZnachBrytto.Text = "0 кг";

                                        if (DBNull.Value != m_MSSQLReader[2])
                                            labelOstatkiSklad_ZnachNetto.Text = m_MSSQLReader[2].ToString() + " кг";
                                        else labelOstatkiSklad_ZnachNetto.Text = "0 кг";

                                        if (DBNull.Value != m_MSSQLReader[0])
                                            labelOstatkiSklad_ZnachCountRylon.Text = m_MSSQLReader[0].ToString() + " шт";
                                        else labelOstatkiSklad_ZnachCountRylon.Text = "0 шт";

                                        if (DBNull.Value != m_MSSQLReader[4])
                                            labelOstatkiSklad_ZnachCountEtik.Text = m_MSSQLReader[4].ToString() + " шт";
                                        else labelOstatkiSklad_ZnachCountEtik.Text = "0 шт";

                                        if (DBNull.Value != m_MSSQLReader[3])
                                            labelOstatkiSklad_ZnachMP.Text = m_MSSQLReader[3].ToString() + " шт";
                                        else labelOstatkiSklad_ZnachMP.Text = "0 м";
                                    }

                                }
                                catch (System.Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                    m_MSSQLReader.Close();
                                }
                            }

                        }
                        else
                        {
                            //label_nettoNaSklade.Text = "0 кг";
                            //label_bryttoNaSklade.Text = "0 кг";
                        }

                        m_MSSQLReader.Close();

                        break;
                    }
                }
            }
        }

        private void dataGridViewSklad_result_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SkladDataGridClick(dataGridViewSklad_AddProduct, m_lsAddSkladZakazchik, textBoxSkladAdd_NumberTZ, dateTimePickerAddSklad_Start, dateTimePickerAddSklad_End,0);
        }


        private void dataGridView_product_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && (m_iTypeRegim == 2|| m_iTypeRegim==3))
            {
                if (e.RowIndex != -1)
                {
                    dataGridView_product.Rows[e.RowIndex].Selected = true;
                    DataGridProductClick();
                }

                if (dataGridView_product.Rows.Count != 0)
                    if (e.RowIndex != -1)
                    {
                        int iListCurSel = e.RowIndex;
                        string strKey = dataGridView_product.Rows[iListCurSel].Cells["Product"].Value.ToString() +
                            "-" + dataGridView_product.Rows[iListCurSel].Cells["Width"].Value.ToString();
                        m_iProductID = m_dicProduct[strKey];
                        
                        change_Product_rezka changeProductForm = new change_Product_rezka(m_iZakazID, m_iProductID, m_iWidth);
                        changeProductForm.m_MSSQLConnection = m_MSSQLConnection;

                        changeProductForm.ShowDialog();
                        if (changeProductForm.GetDialogResult() == 1)
                            SelectProduct(m_iZakazID);
                        changeProductForm = null;
                    }
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            label_square.Text = "0,0 м.кв";

            if (dataGridView_rezkaZakaz.Rows.Count != -1 && e.RowIndex != -1 && button_rezkaPrintZakaz.Enabled == false)
                button_rezkaPrintZakaz.Enabled = true;

            if (e.Button == MouseButtons.Right && m_iTypeRegim == 2)
            {
                if (dataGridView_rezkaZakaz.Rows.Count!=-1)
                {
                    if (e.RowIndex!=-1)
                    {
                        dataGridView_rezkaZakaz.Rows[e.RowIndex].Selected = true;
                        string strListInfo = dataGridView_rezkaZakaz.Rows[e.RowIndex].Cells[1].Value.ToString() +
                                     dataGridView_rezkaZakaz.Rows[e.RowIndex].Cells[2].Value.ToString() +
                                     dataGridView_rezkaZakaz.Rows[e.RowIndex].Cells[3].Value.ToString() +
                                     dataGridView_rezkaZakaz.Rows[e.RowIndex].Cells[4].Value.ToString();
                        int iZakazID = m_dicZakaz[strListInfo];


                        change_Zakaz_rezka changeZakazForm = new change_Zakaz_rezka(iZakazID, m_iTypeRegim);
                        changeZakazForm.m_MSSQLConnection = m_MSSQLConnection;

                        changeZakazForm.ShowDialog();
                        if (changeZakazForm.GetDialogResult()==1)
                        {
                            RefreshBut();
                        }

                    }
                }
            }
            else
            {
                if (e.Button == MouseButtons.Right && m_iTypeRegim == 3)
                {
                    dataGridView_rezkaZakaz.Rows[e.RowIndex].Selected = true;
                    m_delChangeForm = new delChange_form();
                    m_delChangeForm.ShowDialog();

                    if (m_delChangeForm.GetDialogResult() == 1) //change
                    {
                        if (dataGridView_rezkaZakaz.Rows.Count != -1)
                        {
                            if (e.RowIndex != -1)
                            {
                                dataGridView_rezkaZakaz.Rows[e.RowIndex].Selected = true;
                                string strListInfo = dataGridView_rezkaZakaz.Rows[e.RowIndex].Cells[1].Value.ToString() +
                                             dataGridView_rezkaZakaz.Rows[e.RowIndex].Cells[2].Value.ToString() +
                                             dataGridView_rezkaZakaz.Rows[e.RowIndex].Cells[3].Value.ToString() +
                                             dataGridView_rezkaZakaz.Rows[e.RowIndex].Cells[4].Value.ToString();
                                int iZakazID = m_dicZakaz[strListInfo];


                                change_Zakaz_rezka changeZakazForm = new change_Zakaz_rezka(iZakazID, m_iTypeRegim);
                                changeZakazForm.m_MSSQLConnection = m_MSSQLConnection;

                                changeZakazForm.ShowDialog();
                                if (changeZakazForm.GetDialogResult() == 1)
                                {
                                    RefreshBut();
                                }

                            }
                        }
                    }
                    else if (m_delChangeForm.GetDialogResult() == 2)    //del
                    {
                        if (MessageBox.Show(this, "Вы действительно хотите удалить заказ партия №" + dataGridView_rezkaZakaz.Rows[e.RowIndex].Cells[2].Value.ToString().Trim() + "??", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            if (CheckConnect())
                            {
                                dataGridView_rezkaZakaz.Rows[e.RowIndex].Selected = true;
                                string strListInfo = dataGridView_rezkaZakaz.Rows[e.RowIndex].Cells[1].Value.ToString() +
                                             dataGridView_rezkaZakaz.Rows[e.RowIndex].Cells[2].Value.ToString() +
                                             dataGridView_rezkaZakaz.Rows[e.RowIndex].Cells[3].Value.ToString() +
                                             dataGridView_rezkaZakaz.Rows[e.RowIndex].Cells[4].Value.ToString();
                                int iZakazID = m_dicZakaz[strListInfo];
                               // MessageBox.Show(iZakazID.ToString());

                                strMSSQLQuery = "update itak_etiketka.dbo.itak_zakaz set state=0 where id=" + iZakazID;

                                m_MSSQLCommand.CommandText = strMSSQLQuery;
                                m_MSSQLCommand.ExecuteNonQuery();
                                RefreshBut();
                            }
                        }
                    }

                }
            }
        }

        private void PrintSkladTables(DataGridView dataView, int iTypeFunc)
        {
            if (dataView.Rows.Count != 0)
            {
                PrintDocument docPrint = new PrintDocument();
                if (docPrint.PrinterSettings.IsValid)
                {
                    docPrint.DefaultPageSettings.Margins.Top = 10;
                    docPrint.DefaultPageSettings.Margins.Bottom = 10;
                    docPrint.DefaultPageSettings.Margins.Right = 0;
                    docPrint.DefaultPageSettings.Margins.Left = 0;

                    docPrint.DefaultPageSettings.Landscape = false;
                    docPrint.PrintPage += (sender, e) => docPrint_PrintPageAddSkladProduct(e, dataView, iTypeFunc);
                        //new PrintPageEventHandler(this.docPrint_PrintPageAddSkladProduct);

                    docPrint.PrinterSettings.Copies = 1;

                    PrintPreviewDialog dlgPrint = new PrintPreviewDialog();
                    dlgPrint.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height);
                    dlgPrint.Document = docPrint;
                    //dlgPrint.ShowDialog();
                    docPrint.Print();
                }
                else
                    MessageBox.Show("Принтер не доступен!");
            }
        }


        private void button_AddSkladPrint_Click(object sender, EventArgs e)
        {
            PrintSkladTables(dataGridViewSklad_AddProduct,0);   
        }

        private void PrintAddSklad(PrintPageEventArgs e,int iCounter, int iCountRows, int iLeft, int iTableWidth, int t1, DataGridView dataView)
        {
            System.Drawing.Font fnt7 = new Font("Times New Roman", 7, FontStyle.Regular);
            System.Drawing.Font fnt8 = new Font("Times New Roman", 8, FontStyle.Regular);
            System.Drawing.Font fnt10Bold = new Font("Times New Roman", 10, FontStyle.Bold);
            System.Drawing.Font fnt10 = new Font("Times New Roman", 10, FontStyle.Regular);


            int[] iCoord = { iLeft, iLeft + 20, iLeft + 170, iLeft + 400, iLeft + 460, iLeft + 525, iLeft + 600, iLeft + 680, iLeft + 760 };
            int i = 0;

            e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t1), new Point(iTableWidth, t1));
            e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t1 + 35), new Point(iTableWidth, t1 + 35));
            for (int j = 0; j <= 8; j++)
                e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iCoord[j], t1 - 1), new Point(iCoord[j], t1 + 36));

            e.Graphics.DrawString("№", fnt10Bold, Brushes.Black, iLeft + 2, t1 + 9);

            e.Graphics.DrawString("Заказчик", fnt10Bold, Brushes.Black, iLeft + 65, t1 + 9);

            e.Graphics.DrawString("Продукция", fnt10Bold, Brushes.Black, iLeft + 245, t1 + 9);

            e.Graphics.DrawString("Партия", fnt10Bold, Brushes.Black, iLeft + 405, t1 + 9);


            e.Graphics.DrawString("К-во", fnt10Bold, Brushes.Black, iLeft + 475, t1 + 2);
            e.Graphics.DrawString("рулонов", fnt10Bold, Brushes.Black, iLeft + 465, t1 + 15);

            e.Graphics.DrawString("Нетто, кг", fnt10Bold, Brushes.Black, iLeft + 530, t1 + 9);

            e.Graphics.DrawString("Брутто, кг", fnt10Bold, Brushes.Black, iLeft + 605, t1 + 9);


            e.Graphics.DrawString("К-во", fnt10Bold, Brushes.Black, iLeft + 700, t1 + 2);
            e.Graphics.DrawString("этикетки", fnt10Bold, Brushes.Black, iLeft + 690, t1 + 15);

            t1 += 35;

            for (i = iCounter; i < iCountRows; i++)
            {
                for (int j = 0; j <= 8; j++)
                    if (j == 0 || j == 8)
                        e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iCoord[j], t1 - 1), new Point(iCoord[j], t1 + 15));
                    else
                        e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[j], t1 - 1), new Point(iCoord[j], t1 + 15));

                string strTemp1 = "";
                if (dataView.Rows[i].Cells["Number"].Value != null)
                    strTemp1 = dataView.Rows[i].Cells["Number"].Value.ToString();//надо

                if (Convert.ToInt32(strTemp1) > 9)
                    e.Graphics.DrawString(strTemp1, fnt7, Brushes.Black, iCoord[0] + 2, t1);
                else
                    e.Graphics.DrawString(strTemp1, fnt7, Brushes.Black, iCoord[0] + 5, t1);

                if (dataView.Rows[i].Cells["Zakazchik"].Value != null)
                    e.Graphics.DrawString(dataView.Rows[i].Cells["Zakazchik"].Value.ToString(), fnt7, Brushes.Black, iCoord[1] + 5, t1);

                if (dataView.Rows[i].Cells["Product"].Value != null)
                    e.Graphics.DrawString(dataView.Rows[i].Cells["Product"].Value.ToString(), fnt7, Brushes.Black, iCoord[2] + 5, t1);

                if (dataView.Rows[i].Cells["Partiya"].Value != null)
                    e.Graphics.DrawString(dataView.Rows[i].Cells["Partiya"].Value.ToString(), fnt8, Brushes.Black, iCoord[3] + 10, t1);

                if (dataView.Rows[i].Cells["CountRylon"].Value != null)
                    e.Graphics.DrawString(dataView.Rows[i].Cells["CountRylon"].Value.ToString(), fnt8, Brushes.Black, iCoord[4] + 15, t1);

                if (dataView.Rows[i].Cells["Netto"].Value != null)
                    e.Graphics.DrawString(dataView.Rows[i].Cells["Netto"].Value.ToString(), fnt8, Brushes.Black, iCoord[5] + 5, t1);
                if (dataView.Rows[i].Cells["Brytto"].Value != null)
                    e.Graphics.DrawString(dataView.Rows[i].Cells["Brytto"].Value.ToString(), fnt8, Brushes.Black, iCoord[6] + 5, t1);

                if (dataView.Rows[i].Cells["CountEtiketki"].Value != null)
                    e.Graphics.DrawString(dataView.Rows[i].Cells["CountEtiketki"].Value.ToString(), fnt8, Brushes.Black, iCoord[7] + 5, t1);


                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iLeft, t1 + 15), new Point(iTableWidth, t1 + 15));

                t1 += 16;
            }
            m_iAddSkladTop = t1;
            m_iAddSkladCounter = i;

        }

        private void docPrint_PrintPageAddSkladProduct( PrintPageEventArgs e, DataGridView dataView, int iTypeFunc)
        {
            System.Drawing.Font fnt10Bold = new Font("Times New Roman", 10, FontStyle.Bold);
            System.Drawing.Font fnt10 = new Font("Times New Roman", 10, FontStyle.Regular);

            System.Drawing.Font fnt8 = new Font("Times New Roman", 8, FontStyle.Regular);
            System.Drawing.Font fnt7 = new Font("Times New Roman", 7, FontStyle.Regular);

            System.Drawing.Font fnt12Bold = new Font("Times New Roman", 12, FontStyle.Bold);
            System.Drawing.Font fnt12 = new Font("Times New Roman", 12, FontStyle.Regular);
            System.Drawing.Font fnt12BoldUnder = new Font("Times New Roman", 12, FontStyle.Bold | FontStyle.Underline);
            System.Drawing.Font fnt14Bold = new Font("Times New Roman", 14, FontStyle.Bold);
            System.Drawing.Font fnt14 = new Font("Times New Roman", 14, FontStyle.Regular);
            System.Drawing.Font fnt14BoldUnder = new Font("Times New Roman", 14, FontStyle.Bold | FontStyle.Underline);
            System.Drawing.Font fnt16Bold = new Font("Times New Roman", 16, FontStyle.Bold);
            System.Drawing.Font fnt16BoldUnder = new Font("Times New Roman", 16, FontStyle.Bold | FontStyle.Underline);

            int t = 10;
            int iLeft = 20;

            int iTableWidth = iLeft + 760;
            int[] iCoord = { iLeft, iLeft + 20, iLeft + 170, iLeft + 400, iLeft + 460, iLeft + 525, iLeft + 600, iLeft + 680, iLeft + 760};

            m_iAddSkladCountRows = dataView.Rows.Count;
            int iCountRows = 0;

            if (m_iAddSkladPrintPage == m_iAddSkladI)
            {
                if (m_iAddSkladCountRows > m_iAddSkladRecord)
                {
                    int iCountPageRows = 0;
                   
                    if (m_iAddSkladCountRows > (m_iAddSkladRecord += 67))
                        iCountPageRows = m_iAddSkladRecord;
                    else iCountPageRows = m_iAddSkladCountRows;

                    iCountRows = iCountPageRows - m_iAddSkladCounter;

                    if (m_iAddSkladI == 0)
                        PrintAddSklad(e, 0, iCountPageRows, iLeft, iTableWidth, t,dataView);
                    else
                        PrintAddSklad(e, m_iAddSkladCounter, iCountPageRows, iLeft, iTableWidth, t, dataView);

                    if (m_iAddSkladCountRows <= m_iAddSkladRecord)
                    {
                        e.Graphics.DrawString("- " + (m_iAddSkladI + 1).ToString() + " -", fnt14Bold, Brushes.Black, 375, 1130);
                        e.HasMorePages = false;
                        m_iAddSkladPrintPage = 0;

                        if (iTypeFunc == 0)
                        {
                            e.Graphics.DrawString("Нетто, кг: \t " + labelAddSklad_ZnachNetto.Text, fnt10, Brushes.Black, iLeft, (m_iAddSkladTop += 18));
                            e.Graphics.DrawString("Брутто, кг: \t " + labelAddSklad_ZnachBrytto.Text, fnt10, Brushes.Black, iLeft, (m_iAddSkladTop += 18));
                            e.Graphics.DrawString("К-во м.п: \t " + labelAddSklad_ZnachMP.Text, fnt10, Brushes.Black, iLeft, (m_iAddSkladTop += 18));
                            e.Graphics.DrawString("К-во етикетки, шт: \t " + labelAddSklad_ZnachCountEtik.Text, fnt10, Brushes.Black, iLeft, (m_iAddSkladTop += 18));
                            e.Graphics.DrawString("К-во рулонов, шт: \t " + labelAddSklad_ZnachCountRylon.Text, fnt10, Brushes.Black, iLeft, (m_iAddSkladTop += 18));
                        }
                        else if (iTypeFunc==1)
                        {
                            e.Graphics.DrawString("Нетто, кг: \t " + labelOtgryzSklad_ZnachNetto.Text, fnt10, Brushes.Black, iLeft, (m_iAddSkladTop += 18));
                            e.Graphics.DrawString("Брутто, кг: \t " + labelOtgryzSklad_ZnachBrytto.Text, fnt10, Brushes.Black, iLeft, (m_iAddSkladTop += 18));
                            e.Graphics.DrawString("К-во м.п: \t " + labelOtgryzSklad_ZnachMP.Text, fnt10, Brushes.Black, iLeft, (m_iAddSkladTop += 18));
                            e.Graphics.DrawString("К-во етикетки, шт: \t " + labelOtgryzSklad_ZnachCountEtik.Text, fnt10, Brushes.Black, iLeft, (m_iAddSkladTop += 18));
                            e.Graphics.DrawString("К-во рулонов, шт: \t " + labelOtgryzSklad_ZnachCountRylon.Text, fnt10, Brushes.Black, iLeft, (m_iAddSkladTop += 18));                        
                        }
                        else if (iTypeFunc==2)
                        {
                            e.Graphics.DrawString("Нетто, кг: \t " + labelOstatkiSklad_ZnachNetto.Text, fnt10, Brushes.Black, iLeft, (m_iAddSkladTop += 18));
                            e.Graphics.DrawString("Брутто, кг: \t " + labelOstatkiSklad_ZnachBrytto.Text, fnt10, Brushes.Black, iLeft, (m_iAddSkladTop += 18));
                            e.Graphics.DrawString("К-во м.п: \t " + labelOstatkiSklad_ZnachMP.Text, fnt10, Brushes.Black, iLeft, (m_iAddSkladTop += 18));
                            e.Graphics.DrawString("К-во етикетки, шт: \t " + labelOstatkiSklad_ZnachCountEtik.Text, fnt10, Brushes.Black, iLeft, (m_iAddSkladTop += 18));
                            e.Graphics.DrawString("К-во рулонов, шт: \t " + labelOstatkiSklad_ZnachCountRylon.Text, fnt10, Brushes.Black, iLeft, (m_iAddSkladTop += 18));                        
                        }
                        
                        m_iAddSkladCountRows = m_iAddSkladPrintPage = m_iAddSkladCounter = m_iAddSkladRecord = m_iAddSkladI = 0;
                    }
                    else
                    {
                        e.Graphics.DrawString("- " + (m_iAddSkladI + 1).ToString() + " -", fnt14Bold, Brushes.Black, 375, 1130);
                        e.HasMorePages = true;
                        m_iAddSkladPrintPage = m_iAddSkladI + 1;
                        m_iAddSkladI++;
                    }

                }
            }
        }

        private void textBox_tz_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                ButtonSearchClick();
        }

        private void dataGridViewSklad_OtgryzProduct_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (dataGridViewSklad_OtgryzProduct.Rows.Count >= 1)
            {
                dataGridViewSklad_OtgryzProduct.Rows[e.RowIndex].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewSklad_OtgryzProduct.Rows[e.RowIndex].Cells[1].Style.Padding = new Padding(3, 0, 0, 0);
                dataGridViewSklad_OtgryzProduct.Rows[e.RowIndex].Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewSklad_OtgryzProduct.Rows[e.RowIndex].Cells[2].Style.Padding = new Padding(3, 0, 0, 0);
            }
        }

        private void checkBoxOtgryzSklad_EnableDate_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxOtgryzSklad_EnableDate.Checked == true && groupBoxOtgryzSklad_Date.Enabled == false)
                groupBoxOtgryzSklad_Date.Enabled = true;
            else if (checkBoxOtgryzSklad_EnableDate.Checked == false && groupBoxOtgryzSklad_Date.Enabled == true)
                groupBoxOtgryzSklad_Date.Enabled = false;
        }

        private void button_OtgryzSkladRefresh_Click(object sender, EventArgs e)
        {
            RefreshAddSklad(textBoxSkladOtgryz_NymberTZ, comboBoxSkladOtgryz_Zakazchik, dateTimePickerOtgryzSklad_Start, dateTimePickerOtgryzSklad_End, 1, comboBoxSkladOtgryz_Manager);
        }

        private void dataGridViewSklad_OtgryzProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SkladDataGridClick(dataGridViewSklad_OtgryzProduct, m_lsOtgryzSkladZakazchik, textBoxSkladOtgryz_NymberTZ, dateTimePickerOtgryzSklad_Start, dateTimePickerOtgryzSklad_End, 1);
        }

        private void button_OtgryzSkladPrint_Click(object sender, EventArgs e)
        {
            PrintSkladTables(dataGridViewSklad_OtgryzProduct,1);
        }

        private void dataGridViewSklad_OstatkiProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SkladDataGridClick(dataGridViewSklad_OstatkiProduct, m_lsOstatkiSkladZakazchik, textBoxSkladOtgryz_NymberTZ, dateTimePickerOtgryzSklad_Start, dateTimePickerOtgryzSklad_End, 2);
        }

        private void button_OstatkiSkladRefresh_Click(object sender, EventArgs e)
        {
            RefreshAddSklad(textBoxSkladOstatki_NymberTZ, comboBoxSkladOstatki_Zakazchik, dateTimePickerOtgryzSklad_Start, dateTimePickerOtgryzSklad_End, 2, comboBoxSkladOstatki_Manager);
        }

        private void button_PrintSkladRefresh_Click(object sender, EventArgs e)
        {
            PrintSkladTables(dataGridViewSklad_OstatkiProduct, 2);
        }

        private void tabControl_Sklad_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl_Sklad.SelectedIndex==1)
            {
                if (dataGridViewSklad_OtgryzProduct.ColumnCount == 0)
                {
                    string strDateStart = dateTimePickerOtgryzSklad_Start.Value.Year + "-" + dateTimePickerOtgryzSklad_Start.Value.Month + "-" + dateTimePickerOtgryzSklad_Start.Value.Day;
                    string strDateEnd = dateTimePickerOtgryzSklad_End.Value.Year + "-" + dateTimePickerOtgryzSklad_End.Value.Month + "-" + (dateTimePickerOtgryzSklad_End.Value.Day);
                    CreateColumnNeOtgryj(ref dataGridViewSklad_OtgryzProduct);
                    SelectAddSkladProduct("", "", strDateStart, strDateEnd, ref dataGridViewSklad_OtgryzProduct, 1, ref m_lsOtgryzSkladZakazchik, ref m_lsOtgryzSkladProductInfo,"");
                }
            }
            else if (tabControl_Sklad.SelectedIndex == 2)
            {
                
                if (dataGridViewSklad_OstatkiProduct.ColumnCount == 0)
                {
                    string strDateStart = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                    string strDateEnd = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                    CreateColumnNeOtgryj(ref dataGridViewSklad_OstatkiProduct);    
                    SelectAddSkladProduct("", "", strDateStart, strDateEnd, ref dataGridViewSklad_OstatkiProduct, 2, ref m_lsOstatkiSkladZakazchik, ref m_lsOstatkiSkladProductInfo,"");
                }
            }
        }

        private void dataGridViewSklad_OstatkiProduct_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (dataGridViewSklad_OstatkiProduct.Rows.Count >= 1)
            {
                dataGridViewSklad_OstatkiProduct.Rows[e.RowIndex].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewSklad_OstatkiProduct.Rows[e.RowIndex].Cells[1].Style.Padding = new Padding(3, 0, 0, 0);
                dataGridViewSklad_OstatkiProduct.Rows[e.RowIndex].Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewSklad_OstatkiProduct.Rows[e.RowIndex].Cells[2].Style.Padding = new Padding(3, 0, 0, 0);
            }
        }

        private void button_rezkaPrintZakaz_Click(object sender, EventArgs e)
        {

            if (dataGridView_rezkaZakaz.Rows.Count != 0 && dataGridView_rezkaZakaz.CurrentRow.Index != -1 && dataGridView_rezkaZakaz.CurrentRow.Selected == true)
            {
                int iRowIndex = dataGridView_rezkaZakaz.CurrentRow.Index;
                dataGridView_rezkaZakaz.Rows[iRowIndex].Selected = true;
                string strListInfo = dataGridView_rezkaZakaz.Rows[iRowIndex].Cells[1].Value.ToString() +
                             dataGridView_rezkaZakaz.Rows[iRowIndex].Cells[2].Value.ToString() +
                             dataGridView_rezkaZakaz.Rows[iRowIndex].Cells[3].Value.ToString() +
                             dataGridView_rezkaZakaz.Rows[iRowIndex].Cells[4].Value.ToString();

                int iZakazID = m_dicZakaz[strListInfo];

                

                if (CheckConnect())
                {
                    string strQuery = "select zk.zakazchik_name , z.partiya, pr.product_material ,z.dlinaetiketki, m.manager_name, t.product_tols,z.datezakaz " +
                                        " from itak_etiketka.dbo.itak_zakazchik zk, itak_etiketka.dbo.itak_zakaz z , itak_etiketka.dbo.itak_productmaterial pr, itak_etiketka.dbo.itak_manager m , itak_etiketka.dbo.itak_producttols t" +
                                        " where z.zakazchik_id=zk.id and z.productmaterial_id=pr.id and z.manager_id=m.id and z.producttols_id=t.id and z.id=" + iZakazID;

                    m_MSSQLCommand.CommandText = strQuery;
                    m_MSSQLReader = m_MSSQLCommand.ExecuteReader();

                    if (m_MSSQLReader.HasRows)
                    {
                        while (m_MSSQLReader.Read())
                        {
                            m_strRezkaZakazchiName = m_MSSQLReader["zakazchik_name"].ToString().Trim();
                            m_strRezkaPartiya = m_MSSQLReader["partiya"].ToString().Trim();
                            m_strRezkaMaterial = m_MSSQLReader["product_material"].ToString().Trim();
                            m_strRezkaTols = m_MSSQLReader["product_tols"].ToString().Trim();
                            m_strRezkaDlinaEtiketki = m_MSSQLReader["dlinaetiketki"].ToString().Trim();
                            m_strRezkaManager = m_MSSQLReader["manager_name"].ToString().Trim();
                            m_strRezkaDateZakaz = m_MSSQLReader["datezakaz"].ToString().Trim();
                        }
                    }
                    m_MSSQLReader.Close();

                    m_dicRezkaProduct.Clear();
                    m_dicRezkaProductInfo.Clear();

                    strQuery = "select distinct pr.product_name, pr.id from itak_etiketka.dbo.itak_vihidrylon vh , itak_etiketka.dbo.itak_product pr where pr.id=vh.product_id and  vh.zakaz_id=" + iZakazID;
                    m_MSSQLCommand.CommandText = strQuery;
                    m_MSSQLReader = m_MSSQLCommand.ExecuteReader();

                    if (m_MSSQLReader.HasRows)
                        while (m_MSSQLReader.Read())
                            m_dicRezkaProduct.Add(m_MSSQLReader["product_name"].ToString().Trim(), Convert.ToInt32(m_MSSQLReader["id"]));
                    m_MSSQLReader.Close();

                    stProductInfo prInfo = new stProductInfo();

                    foreach (KeyValuePair<string, int> key in m_dicRezkaProduct)
                    {
                        strQuery = "select COUNT(id), ROUND(SUM(brytto),2), ROUND(SUM(brytto-vagatary),2), SUM(koletiketki) from itak_etiketka.dbo.itak_vihidrylon where zakaz_id=" + iZakazID + " and product_id="+key.Value;
                        m_MSSQLCommand.CommandText = strQuery;
                        m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
                        if (m_MSSQLReader.HasRows)
                        {
                            m_MSSQLReader.Read();
                            prInfo.strProductName = key.Key;
                            prInfo.iProductID = key.Value;
                            
                            if (m_MSSQLReader[0]!=DBNull.Value)
                                prInfo.iCountRylon = Convert.ToInt32(m_MSSQLReader[0]);

                            if (m_MSSQLReader[1]!=DBNull.Value)
                                 prInfo.dBrytto = Convert.ToDouble(m_MSSQLReader[1]);

                            if (m_MSSQLReader[2]!=DBNull.Value)
                                prInfo.dNetto = Convert.ToDouble(m_MSSQLReader[2]);

                            if (m_MSSQLReader[3]!=DBNull.Value)
                                 prInfo.iCountEtiket = Convert.ToInt32(m_MSSQLReader[3]);

                            m_dicRezkaProductInfo.Add(prInfo.strProductName , prInfo);
                        }

                        m_MSSQLReader.Close();
                    }

                    /*foreach (KeyValuePair<string, stProductInfo> key in m_dicRezkaProductInfo)
                    {
                     // ширина бобины
                        strQuery = "select distinct width from itak_etiketka.dbo.itak_vihidrylon where zakaz_id="+iZakazID+" and product_id="+key.Value.iProductID;
                        m_MSSQLCommand.CommandText = strQuery;
                        m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
                        if (m_MSSQLReader.HasRows)
                        {
                            m_MSSQLReader.Read();
                            foreach (KeyValuePair<string,stProductInfo> k in m_dicRezkaProductInfo)
                            {
                                if (k.Value.iProductID == key.Value.iProductID)
                                {
                                    if (m_MSSQLReader["width"] != DBNull.Value)
                                    {
                                        key.Value.iWidth = Convert.ToInt32(m_MSSQLReader["width"]);
                                        break;
                                    }
                                }
                            }
                        }

                        m_MSSQLReader.Close();
                    }*/

                    m_dRezkaAllNetto = m_dRezkaAllBrytto = m_iRezkaAllCountEtik = m_iRezkaAllCountRylon = 0;

                    foreach (KeyValuePair<string,stProductInfo> key in m_dicRezkaProductInfo)
                    {
                        m_dRezkaAllNetto+= key.Value.dNetto;
                        m_dRezkaAllBrytto += key.Value.dBrytto;
                        m_iRezkaAllCountEtik += key.Value.iCountEtiket;
                        m_iRezkaAllCountRylon += key.Value.iCountRylon;
                    }

                    PrintDocument docPrint = new PrintDocument();
                    if (docPrint.PrinterSettings.IsValid)
                    {
                        docPrint.DefaultPageSettings.Margins.Top = 10;
                        docPrint.DefaultPageSettings.Margins.Bottom = 10;
                        docPrint.DefaultPageSettings.Margins.Right = 0;
                        docPrint.DefaultPageSettings.Margins.Left = 0;

                        docPrint.DefaultPageSettings.Landscape = false;
                        docPrint.PrintPage += new PrintPageEventHandler(this.docPrint_PrintRezkazZakaz);

                        docPrint.PrinterSettings.Copies = 1;

                        PrintPreviewDialog dlgPrint = new PrintPreviewDialog();
                        dlgPrint.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height);
                        dlgPrint.Document = docPrint;
                        //dlgPrint.ShowDialog();
                         docPrint.Print();
                    }
                    else
                        MessageBox.Show("Принтер не доступен!");
                }
            }


           
        }

        private void docPrint_PrintRezkazZakaz(object sender, PrintPageEventArgs e)
        {
            System.Drawing.Font fnt10Bold = new Font("Times New Roman", 10, FontStyle.Bold);
            System.Drawing.Font fnt10 = new Font("Times New Roman", 10, FontStyle.Regular);

            System.Drawing.Font fnt8 = new Font("Times New Roman", 8, FontStyle.Regular);
            System.Drawing.Font fnt7 = new Font("Times New Roman", 7, FontStyle.Regular);

            System.Drawing.Font fnt12Bold = new Font("Times New Roman", 12, FontStyle.Bold);
            System.Drawing.Font fnt12 = new Font("Times New Roman", 12, FontStyle.Regular);
            System.Drawing.Font fnt12BoldUnder = new Font("Times New Roman", 12, FontStyle.Bold | FontStyle.Underline);
            System.Drawing.Font fnt14Bold = new Font("Times New Roman", 14, FontStyle.Bold);
            System.Drawing.Font fnt14 = new Font("Times New Roman", 14, FontStyle.Regular);
            System.Drawing.Font fnt14BoldUnder = new Font("Times New Roman", 14, FontStyle.Bold | FontStyle.Underline);
            System.Drawing.Font fnt16Bold = new Font("Times New Roman", 16, FontStyle.Bold);
            System.Drawing.Font fnt16BoldUnder = new Font("Times New Roman", 16, FontStyle.Bold | FontStyle.Underline);

            int t = 10;
           
            t += 45;
            e.Graphics.DrawString("Заказчик:", fnt12, Brushes.Black, 40, t);
            e.Graphics.DrawString(m_strRezkaZakazchiName, fnt12BoldUnder, Brushes.Black, 130, t);

            t += 25;
            e.Graphics.DrawString("Партия:", fnt12, Brushes.Black, 40, t);
            e.Graphics.DrawString(m_strRezkaPartiya, fnt12BoldUnder, Brushes.Black, 130, t);
            
            t += 25;
            e.Graphics.DrawString("Материал:", fnt12, Brushes.Black, 40, t);
            m_strRezkaMaterial = m_strRezkaMaterial + " " + m_strRezkaTols;
            e.Graphics.DrawString(m_strRezkaMaterial, fnt12BoldUnder, Brushes.Black, 130, t);

            t += 25;
            e.Graphics.DrawString("Шаг рисунка:", fnt12, Brushes.Black, 40, t);
            e.Graphics.DrawString(m_strRezkaDlinaEtiketki+" мм", fnt12BoldUnder, Brushes.Black, 150, t);

            t += 25;
            e.Graphics.DrawString("Менеджер:", fnt12, Brushes.Black, 40, t);
            e.Graphics.DrawString(m_strRezkaManager, fnt12BoldUnder, Brushes.Black, 130, t);

            t += 25;
            e.Graphics.DrawString("Дата порезки:", fnt12, Brushes.Black, 40, t);
            e.Graphics.DrawString(m_strRezkaDateZakaz.Substring(0,m_strRezkaDateZakaz.IndexOf(' '))+" г.", fnt12BoldUnder, Brushes.Black, 150, t);



            int iLeft = 40;
            t += 40;

            int iTableWidth = iLeft + 630/*730*/;
            int[] iCoord = { iLeft, iLeft + 25, iLeft + 300, iLeft + 395, iLeft + 470, iLeft + 550, iLeft + 630/*, iLeft + 730*/ };

            e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t), new Point(iTableWidth, t));
            e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t + 25), new Point(iTableWidth, t + 25));
            for (int j = 0; j <= 6/*7*/; j++)
                e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iCoord[j], t - 1), new Point(iCoord[j], t + 25));

            t += 4;
            e.Graphics.DrawString("№", fnt10Bold, Brushes.Black, iLeft + 3, t);
            e.Graphics.DrawString("Продукция", fnt10Bold, Brushes.Black, iLeft + 100, t);
            e.Graphics.DrawString("К-во рулонов", fnt10Bold, Brushes.Black, iLeft + 302, t);
            e.Graphics.DrawString("Нетто, кг", fnt10Bold, Brushes.Black, iLeft + 400, t);
            e.Graphics.DrawString("Брутто, кг", fnt10Bold, Brushes.Black, iLeft + 475, t);
            e.Graphics.DrawString("К-во этик.", fnt10Bold, Brushes.Black, iLeft + 555, t);
            //e.Graphics.DrawString("Ширина боб.", fnt10Bold, Brushes.Black, iLeft + 635, t);

            int iCounter = 1;

            t += 20;
            foreach (KeyValuePair<string,stProductInfo>key in m_dicRezkaProductInfo)
            {
                for (int j = 0; j <= /*7*/6; j++)
                    if (j == 0 || j == 6)
                        e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iCoord[j], t - 1), new Point(iCoord[j], t + 15));
                    else
                        e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[j], t - 1), new Point(iCoord[j], t + 15));

                if (iCounter > 9)
                    e.Graphics.DrawString(iCounter.ToString(), fnt8, Brushes.Black, iCoord[0] + 13, t+2);
                else
                    e.Graphics.DrawString(iCounter.ToString(), fnt8, Brushes.Black, iCoord[0] + 10, t+2);

                e.Graphics.DrawString(key.Key, fnt8, Brushes.Black, iCoord[1] + 3, t+2);
                e.Graphics.DrawString(key.Value.iCountRylon.ToString(), fnt8, Brushes.Black, iCoord[2] + 40, t + 2);
                e.Graphics.DrawString(key.Value.dNetto.ToString("0.00"), fnt8, Brushes.Black, iCoord[3] + 20, t + 2);
                e.Graphics.DrawString(key.Value.dBrytto.ToString("0.00"), fnt8, Brushes.Black, iCoord[4] + 20, t + 2);
                e.Graphics.DrawString(key.Value.iCountEtiket.ToString(), fnt8, Brushes.Black, iCoord[5] + 20, t + 2);

               // e.Graphics.DrawString("255", fnt8, Brushes.Black, iCoord[6] + 50, t + 2);
                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iLeft, t + 15), new Point(iTableWidth, t + 15));
                t += 15;
                iCounter++;
            }

            t += 20;
            e.Graphics.DrawString("К-во рулонов: ", fnt12, Brushes.Black, 40, t);
            e.Graphics.DrawString(m_iRezkaAllCountRylon.ToString()+" шт", fnt12BoldUnder, Brushes.Black, 150, t);
            t += 25;
            e.Graphics.DrawString("Масса нетто: ", fnt12, Brushes.Black, 40, t);
            e.Graphics.DrawString(m_dRezkaAllNetto.ToString("0.00") + " кг", fnt12BoldUnder, Brushes.Black, 150, t);

            t += 25;
            e.Graphics.DrawString("Масса брутто: ", fnt12, Brushes.Black, 40, t);
            e.Graphics.DrawString(m_dRezkaAllBrytto.ToString("0.00") + " кг", fnt12BoldUnder, Brushes.Black, 150, t);

            t += 25;
            e.Graphics.DrawString("К-во этикеток: ", fnt12, Brushes.Black, 40, t);
            e.Graphics.DrawString(m_iRezkaAllCountEtik.ToString() + " шт", fnt12BoldUnder, Brushes.Black, 150, t);
                         
        }

        private void button_ObscheePrint_Click(object sender, EventArgs e)
        {
            if (dataGridView_ObscheeZakaz.Rows.Count != 0 && dataGridView_ObscheeZakaz.CurrentRow.Index != -1 && dataGridView_ObscheeZakaz.CurrentRow.Selected == true)
            {
                PrintDocument docPrint = new PrintDocument();
                if (docPrint.PrinterSettings.IsValid)
                {
                    docPrint.DefaultPageSettings.Margins.Top = 10;
                    docPrint.DefaultPageSettings.Margins.Bottom = 10;
                    docPrint.DefaultPageSettings.Margins.Right = 0;
                    docPrint.DefaultPageSettings.Margins.Left = 0;

                    docPrint.DefaultPageSettings.Landscape = true;
                    docPrint.PrintPage += new PrintPageEventHandler(this.docPrint_PrintObscheeZakaz);

                    docPrint.PrinterSettings.Copies = 1;

                    PrintPreviewDialog dlgPrint = new PrintPreviewDialog();
                    dlgPrint.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height);
                    dlgPrint.Document = docPrint;
                    //dlgPrint.ShowDialog();
                    docPrint.Print();
                }
                else
                    MessageBox.Show("Принтер не доступен!");
            }
        }


        private void docPrint_PrintObscheeZakaz(object sender, PrintPageEventArgs e)
        {
            System.Drawing.Font fnt10Bold = new Font("Times New Roman", 10, FontStyle.Bold);
            System.Drawing.Font fnt10 = new Font("Times New Roman", 10, FontStyle.Regular);
            System.Drawing.Font fnt10BoldUnder = new Font("Times New Roman", 10, FontStyle.Bold | FontStyle.Underline);

            System.Drawing.Font fnt8 = new Font("Times New Roman", 8, FontStyle.Regular);
            System.Drawing.Font fnt8Bold = new Font("Times New Roman", 8, FontStyle.Bold);
            System.Drawing.Font fnt7 = new Font("Times New Roman", 7, FontStyle.Regular);

            System.Drawing.Font fnt9Bold = new Font("Times New Roman", 9, FontStyle.Bold);

            System.Drawing.Font fnt12Bold = new Font("Times New Roman", 12, FontStyle.Bold);
            System.Drawing.Font fnt12 = new Font("Times New Roman", 12, FontStyle.Regular);
            System.Drawing.Font fnt12BoldUnder = new Font("Times New Roman", 12, FontStyle.Bold | FontStyle.Underline);
            System.Drawing.Font fnt14Bold = new Font("Times New Roman", 14, FontStyle.Bold);
            System.Drawing.Font fnt14 = new Font("Times New Roman", 14, FontStyle.Regular);
            System.Drawing.Font fnt14BoldUnder = new Font("Times New Roman", 14, FontStyle.Bold | FontStyle.Underline);
            System.Drawing.Font fnt16Bold = new Font("Times New Roman", 16, FontStyle.Bold);
            System.Drawing.Font fnt16BoldUnder = new Font("Times New Roman", 16, FontStyle.Bold | FontStyle.Underline);


            int t = 0;
            int iLeft = 30;

            t += 10;
            e.Graphics.DrawString("Заказчик:", fnt10, Brushes.Black, iLeft, t);
            e.Graphics.DrawString(m_strObscheeZakazchik, fnt10BoldUnder, Brushes.Black, iLeft+90, t);

            t += 20;
            e.Graphics.DrawString("Партия:", fnt10, Brushes.Black, iLeft, t);
            e.Graphics.DrawString(m_strObscheePartiya, fnt10BoldUnder, Brushes.Black, iLeft+90, t);

            t += 20;
            e.Graphics.DrawString("Материал:", fnt10, Brushes.Black, iLeft, t);
            e.Graphics.DrawString(m_strObscheeMaterial, fnt10BoldUnder, Brushes.Black, iLeft + 90, t);

            t += 20;
            e.Graphics.DrawString("Шаг рисунка:", fnt10, Brushes.Black, iLeft, t);
            e.Graphics.DrawString(m_strObscheeDlinaEtiketki + " мм", fnt10BoldUnder, Brushes.Black, iLeft + 90, t);

            t += 20;
            e.Graphics.DrawString("Менеджер:", fnt10, Brushes.Black, iLeft, t);
            e.Graphics.DrawString(m_strObscheeManager, fnt10BoldUnder, Brushes.Black, iLeft + 90, t);

            t += 20;
            e.Graphics.DrawString("Дата порезки:", fnt10, Brushes.Black, iLeft, t);
            e.Graphics.DrawString(m_strObscheeDataPorezki+ " г.", fnt10BoldUnder, Brushes.Black, iLeft + 90, t);

            t += 20;
            e.Graphics.DrawString("Дата печати отчета:", fnt10, Brushes.Black, iLeft, t);
            e.Graphics.DrawString(DateTime.Now.ToString(), fnt10BoldUnder, Brushes.Black, iLeft + 120, t);

            t += 30;
            int iTableWidth = 1050+iLeft;

            int[] iCoord = { iLeft, iLeft + 25, iLeft + 350, iLeft + 525, iLeft + 700, iLeft + 875, iLeft+1050 };
            int[] iCoord2 = {iCoord[2] + 57,iCoord[2] + 114};

            e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t), new Point(iTableWidth, t));
            e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t + 40), new Point(iTableWidth, t + 40));
            for (int j = 0; j <= 6; j++)
                e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iCoord[j], t ), new Point(iCoord[j], t + 40));

            e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iLeft+350, t + 20), new Point(iTableWidth, t + 20));


            e.Graphics.DrawString("№", fnt10Bold, Brushes.Black, iCoord[0] + 3, t + 12);
            e.Graphics.DrawString("ПРОДУКЦИЯ", fnt10Bold, Brushes.Black, iCoord[1] + 100, t + 12);
            e.Graphics.DrawString("РЕЗКА", fnt10Bold, Brushes.Black, iCoord[2] + 65, t + 2);
            e.Graphics.DrawString("ПРИНЯТО С РЕЗКИ", fnt10Bold, Brushes.Black, iCoord[3] + 20, t + 2);
            e.Graphics.DrawString("ОТГРУЖ. СО СКЛАДА", fnt10Bold, Brushes.Black, iCoord[4] + 10, t + 2);
            e.Graphics.DrawString("ОСТАТКИ НА СКЛАДЕ", fnt10Bold, Brushes.Black, iCoord[5] + 10, t + 2);

            t += 20;
            for (int i = 2; i <= 5; i++)
            {
                e.Graphics.DrawString("К-во рул.", fnt8Bold, Brushes.Black, iCoord[i] + 3, t + 3);
                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[i] + 57, t), new Point(iCoord[i] + 57, t + 20));
                e.Graphics.DrawString("Нетто, кг", fnt8Bold, Brushes.Black, iCoord[i] + 60, t + 3);
                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[i] + 114, t), new Point(iCoord[i] + 114, t + 20));
                e.Graphics.DrawString("Брутто, кг", fnt8Bold, Brushes.Black, iCoord[i] + 115, t + 3);
            }

            t += 20;
            int iCounter = 1;
            foreach (KeyValuePair<string, stProductInfoObschee> key in m_dicObscheeProductInfo)
            {
                e.Graphics.DrawString((iCounter + 1).ToString(), fnt8, Brushes.Black, iCoord[0] + 3, t + 3);
                e.Graphics.DrawString(key.Key, fnt8, Brushes.Black, iCoord[1] + 3, t + 3);

                //Rezka
                e.Graphics.DrawString(key.Value.stPrSmallRezka.iCountRylon.ToString(), fnt8, Brushes.Black, iCoord[2] + 20, t + 3);
                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[2] + 57, t), new Point(iCoord[2] + 57, t + 16));
                e.Graphics.DrawString(key.Value.stPrSmallRezka.dNetto.ToString("0.00"), fnt8, Brushes.Black, iCoord[2] + 70, t + 3);
                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[2] + 114, t), new Point(iCoord[2] + 114, t + 16));
                e.Graphics.DrawString(key.Value.stPrSmallRezka.dBrytto.ToString("0.00"), fnt8, Brushes.Black, iCoord[2] + 125, t + 3);
                //End Rezka

                //Prinyato s Rezki
                e.Graphics.DrawString(key.Value.stPrSmallPrinyatoRezka.iCountRylon.ToString(), fnt8, Brushes.Black, iCoord[3] + 20, t + 3);

                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[3] + 57, t), new Point(iCoord[3] + 57, t + 16));
                e.Graphics.DrawString(key.Value.stPrSmallPrinyatoRezka.dNetto.ToString("0.00"), fnt8, Brushes.Black, iCoord[3] + 70, t + 3);
                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[3] + 114, t), new Point(iCoord[3] + 114, t + 16));
                e.Graphics.DrawString(key.Value.stPrSmallPrinyatoRezka.dBrytto.ToString("0.00"), fnt8, Brushes.Black, iCoord[3] + 125, t + 3);
                //EndPrinyato s Rezki

                //Otgryjeno
                e.Graphics.DrawString(key.Value.stPrSmallOtgryjeno.iCountRylon.ToString(), fnt8, Brushes.Black, iCoord[4] + 20, t + 3);
                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[4] + 57, t), new Point(iCoord[4] + 57, t + 16));
                e.Graphics.DrawString(key.Value.stPrSmallOtgryjeno.dNetto.ToString("0.00"), fnt8, Brushes.Black, iCoord[4] + 70, t + 3);
                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[4] + 114, t), new Point(iCoord[4] + 114, t + 16));
                e.Graphics.DrawString(key.Value.stPrSmallOtgryjeno.dBrytto.ToString("0.00"), fnt8, Brushes.Black, iCoord[4] + 125, t + 3);
                //End Otgryjeno

                //Ostatki
                e.Graphics.DrawString(key.Value.stPrSmallOstatki.iCountRylon.ToString(), fnt8, Brushes.Black, iCoord[5] + 20, t + 3);
                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[5] + 57, t), new Point(iCoord[5] + 57, t + 16));
                e.Graphics.DrawString(key.Value.stPrSmallOstatki.dNetto.ToString("0.00"), fnt8, Brushes.Black, iCoord[5] + 70, t + 3);
                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[5] + 114, t), new Point(iCoord[5] + 114, t + 16));
                e.Graphics.DrawString(key.Value.stPrSmallOstatki.dBrytto.ToString("0.00"), fnt8, Brushes.Black, iCoord[5] + 125, t + 3);
                //End Ostatki


                for (int j = 0; j <= 6; j++)
                    e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iCoord[j], t), new Point(iCoord[j], t + 16));
                t += 16;
                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iLeft, t), new Point(iTableWidth, t));
                iCounter++;
            }

           for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j <= 6; j++)
                    e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iCoord[j], t), new Point(iCoord[j], t + 16));

                for (int j = 2; j <= 5; j++)
                {
                    e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[j] + 57, t), new Point(iCoord[j] + 57, t + 16));
                    e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iCoord[j] + 114, t), new Point(iCoord[j] + 114, t + 16));
                }
                
                t += 16;
                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(iLeft, t), new Point(iTableWidth, t));
            }
            e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(iLeft, t), new Point(iTableWidth, t));

            e.Graphics.DrawString("ОБЩЕЕ", fnt8Bold, Brushes.Black, iCoord[1] + 3, t - 14);

            e.Graphics.DrawString(m_iAllCountRylonRezka.ToString(), fnt8Bold, Brushes.Black, iCoord[2] + 20, t - 14);
            e.Graphics.DrawString(m_dAllNettoRezka.ToString("0.00"), fnt8Bold, Brushes.Black, iCoord[2] + 70, t - 14);
            e.Graphics.DrawString(m_dAllBryttoRezka.ToString("0.00"), fnt8Bold, Brushes.Black, iCoord[2] + 125, t - 14);

            e.Graphics.DrawString(m_iAllCountRylonPrinyato.ToString(), fnt8Bold, Brushes.Black, iCoord[3] + 20, t - 14);
            e.Graphics.DrawString(m_dAllNettoPrinyato.ToString("0.00"), fnt8Bold, Brushes.Black, iCoord[3] + 70, t - 14);
            e.Graphics.DrawString(m_dAllBryttoPrinyato.ToString("0.00"), fnt8Bold, Brushes.Black, iCoord[3] + 125, t - 14);

            e.Graphics.DrawString(m_iAllCountRylonOtgryjeno.ToString(), fnt8Bold, Brushes.Black, iCoord[4] + 20, t - 14);
            e.Graphics.DrawString(m_dAllNettoOtgryjeno.ToString("0.00"), fnt8Bold, Brushes.Black, iCoord[4] + 70, t - 14);
            e.Graphics.DrawString(m_dAllBryttoOtgryjeno.ToString("0.00"), fnt8Bold, Brushes.Black, iCoord[4] + 125, t - 14);

            e.Graphics.DrawString(m_iAllCountRylonOstatki.ToString(), fnt8Bold, Brushes.Black, iCoord[5] + 20, t - 14);
            e.Graphics.DrawString(m_dAllNettoOstatki.ToString("0.00"), fnt8Bold, Brushes.Black, iCoord[5] + 70, t - 14);
            e.Graphics.DrawString(m_dAllBryttoOstatki.ToString("0.00"), fnt8Bold, Brushes.Black, iCoord[5] + 125, t - 14);
        }


        private void CreateDataGridOdscheeProduct()
        {
            dataGridView_ObscheeProduct.Columns.Add("ObscheeNumber", "");
            dataGridView_ObscheeProduct.Columns.Add("ObscheeProduct", "");

            dataGridView_ObscheeProduct.Columns.Add("ObscheeRezkaCountRylon", "К-во рулонов");
            dataGridView_ObscheeProduct.Columns.Add("ObscheeRezkaNetto", "Нетто");
            dataGridView_ObscheeProduct.Columns.Add("ObscheeRezkaBrytto", "Брутто");

            dataGridView_ObscheeProduct.Columns.Add("ObscheePrinyatoCountRylon", "К-во рулонов");
            dataGridView_ObscheeProduct.Columns.Add("ObscheePrinyatoNetto", "Нетто");
            dataGridView_ObscheeProduct.Columns.Add("ObscheePrinyatoBrytto", "Брутто");

            dataGridView_ObscheeProduct.Columns.Add("ObscheeOtgryzCountRylon", "К-во рулонов");
            dataGridView_ObscheeProduct.Columns.Add("ObscheeOtgryzNetto", "Нетто");
            dataGridView_ObscheeProduct.Columns.Add("ObscheeOtgryzBrytto", "Брутто");

            dataGridView_ObscheeProduct.Columns.Add("ObscheeOstatkiCountRylon", "К-во рулонов");
            dataGridView_ObscheeProduct.Columns.Add("ObscheeOstatkiNetto", "Нетто");
            dataGridView_ObscheeProduct.Columns.Add("ObscheeOstatkiBrytto", "Брутто");


            dataGridView_ObscheeProduct.Columns["ObscheeNumber"].Width = 26;
            dataGridView_ObscheeProduct.Columns["ObscheeProduct"].Width = 290;

            dataGridView_ObscheeProduct.Columns["ObscheeRezkaCountRylon"].Width = 79;
            dataGridView_ObscheeProduct.Columns["ObscheeRezkaNetto"].Width = 56;
            dataGridView_ObscheeProduct.Columns["ObscheeRezkaBrytto"].Width = 56;

            dataGridView_ObscheeProduct.Columns["ObscheePrinyatoCountRylon"].Width = 79;
            dataGridView_ObscheeProduct.Columns["ObscheePrinyatoNetto"].Width = 56;
            dataGridView_ObscheeProduct.Columns["ObscheePrinyatoBrytto"].Width = 56;

            dataGridView_ObscheeProduct.Columns["ObscheeOtgryzCountRylon"].Width = 79;
            dataGridView_ObscheeProduct.Columns["ObscheeOtgryzNetto"].Width = 56;
            dataGridView_ObscheeProduct.Columns["ObscheeOtgryzBrytto"].Width = 56;

            dataGridView_ObscheeProduct.Columns["ObscheeOstatkiCountRylon"].Width = 79;
            dataGridView_ObscheeProduct.Columns["ObscheeOstatkiNetto"].Width = 56;
            dataGridView_ObscheeProduct.Columns["ObscheeOstatkiBrytto"].Width = 56;

            dataGridView_ObscheeProduct.ColumnHeadersHeight = dataGridView_ObscheeProduct.ColumnHeadersHeight * 2;
            dataGridView_ObscheeProduct.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dataGridView_ObscheeProduct.CellPainting += new DataGridViewCellPaintingEventHandler(dataGridView_ObscheeProduct_CellPainting);
            dataGridView_ObscheeProduct.Paint += new PaintEventHandler(dataGridView_ObscheeProduct_Paint);


            for (int j = 0; j < dataGridView_ObscheeProduct.ColumnCount; j++)
            {
                dataGridView_ObscheeProduct.Columns[j].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView_ObscheeProduct.Columns[j].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            dataGridView_ObscheeProduct.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            if (dataGridView_ObscheeProduct.Rows.Count!=0)
                dataGridView_ObscheeProduct.Rows[0].Selected = false;
        }

       

        void dataGridView_ObscheeProduct_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Font fnt10Bold = new Font("Times New Roman", 10, FontStyle.Bold);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            Rectangle r0 = dataGridView_ObscheeProduct.GetCellDisplayRectangle(0, -1, true); //get the column header cell
            e.Graphics.FillRectangle(new SolidBrush(dataGridView_ObscheeProduct.ColumnHeadersDefaultCellStyle.BackColor), r0.Left + 1, r0.Top + 1, r0.Width - 1, r0.Height - 2);
            e.Graphics.DrawString("№", fnt10Bold, new SolidBrush(dataGridView_ObscheeProduct.ColumnHeadersDefaultCellStyle.ForeColor), r0, format);

            Rectangle r1 = dataGridView_ObscheeProduct.GetCellDisplayRectangle(1, -1, true); //get the column header cell
            e.Graphics.FillRectangle(new SolidBrush(dataGridView_ObscheeProduct.ColumnHeadersDefaultCellStyle.BackColor), r1.Left + 1, r1.Top + 1, r1.Width - 2, r1.Height - 2);
            e.Graphics.DrawString("Продукт", fnt10Bold, new SolidBrush(dataGridView_ObscheeProduct.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);

            Rectangle r2_1 = dataGridView_ObscheeProduct.GetCellDisplayRectangle(2, -1, true); //get the column header cell
            Rectangle r2_2 = dataGridView_ObscheeProduct.GetCellDisplayRectangle(3, -1, true); //get the column header cell
            Rectangle r2_3 = dataGridView_ObscheeProduct.GetCellDisplayRectangle(4, -1, true); //get the column header cell
            r2_1.Width = r2_1.Width + r2_2.Width + r2_3.Width - 2;
            r2_1.Height = r2_1.Height / 2;
            e.Graphics.FillRectangle(new SolidBrush(dataGridView_ObscheeProduct.ColumnHeadersDefaultCellStyle.BackColor), r2_1.Left + 1, r2_1.Top + 1, r2_1.Width, r2_1.Height);
            e.Graphics.DrawString("Резка", fnt10Bold, new SolidBrush(dataGridView_ObscheeProduct.ColumnHeadersDefaultCellStyle.ForeColor), r2_1, format);

            Rectangle r3_1 = dataGridView_ObscheeProduct.GetCellDisplayRectangle(5, -1, true); //get the column header cell
            Rectangle r3_2 = dataGridView_ObscheeProduct.GetCellDisplayRectangle(6, -1, true); //get the column header cell
            Rectangle r3_3 = dataGridView_ObscheeProduct.GetCellDisplayRectangle(7, -1, true); //get the column header cell
            r3_1.Width = r3_1.Width + r3_2.Width + r3_3.Width - 2;
            r3_1.Height = r3_1.Height / 2;
            e.Graphics.FillRectangle(new SolidBrush(dataGridView_ObscheeProduct.ColumnHeadersDefaultCellStyle.BackColor), r3_1.Left + 1, r3_1.Top + 1, r3_1.Width - 1, r3_1.Height);
            e.Graphics.DrawString("Принято с резки", fnt10Bold, new SolidBrush(dataGridView_ObscheeProduct.ColumnHeadersDefaultCellStyle.ForeColor), r3_1, format);

            Rectangle r4_1 = dataGridView_ObscheeProduct.GetCellDisplayRectangle(8, -1, true); //get the column header cell
            Rectangle r4_2 = dataGridView_ObscheeProduct.GetCellDisplayRectangle(9, -1, true); //get the column header cell
            Rectangle r4_3 = dataGridView_ObscheeProduct.GetCellDisplayRectangle(10, -1, true); //get the column header cell
            r4_1.Width = r4_1.Width + r4_2.Width + r4_3.Width - 2;
            r4_1.Height = r4_1.Height / 2;
            e.Graphics.FillRectangle(new SolidBrush(dataGridView_ObscheeProduct.ColumnHeadersDefaultCellStyle.BackColor), r4_1.Left + 1, r4_1.Top + 1, r4_1.Width - 1, r4_1.Height);
            e.Graphics.DrawString("Отгружено со склада", fnt10Bold, new SolidBrush(dataGridView_ObscheeProduct.ColumnHeadersDefaultCellStyle.ForeColor), r4_1, format);

            Rectangle r5_1 = dataGridView_ObscheeProduct.GetCellDisplayRectangle(11, -1, true); //get the column header cell
            Rectangle r5_2 = dataGridView_ObscheeProduct.GetCellDisplayRectangle(12, -1, true); //get the column header cell
            Rectangle r5_3 = dataGridView_ObscheeProduct.GetCellDisplayRectangle(13, -1, true); //get the column header cell
            r5_1.Width = r5_1.Width + r5_2.Width + r5_3.Width - 2;
            r5_1.Height = r5_1.Height / 2;
            e.Graphics.FillRectangle(new SolidBrush(dataGridView_ObscheeProduct.ColumnHeadersDefaultCellStyle.BackColor), r5_1.Left + 1, r5_1.Top + 1, r5_1.Width - 1, r5_1.Height);
            e.Graphics.DrawString("Остатки на складе", fnt10Bold, new SolidBrush(dataGridView_ObscheeProduct.ColumnHeadersDefaultCellStyle.ForeColor), r5_1, format);
        }


        void dataGridView_ObscheeProduct_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                e.PaintBackground(e.CellBounds, false);
                Rectangle r2 = e.CellBounds;
                r2.Y += e.CellBounds.Height / 2;
                r2.Height = e.CellBounds.Height / 2;
                e.PaintContent(r2);
                e.Handled = true;
            }
        }

        private void button_ObscheeSearch_Click(object sender, EventArgs e)
        {
            ObscheeButtonSearch();
        }

        private void ObscheeButtonSearch()
        {
            m_bObscheeSelectZakaz = false;

            dataGridView_ObscheeZakaz.Rows.Clear();
            m_dicObscheeZakaz.Clear();

            dataGridView_ObscheeProduct.Rows.Clear();
            m_dicObscheeProduct.Clear();

            if (textBox_ObscheeNumTZ.Text.Length != 0)
                SelectObscheeZakaz(textBox_ObscheeNumTZ.Text);

            for (int i = 0; i < dataGridView_ObscheeZakaz.Rows.Count; i++)
            {
                dataGridView_ObscheeZakaz.Rows[i].Selected = false;

                for (int j = 1; j < dataGridView_ObscheeZakaz.Columns.Count;j++)
                    dataGridView_ObscheeZakaz.Rows[i].Cells[j].ReadOnly = true;
            }

            m_bObscheeSelectZakaz = true;
        }

        private void SelectObscheeZakaz(string strPartiya)
        {
            
            if (strPartiya.Length != 0 )
            {
                strMSSQLQuery = "select z.id,zn.zakazchik_name, z.partiya, z.datezakaz, z.dlinaetiketki, pm.product_material, pt.product_tols, m.manager_name " +
                                " from itak_etiketka.dbo.itak_zakazchik zn, itak_etiketka.dbo.itak_zakaz z, itak_etiketka.dbo.itak_productmaterial pm, itak_etiketka.dbo.itak_producttols pt, itak_etiketka.dbo.itak_manager m " +
                                "where z.zakazchik_id=zn.id and pm.id=z.productmaterial_id and z.producttols_id=pt.id and m.id=z.manager_id and  z.partiya like'%" + strPartiya + "%' order by z.datezakaz desc";
                try
                {
                    m_MSSQLCommand.CommandText = strMSSQLQuery;
                    m_MSSQLReader = m_MSSQLCommand.ExecuteReader();

                    if (m_MSSQLReader.HasRows)
                    {
                        int i = 1;
                        while (m_MSSQLReader.Read())
                        {
                            string strDate1 = m_MSSQLReader["datezakaz"].ToString().Trim();
                            strDate1 = strDate1.Remove(strDate1.IndexOf(' '));

                            dataGridView_ObscheeZakaz.Rows.Add(null,i.ToString(),
                                        m_MSSQLReader["zakazchik_name"].ToString().Trim(),
                                        m_MSSQLReader["partiya"].ToString().Trim(),
                                        m_MSSQLReader["product_material"].ToString().Trim() +" " + m_MSSQLReader["product_tols"].ToString().Trim(),
                                        m_MSSQLReader["dlinaetiketki"].ToString().Trim(),
                                        m_MSSQLReader["manager_name"].ToString().Trim(),
                                        strDate1
                                        );

                            m_dicObscheeZakaz.Add(i.ToString() + "-" + m_MSSQLReader["zakazchik_name"].ToString().Trim(), Convert.ToInt32(m_MSSQLReader[0]));
                            i++;
                        }
                    }

                    m_MSSQLReader.Close();

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                	m_MSSQLReader.Close();
                }
                
            }
            
        }

        private void dataGridView_ObscheeZakaz_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView_ObscheeZakaz.CurrentRow.Index != -1 && dataGridView_ObscheeZakaz.CurrentRow.Index == ((DataGridView)sender).CurrentRow.Index)
            {
                DataGridViewCheckBoxCell ch1 = new DataGridViewCheckBoxCell();
                ch1 = (DataGridViewCheckBoxCell)dataGridView_ObscheeZakaz.Rows[dataGridView_ObscheeZakaz.CurrentRow.Index].Cells[0];

                if (ch1.Value == null)
                    ch1.Value = false;
                switch (ch1.Value.ToString())
                {
                    case "True":
                        ch1.Value = false;
                        
                        break;
                    case "False":
                        ch1.Value = true;
                        
                        break;
                }
            }
        }

        private void SelectInfoObscheeProduct(ref stProductInfoSmall stProduct, string strQuery)
        {
            try
            {
                m_MSSQLCommand.CommandText = strQuery;
                m_MSSQLReader = m_MSSQLCommand.ExecuteReader();
                if (m_MSSQLReader.HasRows)
                {
                    m_MSSQLReader.Read();
                    if (m_MSSQLReader[0] != DBNull.Value)
                        stProduct.iCountRylon = Convert.ToInt32(m_MSSQLReader[0]);

                    if (m_MSSQLReader[1] != DBNull.Value)
                        stProduct.dBrytto = Convert.ToDouble(m_MSSQLReader[1]);

                    if (m_MSSQLReader[2] != DBNull.Value)
                        stProduct.dNetto = Convert.ToDouble(m_MSSQLReader[2]);

                    if (m_MSSQLReader[3] != DBNull.Value)
                        stProduct.iCountEtiket = Convert.ToInt32(m_MSSQLReader[3]);
                }
                m_MSSQLReader.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                m_MSSQLReader.Close();
            }
        }

        private void textBox_ObscheeNumTZ_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ObscheeButtonSearch();
            }
        }


        private void ChangeSize()
        {
            int iWidth = dataGridView_rezkaZakaz.Size.Width;

            dataGridView_rezkaZakaz.Columns[0].Width = 30;
            dataGridView_rezkaZakaz.Columns[1].Width = (iWidth - 190) / 2;
            iWidth = (((iWidth - 30) / 2)+80) / 5;
            dataGridView_rezkaZakaz.Columns[2].Width = iWidth;
            dataGridView_rezkaZakaz.Columns[3].Width = iWidth;
            dataGridView_rezkaZakaz.Columns[4].Width = iWidth;
            dataGridView_rezkaZakaz.Columns[5].Width = iWidth;
            dataGridView_rezkaZakaz.Columns[6].Width = iWidth;


            iWidth = dataGridView_product.Size.Width;
            dataGridView_product.Columns[0].Width = 30;
            dataGridView_product.Columns[1].Width = (iWidth - 30) / 2;

            iWidth = ((iWidth - 30) / 2) / 4;
            dataGridView_product.Columns[2].Width = iWidth;
            dataGridView_product.Columns[3].Width = iWidth;
            dataGridView_product.Columns[4].Width = iWidth;
            dataGridView_product.Columns[5].Width = iWidth;


            iWidth = dataGridView_ObscheeZakaz.Size.Width;
            dataGridView_ObscheeZakaz.Columns[0].Width = 30;
            dataGridView_ObscheeZakaz.Columns[1].Width = 30;

            dataGridView_ObscheeZakaz.Columns[2].Width = (iWidth - 70) / 3;
            dataGridView_ObscheeZakaz.Columns[4].Width = (iWidth - 70) / 3;

            dataGridView_ObscheeZakaz.Columns[3].Width = ((iWidth - 70) / 3) / 4;
            dataGridView_ObscheeZakaz.Columns[5].Width = ((iWidth - 70) / 3) / 4;
            dataGridView_ObscheeZakaz.Columns[6].Width = ((iWidth - 70) / 3) / 4;
            dataGridView_ObscheeZakaz.Columns[7].Width = ((iWidth - 70) / 3) / 4;


            iWidth = dataGridView_ObscheeProduct.Size.Width;
            dataGridView_ObscheeProduct.Columns["ObscheeProduct"].Width = (iWidth - 35) / 5;

            iWidth = ((iWidth - 35) / 5) / 3;

            dataGridView_ObscheeProduct.Columns["ObscheeRezkaCountRylon"].Width = iWidth + 10;
            dataGridView_ObscheeProduct.Columns["ObscheeRezkaNetto"].Width = iWidth - 5;
            dataGridView_ObscheeProduct.Columns["ObscheeRezkaBrytto"].Width = iWidth - 5;

            dataGridView_ObscheeProduct.Columns["ObscheePrinyatoCountRylon"].Width = iWidth + 10;
            dataGridView_ObscheeProduct.Columns["ObscheePrinyatoNetto"].Width = iWidth - 5;
            dataGridView_ObscheeProduct.Columns["ObscheePrinyatoBrytto"].Width = iWidth - 5;


            dataGridView_ObscheeProduct.Columns["ObscheeOtgryzCountRylon"].Width = iWidth + 10;
            dataGridView_ObscheeProduct.Columns["ObscheeOtgryzNetto"].Width = iWidth - 5;
            dataGridView_ObscheeProduct.Columns["ObscheeOtgryzBrytto"].Width = iWidth - 5;

            dataGridView_ObscheeProduct.Columns["ObscheeOstatkiCountRylon"].Width = iWidth + 10;
            dataGridView_ObscheeProduct.Columns["ObscheeOstatkiNetto"].Width = iWidth - 5;
            dataGridView_ObscheeProduct.Columns["ObscheeOstatkiBrytto"].Width = iWidth - 5;
        }

        private void main_form_SizeChanged(object sender, EventArgs e)
        {
            //ChangeSize();

           // ChangeColumnSize(ref dataGridViewSklad_AddProduct);
            //ChangeColumnSize(ref dataGridViewSklad_OtgryzProduct);
            //ChangeColumnSize(ref dataGridViewSklad_OstatkiProduct);
        }

        private void dataGridView_rezkaZakaz_SizeChanged(object sender, EventArgs e)
        {
            int iWidth = dataGridView_rezkaZakaz.Size.Width;

            dataGridView_rezkaZakaz.Columns[0].Width = 30;
            dataGridView_rezkaZakaz.Columns[1].Width = (iWidth - 190) / 2;
            iWidth = (((iWidth - 30) / 2) + 80) / 5;
            dataGridView_rezkaZakaz.Columns[2].Width = iWidth;
            dataGridView_rezkaZakaz.Columns[3].Width = iWidth;
            dataGridView_rezkaZakaz.Columns[4].Width = iWidth;
            dataGridView_rezkaZakaz.Columns[5].Width = iWidth;
            dataGridView_rezkaZakaz.Columns[6].Width = iWidth;
        }

        private void dataGridView_product_SizeChanged(object sender, EventArgs e)
        {
            int iWidth = dataGridView_product.Size.Width;
            dataGridView_product.Columns[0].Width = 30;
            dataGridView_product.Columns[1].Width = (iWidth - 35) / 2;

            iWidth = ((iWidth - 35) / 2) / 4;
            dataGridView_product.Columns[2].Width = iWidth;
            dataGridView_product.Columns[3].Width = iWidth;
            dataGridView_product.Columns[4].Width = iWidth;
            dataGridView_product.Columns[5].Width = iWidth;
        }

        private void dataGridView_ObscheeZakaz_SizeChanged(object sender, EventArgs e)
        {
            int iWidth = dataGridView_ObscheeZakaz.Size.Width;
            dataGridView_ObscheeZakaz.Columns[0].Width = 30;
            dataGridView_ObscheeZakaz.Columns[1].Width = 30;

            dataGridView_ObscheeZakaz.Columns[2].Width = (iWidth - 70) / 3;
            dataGridView_ObscheeZakaz.Columns[4].Width = (iWidth - 70) / 3;

            dataGridView_ObscheeZakaz.Columns[3].Width = ((iWidth - 70) / 3) / 4;
            dataGridView_ObscheeZakaz.Columns[5].Width = ((iWidth - 70) / 3) / 4;
            dataGridView_ObscheeZakaz.Columns[6].Width = ((iWidth - 70) / 3) / 4;
            dataGridView_ObscheeZakaz.Columns[7].Width = ((iWidth - 70) / 3) / 4;
        }

        private void dataGridView_ObscheeProduct_SizeChanged(object sender, EventArgs e)
        {
            /*int iWidth = dataGridView_ObscheeProduct.Size.Width;
           // dataGridView_ObscheeProduct.Columns["ObscheeProduct"].Width = (iWidth - 35) / 5;

            iWidth = ((iWidth - 35) / 5) / 3;

            dataGridView_ObscheeProduct.Columns["ObscheeRezkaCountRylon"].Width = iWidth + 10;
            dataGridView_ObscheeProduct.Columns["ObscheeRezkaNetto"].Width = iWidth - 5;
            dataGridView_ObscheeProduct.Columns["ObscheeRezkaBrytto"].Width = iWidth - 5;

            dataGridView_ObscheeProduct.Columns["ObscheePrinyatoCountRylon"].Width = iWidth + 10;
            dataGridView_ObscheeProduct.Columns["ObscheePrinyatoNetto"].Width = iWidth - 5;
            dataGridView_ObscheeProduct.Columns["ObscheePrinyatoBrytto"].Width = iWidth - 5;


            dataGridView_ObscheeProduct.Columns["ObscheeOtgryzCountRylon"].Width = iWidth + 10;
            dataGridView_ObscheeProduct.Columns["ObscheeOtgryzNetto"].Width = iWidth - 5;
            dataGridView_ObscheeProduct.Columns["ObscheeOtgryzBrytto"].Width = iWidth - 5;

            dataGridView_ObscheeProduct.Columns["ObscheeOstatkiCountRylon"].Width = iWidth + 10;
            dataGridView_ObscheeProduct.Columns["ObscheeOstatkiNetto"].Width = iWidth - 5;
            dataGridView_ObscheeProduct.Columns["ObscheeOstatkiBrytto"].Width = iWidth - 5;*/
        }

        private void dataGridView_ObscheeZakaz_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           /* if (e.RowIndex != -1)
            {
                DataGridViewCheckBoxCell ch1 = new DataGridViewCheckBoxCell();
                ch1 = (DataGridViewCheckBoxCell)dataGridView_ObscheeZakaz.Rows[e.RowIndex].Cells[0];

                if (ch1.Value == null)
                    ch1.Value = false;
                switch (ch1.Value.ToString())
                {
                    case "True":
                        ch1.Value = false;
                        m_iCountCheckedZakaz--;
                        break;
                    case "False":
                        ch1.Value = true;
                        m_iCountCheckedZakaz++;
                        break;
                }
            }*/
        }

        private void dataGridView_ObscheeZakaz_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView data = (DataGridView)sender;

            if (e.RowIndex != -1 && m_iCountCheckedZakaz == 0 && e.RowIndex == data.CurrentRow.Index)
            {
                SelectObscheeProduct();
            }
        }

        private void SelectObscheeProduct()
        {
            if (m_bObscheeSelectZakaz == true && dataGridView_ObscheeZakaz.CurrentRow.Index != -1 )
            {
                int iRowIndex = -1;
                int iZakazID = -1;
                m_dicObscheeProduct.Clear();
                m_dicObscheeProductInfo.Clear();

                prObscheeInfo = new stProductInfoObschee();
                stRezka = new stProductInfoSmall();
                stPrinyato = new stProductInfoSmall();
                stOtgryjeno = new stProductInfoSmall();
                stOstatki = new stProductInfoSmall();
                dataGridView_ObscheeProduct.Rows.Clear();

                for (int i = 0; i < dataGridView_ObscheeZakaz.Rows.Count; i++)
                {
                    DataGridViewCheckBoxCell ch1 = new DataGridViewCheckBoxCell();
                    ch1 = (DataGridViewCheckBoxCell)dataGridView_ObscheeZakaz.Rows[i].Cells[0];
                    if (ch1.Value == null)
                        ch1.Value = false;
                    switch (ch1.Value.ToString())
                    {
                        case "True":
                            iRowIndex = i;
                            iZakazID = m_dicObscheeZakaz[dataGridView_ObscheeZakaz[1, iRowIndex].Value.ToString() + "-" + dataGridView_ObscheeZakaz[2, iRowIndex].Value.ToString()];
                            string strNumber = dataGridView_ObscheeZakaz.Rows[iRowIndex].Cells[1].Value.ToString().Trim();

                            string strQuery = "select distinct pr.product_name, pr.id " +
                                  "from itak_etiketka.dbo.itak_product pr, itak_etiketka.dbo.itak_vihidrylon vh " +
                                  "where vh.product_id=pr.id and zakaz_id=" + iZakazID;

                            try
                            {
                                m_MSSQLCommand.CommandText = strQuery;
                                m_MSSQLReader = m_MSSQLCommand.ExecuteReader();

                                if (m_MSSQLReader.HasRows)
                                {
                                    while (m_MSSQLReader.Read())
                                        m_dicObscheeProduct.Add(strNumber + " - " + m_MSSQLReader[0].ToString().Trim(), Convert.ToInt32(m_MSSQLReader[1]));
                                }
                                m_MSSQLReader.Close();

                                if (m_dicObscheeProduct.Count != 0)
                                {
                                    foreach (KeyValuePair<string, int> key in m_dicObscheeProduct)
                                    {
                                        strQuery = "select COUNT(id), ROUND(sum(brytto),2) , ROUND(sum(brytto-vagatary),2), SUM(koletiketki) from itak_etiketka.dbo.itak_vihidrylon where zakaz_id=" + iZakazID + " and product_id=" + key.Value;
                                        SelectInfoObscheeProduct(ref stRezka, strQuery);

                                        strQuery = "select COUNT(vh.id), ROUND(sum(vh.brytto),2) , ROUND(sum(vh.brytto-vh.vagatary),2), SUM(vh.koletiketki) " +
                                           " from itak_etiketka.dbo.itak_vihidrylon vh, itak_etiketka.dbo.itak_sklad s " +
                                            " where vh.zakaz_id=" + iZakazID + " and vh.product_id=" + key.Value + " and s.rylon_id=vh.id";
                                        SelectInfoObscheeProduct(ref stPrinyato, strQuery);

                                        strQuery = "select COUNT(vh.id), ROUND(sum(vh.brytto),2) , ROUND(sum(vh.brytto-vh.vagatary),2), SUM(vh.koletiketki) " +
                                           " from itak_etiketka.dbo.itak_vihidrylon vh, itak_etiketka.dbo.itak_sklad s " +
                                            " where vh.zakaz_id=" + iZakazID + " and vh.product_id=" + key.Value + " and s.rylon_id=vh.id and s.rylon_state=2";
                                        SelectInfoObscheeProduct(ref stOtgryjeno, strQuery);

                                        strQuery = "select COUNT(vh.id), ROUND(sum(vh.brytto),2) , ROUND(sum(vh.brytto-vh.vagatary),2), SUM(vh.koletiketki) " +
                                           " from itak_etiketka.dbo.itak_vihidrylon vh, itak_etiketka.dbo.itak_sklad s " +
                                            " where vh.zakaz_id=" + iZakazID + " and vh.product_id=" + key.Value + " and s.rylon_id=vh.id and s.rylon_state=1";
                                        SelectInfoObscheeProduct(ref stOstatki, strQuery);

                                        prObscheeInfo.iProductID = key.Value;
                                        prObscheeInfo.strProductName = key.Key;
                                        prObscheeInfo.stPrSmallRezka = stRezka;
                                        prObscheeInfo.stPrSmallPrinyatoRezka = stPrinyato;
                                        prObscheeInfo.stPrSmallOtgryjeno = stOtgryjeno;
                                        prObscheeInfo.stPrSmallOstatki = stOstatki;

                                        m_dicObscheeProductInfo.Add(prObscheeInfo.strProductName, prObscheeInfo);
                                    }

                                    m_dicObscheeProduct.Clear();
                                }
                            }
                            catch (System.Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                m_MSSQLReader.Close();
                            }

                            break;
                        case "False":
                            //MessageBox.Show("False");
                            break;
                    }
                }
            }

            m_iAllCountRylonRezka = m_iAllCountRylonPrinyato = m_iAllCountRylonOtgryjeno = m_iAllCountRylonOstatki = 0;
            m_dAllNettoRezka = m_dAllNettoPrinyato = m_dAllNettoOtgryjeno = m_dAllNettoOstatki = 0;
            m_dAllBryttoRezka = m_dAllBryttoPrinyato = m_dAllBryttoOtgryjeno = m_dAllBryttoOstatki = 0;

             int iCounter = 1;
            foreach (KeyValuePair<string, stProductInfoObschee> key in m_dicObscheeProductInfo)
            {
                dataGridView_ObscheeProduct.Rows.Add(iCounter.ToString(),
                                                    key.Key.Substring(key.Key.IndexOf('-')+1),
                                                    key.Value.stPrSmallRezka.iCountRylon.ToString(),
                                                    key.Value.stPrSmallRezka.dNetto.ToString("0.00"),
                                                    key.Value.stPrSmallRezka.dBrytto.ToString("0.00"),
                                                    key.Value.stPrSmallPrinyatoRezka.iCountRylon.ToString(),
                                                    key.Value.stPrSmallPrinyatoRezka.dNetto.ToString("0.00"),
                                                    key.Value.stPrSmallPrinyatoRezka.dBrytto.ToString("0.00"),
                                                    key.Value.stPrSmallOtgryjeno.iCountRylon.ToString(),
                                                    key.Value.stPrSmallOtgryjeno.dNetto.ToString("0.00"),
                                                    key.Value.stPrSmallOtgryjeno.dBrytto.ToString("0.00"),
                                                    key.Value.stPrSmallOstatki.iCountRylon.ToString(),
                                                    key.Value.stPrSmallOstatki.dNetto.ToString("0.00"),
                                                    key.Value.stPrSmallOstatki.dBrytto.ToString("0.00")
                                                    );

                m_iAllCountRylonRezka += key.Value.stPrSmallRezka.iCountRylon;
                m_dAllNettoRezka += key.Value.stPrSmallRezka.dNetto;
                m_dAllBryttoRezka += key.Value.stPrSmallRezka.dBrytto;

                m_iAllCountRylonPrinyato += key.Value.stPrSmallPrinyatoRezka.iCountRylon;
                m_dAllNettoPrinyato += key.Value.stPrSmallPrinyatoRezka.dNetto;
                m_dAllBryttoPrinyato += key.Value.stPrSmallPrinyatoRezka.dBrytto;

                m_iAllCountRylonOtgryjeno += key.Value.stPrSmallOtgryjeno.iCountRylon;
                m_dAllNettoOtgryjeno += key.Value.stPrSmallOtgryjeno.dNetto;
                m_dAllBryttoOtgryjeno += key.Value.stPrSmallOtgryjeno.dBrytto;

                m_iAllCountRylonOstatki += key.Value.stPrSmallOstatki.iCountRylon;
                m_dAllNettoOstatki += key.Value.stPrSmallOstatki.dNetto;
                m_dAllBryttoOstatki += key.Value.stPrSmallOstatki.dBrytto;

                iCounter++;
            }

            if (m_dicObscheeProductInfo.Count != 0)
            {
                dataGridView_ObscheeProduct.Rows.Add();
                dataGridView_ObscheeProduct.Rows.Add("",
                                                    "ОБЩЕЕ",
                                                        m_iAllCountRylonRezka.ToString(),
                                                        m_dAllNettoRezka.ToString("0.00"),
                                                        m_dAllBryttoRezka.ToString("0.00"),
                                                        m_iAllCountRylonPrinyato.ToString(),
                                                        m_dAllNettoPrinyato.ToString("0.00"),
                                                        m_dAllBryttoPrinyato.ToString("0.00"),
                                                        m_iAllCountRylonOtgryjeno.ToString(),
                                                        m_dAllNettoOtgryjeno.ToString("0.00"),
                                                        m_dAllBryttoOtgryjeno.ToString("0.00"),
                                                        m_iAllCountRylonOstatki.ToString(),
                                                        m_dAllNettoOstatki.ToString("0.00"),
                                                        m_dAllBryttoOstatki.ToString("0.00")
                                                        );

                if (dataGridView_ObscheeProduct.Rows.Count != 0)
                {
                    dataGridView_ObscheeProduct.Rows[dataGridView_ObscheeProduct.Rows.Count - 1].DefaultCellStyle.Font = new Font(dataGridView_ObscheeProduct.Font.FontFamily, 7, FontStyle.Bold);
                    dataGridView_ObscheeProduct.Rows[0].Selected = false;
                }
            }
            
        }

        private void dataGridViewSklad_AddProduct_SizeChanged(object sender, EventArgs e)
        {
            DataGridView data = (DataGridView)sender;
            int iWidth = data.ClientSize.Width;

            //int iWidth = data.Size.Width;
            iWidth -= 40;
            for (int i = 0; i < 9; i++)
            {
                if (cColumns[2].Visible == true)
                {
                    if (i == 0) cColumns[i].Width = 40;
                    else if (i >= 1 && i < 3) cColumns[i].Width = ((iWidth - (iWidth / 2)) / 2) - 13;
                    else if (i >= 3) cColumns[i].Width = (iWidth - (iWidth / 2)) / 6;
                }
                 else if (cColumns[2].Visible == false)
                 {
                     if (i == 0) cColumns[i].Width = 40;
                     else if (i == 1) cColumns[i].Width = ((iWidth - (iWidth / 2)));
                     else if (i >= 2) cColumns[i].Width = ((iWidth - (iWidth / 2)) / 4) - 8;
                 }
            }
        }

        private void dataGridViewSklad_OtgryzProduct_SizeChanged(object sender, EventArgs e)
        {
            DataGridView data = (DataGridView)sender;
            int iWidth = data.ClientSize.Width;

            //int iWidth = data.Size.Width;
            iWidth -= 40;
            for (int i = 0; i < 9; i++)
            {
                if (cColumns[2].Visible == true)
                {
                    if (i == 0) cColumns[i].Width = 40;
                    else if (i >= 1 && i < 3) cColumns[i].Width = ((iWidth - (iWidth / 2)) / 2) - 13;
                    else if (i >= 3) cColumns[i].Width = (iWidth - (iWidth / 2)) / 6;
                }
                else if (cColumns[2].Visible == false)
                {
                    if (i == 0) cColumns[i].Width = 40;
                    else if (i == 1) cColumns[i].Width = ((iWidth - (iWidth / 2)));
                    else if (i >= 2) cColumns[i].Width = ((iWidth - (iWidth / 2)) / 4) - 8;
                }
            }
        }

        private void dataGridViewSklad_OstatkiProduct_SizeChanged(object sender, EventArgs e)
        {
            DataGridView data = (DataGridView)sender;
            int iWidth = data.ClientSize.Width;

            //int iWidth = data.Size.Width;
            iWidth -= 40;
            for (int i = 0; i < 9; i++)
            {
                if (cColumns[2].Visible == true)
                {
                    if (i == 0) cColumns[i].Width = 40;
                    else if (i >= 1 && i < 3) cColumns[i].Width = ((iWidth - (iWidth / 2)) / 2) - 13;
                    else if (i >= 3) cColumns[i].Width = (iWidth - (iWidth / 2)) / 6;
                }
                else if (cColumns[2].Visible == false)
                {
                    if (i == 0) cColumns[i].Width = 40;
                    else if (i == 1) cColumns[i].Width = ((iWidth - (iWidth / 2)));
                    else if (i >= 2) cColumns[i].Width = ((iWidth - (iWidth / 2)) / 4) - 8;
                }
            }
        }



    }
}
