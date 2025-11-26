sap.ui.define([
    "sap/ui/core/mvc/Controller"
], function(Controller) {
    "use strict";
    return Controller.extend("ui5.ticketui.controller.AppSetting", {
       
    onInit: function () {                    
        
    },

    onToggleSideNav: function () {
            var oSideNav = this.byId("sideNav");
            oSideNav.setExpanded(!oSideNav.getExpanded());
        },
         
    onNavSelect: function (oEvent) {
        var oItem = oEvent.getParameter("item"); 
        var key = oItem.getKey();
        var oNavList = this.byId("navList");

        switch (key) {
            case "dashboard": 
                oNavList.setSelectedItem(this.byId("navDashboard"));
                this.getOwnerComponent().getRouter().navTo("dashboard");
                break;

            case "tickets":    
                oNavList.setSelectedItem(this.byId("navTickets"));
                this.getOwnerComponent().getRouter().navTo("tickets");
                break;

            case "settings":
                oNavList.setSelectedItem(this.byId("navSettings"));
                this.getOwnerComponent().getRouter().navTo("settings");
                break;
            case "logout":
                this.getOwnerComponent().getRouter().navTo("home");
                break;
            }           
        },

        onCancelSettingsPress: function(){
            var oHistory = sap.ui.core.routing.History.getInstance();
            var sPreviousHash = oHistory.getPreviousHash();

            if (sPreviousHash !== undefined) {
                window.history.go(-1); 
            } else {
                this.getOwnerComponent().getRouter().navTo("home", {}, true);
            }
        },

        onSaveSettingsPress: function(){
            var oHistory = sap.ui.core.routing.History.getInstance();
            var sPreviousHash = oHistory.getPreviousHash();

            if (sPreviousHash !== undefined) {
                window.history.go(-1); 
            } else {
                this.getOwnerComponent().getRouter().navTo("home", {}, true);
            }
        }

    });
});