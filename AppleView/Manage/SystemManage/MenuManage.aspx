<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Master/MasterManage.Master" AutoEventWireup="true" CodeBehind="MenuManage.aspx.cs" Inherits="Apple.View.Manage.SystemManage.MenuManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">菜单管理</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadPart" runat="server">
    <script>var app_menu = "menu_menumanage";</script>
    <style>
        tr>td:first-child{padding-left:10px}
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="server">

<div class="row-fluid">
    <div class="responsive span12 fix-offset" data-tablet="span12 fix-offset">
        <div class="portlet box grey">
            <div class="portlet-title">
                <div class="caption"><i class="icon-reorder"></i>菜单管理</div>
                <div class="actions">
                    <a href="#modal_edit" data-toggle="modal" class="btn blue" data-button="addnew"><i class="icon-pencil"></i> 添加</a>
                    <div class="btn-group">
                    </div>
                </div>
            </div>
            <div class="portlet-body">
<%
    int i = 1;
    if (ObjList.Count == 0)
    { %>
                <div style="font-size:larger; text-align:center;">
                    当前没有数据!
                </div>
<%
    }
    else
    { %>
                <table class="gtable glb_table">
                <thead>
                    <tr>
                    <th class="alleft">菜单名</th>
                    <th class="alleft hidden-980">路径</th>
                    <th>排序码</th>
                    <th>操作</th>
                </tr>
                    </thead>
                    <tbody>
        <% 
            foreach (var obj in ObjList.FindAll(m => m.MenuParentID == 0))
            {%>
            <tr data-id="<%=obj.MenuID%>">
                <td class="alleft"><%=obj.MenuName%></td>
                <td class="alleft hidden-980"><%=obj.MenuUrl%></td>
                <td>
                   <%=obj.SortIndex%></td>
                <td>
                    <a href="#modal_edit" data-toggle="modal" data-button="edit" class="glyphicons pen" title="查看/修改"><i></i></a>
                    <a href="javascript:;" data-button="delete" class="glyphicons bin" title="删除"><i></i></a>
                    <textarea data-panel="data" class="hide"><%=JsonString(obj) %></textarea>
                </td>
            </tr>
                <%
    foreach (var obj1 in ObjList.FindAll(m => m.MenuParentID == obj.MenuID))
    {       %>
            <tr data-id="<%=obj1.MenuID%>">
                <td class="alleft">　<%=obj1.MenuName%></td>
                <td class="alleft hidden-980"><%=obj1.MenuUrl%></td>
                <td>
                   <%=obj1.SortIndex%></td>
                <td>
                    <a href="#modal_edit" data-toggle="modal" data-button="edit" class="glyphicons pen" title="查看/修改"><i></i></a>
                    <a href="javascript:;" data-button="delete" class="glyphicons bin" title="删除"><i></i></a>
                    <textarea data-panel="data" class="hide"><%=JsonString(obj1) %></textarea>
                </td>
            </tr>
     <%
        }
    }       %>
                    </tbody>
                </table>
<%
    } %>
            </div>
        </div>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyBottom" runat="server">
<div id="modal_edit" class="modal hide fade in">
<form method="post" class="form-horizontal">
<div class="modal-header">
    <button type="button" class="close" data-dismiss="modal"></button>
    <h3>菜单管理</h3>
</div>
<div class="modal-body">
    <input type="text" name="MenuID" value="0" class="hide"/>
    <div class="control-group">
    <label class="control-label">上级菜单：</label>
        <div class="controls">
            <select name="MenuParentID" id="id_parent">
                <option value="0">顶级菜单</option>
        <%
            foreach (var item in ParentManuList())
            {%>
                <option value="<%=item.MenuID %>"><%=item.MenuName %></option><%
            }
             %>
            </select></div>
    </div>
    <div class="control-group">
    <label class="control-label">名称：<span class="required">*</span></label>
        <div class="controls"><input type="text" name="MenuName" class="m-wrap"/></div>
    </div>
    <div class="control-group">
    <label class="control-label">编码：<span class="required">*</span></label>
        <div class="controls"><input type="text" name="MenuCode" class="m-wrap"/></div>
    </div>
    <div class="control-group">
    <label class="control-label">链接：<span class="required">*</span></label>
        <div class="controls"><input type="text" name="MenuUrl" class="m-wrap large"/></div>
    </div>
    <div class="control-group">
    <label class="control-label">图标标识：</label>
        <div class="controls"><input type="text" name="MenuIcon" class="m-wrap"/></div>
    </div>
    <div class="control-group">
    <label class="control-label">排序码：</label>
        <div class="controls"><input type="number" name="SortIndex" class="m-wrap" value="10" required/></div>
    </div>
    <div class="control-group">
        <div class="control-label">分配角色：</div>
        <div class="controls">

<%
    var _rlist = RoleList();
    if (_rlist.Count > 0)
    {
        foreach (var item in _rlist)
        {%>
            
            <label><input type="checkbox" name="RoleIDs" class="m-wrap" value="<%=item.RoleID %>" /> <%=item.RoleName %></label><%
        }
    }
    else
    {%>
            <a href="#" class="btn red">添加角色</a><%
    }
%>
        </div>
    </div>
</div>
 <div class="modal-footer">
    <button type="button" class="btn green" data-button="formsave"><i class="icon-ok"></i> 保存</button>
    <button type="button" data-dismiss="modal" class="btn">关闭</button>
 </div>
</form>
</div>
</asp:Content>
