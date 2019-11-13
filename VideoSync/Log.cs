using System;
using System.IO;

namespace VideoSync
{
    class Log
    {
        private StreamWriter w;
        public Log()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public void Begin()
        {
            FileStream fileStream = new FileStream(System.Configuration.ConfigurationSettings.AppSettings["LogFileFolder"] + DateTime.Now.ToString("yyyyMMdd") + "_Log.txt", FileMode.OpenOrCreate, FileAccess.Write);
            w = new StreamWriter(fileStream);
            w.BaseStream.Seek(0, SeekOrigin.End);
            //w = System.IO.File.AppendText("Log_" + ".txt");
        }

        public void WriteLog(string msg)
        {
            w.WriteLine(DateTime.Now.ToString() + " : " + msg);
            w.Flush();
        }
        public void Flush()
        {
            w.Flush();
        }
        public void End()
        {
            w.Close();
            w.Dispose();
        }
    }
}
