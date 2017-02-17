<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AppleView.Manage.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>登录</title>
	<link href="/media/css/bootstrap.min.css" rel="stylesheet" type="text/css"/>
	<link href="/media/css/font-awesome.min.css" rel="stylesheet" type="text/css"/>
	<link href="/media/css/style-metro.css" rel="stylesheet" type="text/css"/>
	<link href="/media/css/style.css" rel="stylesheet" type="text/css"/>
	<link href="/media/css/login.css" rel="stylesheet" type="text/css"/>

    <script src="/media/js/jquery-1.10.1.min.js"></script>
    <script src="/Scripts/global.js"></script>
    <script src="/media/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="/media/js/jquery.blockui.min.js"></script>
    <script src="/media/js/app.js"></script>
    <script src="Scripts/global-ajax.js"></script>
    <script src="Scripts/ui-ext.js"></script>
</head>
<body class="login">
	<div class="logo">
		<%--<img src="media/image/logo-big.png" alt="" />--%> 
	</div>
	<div class="content">
		<!-- BEGIN 登陆 FORM -->
		<form class="form-vertical login-form" method="post">
			<h3 class="form-title">请登录</h3>
			<div class="control-group">
                <input type="hidden" name="action" value="login" />
				<label class="control-label visible-ie8 visible-ie9">用户名</label>
				<div class="controls">
					<div class="input-icon left">
						<i class="icon-user"></i>
						<input class="m-wrap placeholder-no-fix" type="text" placeholder="用户名" name="username" id="username" value="admin" autofocus/>
					</div>
				</div>
			</div>
			<div class="control-group">
				<label class="control-label visible-ie8 visible-ie9">密码</label>
				<div class="controls">
					<div class="input-icon left">
						<i class="icon-lock"></i>
						<input class="m-wrap placeholder-no-fix" type="password" placeholder="密码" name="password" value="123456"/>
					</div>
				</div>
			</div>
			<div class="form-actions">
				<button type="submit" class="btn green pull-right" name="LoginBtn" value="LoginBtn" onclick="return login()">登陆 <i class="m-icon-swapright m-icon-white"></i></button>                 
			</div>
		</form>
	</div>
    <script>
        $(function () {
            App.init();
        })
        function login() {
            var el_n = $("#username");
            if (!el_n.val()) {
                el_n.focus();
                toast.warning({ text: "请填写用户名", panel: ".content" });
                return false;
            }
            postjsonl($(".login-form").serializeObject(), function (r) {
                if (r.success) {
                    location.href = r.result;
                }
                else {
                    toast.warning({ text: r.result, panel: ".content" });
                }
            })
            return false;
        }
    </script>
</body>
</html>
