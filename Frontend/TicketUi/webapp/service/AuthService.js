sap.ui.define([
    "ui5/ticketui/service/ApiService",
    "ui5/ticketui/service/TokenService"
], function (ApiService, TokenService) {

    "use strict";

    return {
        async login(username, password) {
            const endpoint = "/api/User/login";
            
            const res = await ApiService.post(endpoint, {
                username,
                password
            }, TokenService.getToken());

            TokenService.setToken(res.token);
            return res;
        },

        logout() {
            TokenService.clear();
        }
    };
});