function MenuInit() {
    var elmenu = $(".page-sidebar-menu>li");
    elmenu.each(function () {
        if ($(this).find("ul li").length == 0) {
            $(this).find(".arrow").remove();
        }
    });

    if (typeof (app_menu) != "undefined" && app_menu) {
        var el_menu = $("." + app_menu);
        if (el_menu.length) {
            el_menu.addClass("active");
            el_menu.parent().show();
            var el_p = el_menu.parents("li").last();
            el_p.addClass("active");
            var _el = el_p.find(".arrow");
            if (_el.length) {
                //_el.addClass("open");
                $('<span class="selected"></span>').insertBefore(_el);
            }
        }
    }
    if ( elmenu.length <= 2 && ispc()) {
        $(".page-header-fixed").addClass("page-sidebar-closed")
    }
}