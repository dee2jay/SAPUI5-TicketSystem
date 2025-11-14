sap.ui.define([
    "sap/ui/core/mvc/Controller",   
    "sap/ui/model/resource/ResourceModel",
    "sap/ui/model/json/JSONModel",
    "sap/ui/core/Fragment"
], (Controller, ResourceModel, JSONModel, Fragment) => {
    "use strict";

    return Controller.extend("ui5.ticketui.controller.TicketOverview", {
        onInit: async function () {            
            return new Promise((resolve, reject) => {
                // const oModel = new JSONModel();
                // this.getView().setModel(oModel, "ticketsModel");
                // this._loadTickets().then(resolve).catch(reject);
            });             
        },

        

        _loadTickets: async function () {
            try {
                const response = await fetch("/api/tickets");
                if (!response.ok) {
                    throw new Error("Network response was not ok");
                }
                const tickets = await response.json();
                this.getView().getModel("ticketsModel").setData({ tickets: tickets });
            } catch (error) {
                console.error("Failed to load tickets:", error);
            }
        },

        onNewTicketButtonPress: function () {
            let that = this;

            if (!this._ticketCreate) {
                Fragment.load({
                    name: "ui5.ticketui.view.TicketCreate",
                    controller: this
                }).then(function (oDialog) {
                    that._ticketCreate = oDialog;
                    that.getView().addDependent(oDialog);
                    oDialog.open();
                });
            } else {
                this._ticketCreate.open();
            }
        },

        onSearchTickets: function (oEvent) {
            const sQuery = oEvent.getParameter("query");
            const aFilters = [];  
        },

        onTicketSelect: function (oEvent) {
            // const oSelectedItem = oEvent.getParameter("listItem");
            // const oContext = oSelectedItem.getBindingContext("ticketsModel");
            // const sTicketId = oContext.getProperty("id");
            // const oRouter = sap.ui.core.UIComponent.getRouterFor(this);
            //oRouter.navTo("TicketDetails", { ticketId: sTicketId });
        }
    });
});
