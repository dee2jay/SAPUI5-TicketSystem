sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "sap/ui/model/resource/ResourceModel",
    "sap/ui/core/Fragment",
    "sap/m/MessageToast"
], function (Controller, ResourceModel, Fragment, MessageToast) {
    "use strict";

    return Controller.extend("ui5.ticketui.controller.App", {

        onInit: function () {
            const i18nModel = new ResourceModel({
                bundleName: "ui5.ticketui.i18n.i18n"
            });
            this.getView().setModel(i18nModel, "i18n");

            this._loginDialog = null;
        },

        onLoginButtonPress: function (){       
           let that = this;

            if (!this._loginDialog) {
                Fragment.load({
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
                const oResult = await UserService.login((this.byId("loginDialog")).byId("username").getValue(), (this.byId("loginDialog")).byId("password").getValue());
                
                MessageToast.show("Login success!");

                // Load current user
        const me = await UserService.getCurrentUser();
        this.getOwnerComponent().setModel(new JSONModel(me), "userModel");

        this.getRouter().navTo("Overview");
            

            this._loginDialog.close();
            }catch{
                MessageToast.show("Login failed");
                this._loginDialog.close();
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
