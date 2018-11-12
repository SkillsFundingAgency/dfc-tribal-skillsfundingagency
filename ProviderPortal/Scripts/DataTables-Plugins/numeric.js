jQuery.extend(jQuery.fn.dataTableExt.oSort, {
    "numeric-pre": function (a) {
        if (parseFloat(a) == NaN) {
            return 0;
        }
        return parseFloat(a);
    },

    "numeric-asc": function (a, b) {
        return ((a < b) ? -1 : ((a > b) ? 1 : 0));
    },

    "numeric-desc": function (a, b) {
        return ((a < b) ? 1 : ((a > b) ? -1 : 0));
    }
});