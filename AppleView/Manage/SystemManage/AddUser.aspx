<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Master/MasterManage.Master" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="Apple.View.Manage.SystemManage.AddUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">添加用户
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadPart" runat="server">
        <script>var app_menu = "menu_adduser";</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="server">
     <div class="responsive span12 fix-offset" data-tablet="span12 fix-offset">
        <div class="portlet box grey">
            <div class="portlet-title">
                <div class="caption"><i class="icon-reorder"></i>添加用户</div>
                <div class="actions">
                    <a href="#modal_edit" data-toggle="modal" class="btn blue" data-button="addnew"><i class="icon-pencil"></i> 添加</a>
                    <div class="btn-group">
                    </div>
                </div>
            </div>
            <div class="portlet-body">
                <form class="searchform" method="get" action="?<%=SearchParams %>">
                    <input type="text" name="skey" class="m-wrap medium" value="<%=SearchValue(SearchKeys.skey.ToString()) %>" placeholder="用户名" />
                    <button type="submit" class="btn blue"><i class="icon-search"></i> 搜索</button>
                </form>
<%
            int i = 1; 
            if (ObjList.Count == 0)
            { %>
                <div style="font-size:larger; text-align:center;">
                    当前没有数据,请添加!
                </div><%
            }
            else
            { %>
                <table class="gtable glb_table">
                <thead>
                    <tr>
                    <th class="index">序号</th>
                    <th><a href="<% = SortLink() %>">用户名</a></th>
                    <th><a href="#">电话</a></th>
                    <th><a href="#">E-Mail</a></th>
                    <th><a href="<% = SortLink() %>">创建时间</a></th>
                    <th>操作</th>
                </tr>
                    </thead>
                    <tbody><% 
                foreach (var obj in ObjList)
                {
            %>
            <tr data-id="<%=obj.UserID%>">
                <td><%=i%></td>
                <td><%=obj.UserName%></td>
                <td><%=obj.UserPhone%></td>
                <td><%=obj.Email%></td>               
                <td><%=DateTimeStringSingle(obj.CreateTime)%></td>
                <td>
                    <a href="#modal_edit" data-toggle="modal" data-button="edit" class="glyphicons pen" title="查看/修改"><i></i></a>
                    <a href="javascript:;" data-button="delete" class="glyphicons bin" title="删除"><i></i></a>
                    <textarea data-panel="data" class="hide"><%=JsonString(obj) %></textarea>
                </td>
            </tr><%
                     i++;
                }
            %>
                    </tbody>
                </table><%
            } %>

                <div class="row-fluid">
                <div class="span6">
                    <div class="dataTables_info">第 <% = PagingObj.StartNumber %> - <% =PagingObj.EndNumber %> 条 共 <% = RowCount %> 条</div></div>
                <div class="span6">
                    <div class="glb_pagingsize">
                        <select id="glb_pagingsize" data-val="<%=PagingSize %>" data-param="<%=PaingSizeParams %>" data-count="<%=RowCount %>"></select>
                    </div>
                    <div class="dataTables_paginate paging_bootstrap pagination">
                    <ul>
                        <li class="prev<% if (!PagingObj.PagingList[1].Enable) { %> disabled<%} %>"><a href="<% =PagingObj.PagingList[1].Link %>">← <span class="hidden-480"><% = PagingObj.PagingList[1].Text %></span></a></li>
                        <% for (i = 4; i < PagingObj.PagingList.Count; i++)
                            {%>
                        <li <%if (PagingObj.PagingList[i].Active)
                                { %> class="active"<%} %>><a href="<% =PagingObj.PagingList[i].Link %>"><% =PagingObj.PagingList[i].Text %></a></li><% 
                            }
                    if (PagingObj.PagingList.Count==4)
                    {
                        %>
                    <li  class="active"><a href="javascript::">1</a></li><% 
                    } %>
                        <li class="next<% if (!PagingObj.PagingList[2].Enable) { %> disabled<%} %>"><a href="<% =PagingObj.PagingList[2].Link %>"><span class="hidden-480"><% =PagingObj.PagingList[2].Text %></span> → </a></li>
                    </ul>

                    </div></div></div>
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
                <h3>用户维护</h3>
            </div>
            <div class="modal-body">
                <input type="text" name="UserID" class="hide" value="0" />
                <div class="control-group">
                    <div class="control-label">用户名：</div>
                    <div class="controls">
                         <div class="input-icon left">
                            <i class="icon-user"></i>
                        <input type="text" name="UserName" class="m-wrap" />
                             </div>
                    </div>
                </div>
                <div class="control-group">
                    <div class="control-label">密码：</div>
                    <div class="controls">
                         <div class="input-icon left">
                            <i class="icon-star"></i>
                        <input type="text" name="PassWord" class="m-wrap" value="123456" />
                             </div>
                    </div>
                </div>
                <div class="control-group">
                    <div class="control-label">手机号：</div>
                    <div class="controls">
                         <div class="input-icon left">
                            <i class="icon-phone"></i>
                        <input type="text" name="UserPhone" class="m-wrap" />
                             </div>
                    </div>
                </div>
                <div class="control-group">
                    <div class="control-label">电子邮件：</div>
                    <div class="controls">
                         <div class="input-icon left">
                            <i class="icon-envelope"></i>
                        <input type="text" name="Email" class="m-wrap" />
                             </div>
                    </div>
                </div>
                <div class="control-group">
                    <div class="control-label">性别：</div>
                    <div class="controls">
                        <label>
                            <input type="radio" name="Gender" class="m-wrap" value="男" checked />
                            男</label>
                        <label>
                            <input type="radio" name="Gender" class="m-wrap" value="女" />
                            女</label>
                    </div>
                </div>  
            </div>
            <div class="modal-footer">
                <button type="button" class="btn green" data-button="formsave"><i class="icon-ok"></i>保存</button>
                <button type="button" data-dismiss="modal" class="btn">关闭</button>
            </div>
        </form>
    </div>
</asp:Content>
