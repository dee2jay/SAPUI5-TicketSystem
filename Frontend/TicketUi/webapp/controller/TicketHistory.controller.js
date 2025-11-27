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
            const id = oEvent.getParameter("arguments").ticketId;
            const sTicketPath = this._findTicketPathById(id);
            if (sTicketPath) {
                this.getView().bindElement({
                    path: sTicketPath,
                    model: "ticketsModel"
            });
            } else {
                console.error("Ticket not found!", id);
            }
         },

         _findTicketPathById: function (id) {
            const oModel = this.getView().getModel("ticketsModel");
            const aTickets = oModel.getProperty("/tickets");
            const index = aTickets.findIndex(t => t.id == id);
            if (index !== -1) {
                return "/tickets/" + index;
            }
            return null;
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