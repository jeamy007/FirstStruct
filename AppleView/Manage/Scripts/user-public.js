/// <reference path="common.js" />
$(function () {
    // el 全局 事件内部不允许使用
    var el;
    /* ===Start ==========
       自定义分页数    */
    el = $("#glb_pagingsize");
    if (el.length) {
        var c = ~~el.data("count");
        el[0].options.add(new Option("全部", 1000000));
        if(_tmp_pagingsize_addoption(el, c, 10))
            if(_tmp_pagingsize_addoption(el, c, 20))
                if(_tmp_pagingsize_addoption(el, c, 50))
                    if(_tmp_pagingsize_addoption(el, c, 100))
                        _tmp_pagingsize_addoption(el, c, 200)
        el.val(el.data("val"));
        if (!el.val())
            el[0].selectedIndex = 0;
        el.on("change", function () {
            var p = $(this).data("param") || "";
            if (p)
                p += "&";
            window.location.href = "{0}?{1}pagingsize={2}".format(window.location.pathname, p, this.value);
        });
    }
    /* === End  ==========
       ===Start ==========
       搜索    */
    el = $("form.searchform");
    if (el.length) {
        var btn = el.find("input[type='submit']");
        if (btn.length) {
            btn.on("click", function () {
                var a = el.serializeArray();
                var pstr = [el[0].action];
                $.each(a, function () {
                    if (this.value)
                        pstr.push(this.name + "=" + this.value);
                });
                window.location.href = pstr.join("&");
                return false;
            })
        }

        el.find("select").each(function () {
            var th = $(this);
            var thv = th.data("val");
            if (thv != "undefined") {
                th.val(thv);
            }
        })
    }
    /* === End  ========== */
})
function _tmp_pagingsize_addoption(e, c, n) {
    if (n < c) {
        e[0].options.add(new Option("每页 " + n + " 条", n));
        return true;
    }
    return false;
}

function selectOptions(d, e, tid, l, f) {
    if (!l) l = 0;
    e.options.length = l;
    postjson(d, function (r) {
        for (var i = 0; i < r.length; i++) {
            e.options.add(new Option(r[i].name, r[i].id));
        }
        $(e).val(tid);
        if (!$(e).val())
            e.selectedIndex = 0;
        $(e).change();
        if (f) f();
    })
}

function loadtogglebutton(d) {
    if (!jQuery().toggleButtons) {
        return;
    }
    d = d || {};
    if (!d.flag) d.flag = ".info-toggle-button";
    if (!d.text1) d.text1 = "已启用";
    if (!d.text2) d.text2 = "已停用";
    $(d.flag).toggleButtons({
        style: {
            enabled: "info",
            disabled: ""
        },
        label: {
            enabled: d.text1,
            disabled: d.text2
        }
    });
}