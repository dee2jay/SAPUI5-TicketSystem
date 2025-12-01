sap.ui.define([
    "ui5/ticketui/service/TokenService",
    "sap/m/MessageToast"
], function (
	TokenService,
	MessageToast) {
    "use strict";

    const BASE_URL = "https://localhost:7187";
    
    return{
        getCurrentUser: async function () {
            const oToken = localStorage.getItem("auth_token");

            if (!TokenService.isTokenValid(oToken)) {
                MessageToast.show("Session is expired! log again before procees");
                return;
            }
            const response = await fetch(`${BASE_URL}/api/User/me`, {
                method: "GET",
                headers: {
                    "Authorization": `Bearer ${oToken}`
                }
            });

            if (!response.ok) {
                throw new Error("Unauthorized");
            }

            return response.json();

        }
    }    
});