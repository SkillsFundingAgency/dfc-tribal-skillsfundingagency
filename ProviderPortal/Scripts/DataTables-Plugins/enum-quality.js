jQuery.extend(jQuery.fn.dataTableExt.oSort, {
    "enum-quality-pre": function (a) {
        switch (a) {
            case "Very Good": return 1;
            case "Good": return 2;
            case "Average": return 3;
            case "Poor": return 4;
            default: return 5;
        }
    },

    "enum-quality-asc": function (a, b) {
        return ((a < b) ? -1 : ((a > b) ? 1 : 0));
    },

    "enum-quality-desc": function (a, b) {
        return ((a < b) ? 1 : ((a > b) ? -1 : 0));
    }
});