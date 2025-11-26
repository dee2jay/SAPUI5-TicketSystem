sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "sap/ui/model/resource/ResourceModel"
], function(Controller, ResourceModel) {
    "use strict";

    return Controller.extend("ui5.ticketui.controller.App", {

        onInit: function () {            
            
            
        },

        onToggleSideNav: function () {
            var oSideNav = this.getOwnerComponent().byId("sideNav");
            oSideNav.setExpanded(!oSideNav.getExpanded());
        },

        onNavSelect: function (oEvent) {
            var key = oEvent.getSource().getKey();
            this.getOwnerComponent().getRouter().navTo(key);
        }

    });
});