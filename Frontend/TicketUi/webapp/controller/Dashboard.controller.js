sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "ui5/ticketui/service/TokenService",
    "sap/m/MessageToast"
], function (Controller,
	TokenService, MessageToast) {
    "use strict";

    return Controller.extend("ui5.ticketui.controller.Dashboard", {
        onInit: function () {
           const token = TokenService.getToken();
            if (!token) {

                MessageToast.show("Please login");
                // Redirect to login
                this.getOwnerComponent().getRouter().navTo("home", {}, true);
                return;
            }

        },

        onToggleSideNav: function () {
            var oSideNav = this.byId("sideNav");
            oSideNav.setExpanded(!oSideNav.getExpanded());
        },
        
         onNavSelect: function (oEvent) {
            var oItem = oEvent.getParameter("item"); 
            
            if(!oItem){
                return;
            }
            this.byId("sideNav").setSelectedItem(oItem);
            
            var key = oItem.getKey();
            
            var oNavList = this.byId("navList");

    switch (key) {
        case "dashboard": 
            //oNavList.setSelectedItem(this.byId("navDashboard"));
            this.getOwnerComponent().getRouter().navTo("dashboard");
            break;

        case "tickets":    
            //oNavList.setSelectedItem(this.byId("navTickets"));
            this.getOwnerComponent().getRouter().navTo("tickets");
            break;

        case "settings":
            //oNavList.setSelectedItem(this.byId("navSettings"));
            this.getOwnerComponent().getRouter().navTo("settings");
            break;
         case "logout":
                if(TokenService.getToken()){
                    TokenService.clear();   
                }            
                MessageToast.show("session logged out successfully")
                this.getOwnerComponent().getRouter().navTo("home");                      
                break;
    } 
        }
    });
});
