// Case insensitive sorting
jQuery.extend(jQuery.fn.dataTableExt.oSort, {

    "string-case-insensitive-pre": function (a) {
        return a.toLowerCase();
    },

    "string-case-insensitive-asc": function (x, y) {
        return ((x < y) ? -1 : ((x > y) ? 1 : 0));
    },

    "string-case-insensitive-desc": function (x, y) {
        return ((x < y) ? 1 : ((x > y) ? -1 : 0));
    }
});