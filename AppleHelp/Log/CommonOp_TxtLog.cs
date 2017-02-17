using System;
using System.IO;

namespace AppleHelp
{
    public partial class Log
    {
        /// <summary>
        /// 文件分区大小（kb）
        /// </summary>
        public static int PartFileSize = 512;

        /// <summary>
        /// 测试记录
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileflag"></param>
        /// <returns></returns>
        public static bool WriteBugLog(string content, string fileflag)
        {
            return WriteLog(content, string.Format("debuglog_{0}", fileflag));
        }
        /// <summary>
        /// 程序错误记录文件
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileflag"></param>
        /// <returns></returns>
        public static bool WriteBugLogWarning(string content, string fileflag)
        {
            return WriteLog(content, string.Format("debuglog_warning_{0}", fileflag));
        }

        /// <summary>
        /// 行记录（当前时间独占一行，内容独占一行，最后有换行）
        /// </summary>
        /// <param name="content">单次内容</param>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        public static bool WriteLog(string content, string filename)
        {
            return WriteWhiteLog(string.Format("[{0}]:\r\n{1}\r\n",
                        System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), content), filename);
        }

        /// <summary>
        /// 行前加当前时间记录（行之间无空行）
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool WriteLineTimeLog(string content, string filename)
        {
            return WriteWhiteLog(string.Format("[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), content), filename);
        }
        /// <summary>
        /// 行记录（行之间无空行）
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool WriteWhiteLog(string content, string filename)
        {
            try
            {
                string scpath = System.AppDomain.CurrentDomain.BaseDirectory;
                string fileext = ".txt";
                string filedir = scpath + "App_Data\\";
                string sfile = filedir + filename + fileext;
                if (File.Exists(sfile))
                {
                    FileInfo finfo = new FileInfo(sfile);
                    if (finfo.Length > PartFileSize * 1024)
                    {
                        string svdir = filedir + "logfilebak\\";
                        if (!Directory.Exists(svdir))
                        {
                            Directory.CreateDirectory(svdir);
                        }
                        finfo.MoveTo(string.Format("{0}{1}_{2}{3}", svdir, filename, DateTime.Now.ToString("yyMMdd_HHmmss"), fileext));
                    }
                }
                else
                {
                    if (!Directory.Exists(filedir))
                    {
                        Directory.CreateDirectory(filedir);
                    }
                }
                using (StreamWriter sw = new StreamWriter(sfile, true))
                {
                    sw.WriteLine(content);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}