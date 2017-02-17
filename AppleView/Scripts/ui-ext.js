/// <reference path="jquery-1.8.3.min.js" />
/*
  2015-02-09 
  基于 bootstrap alert 样式
  show：原生函数

  参数：
  d = { 
    panel: String   // 装载到指定元素（可选）默认元素：.modal:visible 其次：.page-content 其次：document.body
    type: String,   // 消息类型 success | warning | error ...
    text: String,   // 消息文字
    icon: String,   // 显示图标（可选）
    keep: Boolean,  // 保持显示 调.hide() 隐藏
    interval: Int,  // 停留时间（ms） 默认：3000
    call: Function, // 隐藏回调
    width: String   // 宽度
  }
*/
var toast = {
    show: function (d) {
        /// <summary>消息显示 原生</summary>
        alert(d.text);
    },
    success: function (s, f) {
        /// <summary>显示成功消息</summary>
        /// <param name="s" type="String">消息文字</param>
        /// <param name="f" type="Function">回调</param>
        return this._defaultevent(s, f, "success", "ok-sign");
    },
    warning: function (s, f) {
        /// <summary>显示警告消息</summary>
        /// <param name="s" type="String">消息文字</param>
        /// <param name="f" type="Function">回调</param>
        return this._defaultevent(s, f, "warning", "exclamation-sign");
    },
    error: function (s, f) {
        /// <summary>显示错误消息</summary>
        /// <param name="s" type="String">消息文字</param>
        /// <param name="f" type="Function">回调</param>
        return this._defaultevent(s, f, "error", "remove-sign");
    },
    _hide: function (h, d) {
        h.animate({ top: -h.height() }, function () {
            h.remove();
            if (d.call) d.call();
        });
    },
    _defaultevent: function (s, f, t, i) {
        if (typeof s == "object") {
        }
        else {
            s = { text: s };
        }
        if (!s.type) s.type = t;
        if (!s.icon) s.icon = i;
        if (f) s.call = f;
        return this.show(s);
    },
    _template_html: function (d) {
        var h = '<div class="alert alert-{0}">';
        if (d.icon)
            h += '<i class="icon-{0}"></i> '.format(d.icon);
        h += '<strong>{1}</strong></div>';
        return h.format(d.type, d.text);
    }
}
var loading = {
    show: function (t) {
    },
    hide: function (t) {
    }
}