sap.ui.define([
	"sap/ui/core/UIComponent",
    "sap/ui/model/resource/ResourceModel",
    "sap/ui/model/json/JSONModel"
], function(UIComponent, ResourceModel, JSONModel) {
	"use strict";

	return UIComponent.extend("ui5.ticketui.Component", {
       metadata: {
            "manifest": "json",
            "interfaces": ["sap.ui.core.IAsyncContentCreation"],
            "rootView": {
                "viewName": "ticket-ui.view.App",
                "type": "XML",
                "id": "app",
                "async": true
            }
        },
        
        init() {
            UIComponent.prototype.init.apply(this, arguments);

            const i18nModel = new ResourceModel({
                bundleName: "ticket.ui.i18n.i18n"
            }, "i18n");           
            
            this.getRouter().initialize();            
        }
	});
});