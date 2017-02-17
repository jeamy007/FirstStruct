<%@ Page Language="C#" %>
<%@ Import Namespace="System.Text" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="SDIHelper" %>
<!DOCTYPE html>
<html>
<head>
    <title>tool</title>
    <style>
        html,body{width:100%; height:100%;padding:0px;margin:0px}
        textarea{width:100%; height:100%}
    </style>
</head>
<body>
<% 
    var dbop = SDI.Global.Dbop;

    string tbname = "WA_CancelRight";
    int sidx = 1;
    string[] a_stringtype = new string[] { "nvarchar", "varchar", "datetime","char","text" };
    string[] a_escapefield = new string[] { "CreateDate", "ModifyDate" };
    string s_objexp = "obj.";


    StringBuilder sql = new StringBuilder();
    sql.AppendFormat("declare @name sysname='{0}'\r\n", tbname);
    sql.Append(@"declare @id int
select @id=id from sysobjects where type='U' and name=@name
if @id>0 
begin
  print @id
  select sc.name,s.name tname,e.value remark,sc.length from syscolumns sc
  left join systypes s on s.xusertype=sc.xtype
  left join sys.extended_properties e on e.major_id=@id and e.minor_id=sc.colid and e.class=1
  where sc.id=@id
end 
else
  select 1 a where 1=0");
    var dt = dbop.GetDataTable(sql.ToString());
    if (dt.Rows.Count > 0)
    {
        List<string> ls_temp, ls_temp_fn, ls_temp_i, ls_temp_u, ls_temp_m1, ls_temp_m2,
            ls_result = new List<string>(),
            ls_fieldname = new List<string>(),
            ls_fieldtype = new List<string>(),
            ls_fielddes = new List<string>();
        string pk = string.Empty;
        foreach (DataRow dr in dt.Rows)
        {
            ls_fieldname.Add(dr["name"] + "");
            ls_fieldtype.Add(dr["tname"] + "");
            ls_fielddes.Add(dr["remark"] + "");
        }
        pk = ls_fieldname[0];
        string summary =
@"/// <summary>
    /// {0}
    /// </summary>
    ";
        string s_fields = string.Join(",", ls_fieldname);
        // select 
        //sql.Append("【SQL SELECT】");
        //sql.Append("\r\n");
        //ls_result.Add(string.Format("SELECT {0} FROM {1}", s_fields, tbname));

        // insert
        #region insert
        StringBuilder sql_insert = new StringBuilder();
        sql = new StringBuilder();
        //sql.Append("【SQL INSERT】");
        //sql.Append("\r\n");
        ls_temp_fn = new List<string>();
        ls_temp_m1 = new List<string>();
        ls_temp_m2 = new List<string>();
        // format值 和 值
        ls_temp = new List<string>();
        ls_temp_i = new List<string>();
        // update
        ls_temp_u = new List<string>();
        int j = 0;
        bool isStringTp;
        for (int i = 0; i < ls_fieldname.Count; i++)
        {
            string _fn = ls_fieldname[i], _fv, _fv_u;
            string _fnt = ls_fieldtype[i];
            string _fnCT = string.Empty;
            string _fnr = string.IsNullOrEmpty(ls_fielddes[i]) ? _fn : ls_fielddes[i];
            isStringTp = Array.IndexOf(a_stringtype, _fnt) >= 0;
            switch (_fnt)
            {
                case "nvarchar":
                case "varchar":
                case "char":
                case "nchar":
                case "text":
                    _fnCT = "string";
                    ls_temp_m1.Add(string.Format("{0} = dr[\"{0}\"] + \"\"", _fn));
                    break;
                case "int":
                    _fnCT = "int";
                    ls_temp_m1.Add(string.Format("{0} = CommonOp.SafeObjectInt(dr[\"{0}\"])", _fn));
                    break;
                case "decimal":
                    _fnCT = "decimal";
                    ls_temp_m1.Add(string.Format("{0} = CommonOp.SafeObjectDecimal(dr[\"{0}\"])", _fn));
                    break;
                case "float":
                    _fnCT = "float";
                    ls_temp_m1.Add(string.Format("{0} = CommonOp.SafeObjectFloat(dr[\"{0}\"])", _fn));
                    break;
                case "datetime":
                case "date":
                    _fnCT = "DateTime";
                    ls_temp_m1.Add(string.Format("{0} = CommonOp.SafeObjectDateTime(dr[\"{0}\"])", _fn));
                    break;
                default:
                    _fnCT = "string";
                    ls_temp_m1.Add(string.Format("{0} = dr[\"{0}\"] + \"\"", _fn));
                    break;
            }
            ls_temp_m2.Add(string.Format(summary, _fnr) + string.Format("public {1} {0} ", _fn, _fnCT) + "{ get; set; }");

            if (i < sidx)
            {
                continue;
            }
            if (a_escapefield.Length > 0 && Array.IndexOf(a_escapefield, _fn) >= 0)
            {
                continue;
            }
            ls_temp_fn.Add(_fn);
            if (isStringTp)
            {
                _fv = "'{" + j + "}'";
                _fv_u = "'{0}'";
            }
            else
            {
                _fv = "{" + j + "}";
                _fv_u = "{0}";
            }
            ls_temp.Add(_fv);
            ls_temp_u.Add(string.Format("            sql.AppendFormat(\"{0}={1},\", {2});", _fn, _fv_u, s_objexp + _fn));
            ls_temp_i.Add(s_objexp + _fn);
            j++;
        }
        //sql_insert.Append("            StringBuilder sql = new StringBuilder();");
        //sql_insert.Append("\r\n");
        sql_insert.AppendFormat("            sql.Append(\"INSERT {0} ({1}) VALUES\");", tbname, string.Join(",", ls_temp_fn));
        sql_insert.Append("\r\n");
        sql_insert.AppendFormat("            sql.AppendFormat(\"({0})\"", string.Join(",", ls_temp));
        sql_insert.Append("\r\n");
        sql_insert.AppendFormat("                ,{0});", string.Join(", ", ls_temp_i));
        #endregion
        //ls_result.Add(sql.ToString());

        // update

        // model create
        sql = new StringBuilder();
        sql.Append(@"
using System;
using System.Data;
using System.Text;

");
        //sql.Append("【MODEL CONSTRUCT】");
        //sql.Append("\r\n");
        sql.AppendFormat("public partial class {0} : SDIModelDbop.ModelDbopBaseT<{0}>", tbname.Substring(3));
        sql.Append("\r\n");
        sql.Append("{");
        sql.Append("\r\n    ");
        sql.Append("#region 基本属性");
        sql.Append("\r\n    ");
        sql.AppendFormat("{0}", string.Join("\r\n\r\n    ", ls_temp_m2));
        sql.Append("\r\n    ");
        sql.Append("#endregion");
        sql.Append("\r\n");
        sql.Append("}");

        sql.Append("\r\n");
        sql.Append("\r\n");
        sql.AppendFormat("public partial class {0}", tbname);
        sql.Append("\r\n");
        sql.Append("{");
        //sql.Append("\r\n    ");
        //sql.Append("#region 构造函数");
        //sql.Append("\r\n    ");
        //sql.AppendFormat("public {0}()", tbname).Append(" { }");
        //sql.Append("\r\n");
        //sql.Append("\r\n    ");
        //sql.AppendFormat("public {0}(System.Data.DataRow dr)", tbname);
        //sql.Append("\r\n    ");
        //sql.Append("{");
        //sql.Append("\r\n        ");
        //sql.AppendFormat("return {0}.Get(dr);", tbname);
        //sql.Append("\r\n    ");
        //sql.Append("}");
        //sql.Append("\r\n    ");
        //sql.Append("#endregion");
        //sql.Append("\r\n");

        //sql.Append("\r\n");
        sql.Append("\r\n    ");
        sql.Append("#region 内部函数");
        sql.Append("\r\n    ");
        sql.Append("public bool Modify()");
        sql.Append("\r\n    ");
        sql.Append("{");
        sql.Append("\r\n        ");
        sql.AppendFormat("return {0}.Modify(this);", tbname);
        sql.Append("\r\n    ");
        sql.Append("}");
        sql.Append("\r\n");
        sql.Append("\r\n    ");
        sql.Append("public bool Delete()");
        sql.Append("\r\n    ");
        sql.Append("{");
        sql.Append("\r\n        ");
        sql.AppendFormat("return {0}.Delete(this);", tbname);
        sql.Append("\r\n    ");
        sql.Append("}");
        sql.Append("\r\n    ");
        sql.Append("#endregion");
        sql.Append("\r\n");

        sql.Append("\r\n    ");
        sql.Append("#region 公共函数");
        sql.Append("\r\n    ");
        sql.AppendFormat("public static {0} Get(int id)", tbname);
        sql.Append("\r\n    ");
        sql.Append("{");
        sql.Append("\r\n        ");
        sql.Append("string sql = string.Format(");
        sql.Append("\r\n");
        sql.Append("@\"");
        sql.AppendFormat("SELECT {0} ", s_fields);
        sql.Append("\r\n  ");
        sql.AppendFormat("FROM {0} ", tbname);
        sql.Append("\r\n  ");
        sql.AppendFormat("WHERE {0}=", pk).Append("{0}");
        sql.Append("\", id);");
        sql.Append("\r\n        ");
        sql.Append("DataTable dt = DbHelper.Dbop.GetDataTable(sql);");
        sql.Append("\r\n        ");
        sql.Append("if (dt.Rows.Count == 0)");
        sql.Append("\r\n        ");
        sql.Append("{");
        sql.Append("\r\n            ");
        sql.Append("return null;");
        sql.Append("\r\n        ");
        sql.Append("}");
        sql.Append("\r\n        ");
        sql.Append("DataRow dr = dt.Rows[0];");
        sql.Append("\r\n        ");
        sql.Append("return Get(dr);");
        sql.Append("\r\n    ");
        sql.Append("}");
        sql.Append("\r\n    ");
        sql.AppendFormat("public static {0} Get(System.Data.DataRow dr)", tbname);
        sql.Append("\r\n    ");
        sql.Append("{");
        sql.Append("\r\n        ");
        sql.AppendFormat("return new {0} ", tbname);
        sql.Append("{\r\n            ");
        sql.AppendFormat("{0}", string.Join(",\r\n            ", ls_temp_m1));
        sql.Append("\r\n        ");
        sql.Append("};");
        sql.Append("\r\n    ");
        sql.Append("}");
        sql.Append("\r\n");
        sql.Append("\r\n    ");
        sql.AppendFormat("public static bool Modify({0} obj)", tbname);
        sql.Append("\r\n    ");
        sql.Append("{");
        sql.Append("\r\n        ");
        sql.Append("StringBuilder sql = new StringBuilder();");
        sql.Append("\r\n        ");
        sql.AppendFormat("if (obj.{0} > 0)", pk);
        sql.Append("\r\n        {");
        sql.Append("\r\n");
        sql.Append("\r\n        }");
        sql.Append("\r\n        ");
        sql.Append("else");
        sql.Append("\r\n        {");
        sql.Append("\r\n");
        sql.Append(sql_insert);
        sql.Append("\r\n        }");
        sql.Append("\r\n        ");
        sql.Append("return DbHelper.Dbop.ExecuteSqlSuc(sql.ToString());");
        sql.Append("\r\n    ");
        sql.Append("}");
        sql.Append("\r\n");
        sql.Append("\r\n    ");
        sql.AppendFormat("public static bool Delete({0} obj)", tbname);
        sql.Append("\r\n    ");
        sql.Append("{");
        sql.Append("\r\n        ");
        sql.AppendFormat("return DbHelper.Dbop.ExecuteSqlSucFormat(\"DELETE {0} WHERE {1}='{2}'\", obj.{1});", tbname, pk, "{0}");
        sql.Append("\r\n    ");
        sql.Append("}");
        sql.Append("\r\n");

        sql.Append("    #endregion");
        sql.Append("\r\n");
        sql.Append("}");

        ls_result.Add(sql.ToString());
        %>
    <textarea><%
                  
        Response.Write(string.Join("\r\n\r\n", ls_result)); %>
    </textarea>
    <%
    }
%>
</body>
</html>
