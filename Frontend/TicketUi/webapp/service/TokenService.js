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

        isTokenValid: function (token) {
            
            if (!token) return false;
            try {
                const payload = JSON.parse(atob(token.split('.')[1])); // Decode base64 payload
                const now = Math.floor(Date.now() / 1000); // timestamp en secondes
                return payload.exp && payload.exp > now;
            } catch (e) {
                console.error("Invalid token format", e);
                return false;
    }
}
    };
});
