sap.ui.define([
   "sap/ui/core/UIComponent",
   "sap/ui/model/json/JSONModel"   
], (UIComponent, JSONModel) => {
   "use strict";

   return UIComponent.extend("ticket-ui.Component", {
    metadata:{
         "interfaces": ["sap.ui.core.IAsyncContentCreation"],
         "manifest": "json"
        },

    init() {
         // call the init function of the parent
         UIComponent.prototype.init.apply(this, arguments);
         // set data model
         const oData = {
            recipient : {
               name : "World"
            }
         };
         const oModel = new JSONModel(oData);
         this.setModel(oModel);         

         this.getModel("Ticket").attachEventOnce("metadataFailed", function (oEvent) {
				/*eslint-disable no-alert */
				alert("Request to the OData remote service failed.\nDownload the sample to your local machine and read the Walkthrough Tutorial Step 25 to see any data here.");
				/*eslint-enable no-alert */
			});
      }
   });
});