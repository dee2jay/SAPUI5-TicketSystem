sap.ui.define(["sap/ui/model/json/JSONModel"], function (JSONModel) {
    "use strict";

    const model = new JSONModel({
        token: localStorage.getItem("auth_token") || null
    });

    return {
        setToken(token) {
            model.setProperty("/token", token);
            localStorage.setItem("auth_token", token);
        },

        getToken() {
            return model.getProperty("/token") || localStorage.getItem("auth_token");
        },

        clear() {
            model.setProperty("/token", null);
            localStorage.removeItem("auth_token");
        },

        getModel() {
            return model;
        }
    };
});
