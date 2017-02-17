/// <reference path="user-sortnum.js" />
/// <reference path="global-ajax.js" />
$(function () {
    var flag = {
        datapanel: ".glb_table",

        bt_delete: "delete",
        bt_edit: "edit",
        bt_addnew: "addnew",
        bt_savesort: "savesort",
        bt_formsave: "formsave",
        jsondata: '[data-panel="data"]'
    };
    var t = $(flag.datapanel);
    var dbtn_exp = '[data-button="{0}"]';
    if (t.length) {
        var btns = t.find('[data-button]');
        if (btns.length) {
            // 绑定删除和编辑事件
            btns.filter(dbtn_exp.format(flag.bt_delete)).click(function () {
                objDelete($(this));
            });
            btns.filter(dbtn_exp.format(flag.bt_edit)).click(function () {
                objAdd($(this));
            });

            // 数据处理
            var _d = t.find(flag.jsondata);
            if (_d.length) {
                _d.each(function () {
                    var _v = $(this);
                    $(_v).parents("tr").eq(0).data("obj",
                        JSON.parse("[{0}]".format(_v.val()))[0]);
                    _v.remove();
                });
            }
        }
    }
    // 新增
    $(dbtn_exp.format(flag.bt_addnew)).click(function () {
        objAdd();
    });

    // 表单保存
    $(dbtn_exp.format(flag.bt_formsave)).click(function () {
        objSave(this);
    });

    // 排序码保存时间注册
    if (typeof (sortNum) == "object") {
        var _el = $(dbtn_exp.format(flag.bt_savesort));
        if (_el.length) {
            _el.click(function () {
                sortNum.save();
            })
            sortNum.init();
        }
    }
})

function objDelete(e, k, f) {
    //e.stopPropagation();
    if (!e) return;
    if (!confirm("确定删除吗？")) return false;
    var key, kval, act;
    if (typeof (k) == "object") {
        key = k.key;
        kval = k.val;
        act = k.action;
    }
    else {
        key = k;
    }
    var eltr = $(e).parents("tr").eq(0);
    var obj = eltr.data("obj");
    act = act || "glb_delete";
    data = { action: act, data: JSON.stringify(obj) };
    e.disabled = 1;
    postjsonl(data, function (r) {
        if (r) {
            if (f && typeof (f) == "function") {
                f(e);
            }
            else {
                toast.success("删除成功")
                setTimeout(function () {
                    location.reload();
                }, 1000);
            }
        }
        else {
            toast.warning(r.result);
        }
    });
}

function objAdd(e) {
    var rtn;
    var _fm = "form.form-horizontal";
    var fm = $(_fm)
    fm[0].reset();
    var oid = 0;
    if (e) {
        var tr = $(e).parents("tr").eq(0);
        rtn = objFormSetVal(tr, _fm);
    }
    return rtn;
}
function objSave(e, f) {
    /// <summary>保存</summary>
    /// <param name="e" type="Element">this</param>
    /// <param name="f" type="Function">call</param>
    var _tmp_tx;
    if (e) {
        _tmp_tx = e.innerHTML;
        e.disabled = true;
        e.innerText = "正在保存...";
    }
    postformjson("form.form-horizontal", function (r) {
        if (r.success) {
            if (f) { f(); }
            else {
                toast.success(r.result || "保存成功");
                setTimeout(function () { location.reload(); }, 400);
            }
        }
        else {
            toast.warning(r.result);
            if (f) f();
        }
    }, function () {
        if (e) {
            e.disabled = false;
            e.innerHTML = _tmp_tx;
        }
    })
}

function objFormSetVal(tr, fm) {
    fm = typeof (fm) == "string" ? $(fm || "form.form-horizontal") : fm;
    // 隐藏域不会清理
    fm[0].reset();
    if (!tr) {
        return 0;
    }
    var rtn = {};
    var fminp = fm.find('.modal-body [name]');
    var data = tr.data("obj");
    if (data) {
        fminp.each(function () {
            var t = $(this);
            var fg = t[0].name;
            var t_v = data[fg];
            if (setFormValue(t, t_v)) {
                rtn[fg] = t;
            }
        });
    }
    else {
        fminp.each(function () {
            var t = $(this);
            var fg = t[0].name;
            var t_v = tr.data(fg);
            if (typeof (t_v) == "undefined") {
                t_v = tr.find(".c" + fg).val();
            }
            if (setFormValue(t, t_v)) {
                rtn[fg] = t;
            }
        });
    }
    return rtn;
}
function setFormValue(el, v) {
    if (typeof (v) != "undefined") {
        var inptype = el.attr("type");
        if (inptype) {
            var _inptype = inptype.toLowerCase();
            if (_inptype == "checkbox") {   
                el[0].checked = (v + "").split(',').indexOf(el.val()) > -1;
            }
            else if (_inptype == "radio") {
                el.attr('checked', el.val().trim() == (v + "").trim());
            }
            else if (_inptype == "date") {
                if ((v + "").indexOf("T") > 0) {
                    el.val(((v + "").split('T')[0]).replaceAll('/', '-'));
                }
                else if ((v + "").indexOf("Date") > 0) {
                    // json 格式
                }
                else {
                    el.val((v + "").split(' ')[0]);
                }
            }
            else {
                el.val(v);
            }
        }
        else {
            el.val(v).data("val", v).change();
        }
        return 1;
    }
    return 0;
}
function objUploadImage() {
    /// <summary>上传图片</summary>
    fileuploadinit({
        flag: "#id_file",
        call: function (url) {
            $("#id_img")[0].src = url;
            $("#id_filehd").val(url);
        }
    });
}
function objDownExcel(d) {
    /// <summary>导出excel</summary>
    if (typeof (d) == "string") {
        d = { action: d };
    }
    else {
        d = d || {};
        d.action = d.action || "exportexcel";
    }
    postjsonl(d, function (r) {
        if (r) {
            if (r.url) {
                var fm = $("<iframe style='display:none' src=" + r.url + "></iframe>");
                $("body").append(fm);
                setTimeout(function () { fm.remove();}, 3000);
            }
            else {
                toast.warning("无数据导出");
            }
        }
        else {
            toast.error("获取失败");
        }
    });
}
/*
  2015-04-13 21:42
  By: LiuTao
  tableeditable 表格行编辑（简单模式）
  参数：
  d: "" => 编辑元素检索表达式，默认 gtable .edit
  d: {
    flag: 编辑元素检索表达式
  }
  action: update
*/
var tableeditable = {
    init: function (d) {
        if (!d) {
            d = { flag: ".gtable .edit" };
        }
        else if (typeof d == "string") {
            d = { flag: d };
        }
        if (d.flag) {
            // 配置
            $(d.flag).on("click", function () {
                tableeditable._editevent($(this), d);
            })
        }
    },
    _editevent: function (e, d) {
        var el_tr = e.parent().parent();
        var el_tds = el_tr.find("td[data-key]");
        if (!el_tds.length) {
            return
        }
        var t_td, _t_td_v;
        for (var i = 0; i < el_tds.length; i++) {
            t_td = $(el_tds[i]);
            _t_td_v = t_td.text();
            t_td.data("v", _t_td_v);
            t_td.html('<input type="{0}" value="{1}" class="tableeditinput m-wrap"/>'.format(t_td.attr("type") ? t_td.attr("type") : "text", _t_td_v));
        }
        // code
        var ahtml;
        //var ahtml = '<a href="javascript:" class="{0}">{1}</a>';
        ahtml = '<a href="javascript:" class="glyphicons {0}" title="{1}"><i></i></a>';
        var el_save = $(ahtml.format("check save", "保存"));
        var el_cancel = $(ahtml.format("share cancel", "取消"));
        el_save.click(function () {
            tableeditable._saveclick($(this), el_tr);
        });
        el_cancel.click(function () {
            tableeditable._cancelclick($(this), el_tr);
        })
        e.parent().append(el_save).append(el_cancel);
        e.hide();
    },
    _saveclick: function (e, el_tr, da, f) {
        /// <summary>保存事件</summary>
        if (!el_tr)
            el_tr = e.parent().parent();
        var el_inps = el_tr.find("td[key] .tableeditinput");
        var data = {};
        if (da) {
            data = da;
        }
        data.action = "update";
        data.id = ~~el_tr.attr("tag");
        el_inps.each(function () {
            var _that = $(this);
            data[_that.parent().attr("key")] = _that.val();
        })
        postjsonl(data, function (r) {
            if (r.success) {
                toast.success("已保存");
                el_inps.each(function () {
                    var _that = $(this);
                    _that.parent().html(_that.val());
                });
                with (e.parent()) {
                    find(".cancel").remove();
                    find(".edit").show();
                }
                e.remove();
                if (f) f(r);
            }
            else {
                toast.warning(r.result);
            }
        });
    },
    _cancelclick: function (e, el_tr, f) {
        if (!el_tr)
            el_tr = e.parent().parent();
        var el_inps = el_tr.find("td[key] .tableeditinput");
        el_inps.each(function () {
            var that_td = $(this).parent();
            that_td.html(that_td.data("v"));
        })
        with (e.parent()) {
            find(".save").remove();
            find(".edit").show();
        }
        e.remove();
        if (f) f();
    }
};