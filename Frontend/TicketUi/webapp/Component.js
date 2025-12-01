sap.ui.define([
    "sap/ui/core/UIComponent",
    "sap/ui/model/resource/ResourceModel",
    "sap/ui/model/json/JSONModel",
    "ui5/ticketui/model/CommentViewModel"
], function(UIComponent,
	ResourceModel,
	JSONModel,
	CommentViewModel) {
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
                  
            //define and set comment model
            const oCommentModel= CommentViewModel.create();
            this.setModel(oCommentModel, "commentViewModel");     

            const oModel = this.getModel("ticketsModel");

         // Restore model state from localStorage
            const savedState = localStorage.getItem("ticketsModelState");
            if (savedState) {
                try {
                    oTicketsModel.setData(JSON.parse(savedState));
                } catch (e) {
                    console.error("Failed to parse saved ticketsModelState:", e);
                }
            }

            // Hook setData to save automatically to localStorage
            const originalSetData = oTicketsModel.setData.bind(oTicketsModel);
            oTicketsModel.setData = function (data) {
                originalSetData(data);
                try {
                    localStorage.setItem("ticketsModelState", JSON.stringify(this.getData()));
                } catch (e) {
                    console.error("Failed to save ticketsModelState:", e);
                }
            };         
            
            
            this.getRouter().initialize();
        }
    });
});