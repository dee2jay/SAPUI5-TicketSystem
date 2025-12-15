sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "sap/m/MessageToast",
    "ui5/ticketui/service/TokenService"
], function(Controller, MessageToast,TokenService) {
    "use strict";
    return Controller.extend("ui5.ticketui.controller.AppSetting", {
       
    onInit: function () {
        const token = TokenService.getToken();
            if (!token) {
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