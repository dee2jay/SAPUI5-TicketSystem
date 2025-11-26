sap.ui.define([], function () {
    "use strict";

    const BASE_URL = "https://localhost:7187/";
    
    return{
        getCurrentUser: async function () {
            const oToken = localStorage.getItem("token");

            const response = await fetch(`${API_URL}/User/me`, {
                method: "GET",
                headers: {
                    "Authorization": `Bearer ${token}`
                }
            });

            if (!response.ok) {
                throw new Error("Unauthorized");
            }

            return response.json();

        }
    }    
});