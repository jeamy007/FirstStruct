/// <reference path="../BoManager/CommTools.aspx" />
/// <reference path="../BoManager/CommTools.aspx" />
/// <reference path="../BoManager/CommTools.aspx" />
/// <reference path="ui-ext.js" />
/// <reference path="json2.min.js" />
function postformjsonsimple(form, sfunc, action) {
    postformjson(form, function (r) {
        _postjsondealresult(r,sfunc);
    },0, action);
}
function _postjsondealresult(r, f) {
    if (r.success) {
        if (f) f(r);
    }
    else { toast.warn(r.result || "操作失败");}
}
function postformjson(form, sfunc, ffunc, action, url) {
    /// <summary>post form json</summary>
    /// <param name="data" type="Object">参数集</param>
    /// <param name="sfunc" type="Function">成功回调函数</param>
    /// <param name="ffunc" type="Function">最终调用函数</param>
    /// <param name="url" type="String">处理页面 默认当前</param>
    action = action || "glb_save";
    var fd = $(form).serializeObject();
    fd = { action: action, data: JSON.stringify(fd) };
    postjsonl(fd, sfunc, ffunc, url);
}
function postjsonl(data, sfunc, ffunc, url) {
    /// <summary>post json</summary>
    ffunc = _ajaxloadingfunc(ffunc);
    loading.show();
    postjson(data, sfunc, ffunc, url);
}
function postobjectsimple(action, obj, sfunc) {
    postobject(action, obj, function (r) {
        _postjsondealresult(r, sfunc);
    });
}
function postobject(action, obj, sfunc, ffunc, url) {
    var jsonstr = JSON.stringify(obj);
    postjsondata(action, jsonstr, sfunc, ffunc, url);
}
function postjsondata(action, jsonstr, sfunc, ffunc, url) {
    var data = { action: action, data: jsonstr };
    postjson(data, sfunc, ffunc, url);
}
function postjson(data, sfunc, ffunc, url) {
    /// <summary>post json</summary>
    /// <param name="data" type="Object">参数集</param>
    /// <param name="sfunc" type="Function">成功回调函数</param>
    /// <param name="ffunc" type="Function">最终调用函数</param>
    /// <param name="url" type="String">处理页面 默认当前</param>
    var loc = window.location;
    var opt = {
        url: (url ? url : loc.pathname + loc.search),
        data: data,
        type: "POST",
        dataType: "json",
        success: function (r) {
            if (sfunc) sfunc(r);
            if (ffunc) ffunc();
        },
        error: function (r) {
            if (ffunc) ffunc();
            if (r.responseText) {
                var ptt = r.responseText.match(/<title>.+<\/title>/);
                if (ptt && ptt.length > 0) {
                    ptt = ptt[0];

                }
                else {
                    toast.error("服务器处理错误，请关闭重试");
                }
            } else {
                toast.error("无效处理");
            }
        }
    }
    $.ajax(opt);
}

function _ajaxloadingfunc(f) {
    if (f) {
        return function () {
            f();
            loading.hide();
        }
    }
    else {
        return loading.hide;
    }
}

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    var n, v;
    $.each(a, function () {
        n = this.name;
        v = this.value;
        if (o[n] !== undefined) {
            o[n] = o[n].split(',');
            if (!o[n].push) {
                o[n] = [o[n]];
            }
            o[n].push(v || '');
            o[n] = o[n].join(',');
        } else {
            o[n] = v || '';
        }
    });
    return o;
};
function fileuploadinit(d) {
    $(d.flag).fileupload({
        url: window.location.pathname,
        formData: { action: "uploadpicture" },
        done: function (e, r) {
            if (d.call) d.call(r.result);
        }
    });

}

