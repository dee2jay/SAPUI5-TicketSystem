sap.ui.define([
    "sap/ui/core/mvc/Controller", 
    "sap/ui/core/Fragment",
	"ui5/ticketui/service/TicketService",
    "sap/ui/model/json/JSONModel"
], function (Controller,
    Fragment,
    TicketService,
    JSONModel ) {
    "use strict";

    return Controller.extend("ui5.ticketui.controller.TicketOverview", {
        onInit:  function () {             
            
            const oModel = new JSONModel({ tickets: [] });        
            this.getView().setModel(oModel, "ticketsModel");         
            
            this._loadTickets();             
        
        },
        
        _loadTickets: async function () {
            try {
                const tickets = await TicketService.getAllTickets();                
                
                this.getView().getModel("ticketsModel").setProperty("/", tickets);
                this.getView().getModel("ticketsModel").getData();
                console.log("Tickets set in model:", this.getView().getModel("ticketsModel").getData());                

            } catch (error) {
                console.error("Failed to load tickets:", error);
            }
        },

        onNewTicketButtonPress: function () {

            //const oTicket = TicketModel.createEmptyTicket()
            let that = this;
            
            //const oModel = new sap.ui.model.json.JSONModel(oTicket);
            //this.getView().setModel(oModel, "newTicket");

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

        onColumnListItemPress: function (oEvent) {
           const id = oEvent.getSource().getBindingContext("ticketsModel").getProperty("id");

           this.getOwnerComponent().getRouter().navTo("detail", { ticketId: id });
        }
    });
});
