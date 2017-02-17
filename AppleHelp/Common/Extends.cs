using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

public static class Extends
{
    #region 私有方法

    private static object GetDefaultValue(object obj, Type type)
    {
        if (obj == DBNull.Value)
        {
            return default(object);
        }
        else
        {
            return Convert.ChangeType(obj, type);
        }

    }

    #endregion


    #region 公用方法

    #region ========加密========

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="Text"></param>
    /// <returns></returns>
    public static string Encrypt(this string Text)
    {
        return Encrypt(Text, "xj");
    }
    /// <summary> 
    /// 加密数据 
    /// </summary> 
    /// <param name="Text"></param> 
    /// <param name="sKey"></param> 
    /// <returns></returns> 
    public static string Encrypt(this string Text, string sKey)
    {
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        byte[] inputByteArray;
        inputByteArray = Encoding.Default.GetBytes(Text);
        des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
        des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();
        StringBuilder ret = new StringBuilder();
        foreach (byte b in ms.ToArray())
        {
            ret.AppendFormat("{0:X2}", b);
        }
        return ret.ToString();
    }

    #endregion

    #region ========解密========


    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="Text"></param>
    /// <returns></returns>
    public static string Decrypt(this string Text)
    {
        return Decrypt(Text, "xj");
    }
    /// <summary> 
    /// 解密数据 
    /// </summary> 
    /// <param name="Text"></param> 
    /// <param name="sKey"></param> 
    /// <returns></returns> 
    public static string Decrypt(this string Text, string sKey)
    {
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        int len;
        len = Text.Length / 2;
        byte[] inputByteArray = new byte[len];
        int x, i;
        for (x = 0; x < len; x++)
        {
            i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
            inputByteArray[x] = (byte)i;
        }
        des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
        des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();
        return Encoding.Default.GetString(ms.ToArray());
    }

    #endregion


    public static T JsonDeserializeObject<T>(this string jsonString)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
    }
    public static string JsonSerializeObject(this object obj)
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
    }


    public static int ToInt(this string str)
    {
        int result = -1;
        if (Int32.TryParse(str, out result))
        {
            return result;
        }
        else
        {
            return Convert.ToInt32(str);
        }

    }

    public static double ToDouble(this string str)
    {
        double result = -1;
        if (Double.TryParse(str, out result))
        {
            return result;
        }
        else
        {
            return Convert.ToDouble(str);
        }

    }

    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }


    /// <summary>
    /// DateTime时间格式转换为Unix时间戳格式
    /// </summary>
    /// <param name=”time”></param>
    /// <returns></returns>
    public static int ToUnixTime(this DateTime time)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        return (int)(time - startTime).TotalSeconds;
    }

    /// <summary>
    /// DataSet 第一个表转换为实体类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ds"></param>
    /// <returns>实体类泛型集合</returns>
    public static List<T> ToList<T>(this DataSet ds)
    {
        if (ds.Tables.Count == 0)
            return new List<T>();

        return ToList<T>(ds.Tables[0]);
    }

    /// <summary>
    /// DataTable 第一个表转换为实体类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dt"></param>
    /// <returns>实体类泛型集合</returns>
    public static List<T> ToList<T>(this DataTable dt)
    {
        List<T> l = new List<T>();

        foreach (DataRow dr in dt.Rows)
        {
            T model = Activator.CreateInstance<T>();

            foreach (DataColumn dc in dr.Table.Columns)
            {
                PropertyInfo pi = model.GetType().GetProperty(dc.ColumnName);
                if (pi == null) continue;
                if (pi.CanWrite == false) continue;

                try
                {
                    if (dr[dc.ColumnName] != DBNull.Value)
                        pi.SetValue(model, dr[dc.ColumnName], null);
                    else
                        pi.SetValue(model, default(object), null);
                }
                catch
                {
                    pi.SetValue(model, GetDefaultValue(dr[pi.Name], pi.PropertyType), null);
                }

            }
            l.Add(model);
        }

        return l;
    }

    /// <summary>
    /// 将DATAROW转化为List对象
    /// </summary>
    /// <typeparam name="T">需要转化的对象</typeparam>
    /// <param name="dr">DataRow对象</param>
    /// <returns>转换后的对象</returns>
    public static T ToList<T>(this DataRow dr) where T : new()
    {
        T t = new T();
        Type modelType = t.GetType();
        foreach (PropertyInfo pi in modelType.GetProperties())
        {
            if (pi == null) continue;
            if (pi.CanWrite == false) continue;

            if (dr.Table.Columns.Contains(pi.Name))
            {
                try
                {
                    if (dr[pi.Name] != DBNull.Value)
                        pi.SetValue(t, dr[pi.Name], null);
                    else
                        pi.SetValue(t, default(object), null);
                }
                catch
                {
                    pi.SetValue(t, GetDefaultValue(dr[pi.Name], pi.PropertyType), null);
                }
            }

        }
        return t;
    }

    /// <summary> 
    /// 将实体类转换成DataTable 
    /// </summary> 
    /// <typeparam name="T"></typeparam> 
    /// <param name="i_objlist"></param> 
    /// <returns></returns> 
    public static DataTable ToDataTable<T>(this IList<T> objlist)
    {
        if (objlist == null || objlist.Count <= 0)
        {
            return null;
        }
        DataTable dt = new DataTable(typeof(T).Name);
        DataColumn column;
        DataRow row;

        System.Reflection.PropertyInfo[] myPropertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (T t in objlist)
        {
            if (t == null)
            {
                continue;
            }

            row = dt.NewRow();

            for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
            {
                System.Reflection.PropertyInfo pi = myPropertyInfo[i];

                string name = pi.Name;

                if (dt.Columns[name] == null)
                {
                    column = new DataColumn(name, pi.PropertyType);
                    dt.Columns.Add(column);
                }

                row[name] = pi.GetValue(t, null);
            }

            dt.Rows.Add(row);
        }
        return dt;
    }

    /// <summary>
    /// 字符串格式化
    /// </summary>
    /// <param name="str">需要格式化的字符串</param>
    /// <param name="param">参数</param>
    /// <returns>格式化之后的字符串</returns>
    public static string ForamtString(this string str, params object[] param)
    {
        return string.Format(str, param);
    }


     

    #endregion
}
