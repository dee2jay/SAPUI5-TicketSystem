sap.ui.define([], function () {
    "use strict";

    return {
        createEmptyTicket: function () {
            return {
                Category: "",
                Location: "",
                CostCenter: "",
                OrderNumber: "",
                Title: "",
                Description: "",
                Attachments: [],
                Comments: []
            };
        }
    };
});