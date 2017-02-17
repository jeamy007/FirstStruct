(function () {
    $('form select[data-val]').each(function () {
        var te = this;
        var _v = $(te).data("val");
        if (_v) {
            te.value = _v;
            if (!te.value) {
                te.selectedIndex = 0;
            }
        }
    });
    $('form input[type="checkbox"]').each(function () {
        var te = this;
        te.checked = $(te).data("val") == 1;
    })
})();