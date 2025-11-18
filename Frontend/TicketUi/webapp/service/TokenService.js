sap.ui.define(["sap/ui/model/json/JSONModel"], function (JSONModel) {
    "use strict";

    const model = new JSONModel({ token: null });

    return {
        setToken(token) {
            model.setProperty("/token", token);
        },

        getToken() {
            return model.getProperty("/token");
        },

        clear() {
            model.setProperty("/token", null);
        },

        getModel() {
            return model;
        }
    };
});
