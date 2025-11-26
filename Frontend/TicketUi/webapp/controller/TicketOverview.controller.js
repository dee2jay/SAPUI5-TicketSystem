sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "ui5/ticketui/service/TicketService",
    "sap/ui/core/Fragment",
    "ui5/ticketui/util/formatter"
], function(Controller, TicketService, Fragment, formatter) {
    "use strict";
    return Controller.extend("ui5.ticketui.controller.TicketOverview", {
        
        formatter: formatter,

        onInit: function () {            
            var oModel = this.getOwnerComponent().getModel("ticketsModel");
            this.getView().setModel(oModel, "ticketsModel");

            this._loadTickets();
        
        },

         _loadTickets: async function () {
            try {
                const tickets = await TicketService.getAllTickets();                
                
                this.getView().getModel("ticketsModel").setProperty("/", tickets);
                this.getView().getModel("ticketsModel").getData();                

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

        onCreateButtonPress: async function(){
            const newTicket = this.getView().getModel("newTicket").getData();
            try {
                    await TicketService.createTicket(newTicket);
                    sap.m.MessageToast.show("Ticket created successfully");
                    this._oCreateDialog.close();
                    this._loadTickets(); // reload list
            } catch (err) {
                sap.m.MessageToast.show("Error creating ticket");
            }
        },

        onSearchFieldsLiveChange: function (oEvent) {
            const sQuery = oEvent.getParameter("query");
            const aFilters = [];  
        },

        onTicketItemPress: function (oEvent) {
            const oItem = oEvent.getSource();
            const oContext = oItem.getBindingContext("ticketsModel");
            console.log("Context:", oContext);
            const sTicketId = oContext.getProperty("id");
            console.log("Navigating to ticket ID:", sTicketId);
            this.getOwnerComponent().getRouter().navTo("ticketDetails", { ticketId: sTicketId });
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
                } 
        }       

    });
});