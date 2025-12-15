sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "sap/m/MessageToast",
    "ui5/ticketui/model/LoginUserViewModel",
    "ui5/ticketui/service/AuthService",
	"sap/m/Token",    
], function(Controller,
	MessageToast,
	LoginUserViewModel,
	AuthService,
	Token
	) {
    "use strict";

    return Controller.extend("ui5.ticketui.controller.Home", {

        onInit: function () {                                        
            const oLoginModel = LoginUserViewModel.create();
            this.getView().setModel(oLoginModel, "loginUserViewModel");
        },                

        onLoginButtonPress: async function () {
           
           var newLogin = this.getView().getModel("loginUserViewModel").getData();
           
           try{
            var response = await AuthService.login(newLogin.email, newLogin.password);
            if(!response){
            MessageToast.show("User login failed!!!");  
            return;  
            }

            MessageToast.show("User logged in!");            
            
            this.getOwnerComponent().getRouter().navTo("tickets");
           }catch(error){
            MessageToast.show("User login failed!!!"); 
            console.error(error);
           }            
        },

        onLoginWithEntraIDButtonPress: async function () {     
           
           MessageToast.show("User logged in!");            
            
            this.getOwnerComponent().getRouter().navTo("tickets");            
        }

    });
});
