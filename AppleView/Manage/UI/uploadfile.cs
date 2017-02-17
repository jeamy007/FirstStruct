using System;
using System.IO;
using System.Web;

namespace AppleView
{
    public class uploadfile
    {
        int i = 0;     
        public uploadfile() { }
 
        /// <summary>
        /// 图片相对路径
        /// </summary>
        public string ServerPictureUrl;
      

        public string UpLoad(HttpPostedFile file,string Dir)
        {
            if (file == null || string.IsNullOrEmpty(file.FileName))
                return string.Empty;
            string SvPath = System.Web.HttpContext.Current.Server.MapPath("/");
            string ExtenName = Path.GetExtension(file.FileName);//获取扩展名
            string url = Dir+"/";  //文件保存的路径  没有后缀
            string path = Path.Combine(SvPath, url);


            //url += DateTime.Now.ToString("yyyyMMddHHmmss");
            url += Guid.NewGuid().ToString("N").ToLower();
            string SaveFileName = Path.Combine(SvPath, url + ExtenName);//合并两个路径为上传到服务器上的全路径 

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                if (File.Exists(SaveFileName))
                {
                    url = url + i;
                    SaveFileName = Path.Combine(SvPath, url + ExtenName);
                    i++;
                }
            }
            file.SaveAs(SaveFileName);
            ServerPictureUrl = "/" + url + ExtenName;
            return ServerPictureUrl;
        }
    }
}
