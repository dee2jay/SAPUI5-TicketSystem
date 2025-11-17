sap.ui.define([
	"sap/ui/core/mvc/Controller",
    "sap/m/MessageToast",
    "sap/ui/core/routing/History"
], (Controller, MessageToast, History) => {
	"use strict";

	return Controller.extend("ui5.ticketui.controller.TicketDetails", {

        onInit: function() {
             this.getOwnerComponent()
        .getRouter()
        .getRoute("TicketDetail")
        .attachPatternMatched(this._onMatched, this);
        },

        _onMatched(oEvent){
            const id = oEvent.getParameter("arguments").ticketId;

            // Load ticket
            const oDetailModel = new JSONModel();
            oDetailModel.loadData(`/api/tickets/${id}`);
            this.getView().setModel(oDetailModel, "detailModel");

            // load comments
            const oCommentsModel = this.getOwnerComponent().getModel("commentsModel");
            oCommentsModel.loadData(`/api/tickets/${id}/comments`);
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
	});
});