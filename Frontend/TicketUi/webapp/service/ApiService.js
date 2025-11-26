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

            const options = { method, headers };
            if (body) {
                options.body = JSON.stringify(body);
            }

            const res = await fetch(BASE_URL + endpoint, options);

            if (!res.ok) {
                const error = await res.text();
                throw new Error("HTTP " + res.status + ": " + error);
            }

            // Case OK but no content (204)
            if (res.status === 204) {
                return null;
            }

            return res.json();
        },

        get(endpoint, token) {
            return this.request("GET", endpoint, null, token);
        },

        post(endpoint, body, token = null) {
            return this.request("POST", endpoint, body, token);
        },

        put(endpoint, body, token = null) {
            return this.request("PUT", endpoint, body, token);
        }
    };
});
