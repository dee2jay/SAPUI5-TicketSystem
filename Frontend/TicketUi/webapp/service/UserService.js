sap.ui.define([], function () {
    "use strict";

    const BASE_URL = "https://localhost:7187/";
    
    return{
        login: async function (username, password) {
            const oResponse = await fetch('${BASE_URL}/User/login',{
                method:"POST",
                headers:{
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({username, password})
            });
            
            if(!oResponse.ok){
                throw new Error("Login failed");
            }

            const oData = await oResponse.json();
            localStorage.setItem("token", oData.token);

            return oData;
            
        },

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