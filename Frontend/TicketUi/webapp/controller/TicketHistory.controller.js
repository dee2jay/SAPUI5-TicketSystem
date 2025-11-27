sap.ui.define([
	"sap/ui/core/mvc/Controller"
], function(
	Controller
) {
	"use strict";

	return Controller.extend("ui5.ticketui.controller.TicketHistory", {
        
        onInit: function() {
            var oRouter = this.getOwnerComponent().getRouter();

            oRouter.getRoute("ticketHistory").attachPatternMatched(this._onMatched, this);
        },

        _onMatched(oEvent){
            const ticketId = oEvent.getParameter("arguments").ticketId;

            // Récupère le modèle global 'ticketsModel'
            const oTicketsModel = this.getView().getModel("ticketsModel");
            const aTickets = oTicketsModel.getProperty("/tickets");

            // Cherche le ticket correspondant
            const oTicket = aTickets.find(t => t.id == ticketId);
            if (oTicket) {                
                this.getView().bindObject({
                    path: "/tickets/" + aTickets.indexOf(oTicket),
                    model: "ticketsModel"
            });
            } else {
                console.error("Ticket not found with id:", ticketId);
            }
         },

         onCloseButtonPress: function(){
            const oHistory = sap.ui.core.routing.History.getInstance();
            const sPreviousHash = oHistory.getPreviousHash();
            if (sPreviousHash !== undefined) {
                window.history.go(-1);
            } else {
                const oRouter = this.getOwnerComponent().getRouter();
                oRouter.navTo("tickets", {}, { skipHistory: true });
            }
        },

        onPageTicketHistoryNavButtonPress: function() {
            this.onCloseButtonPress();
        }   
	});
});