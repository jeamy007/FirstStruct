/// <reference path="jquery-1.8.3.min.js" />
String.prototype.format = function () {
    /// <summary>字符串格式化</summary>
    var args = arguments;
    if (typeof (args[0]) == "object" && args[0] != null) {
        args = args[0];
    }
    return this.replace(/\{(\d+)\}/g,
         function (m, i) {
             return args[i];
         });
}
String.prototype.replaceAll = function (s1, s2) {
    return this.replace(new RegExp(s1, "gm"), s2);
}
function cloneObj(obj) {
    ///	<summary>
    ///	克隆对象
    ///	</summary>
    ///	<param name="jsonstr" type="String">
    /// 要克隆对象
    ///	</param>
    if (typeof (obj) != 'object') return obj;
    if (obj == null) return obj;

    var newObj = new Object();

    for (var i in obj) {
        newObj[i] = cloneObj(obj[i]);
    }

    return newObj;
}

var getElm = document.getElementById;

function redirect(url) {
    window.location.href = url;
}
function redirectLastPage() {
    var _ul = getLastUrl();
    if (_ul) {
        redirect(_ul);
        return 1;
    }
    return 0;
}
function getPageUrl() {
    return window.location.pathname;
}
function goback() {
    window.history.back();
}
function fileDataLoad(e, imgId) {
    fileData(e, function (r) {
        document.getElementById(imgId).src = r;
    });
}
function fileData(fileId, f) {
    var e;
    if (typeof fileId == "String") {
        e = document.getElementById(fileId);
    }
    else {
        e = fileId;
    }
    if (e.files) {
        if (!e.files[0]) return;
        var reader = new FileReader();
        reader.onload = function (evt) {
            if(f) f(evt.target.result);
        }
        reader.readAsDataURL(e.files[0]);
    }
    else {
        var sFilter = 'filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale,src="';
        file.select();
        f(document.selection.createRange().text);
    }
}
function checkboxChangeState(e) {
    /// <summary>checkbox 全选 取消</summary>
    var f = $(e).data("set");
    if (f) {
        var _l = $(f);
        if (_l.length) {
            _l.prop("checked", e.checked)
        }
    }
}
function getQuery(name, def) {
    /// <summary>获取地址栏参数值</summary>
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return def;
}
function getLastUrl() {
    return getQuery("lasturl");
}
function getLastUrlParam() {
    return "lasturl=" + getPageUrl();
}
function toThousands(num) {
    if (!num) {
        return 0;
    }
    var t = num < 0;
    if (t) {
        num = Math.ceil(-num) + "";
    }
    else {
        num = Math.ceil(num) + "";
    }
    var result = '';
    while (num.length > 3) {
        result = ',' + num.slice(-3) + result;
        num = num.slice(0, num.length - 3);
    }
    if (num) { result = num + result; }
    return t ? "-" + result : result;
}
function getShortDate(v) {
    var d = new Date(v);
    return [d.getMonth() + 1, "月", d.getDate(), "日"].join("");
}
function ispc() {
    var userAgentInfo = navigator.userAgent;
    var Agents = new Array("Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod");
    var flag = true;
    for (var v = 0; v < Agents.length; v++) {
        if (userAgentInfo.indexOf(Agents[v]) > 0) { flag = false; break; }
    }
    return flag;
}


function checkMobile(s) {
    /// <summary>验证手机号码</summary>
    s = s + "";
    if (s.length == 0) {
        return false;
    }
    var regu = /1[3-8]+\d{9}/;
    var r = regu.test(s);
    return r ? s.length == 11 : false;
}