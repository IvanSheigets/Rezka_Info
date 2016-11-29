using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RezkaInfo
{
    class Helper
    {
        static public void WriteLog(string err, System.Exception ex)
        {
            ///////log file
            string strError = "";
            string current_time_str;
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
    }
}
