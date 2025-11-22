sap.ui.define([
	"sap/ui/core/UIComponent"
], function(UIComponent) {
	"use strict";

	return UIComponent.extend("ui5.ticketui.Component", {
       metadata: {
            "manifest": "json",
            "interfaces": ["sap.ui.core.IAsyncContentCreation"],
            "rootView": {
                "viewName": "ui5.ticketui.view.App",
                "type": "XML",
                "id": "app",
                "async": true
            }
        },
        
        init: function() {
            UIComponent.prototype.init.apply(this, arguments);
          
            
            this.getRouter().initialize();            
        }
	});
});