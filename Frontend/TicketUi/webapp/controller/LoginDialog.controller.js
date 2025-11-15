sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "sap/m/MessageToast",       
    "sap/ui/model/json/JSONModel"
], (Controller, MessageToast, JSONModel) => {
    "use strict";

    return Controller.extend("ui5.ticketui.controller.LoginDialog", {
        onLogin: function () {
             const username = this.byId("usernameInput").getValue();
            const password = this.byId("passwordInput").getValue();

            if (!username || !password) {
                MessageToast.show("Please fill in both fields.");
                return;
            }

            MessageToast.show("Logging in...");

            //Call backend API for authentication
             fetch("/api/login", {
                 method: "POST",
                 headers: {
                     "Content-Type": "application/json"
                 },
                 body: JSON.stringify({ username, password })
             })
             .then(response => response.json())
             .then(data => {
                 if (data.success) {
                     MessageToast.show("Login successful!");
                     // Navigate to the next view or perform other actions
                 } else {
                     MessageToast.show("Login failed: " + data.message);
                 }
             })
             .catch(error => {
                 MessageToast.show("Error during login: " + error.message);
             });
        }
    });
});