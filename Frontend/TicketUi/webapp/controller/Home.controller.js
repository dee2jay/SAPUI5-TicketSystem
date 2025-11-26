sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "sap/m/MessageToast"
], function(Controller, MessageToast) {
    "use strict";

    return Controller.extend("ui5.ticketui.controller.Home", {

        init: function () {
            
        },                

        onButtonSubmitPress: function () {
            MessageToast.show("User logged in!");
            this.getOwnerComponent().getRouter().navTo("dashboard");
        },

        onButtonCancelPress: function () {

            //empty user input fields
            var oInput1 = this.byId("usernameInput");
            var oInput2 = this.byId("passwordInput");
            oInput1.setValue("");
            oInput2.setValue("");
            // Show a cancellation message
            MessageToast.show("Login cancelled!");
        }
    });
});
