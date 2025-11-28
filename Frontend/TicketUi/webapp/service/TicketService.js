sap.ui.define([
    "ui5/ticketui/service/TokenService"
], function (
	TokenService) {
    "use strict";

    const BASE_URL = "https://localhost:7187";

    async function request(url, method = "GET", body = null) {
        const options = {
            method,
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${TokenService.getToken()}`
            }
        };

        if (body) {
            options.body = JSON.stringify(body);
        }

        try {
            const response = await fetch(url, options);
            if (!response.ok) {
                const text = await response.text();
                throw new Error(`HTTP ${response.status} - ${text}`);
            }
            return await response.json();
        } catch (err) {
            console.error("API request failed:", err);
            throw err;
        }
    }

    return {
        /** ---------------------------------------------
         *  GET all tickets
         * ----------------------------------------------*/
        getAllTickets: function () {
            return request(`${BASE_URL}/api/Tickets`, "GET");
        },

        /** ---------------------------------------------
         *  GET ticket by ID
         * ----------------------------------------------*/
        getTicketById: function (ticketId) {
            return request(`${BASE_URL}/ticket/${ticketId}`, "GET");
        },

        /** ---------------------------------------------
         *  CREATE new ticket
         * ----------------------------------------------*/
        createTicket: function (ticketDto) {
            return request(`${BASE_URL}/create`, "POST", ticketDto);
        },

        /** ---------------------------------------------
         *  UPDATE ticket
         * ----------------------------------------------*/
        updateTicket: function (ticketId, ticketDto) {
            return request(`${BASE_URL}/ticket/${ticketId}/update`, "PUT", ticketDto);
        },
        
    };
});
