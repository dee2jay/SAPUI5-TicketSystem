sap.ui.define([
    "sap/ui/core/ComponentContainer",
], (ComponentContainer) => {
    "use strict";
    new ComponentContainer({
        name: "ui5.ticketui",
        settings: {
            id: "ticketui"
        },
        async: true
    }).placeAt("content");    
    
});