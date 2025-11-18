sap.ui.define([], function () {
    "use strict";

    const BASE_URL = "https://localhost:7187";

    return {
        async request(method, endpoint, body = null, token = null) {
            const headers = {
                "Content-Type": "application/json"
            };

            if (token) {
                headers["Authorization"] = "Bearer " + token;
            }

            const res = await fetch(BASE_URL + endpoint, {
                method,
                headers,
                body: body ? JSON.stringify(body) : null
            });

            if (!res.ok) {
                const error = await res.text();
                throw new Error("HTTP " + res.status + ": " + error);
            }

            return res.json();
        },

        get(endpoint, token) {
            return this.request("GET", endpoint, null, token);
        },

        post(endpoint, body, token) {
            return this.request("POST", endpoint, body, token);
        }
    };
});
