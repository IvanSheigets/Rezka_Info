using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace PrintEtiketkaMy
{
    class PrintEtiketka
    {
        string m_strPrinterName="";
        int m_iPaperWidth = 0;
        int m_iPaperHeight = 0;
        bool m_bLandscape = false; //false - книжная ;true - альбомная
        short m_sNumCopies = 1;
        bool m_bPreview = false;

        PrintDocument docPrint;

        string m_strZakazchik = "";
        string m_strProduct = "";
        string m_strPartiya = "";
        int m_iNumCar = 0;
        int m_iNumSmena = 0;
        string m_strSmena = "";
        string m_strMaterial = "";
        int m_iTols = 0;
        int m_iProductWidth = 0;
        int m_iDlinaRylona = 0;
        int m_iCountEtiketki = 0;
        string m_strDate = "";
        int m_iNumRylona = 0;
        double m_dNetto = 0;
        double m_dBrytto = 0;
        int m_iProductId = 0;


        public void SetParametrs(string strZakazchik, string strProduct, string strPartiya, int iNumCar, int iNumSmena,
                                 string strMaterial, int iTols, int iProductWidth, int iDlinaRylona, int iCountEtiketki,
                                 string strDate, int iNumRylona, double dNetto, double dBrytto, int iProductID)
        {
            if (strZakazchik.Length!=0)
                m_strZakazchik = strZakazchik;
            else m_strZakazchik ="";

            if (strProduct.Length != 0)
                m_strProduct = strProduct;
            else m_strProduct = "";

            if (strPartiya.Length != 0)
                m_strPartiya = strPartiya;
            else m_strPartiya = "";

            if (iNumCar > 0)
                m_iNumCar = iNumCar;
            else m_iNumCar = 0;

            if (iNumSmena > 0)
                m_iNumSmena = iNumSmena;
            else m_iNumSmena = 0;

            m_strSmena = m_iNumCar + "/" + m_iNumSmena;

            if (strMaterial.Length != 0)
                m_strMaterial = strMaterial;
            else m_strMaterial = "";

            if (iTols >= 0)
                m_iTols = iTols;
            else m_iTols = 0;

            m_strMaterial = m_strMaterial + " " + m_iTols.ToString();

            if (iProductWidth >= 0)
                m_iProductWidth = iProductWidth;
            else m_iProductWidth = 0;

            if (iDlinaRylona >= 0)
                m_iDlinaRylona = iDlinaRylona;
            else m_iDlinaRylona = 0;

            if (iCountEtiketki >= 0)
                m_iCountEtiketki = iCountEtiketki;
            else m_iCountEtiketki = 0;

            if (strDate.Length != 0)
            {
                m_strDate = strDate;
                m_strDate = m_strDate.Substring(0, m_strDate.IndexOf(' '));
            }
            else m_strDate = "";

            if (iNumRylona >= 0)
                m_iNumRylona = iNumRylona;
            else m_iNumRylona = 0;

            if (dNetto >= 0)
                m_dNetto = dNetto;
            else m_dNetto = 0;

            if (dBrytto >= 0)
                m_dBrytto = dBrytto;
            else m_dBrytto = 0;

            if (iProductID >= 0)
                m_iProductId = iProductID;
            else m_iProductId = 0;

        }

        public void SetPrinterName(string strPrinterName)
        {
            if (strPrinterName.Length != 0)
                m_strPrinterName = strPrinterName;
            else m_strPrinterName = "";
        }

        

        public void SetPaperSize(int iPaperWidth, int iPaperHeight)
        {
            if (iPaperWidth != 0)
                m_iPaperWidth = iPaperWidth;
            if (iPaperHeight != 0)
                m_iPaperHeight = iPaperHeight;
        }

        public void SetLandscape(bool bLandscape)
        {   m_bLandscape = bLandscape;  }

        public void SetNumCopies(short sNumCopies)
        {
            if (sNumCopies > 0)
                m_sNumCopies = sNumCopies;
        }   

        public void SetPreview(bool bPreview)
        {
            m_bPreview = bPreview;
        }

        public bool SetPrinter(string strPrinterName, int iPaperWidth, int iPaperHeight, bool bLandscape)
        {
            SetPrinterName(strPrinterName);
            SetPaperSize(iPaperWidth, iPaperHeight);
            SetLandscape(bLandscape);

            return SetPrinter();
        }

        private bool SetPrinter()
        {
            docPrint = new PrintDocument();
            docPrint.PrinterSettings.PrinterName = m_strPrinterName;

            if (docPrint.PrinterSettings.IsValid)
            {
                PaperSize sz = new PaperSize();
                sz.Height = (int)(m_iPaperHeight / 0.254);
                sz.Width = (int)(m_iPaperWidth / 0.254);
                docPrint.DefaultPageSettings.PaperSize = sz; //установка размера бумаги

                docPrint.DefaultPageSettings.Landscape = m_bLandscape;

                docPrint.DefaultPageSettings.Margins.Top = 0;
                docPrint.DefaultPageSettings.Margins.Left = 0;
                docPrint.DefaultPageSettings.Margins.Bottom = 0;
                docPrint.DefaultPageSettings.Margins.Right = 0;

                docPrint.PrinterSettings.Copies = m_sNumCopies;

                docPrint.PrintPage += new PrintPageEventHandler
                       (this.docPrint_PrintPage);

                PrintPreviewDialog dlgPrint = new PrintPreviewDialog();
                dlgPrint.Document = docPrint;

                if (m_bPreview == true)
                    dlgPrint.ShowDialog();
                else docPrint.Print();

                return true;
            }
            else
                return false;

        }

        private void docPrint_PrintPage(object sender, PrintPageEventArgs e)
        {
            Pen mP = new Pen(Color.Black, 1);
            System.Drawing.Font fnt10Bold = new Font("Times New Roman", 10, FontStyle.Bold);
            System.Drawing.Font fnt10 = new Font("Times New Roman", 10, FontStyle.Regular);
            System.Drawing.Font fnt10Under = new Font("Times New Roman", 10, FontStyle.Underline);

            System.Drawing.Font fnt9Bold = new Font("Times New Roman", 9, FontStyle.Bold);
            System.Drawing.Font fnt9 = new Font("Times New Roman", 9, FontStyle.Regular);
            System.Drawing.Font fnt9Under = new Font("Times New Roman", 9, FontStyle.Underline);

            System.Drawing.Font fnt8Bold = new Font("Times New Roman", 8, FontStyle.Bold);
            System.Drawing.Font fnt8 = new Font("Times New Roman", 8, FontStyle.Regular);
            System.Drawing.Font fnt8Under = new Font("Times New Roman", 8, FontStyle.Underline);

            int iLeft = (int)e.PageSettings.PrintableArea.Left + 3;
            int iTop = (int)e.PageSettings.PrintableArea.Top;
            int iRight = (int)e.PageSettings.PrintableArea.Right - 7;
            int iBottom = (int)e.PageSettings.PrintableArea.Bottom - 10;

            e.Graphics.DrawString("ТОВ \"ІТАК\"", fnt8Bold, Brushes.Black, 80, iTop);

            iTop += 12;
            e.Graphics.DrawString("м.Київ, вул. Червоноткацька, 44", fnt8, Brushes.Black, 37, iTop);

            iTop += 14;
            e.Graphics.DrawString("Етикетка для пакування", fnt9Bold, Brushes.Black, 45, iTop);

            iTop += 18;
            e.Graphics.DrawString(m_strZakazchik, fnt9, Brushes.Black, iLeft, iTop);

            iTop += 15;
            e.Graphics.DrawString(m_strProduct, fnt9, Brushes.Black, iLeft, iTop);

            iTop += 15;
            e.Graphics.DrawString("ТУ У 22.1-16476839-001-2004", fnt9, Brushes.Black, iLeft, iTop);

            iTop += 15;
            e.Graphics.DrawString("Партія", fnt9, Brushes.Black, iLeft, iTop);
            e.Graphics.DrawString(m_strPartiya, fnt9Under, Brushes.Black, iLeft + 40, iTop);

            e.Graphics.DrawString("Зміна", fnt9, Brushes.Black, iLeft + 140, iTop);
            e.Graphics.DrawString(m_strSmena, fnt9Under, Brushes.Black, iLeft + 175, iTop);

            iTop += 16;
            e.Graphics.DrawString(m_strMaterial + "x" + m_iProductWidth.ToString(), fnt9, Brushes.Black, iLeft, iTop);

            iTop += 14;
            e.Graphics.DrawString(m_iDlinaRylona.ToString() + " м.п.", fnt9, Brushes.Black, iLeft, iTop);
            e.Graphics.DrawString(m_iCountEtiketki.ToString() + " шт.", fnt9, Brushes.Black, iLeft + 120, iTop);

            iTop += 15;
            e.Graphics.DrawString(m_strDate.ToString(), fnt9, Brushes.Black, iLeft, iTop);
            e.Graphics.DrawString("№ рулона", fnt9, Brushes.Black, iLeft + 120, iTop);
            e.Graphics.DrawString(m_iNumRylona.ToString(), fnt9Under, Brushes.Black, iLeft + 175, iTop);

            iTop += 18;
            e.Graphics.DrawString("Маса нетто", fnt10Bold, Brushes.Black, iLeft, iTop);
            e.Graphics.DrawString("_  " + m_dNetto.ToString("0.00") + "  _", fnt10Under, Brushes.Black, iLeft + 90, iTop);

            iTop += 18;
            e.Graphics.DrawString("Маса брутто", fnt10Bold, Brushes.Black, iLeft, iTop);
            e.Graphics.DrawString("_  " + m_dBrytto.ToString("0.00") + "  _", fnt10Under, Brushes.Black, iLeft + 90, iTop);

            iTop += 18;
            e.Graphics.DrawString("Гарантійний термін зберігання - 12 міс.", fnt8, Brushes.Black, iLeft + 10, iTop);

            /*Code39Settings st = new Code39Settings();
            st.BarCodeHeight = 40;
            st.NarrowWidth = 1;
            st.WideWidth = 3;
            st.DrawText = false;
            st.TopMargin = 0;
            st.LeftMargin = 0;
            Code39 code = new Code39(m_iProductId.ToString(), st);

            MemoryStream destination = new MemoryStream();
            code.Paint().Save(destination, ImageFormat.Png);
            iTop += 17;
            Image img = Image.FromStream(destination);
            e.Graphics.DrawImage(img, 10, iTop);*/

            Dictionary<char, string> a = new Dictionary<char, string>();
           /* a.Add('0', "bwbwwwbbbwbbbwbw");
            a.Add('1', "bbbwbwwwbwbwbbbw");
            a.Add('2', "bwbbbwwwbwbwbbbw");
            a.Add('3', "bbbwbbbwwwbwbwbw");
            a.Add('4', "bwbwwwbbbwbwbbbw");
            a.Add('5', "bbbwbwwwbbbwbwbw");
            a.Add('6', "bwbbbwwwbbbwbwbw");
            a.Add('7', "bwbwwwbwbbbwbbbw");
            a.Add('8', "bbbwbwwwbwbbbwbw");
            a.Add('9', "bwbbbwwwbwbbbwbw");
            a.Add('*', "bwwwbwbbbwbbbwbw");*/


            a.Add('*', "wnnwnwwnwwnwnn");
            a.Add('0', "wnwnnwwnwwnwnn");
            a.Add('1', "wwnwnnwnwnwwnn");
            a.Add('2', "wnwwnnwnwnwwnn");
            a.Add('3', "wwnwwnnwnwnwnn");
            a.Add('4', "wnwnnwwnwnwwnn");
            a.Add('5', "wwnwnnwwnwnwnn");
            a.Add('6', "wnwwnnwwnwnwnn");
            a.Add('7', "wnwnnwnwwnwwnn");
            a.Add('8', "wwnwnnwnwwnwnn");
            a.Add('9', "wnwwnnwnwwnwnn");

            iTop += 17;
            int iHeight = iTop + 45;
            int x = 10;

            string strPrintBar = "*3" + m_iProductId.ToString() + "*";

            //string strPrintBar = "*0123456789*";
            int iLenght = strPrintBar.Length;

            for (int i = 0; i < iLenght; i++)
            {
                string strSymbol = strPrintBar.Substring(0, 1);
                strPrintBar = strPrintBar.Substring(1);
                string strCodeSymbol = a[strSymbol[0]];
                int ssL = strCodeSymbol.Length;

                for (int j = 0; j < ssL; j++)
                {
                    /*if (strCodeSymbol[j] == 'b')
                        e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(x, iTop), new Point(x, iHeight));
                    else if (strCodeSymbol[j] == 'w')
                        e.Graphics.DrawLine(new Pen(Color.White, 1), new Point(x, iTop), new Point(x, iHeight));*/

                    if (strCodeSymbol[j] == 'w')
                        e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(x, iTop), new Point(x, iHeight));
                    else if (strCodeSymbol[j] == 'n')
                        e.Graphics.DrawLine(new Pen(Color.White, 1), new Point(x, iTop), new Point(x, iHeight));

                    x += 1;
                }
            }

            iTop += 45;
            e.Graphics.DrawString("3"+m_iProductId.ToString(), fnt9, Brushes.Black, iLeft + 50, iTop);
        }
    }
}

