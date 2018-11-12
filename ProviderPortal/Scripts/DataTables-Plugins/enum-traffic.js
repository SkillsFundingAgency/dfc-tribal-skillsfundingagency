jQuery.extend(jQuery.fn.dataTableExt.oSort, {
    "enum-traffic-pre": function (a) {
        switch (a) {
            case "Green": return 1;
            case "Amber": return 2;
            case "Red": return 3;
            default: return 4;
        }
    },

    "enum-traffic-asc": function (a, b) {
        return ((a < b) ? -1 : ((a > b) ? 1 : 0));
    },

    "enum-traffic-desc": function (a, b) {
        return ((a < b) ? 1 : ((a > b) ? -1 : 0));
    }
});