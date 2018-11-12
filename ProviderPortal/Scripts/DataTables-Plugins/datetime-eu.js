// Was date-euro
jQuery.extend(jQuery.fn.dataTableExt.oSort, {
    "datetime-eu-pre": function (a) {
        var x;

        if ($.trim(a) !== '') {
            var frDatea = $.trim(a).split(' ');
            var frTimea = frDatea[1].split(':');
            var frDatea2 = frDatea[0].split('/');
            if (frTimea[2] === undefined) { frTimea[2] = '00'; }
            x = ('' + frDatea2[2] + frDatea2[1] + frDatea2[0] + frTimea[0] + frTimea[1] + frTimea[2]) * 1;
        }
        else {
            x = Infinity;
        }

        return x;
    },

    "datetime-eu-asc": function (a, b) {
        return ((a < b) ? -1 : ((a > b) ? 1 : 0));
    },

    "datetime-eu-desc": function (a, b) {
        return ((a < b) ? 1 : ((a > b) ? -1 : 0));
    }
});