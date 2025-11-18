sap.ui.define([
	"sap/ui/core/UIComponent",
    "sap/ui/model/resource/ResourceModel",
    "sap/ui/model/json/JSONModel",
    "ui5/ticketui/service/TokenService"
], function(UIComponent, ResourceModel, JSONModel, TokenService) {
	"use strict";

	return UIComponent.extend("ui5.ticketui.Component", {
       metadata: {
            "manifest": "json",
            "interfaces": ["sap.ui.core.IAsyncContentCreation"],
            "rootView": {
                "viewName": "ticketui.view.App",
                "type": "XML",
                "id": "app",
                "async": true
            }
        },
        
        init: function() {
            UIComponent.prototype.init.apply(this, arguments);

            const i18nModel = new ResourceModel({
                bundleName: "ticket.ui.i18n.i18n"
            }, "i18n");

            sap.ui.getCore().setModel(TokenService.getModel(), "auth");
            
            //Model Ticket
            var oTicketModel = new JSONModel();
            oTicketModel.loadData("/api/tickets");
            this.setModel(oTicketModel, "ticketsModel")

            //Model Comments
            var oCommentModel = new JSONModel({ comments: [] });            
            this.setModel(oCommentModel, "CommentsModel")
            
            this.getRouter().initialize();            
        }
	});
});