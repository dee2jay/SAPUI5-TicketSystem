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
                "viewName": "ui5.ticketui.view.App",                
                "type": "XML",
                "async": true,
            }
        },

        init: function() {
            // call the init function of the parent
            UIComponent.prototype.init.apply(this, arguments);

            // set i18n model
            const i18nModel = new ResourceModel({
                bundleName: "ui5.ticketui.i18n.i18n",
                supportedLocales: ["","en", "de"],                
                fallbackLocale: "en",
                async: true
            });
            this.setModel(i18nModel, "i18n");

            //define and set ticket model
            const oTicketsModel= new JSONModel({tickets: []});
            this.setModel(oTicketsModel, "ticketsModel");           

            this.getRouter().initialize();
        }
    });
});