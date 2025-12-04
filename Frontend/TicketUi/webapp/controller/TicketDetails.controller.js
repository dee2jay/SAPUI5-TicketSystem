sap.ui.define([
	"sap/ui/core/mvc/Controller",
    "sap/m/MessageToast",
    "sap/ui/core/routing/History",
    "ui5/ticketui/service/TicketService",
    "ui5/ticketui/service/TokenService",
    "ui5/ticketui/service/UserService",
    "sap/ui/core/Fragment",
    "ui5/ticketui/util/formatter"
], function(Controller,
	MessageToast,
	History,
	TicketService,
    TokenService,
	UserService,
    Fragment,
	formatter){
	"use strict";

	return Controller.extend("ui5.ticketui.controller.TicketDetails", {

        formatter: formatter,
       onInit: function() {

            const token = TokenService.getToken();
            if (!token || !TokenService.isTokenValid(token)) {
                MessageToast.show("login session expired ")
                // Redirect to login
                this.getOwnerComponent().getRouter().navTo("home", {}, true);
                return;
            }    

        const oModel = this.getOwnerComponent().getModel("ticketsModel");        

        const savedState = localStorage.getItem("ticketsModelState");
        if (savedState) {
            oModel.setData(JSON.parse(savedState));
        }
        
        oModel.attachPropertyChange(() => {
        localStorage.setItem("ticketsModelState", JSON.stringify(oModel.getData()));
        });      
        
        const oRouter = this.getOwnerComponent().getRouter();
        oRouter.getRoute("ticketDetails").attachPatternMatched(this._onMatched, this);
},

        _onMatched(oEvent){
             const ticketId = oEvent.getParameter("arguments").ticketId;

            if (!ticketId) {
                ticketId = localStorage.getItem("lastOpenedTicketId");
            }

            localStorage.setItem("lastOpenedTicketId", ticketId);
            
            // Load ticket details by ID
             const sTicketPath = this._findTicketPathById(ticketId);
            
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
                return  `/tickets/${index}`;
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
       onSaveButtonPress: function(oEvent){
            const oCtx = oEvent.getSource().getBindingContext("ticketsModel");            
            if (!oCtx) {
                console.error("No binding context");
                return;
            }

            const sPath = oCtx.getPath();
            const ticket = this.getView().getModel("ticketsModel").getProperty(sPath);

            const response = TicketService.updateTicket(ticket.id, ticket);
            if(response.OK){
                MessageToast.show("Ticket saved");
            }
        
        
        this.onNavBack();

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
            
            if (!this._attachmentDialog) {
                Fragment.load({
                    name: "ui5.ticketui.view.AttachmentDialog",
                    controller: this
                }).then(oDialog => {
                    this._attachmentDialog = oDialog;
                    this.getView().addDependent(oDialog);                    

                    oDialog.open();
                });
            } else {
                this._attachmentDialog.open();
            }           
        },

        onFileSelected: function(oEvent) {
            this._file = oEvent.getParameter("files")[0];            
        },

        onUploadComplete: function(oEvent) {
            const response = oEvent.getParameter("response");
            try {
                const oJson = JSON.parse(response); // { id, filename, url }

                const sPath = this.getView().getBindingContext("ticketsModel").getPath();
                const oModel = this.getView().getModel("ticketsModel");

                const aAttachments = oModel.getProperty(sPath + "/attachments") || [];
                aAttachments.push(oJson);
                oModel.setProperty(sPath + "/attachments", aAttachments);

                sap.m.MessageToast.show("Upload OK !");
            } catch (e) {
                console.log(e);
                sap.m.MessageToast.show("Upload NOK, but invalid response.");
            }
        },

        onDialogClose: function(){
            this._attachmentDialog.close();
        },

        onUploadPress: function() {
            
            const oBundle = this.getView().getModel("i18n").getResourceBundle();

            if (!this._file) {
                sap.m.MessageToast.show("Select a file first!");
                return;
            }
            const oToken = localStorage.getItem("auth_token");
            console.log(TokenService.isTokenValid(oToken));
            
            if(!TokenService.isTokenValid(oToken))
            {
                MessageToast.show("Session expired");
                this.getOwnerComponent().getRouter().navTo("home", {}, true);
                return;
            }

            const headers = {
                "Authorization": "Bearer " + TokenService.getToken(),
            };

            const oUploader = sap.ui.getCore().byId("fileUploaderDialog");

            oUploader.removeAllHeaderParameters();
            Object.keys(headers).forEach(key => {
                oUploader.addHeaderParameter(new sap.ui.unified.FileUploaderParameter({
                    name: key,
                    value: headers[key]
                }));
            });

            const sPath = this.getView().getBindingContext("ticketsModel").getPath();
            const oTicket = this.getView().getModel("ticketsModel").getProperty(sPath);                       
            oUploader.setUploadUrl(`https://localhost:7187/tickets/${oTicket.id}/uploadAttachment`);
            oUploader.upload();
        },

        onDeleteAttachmentPress: async function (oEvent) {
    const oBundle = this.getView().getModel("i18n").getResourceBundle();

    // Récupérer le contexte de la ligne
    const oItem = oEvent.getSource().getParent().getBindingContext("ticketsModel");
    const oAttachment = oItem.getObject();

    const attachmentId = oAttachment.id;
    const ticketId = this.currentTicketId;

    try {
        const sToken = TokenService.getToken();

        const response = await fetch(`/api/tickets/${ticketId}/attachments/${attachmentId}`, {
            method: "DELETE",
            headers: {
                "Authorization": "Bearer " + sToken
            }
        });

        if (!response.ok) {
            MessageToast.show(oBundle.getText("attachmentDeletionError") || "Fehler beim Löschen");
            return;
        }

        // Mise à jour du modèle local
        const sPath = this.getView().getBindingContext("ticketsModel").getPath();
        const oModel = this.getView().getModel("ticketsModel");

        let aAttachments = oModel.getProperty(sPath + "/attachments") || [];
        aAttachments = aAttachments.filter(a => a.id !== attachmentId);
        oModel.setProperty(sPath + "/attachments", aAttachments);

        MessageToast.show(oBundle.getText("attachmentDeleted") || "Attachment supprimé");

    } catch (err) {
        console.error(err);
        MessageToast.show(oBundle.getText("attachmentDeletionError") || "Erreur suppression");
    }
},
        
        onSendenButtonPress: async function(oEvent)
        {
            const oItem = oEvent.getSource();
            const oContext = oItem.getBindingContext("ticketsModel");
            const ticketData = oContext.getObject();           // ← ticket
            const ticketId = ticketData.id;

            // -- Comment model
            const oCommentModel = this.getView().getModel("commentViewModel");
            
            const commentData = oCommentModel.getData();

            if(!commentData.text){
                MessageToast.show("Please, empty comment is not supported");
                return;
            }

            
            const comment = { ...oCommentModel.getData() };


            // -- Get current user 
            const currentUser = await UserService.getCurrentUser();
            comment.author = currentUser.fullname;
            comment.createAt = new Date().toISOString();

            // -- Add new comment safely
            ticketData.comments = ticketData.comments || [];
            ticketData.comments.push(comment);           

            // -- Update API
            await TicketService.updateTicket(ticketId, ticketData);

                // -- Refresh UI bindings
            this.getView().getModel("ticketsModel").refresh(true);

            oCommentModel.setData({ author: "", text: "", createAt: "" });

            MessageToast.show("Comment sent!");
        }
           
    });
});