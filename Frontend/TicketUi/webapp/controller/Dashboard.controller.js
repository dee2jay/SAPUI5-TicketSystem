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
                this.getOwnerComponent().getRouter().navTo("home", {});
                return;
            }

        },

        onButtonToggleSideNavPress: function () {
            var oSideNav = this.byId("id1SideNavigation");
            oSideNav.setExpanded(!oSideNav.getExpanded());
        },

        onDashboardNavigationListItemSelect: function(){
            this.getOwnerComponent().getRouter().navTo("dashboard");
        },

        onTicketsNavigationListItemSelect: function(){
            this.getOwnerComponent().getRouter().navTo("tickets");
        },

        onSettingsNavigationListItemSelect: function(){
            this.getOwnerComponent().getRouter().navTo("settings");
        },

        onLogoutNavigationListItemSelect: function(){
            this.getOwnerComponent().getRouter().navTo("home");
        },
        
         onSideNavigationItemSelect: function (oEvent) {
            const oItem = oEvent.getParameter("item");
            if (!oItem) return;

            const key = oItem.getKey();
            const oNavList = this.byId("id2NavigationList");

            const actions = {
                "dashboard": {
                    item: this.byId("idDashboardNavigationListItem"),
                    fn: this.onDashboardNavigationListItemSelect?.bind(this)
                },
                "tickets": {
                    item: this.byId("idTicketsNavigationListItem"),
                    fn: this.onTicketsNavigationListItemSelect?.bind(this)
                },
                "settings": {
                    item: this.byId("idSettingsNavigationListItem"),
                    fn: this.onSettingsNavigationListItemSelect?.bind(this)
                },
                "logout": {
                    item: this.byId("idLogoutNavigationListItem"),
                    fn: () => {
                        if (TokenService.getToken()) TokenService.clear();
                        MessageToast.show("session logged out successfully");
                        this.onLogoutNavigationListItemSelect?.();
                    }
            }
            };

            const action = actions[key];
            if (!action) return;

            if (action.item) {
                oNavList.setSelectedItem(action.item);
            } else {
                oNavList.setSelectedItem(oItem);
            }

            if (action.fn) {
                action.fn();
            }

        }
    });
});
