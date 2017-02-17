<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Master/MasterManage.Master" AutoEventWireup="true" CodeBehind="RepassManage.aspx.cs" Inherits="AppleView.Manage.SystemManage.RepassManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">密码修改</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadPart" runat="server">
         <script>var app_menu = "menu_repassmanage";</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="server">
    <div class="row-fluid">
    <div class="responsive span12 fix-offset" data-tablet="span12 fix-offset">
        <div class="portlet box grey">
            <div class="portlet-title">
                <div class="caption"><i class="icon-reorder"></i>密码修改</div>
                <div class="actions">                  
                    <div class="btn-group">

                    </div>
                </div>
            </div>
            
        <form action="#" id="subForm" class="form-horizontal">
            <div class="portlet-body">
                    <div class="control-group">
                        <div class="control-label">原来的密码：</div>
                        <div class="controls">
                            <input type="text" name="oldPwd" class="m-wrap" />
                        </div>
                    </div>
                    <div class="control-group">
                        <div class="control-label">新密码：</div>
                        <div class="controls">
                            <input type="tel" name="newPwd" class="m-wrap" value="" />
                        </div>
                    </div>
                      <div class="control-group">
                        <div class="control-label">确认新密码：</div>
                        <div class="controls">
                            <input type="tel" name="reNewPwd" class="m-wrap" value="" />
                        </div>
                    </div>
                 <div class="modal-footer">
                <button type="button" class="btn green" data-button="formsave" onclick="save()"><i class="icon-ok"></i>保存</button>
                <button type="button" data-dismiss="modal" class="btn" onclick="Reset()">重置</button>
            </div>
            </div>
            </form>
            
        </div>
    </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyBottom" runat="server">
    <script type="text/javascript">
        function save()
        {
            var obj = $("#subForm").serializeObject();
            if (obj.oldPwd == "")
            {
                toast.error("请填写原始密码");
            }
            if (obj.newPwd == "" || obj.reNewPwd == "")
            {
                toast.error("请填写新密码");
            }
            if (obj.reNewPwd != obj.newPwd)
            {
                toast.error("两次密码填写不一致！");
            }
            postjson({ action: "remark", data: JSON.stringify(obj) }, function (r) {
                if (r.success) {
                    toast.success("密码修改成功！");
                } else {
                    toast.error(r.result);
                }
            });
        }

        function Reset()
        {
            location.reload();
        }
     
      

    </script>
</asp:Content>
