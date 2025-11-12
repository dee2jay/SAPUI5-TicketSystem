sap.ui.define([
   "sap/ui/core/mvc/Controller",   
], (Controller) => {
   "use strict";
      
  return Controller.extend("ticket-ui.controller.App", {
   onInit: function () {
      console.log("App Controller initialized");
   }
   });
});