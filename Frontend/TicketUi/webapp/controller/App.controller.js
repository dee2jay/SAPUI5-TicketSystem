sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "sap/ui/model/resource/ResourceModel",
    "sap/ui/core/Fragment",
    "sap/m/MessageToast",
    "ui5/ticketui/service/AuthService"
], function (Controller,
	ResourceModel,
	Fragment,
	MessageToast,
	AuthService) {
    "use strict";

    return Controller.extend("ui5.ticketui.controller.App", {

        onInit: function () {
             const i18nModel = new ResourceModel({
                 bundleName: "ui5.ticketui.i18n.i18n",
                 supportedLocales: ["en-US", "de-DE"],
                 fallbackLocale: "en-US",
                 async: true
             });
             this.getView().setModel(i18nModel, "i18n");

            this._loginDialog = null;
        },

        onLoginButtonPress: function (){       
           let that = this;

            if (!this._loginDialog) {
                this._loginDialog = Fragment.load({    
                    id: "loginDialogFragment",
                    name: "ui5.ticketui.view.LoginDialog",
                    controller: this
                }).then(function (oDialog) {
                    that._loginDialog = oDialog;
                    that.getView().addDependent(oDialog);
                    oDialog.open();
                });                
            } else {
                this._loginDialog.open();
            }
            
        },       
        

        onButtonSubmitPress: async function () {        

            try{
                
                const username = this._loginDialog.getContent()[0].getItems()[1].getValue();
                const password = this._loginDialog.getContent()[0].getItems()[3].getValue();

                console.log("USERNAME =", username);
                console.log("PASSWORD =", password);
                
                const oResult = await AuthService.login(username, password);
                
                MessageToast.show("Login success!");        

        this.getOwnerComponent().getRouter().navTo("Overview");
            

            this._loginDialog.close();
            }catch(e){
                MessageToast.show("Login failed");                
                console.error(e);
            }
        },

        onButtonCancelPress: function () {
            this._loginDialog.close();
            MessageToast.show("Login aborted");
        },

         onCancelSettingPress: function () {
            this._configDialog.close();
        },

        onSettingsButtonPress: function () {
           let that = this;

            if (!this._configDialog) {
                Fragment.load({
                    name: "ui5.ticketui.view.ConfigDialog",
                    controller: this
                }).then(function (oDialog) {
                    that._configDialog = oDialog;
                    that.getView().addDependent(oDialog);
                    oDialog.open();
                });
            } else {
                this._configDialog.open();
            }
        },

        onSaveButtonPress: function () {
            const oView = this.getView();
            const oThemeSelect = oView.byId("themeSelect");
            const oLangSelect = oView.byId("langSelect");
            const sSelectedTheme = oThemeSelect.getSelectedKey();
            const sSelectedLang = oLangSelect.getSelectedKey();
            
            // Save the selected theme and language to local storage or backend
            localStorage.setItem("appTheme", sSelectedTheme);
            localStorage.setItem("appLanguage", sSelectedLang);
        },

        onCancelButtonPress: function () {
            this.getView().byId("themeSelect").setSelectedKey(localStorage.getItem("appTheme") || "Light");
            this.getView().byId("langSelect").setSelectedKey(localStorage.getItem("appLanguage") || "English");
            this._oCreateDialog.close();
        }
    });
});
