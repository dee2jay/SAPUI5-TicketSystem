sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "ui5/ticketui/service/TicketService",
    "sap/ui/core/Fragment",
    "ui5/ticketui/util/formatter",
    "sap/m/MessageToast",
    "ui5/ticketui/model/TicketViewModel",
    "ui5/ticketui/service/TokenService"
], function(Controller,
	TicketService,
	Fragment,
	formatter,
	MessageToast,
	TicketViewModel,
	TokenService) {
    "use strict";
    return Controller.extend("ui5.ticketui.controller.TicketOverview", {
        
        formatter: formatter,

        onInit: function () { 
            
            const token = TokenService.getToken();
            if (!token || !TokenService.isTokenValid(token)) {
                MessageToast.show("login session expired ")
                // Redirect to login
                this.getOwnerComponent().getRouter().navTo("home", {}, true);
                return;
            }            

            var oModel = this.getOwnerComponent().getModel("ticketsModel");
            this.getView().setModel(oModel, "ticketsModel");
            
            this._loadTickets();

            
            this.getView().setModel(TicketViewModel.create(), "ticketViewModel");
        
        },

         _loadTickets: async function () {
            try {
                var tickets = await TicketService.getAllTickets();                
                
                this.getView().getModel("ticketsModel").setProperty("/", tickets);
                this.getView().getModel("ticketsModel").getData();                

            } catch (error) {
                console.error("Failed to load tickets:", error);
            }
        },

        onNewTicketButtonPress: function () {
            
            let that = this;            

            if (!this._ticketCreate) {
                Fragment.load({
                    name: "ui5.ticketui.view.TicketCreate",
                    controller: this
                }).then(function (oDialog) {
                    that._ticketCreate = oDialog;
                    that.getView().addDependent(oDialog);
                    oDialog.open();
                });
            } else {
                this._ticketCreate.open();
            }  
            
            this.getView().getModel("ticketViewModel").setData(TicketViewModel.create().getData());
        },

        onCreateButtonPress: async function(){
            const newTicket = this.getView().getModel("ticketViewModel").getData();
            console.log("newTicket", newTicket);
            try {
                    var response = await TicketService.createTicket(newTicket);
                    console.log("response", response);
                    if(!response)
                    {
                        sap.m.MessageToast.show("Error creating ticket");
                        this._ticketCreate.close();
                        return;
                    }
                    sap.m.MessageToast.show("Ticket created successfully");
                    this._ticketCreate.close();
                    this._loadTickets(); // reload list
            } catch (err) {
                console.log("error: ", err);
                sap.m.MessageToast.show("Error creating ticket");
            }
        },

        onSearchFieldsLiveChange: function (oEvent) {
            MessageToast.show("Searching...");
            //const sQuery = oEvent.getParameter("query");
            //const aFilters = [];  
        },

        onViewDetailsButtonPress: function (oEvent) {            
            const oItem = oEvent.getSource();
            const oContext = oItem.getBindingContext("ticketsModel");            
            const sTicketId = oContext.getProperty("id");
            
            localStorage.setItem("lastOpenedTicketId", sTicketId);

            this.getOwnerComponent().getRouter().navTo("ticketDetails", { ticketId: sTicketId });
        },

        onToggleSideNav: function () {
            var oSideNav = this.byId("sideNav");
            oSideNav.setExpanded(!oSideNav.getExpanded());
        },

        onNavSelect: function (oEvent) {
            var oItem = oEvent.getParameter("item"); 
            
            if(!oItem){
                return;
            }
            this.byId("sideNav").setSelectedItem(oItem);
            
            var key = oItem.getKey();
            
            var oNavList = this.byId("navList");

        switch (key) {
            case "dashboard": 
                //oNavList.setSelectedItem(this.byId("navDashboard"));
                this.getOwnerComponent().getRouter().navTo("dashboard");
                break;

            case "tickets":    
                //oNavList.setSelectedItem(this.byId("navTickets"));
                this.getOwnerComponent().getRouter().navTo("tickets");
                break;

            case "settings":
                //oNavList.setSelectedItem(this.byId("navSettings"));
                this.getOwnerComponent().getRouter().navTo("settings");
                break;
            case "logout":
                if(TokenService.getToken()){
                    TokenService.clear();   
                }            
                MessageToast.show("session logged out successfully")
                this.getOwnerComponent().getRouter().navTo("home");                      
                break;
            } 
        }      

    });
});