sap.ui.define([
    "ui5/ticketui/service/ApiService",
    "ui5/ticketui/service/TokenService"
], function (ApiService, TokenService) {

    "use strict";

    return {
        async getCurrentUser() {
            const token = TokenService.getToken();
            return ApiService.get("/api/User/me", token);
        }
    };
});