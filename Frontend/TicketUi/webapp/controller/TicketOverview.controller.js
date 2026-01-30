sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "ui5/ticketui/service/TicketService",
    "sap/ui/core/Fragment",
    "ui5/ticketui/util/formatter",
    "sap/m/MessageToast",
    "ui5/ticketui/model/TicketViewModel",    
    "ui5/ticketui/model/TicketCategoryViewModel",
    "ui5/ticketui/service/TokenService",
    "ui5/ticketui/service/CategoryService",
    "sap/ui/core/library"
], function(Controller,
    TicketService,
    Fragment,
    formatter,
    MessageToast,
    TicketViewModel,
    TicketCategoryViewModel,
    TokenService,
    CategoryService,
    coreLibrary) {
    "use strict";
    return Controller.extend("ui5.ticketui.controller.TicketOverview", {
        
        formatter: formatter,

        onInit: function () { 
            
            const token = TokenService.getToken();
            if (!token || !TokenService.isTokenValid(token)) {
                MessageToast.show("login session expired ")
                // Redirect to login
                this.getOwnerComponent().getRouter().navTo("home", {});
                return;
            }
            
            var oModel = this.getOwnerComponent().getModel("ticketsModel");
            this.getView().setModel(oModel, "ticketsModel");  
            
            var oTicketCategoryViewModel = this.getOwnerComponent().getModel("ticketCategoryViewModel");           

            this.getView().setModel(oTicketCategoryViewModel, "ticketCategoryViewModel");
            
            this._loadTickets();
            this._loadCategories();                 
           
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
            var oCategoryModel = this.getOwnerComponent().getModel("categoryViewModel");
            var oModel = this.getOwnerComponent().getModel("ticketsModel")
            
            this.getView().setModel(oCategoryModel, "categoryViewModel");
            

            this.getView().setModel(oModel, "ticketsModel");  
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
            this  
            //this.getView().getModel("categorytViewModel").setData(TicketCategoryViewModel.create().getData());
            this.getView().getModel("ticketViewModel").setData(TicketViewModel.create().getData());
        },

         _loadCategories: async function() {
            const oCategoryModel = this.getView().getModel("ticketCategoryViewModel"); 

            try {
                const list = await CategoryService.getCategories();                
                oCategoryModel.setProperty("/categories", list);                
            } catch (err) {
                console.error("Unable to load categories", err);
            } 
            
        },

        onSelectChange: function(oEvent) {                        
            var oSelectedItem = oEvent.getParameter("selectedItem");
            var oSelectedItem = oEvent.getParameter("selectedItem");
            var sKey = oSelectedItem.getKey();
            var sText = oSelectedItem.getText();          
            
            const oModel = this.getView().getModel("ticketViewModel");
            

            const oData = oModel.getData();
            
            oData.category = sText;
            
            oModel.refresh(true);
           
        },

        onCreateButtonPress: async function(){
            const oModel = this.getView().getModel("ticketViewModel");
            const newTicket = oModel.getData();

            let bValid = true;            
            try {  
                                
                this.getView().findAggregatedObjects(true, o =>
                    o.isA("sap.m.Input") && o.getRequired()
                )
                .forEach(oInput => {
                    if (!oInput.getValue()) {
                    oInput.setValueState("Error");
                    bValid = false;
                    }
                });                
                

                if(!bValid){
                    MessageToast.show("Please insert a valid location");
                    return;
                }

                var response = await TicketService.createTicket(newTicket);                
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

        onLocationInputLiveChange: function(oEvent) {
            
            const ValueState = coreLibrary.ValueState;
            var sValue = oEvent.getParameter("value");
            var oInput = oEvent.getSource();
            if (!sValue || sValue.trim() === "") {
                oInput.setValueState(ValueState.Error);
                var oI18n = this.getOwnerComponent().getModel("i18n");
                var sText = "Location is required";
                try {
                    sText = oI18n.getResourceBundle().getText("validationLocationRequired");
                } catch (e) {}
                oInput.setValueStateText(sText);
            } else {
                oInput.setValueState(ValueState.None);
                oInput.setValueStateText("");
            }
        },

        onSearchFieldsLiveChange: function (oEvent) {
            MessageToast.show("Searching...");
            const sQuery = oEvent.getParameter("query");
            const aFilters = [];  
        },

        onButtonViewDetailsPress: function (oEvent) {            
            const oItem = oEvent.getSource();
            const oContext = oItem.getBindingContext("ticketsModel");            
            const sTicketId = oContext.getProperty("id");
            
            localStorage.setItem("lastOpenedTicketId", sTicketId);

            this.getOwnerComponent().getRouter().navTo("ticketDetails", { ticketId: sTicketId });
        },

        onButtonToggleSideNavPress: function () {
            var oSideNav = this.byId("idSideNavigation");
            oSideNav.setExpanded(!oSideNav.getExpanded());
        },

        onSideNavigationItemSelect: function (oEvent) {
            const oItem = oEvent.getParameter("item");
            if (!oItem) return;

            const key = oItem.getKey();
            const oNavList = this.byId("id2NavigationList");

            const actions = {                                
                "tickets": {
                    item: this.byId("idTicketsNavigationListItem"),
                    fn: this.onTicketsNavigationListItemSelect?.bind(this)
                },
                "settings": {
                    item: this.byId("idSettingsNavigationListItem"),
                    fn: this.onSettingsNavigationListItemSelect?.bind(this)
                },
                "logout": {
                    item: this.byId("idLogoutNavigationListItem"),
                    fn: () => {
                        if (TokenService.getToken()) TokenService.clear();
                        MessageToast.show("session logged out successfully");
                        this.onLogoutNavigationListItemSelect?.();
                    }
            }
            };

            const action = actions[key];
            if (!action) return;

            if (action.item) {
                oNavList.setSelectedItem(action.item);
            } 

            if (action.fn) {
                action.fn();
            }
        },
        
        onTicketsNavigationListItemSelect: function(){
            this.getOwnerComponent().getRouter().navTo("tickets");
        },

        onSettingsNavigationListItemSelect: function(){
            this.getOwnerComponent().getRouter().navTo("settings");
        },

        onLogoutNavigationListItemSelect: function(){
            this.getOwnerComponent().getRouter().navTo("home");
        }, 
        
        onColumnListItemDetailPress: function(oEvent){
             const oItem = oEvent.getSource();
            const oContext = oItem.getBindingContext("ticketsModel");            
            const sTicketId = oContext.getProperty("id");
            
            localStorage.setItem("lastOpenedTicketId", sTicketId);

            this.getOwnerComponent().getRouter().navTo("ticketDetails", { ticketId: sTicketId });

        }

    });
});