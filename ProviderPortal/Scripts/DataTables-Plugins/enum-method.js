jQuery.extend(jQuery.fn.dataTableExt.oSort, {
    "enum-method-pre": function (a) {
        switch (a) {
            case "Bulk Upload": return 1;
            case "Portal": return 2;
            default: return 3;
        }
    },

    "enum-method-asc": function (a, b) {
        return ((a < b) ? -1 : ((a > b) ? 1 : 0));
    },

    "enum-method-desc": function (a, b) {
        return ((a < b) ? 1 : ((a > b) ? -1 : 0));
    }
});