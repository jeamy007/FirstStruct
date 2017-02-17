/*
  2015-02-27
  排序码处理
*/
var sortNum = {
    ids: [],
    nos: [],
    init: function (r) {
        if (!r) r = ".gtable .sortnum";
        var e = $(r);
        if (!e.length) return;
        if (e[0].tagName.toLowerCase() != "input") return;
        e.on("change", function () {
            sortNum.change($(this));
        })
    },
    change: function (e) {
        if (!e) return;
        var _id = ~~e.parents("tr").eq(0).data("id");
        var _no = e.val();
        if (!_no || !_id) {
            return;
        }
        var _ix = this.ids.indexOf(_id);
        if (_ix == -1) {
            this.ids.push(_id);
            this.nos.push(_no);
        }
        else {
            this.nos[_ix] = _no;
        }
    },
    save: function (d) {
        /// <summary>排序码保存</summary>
        d = d || {};
        d.action = d.action || "glb_save_sortindex";
        if (!this.ids.length) {
            toast.warning("无修改");
            return;
        }
        postjsonl({ action: d.action, ids: this.ids.join(","), nos: this.nos.join(",") }, function (r) {
            if (r.success) {
                toast.success(r.result || "已更新");
                if (!d.static)
                    location.reload();
            }
            else
                toast.error(r.result);
        });
    }
}