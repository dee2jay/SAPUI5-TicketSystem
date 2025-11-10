sap.ui.define([
  "sap/ui/core/mvc/Controller",
  "sap/m/MessageToast"
], function (Controller, MessageToast) {
  "use strict";

  return Controller.extend("com.ticketapp.controller.TicketOverview", {

    onInit: function () {
      this._loadTickets();
    },

    _loadTickets: function () {
      var oModel = new sap.ui.model.json.JSONModel();
      oModel.loadData("/api/tickets");

      oModel.attachRequestCompleted(function() {
        MessageToast.show("Tickets geladen!");
      });

      this.getView().setModel(oModel, "tickets");
    }
  });
});