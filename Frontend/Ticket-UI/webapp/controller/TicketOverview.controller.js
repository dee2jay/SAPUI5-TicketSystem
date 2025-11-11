sap.ui.define([
  "sap/ui/core/mvc/Controller",
  "sap/m/MessageToast"
], function (Controller, MessageToast) {
  "use strict";

  return Controller.extend("ticket-ui.controller.TicketOverview", {

   onInit: async function () {
            const oModel = new JSONModel();
            try {
                const response = await fetch("/api/tickets");
                const data = await response.json();
                oModel.setData({ Tickets: data });
            } catch (err) {
                console.error("Issue while loading tickets:", err);
            }
            this.getView().setModel(oModel);
        }
    });
});