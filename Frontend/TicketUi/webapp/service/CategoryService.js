sap.ui.define([], function () {
    "use strict";
    const BASE_URL = "https://localhost:7187";
    return {

        async getCategories() {
            try {
                const response = await fetch(`${BASE_URL}/api/categories`, {
                    method: "GET",
                    headers: { "Content-Type": "application/json" }
                });

                if (!response.ok) {
                    throw new Error("API Error : " + response.status);
                }

                return await response.json(); // tableau [{key, text}, ...]
            } catch (err) {
                console.error("Error CategoryService.getCategories()", err);
                throw err;
            }
        }
    };
});