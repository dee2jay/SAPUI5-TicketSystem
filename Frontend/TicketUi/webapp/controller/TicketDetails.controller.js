sap.ui.define([
	"sap/ui/core/mvc/Controller",
    "sap/m/MessageToast",
    "sap/ui/core/routing/History"
], (Controller, MessageToast, History) => {
	"use strict";

	return Controller.extend("ui5.ticketui.controller.TicketDetails", {

        onInit: function() {
            var oRouter = this.getOwnerComponent().getRouter();

            oRouter.getRoute("ticketDetails").attachPatternMatched(this._onMatched, this);
        },

        _onMatched(oEvent){
             const id = oEvent.getParameter("arguments").ticketId;

             // Load ticket details by ID
             const sTicketPath = this._findTicketPathById(id);
             if (sTicketPath) {
                 this.getView().bindElement({
                    path: sTicketPath,
                    model: "ticketsModel"
                 });
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

       onHistoryButtonPress: function(){
        MessageToast.show("Ticket History")

       },
       onPrintButtonPress: function(){
        window.print();

       },
       onSaveButtonPress: function(){
        MessageToast.show("Ticket saved")

       },
        onCancelButtonPress: function(){
            let oHistory = History.getInstance();
            let sPrevHash = oHistory.getPreviousHash();

            if(sPrevHash !== undefined){
                window.history.go(-1);
            } else{
                this.getOwnerComponent().getRouter().navTo("overview", {}, { skipHistory: true });
            }
        },
        onNavBack() {
			const oHistory = History.getInstance();
			const sPreviousHash = oHistory.getPreviousHash();

			if (sPreviousHash !== undefined) {
				window.history.go(-1);
			} else {
				const oRouter = this.getOwnerComponent().getRouter();
				oRouter.navTo("tickets", {}, true);
			}
		}
	});
});