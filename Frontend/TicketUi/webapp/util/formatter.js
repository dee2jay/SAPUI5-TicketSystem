sap.ui.define([
    "sap/ui/core/format/DateFormat"
], function(DateFormat) {
    "use strict";

    return {
        formatDateTime: function (value) {
            if (!value) return "";

            /** @type {Date} */
            let oDate = new Date(value);

            return sap.ui.core.format.DateFormat.getDateTimeInstance({
                pattern: "dd.MM.yyyy HH:mm:ss"
            }).format(oDate);
        },

        formatDate: function (value) {
            if (!value) return "";

            /** @type {Date} */
            let oDate = new Date(value);

            return sap.ui.core.format.DateFormat.getDateInstance({
                pattern: "dd.MM.yyyy"
            }).format(oDate);
        }
    };
});