sap.ui.define([
    "ui5/ticketui/service/ApiService",
    "ui5/ticketui/service/TokenService"
], function (ApiService, TokenService) {

    "use strict";

    return {
        async login(email, password) {
            const endpoint = "/api/User/login";

            try {
                // Call API to login
                const res = await ApiService.post(endpoint, {
                    email,
                    password
                }, null);

                // We only store the token if it has been received.
                if (res && res.token) {
                    TokenService.setToken(res.token);
                }

                return res;

            } catch (e) {
                // Rethrow the error so the controller can display it
                throw new Error("Login failed: " + e.message);
            }
        },

        logout() {
            TokenService.clear();
        },
        isAuthenticated: function () {
            const token = localStorage.getItem("auth_Token");
            return !!token;
        }
    };
});
