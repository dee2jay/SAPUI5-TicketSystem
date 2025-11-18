sap.ui.define([
    "ui5/ticketui/service/ApiService",
    "ui5/ticketui/service/TokenService"
], function (ApiService, TokenService) {

    "use strict";

    return {
        async login(username, password) {
            const res = await ApiService.post("/api/User/login", {
                username,
                password
            });

            TokenService.setToken(res.token);
            return res;
        },

        logout() {
            TokenService.clear();
        }
    };
});