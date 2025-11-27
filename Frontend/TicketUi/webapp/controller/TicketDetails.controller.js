sap.ui.define([
	"sap/ui/core/mvc/Controller",
    "sap/m/MessageToast",
    "sap/ui/core/routing/History"
], (Controller, MessageToast, History) => {
	"use strict";

	return Controller.extend("ui5.ticketui.controller.TicketDetails", {

        onInit: function() {
            var oRouter = this.getOwnerComponent().getRouter();

            oRouter.getRoute("ticketDetails").attachPatternMatched(this._onMatched, this);
        },

        _onMatched(oEvent){
             const id = oEvent.getParameter("arguments").ticketId;

             // Load ticket details by ID
             const sTicketPath = this._findTicketPathById(id);
            if (sTicketPath) {
                 this.getView().bindElement({
                    path: sTicketPath,
                    model: "ticketsModel"
                 });
            }            
        
        },

        _findTicketPathById: function (id) {
            const oModel = this.getView().getModel("ticketsModel");
            const aTickets = oModel.getProperty("/tickets");

            const index = aTickets.findIndex(t => t.id == id);
            if (index !== -1) {
                return "/tickets/" + index;
            }
            return null;
        },

       onHistoryButtonPress: function(){
        MessageToast.show("Ticket History");
        const oRouter = this.getOwnerComponent().getRouter();
        const sTicketPath = this.getView().getBindingContext("ticketsModel").getPath();
        const oTicket = this.getView().getModel("ticketsModel").getProperty(sTicketPath);

        console.log("ticket", oTicket);
        const sTicketId = oTicket && oTicket.id;

        oRouter.navTo("ticketHistory", { ticketId: sTicketId });

       },
       onPrintButtonPress: function(){
        window.print();

       },
       onSaveButtonPress: function(){
        MessageToast.show("Ticket saved")

       },
        onCancelButtonPress: function(){
            let oHistory = History.getInstance();
            let sPrevHash = oHistory.getPreviousHash();

            if(sPrevHash !== undefined){
                window.history.go(-1);
            } else{
                this.getOwnerComponent().getRouter().navTo("tickets", {}, { skipHistory: true });
            }
        },
        onNavBack() {
            const oHistory = History.getInstance();
            const sPreviousHash = oHistory.getPreviousHash();

            if (sPreviousHash !== undefined) {
                window.history.go(-1);
            } else {
                const oRouter = this.getOwnerComponent().getRouter();
                oRouter.navTo("tickets", {}, { skipHistory: true });
            }
        },
        onAttachmentsButtonPress: function(){
            MessageToast.show("Attachments button pressed");

            // Implement attachment handling logic here
            //call file upload dialog or navigate to attachments view            
            if (!this._oAttachmentsDialog) {
                const oView = this.getView();
                const oUpload = new sap.m.UploadCollection({
                    uploadUrl: "/api/tickets/upload", 
                    maximumFilenameLength: 100,
                    multiple: true,
                    instantlyUpload: false, 
                    change: this._onUploadChange.bind(this),
                    beforeUploadStarts: this._onBeforeUploadStarts.bind(this),
                    uploadComplete: this._onUploadComplete.bind(this),
                    fileDeleted: this._onFileDeleted.bind(this)
                });

            this._oAttachmentsDialog = new sap.m.Dialog({
                title: "Ticket - {i18n>titleTicketAttachments}",
                content: [oUpload],
                beginButton: new sap.m.Button({
                    text: "{i18n>buttonUpload}",
                    press: function () {
                        oUpload.upload(); // lance upload pour les fichiers sélectionnés
                    }
                }),                
                endButton: new sap.m.Button({
                    text: "{i18n>buttonClose}",
                    press: function () { this._oAttachmentsDialog.close(); }.bind(this)
                }),
                
                afterClose: function () 
                { 
                    this._oAttachmentsDialog.close(); 
                }
            });

            oView.addDependent(this._oAttachmentsDialog);
            this._oUploadCollection = oUpload;
            }
        const sTicketPath = this.getView().getBindingContext("ticketsModel").Path; // ex: /tickets/3
        const oTicket = this.getView().getModel("ticketsModel").getProperty(sTicketPath);
        const sTicketId = oTicket && oTicket.id;
        this._oUploadCollection.setUploadUrl(`/api/tickets/${sTicketId}/attachments`);

        this._oAttachmentsDialog.open();
        },
        
        _onBeforeUploadStarts: function (oEvent) {
    // Ajouter en-têtes (token ...). Utilisez votre TokenService réel.
    const sToken = sap.ui.require("ui5.ticketui.service.TokenService")?.getToken?.() || null;
    if (sToken) {
        const oHeader = new sap.m.UploadCollectionParameter({
            name: "Authorization",
            value: "Bearer " + sToken
        });
        oEvent.getParameters().addHeaderParameter(oHeader);
    }
    // Contrôle taille/type:
    const oFile = oEvent.getParameter("file");
    if (oFile.size > 10 * 1024 * 1024) { // 10 MB
        sap.m.MessageToast.show("Fichier trop volumineux (max 10MB).");
        oEvent.preventDefault(); // annule l'upload de ce fichier
    }
        },

        _onUploadChange: function (oEvent) {
        // Optionnel: validation immédiate des fichiers choisis
            const aFiles = oEvent.getParameter("files");
            for (let i = 0; i < aFiles.length; i++) {
                const oFile = aFiles[i];
                if (oFile.size > 10 * 1024 * 1024) { // 10 MB
                    sap.m.MessageToast.show("{i18n>attachementValidationError}");                
         
                    oEvent.preventDefault(); // annule l'upload de ce fichier                   
                } 
            }
        },

        _onUploadComplete: function (oEvent) {
            // Parsez la réponse du serveur et mettez à jour le modèle
            const sResponse = oEvent.getParameter("response");
            // Ex: le serveur renvoie JSON { id, filename, url }
            try {
                const oJson = JSON.parse(sResponse);
                // ajouter l'attachment dans le modèle ticketsModel sous le ticket en cours
                const oModel = this.getView().getModel("ticketsModel");
                const sPath = this.getView().getBindingContext("ticketsModel").getPath();
                const aAttachments = oModel.getProperty(sPath + "/attachments") || [];
                aAttachments.push(oJson);
                oModel.setProperty(sPath + "/attachments", aAttachments);
                sap.m.MessageToast.show(this.getView().getModel("i18n").getResourceBundle().getText("messageTicketSaved") || "Upload OK");
            } catch (e) {
                // certaines implémentations renvoient vide; handle accordingly
                sap.m.MessageToast.show("Upload terminé. Actualisez la liste si nécessaire.");
            }
        },
        _onFileDeleted: function (oEvent) {

            const sDocumentId = oEvent.getParameter("documentId"); 
                fetch(`/api/attachments/${sDocumentId}`, { method: "DELETE", headers: { "Authorization": "Bearer " + /* token */ "" }})
                    .then(resp => {
                        if (resp.ok) {
                            // retirer du modèle
                            const oModel = this.getView().getModel("ticketsModel");
                            const sPath = this.getView().getBindingContext("ticketsModel").getPath();
                            const aAttachments = oModel.getProperty(sPath + "/attachments") || [];
                            const i = aAttachments.findIndex(a => a.id == sDocumentId);
                            if (i !== -1) {
                                aAttachments.splice(i, 1);
                                oModel.setProperty(sPath + "/attachments", aAttachments);
                            }
                            sap.m.MessageToast.show("Attachment deleted");
                        } else {
                            sap.m.MessageToast.show("Delete failed");
                        }
                    }).catch(err => {
                        sap.m.MessageToast.show("Error deleting attachment");
                    });                 
        }   
           
    });
});