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
                this.getOwnerComponent().getRouter().navTo("home", {});
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
        //this._refreshHistory();
        const oRouter = this.getOwnerComponent().getRouter();        
        const sTicketPath = this.getView().getBindingContext("ticketsModel").getPath();       
        const oTicket = this.getView().getModel("ticketsModel").getProperty(sTicketPath);     
        const sTicketId = oTicket && oTicket.id;
        oRouter.navTo("ticketHistory", { ticketId: sTicketId });
       },
       onPrintButtonPress: function(){
        window.print();

       },
       onSaveButtonPress: function(oEvent){
            try{
                const oCtx = oEvent.getSource().getBindingContext("ticketsModel");            
                if (!oCtx) {
                    console.error("No binding context");
                    return;
                }

                const sPath = oCtx.getPath();
                const ticket = this.getView().getModel("ticketsModel").getProperty(sPath);

                const response = TicketService.updateTicket(ticket.id, ticket);
                
                MessageToast.show("Ticket saved"); 
                this._refreshHistory;           
            
                this.onNavBack();
            }catch(error){
                console.log(error);
            }
            
            

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
        onPageNavButtonPress() {
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

        onFileUploaderChange: function(oEvent) {
            
            const oItem = oEvent.getSource();
            const oContext = oItem.getBindingContext("ticketsModel");
            const ticketData = oContext.getObject();           // ← ticket            
            const ticketId = ticketData.id;
            const oFile = oEvent.getParameter("files")[0];
            
            if (!oFile) {
                return;
            }

            const oModel = this.getView().getModel("attachmentViewModel");            

            oModel.setProperty("/attachment/file", oFile);
            oModel.setProperty("/attachment/fileName", oFile.name);
            oModel.setProperty("/attachment/size", oFile.size);
            oModel.setProperty("/ticketId", ticketId);
        },

        onFileUploderUploadComplete: function(oEvent) {
            const response = oEvent.getParameter("responseRaw");

            console.log("response: ", response);
            try {
                const oJson = JSON.parse(response); // { id, filename, url }

                console.log("oJson: ", oJson);

                const sPath = this.getView().getBindingContext("ticketsModel").getPath();
                const oModel = this.getView().getModel("ticketsModel");

                const aAttachments = oModel.getProperty(sPath + "/attachments") || [];
                aAttachments.push(oJson);
                oModel.setProperty(sPath + "/attachments", aAttachments);

                sap.m.MessageToast.show("Upload OK !");
                this._refreshAttachmentList();
                this._refreshHistory();
            } catch (e) {
                console.log(e);
                sap.m.MessageToast.show("Upload NOK, but invalid response.");
            }
        },

        onDialogAfterClose: function(){
            this._refreshAttachmentList();
            this._refreshHistory();
            this._attachmentDialog.close();
        },

        onButtonClosePress: function(){
            this._attachmentDialog.close();
        },

        onUploadButtonPress: function(oEvent) {                        
            
            const oModel = this.getView().getModel("attachmentViewModel");            
           
            const data = oModel.getData();
            const oToken = localStorage.getItem("auth_token");            
            
            if(!TokenService.isTokenValid(oToken))
            {
                MessageToast.show("Session expired");
                this.getOwnerComponent().getRouter().navTo("home", {}, true);
                return;
            }
            
            if (!data.attachment.file) {
                sap.m.MessageToast.show("Select a file first!");
                return;
            }
            
            const formData = new FormData();
            formData.append("file", data.attachment.file);
    
            formData.append("attachmentContext", JSON.stringify({
                description: data.attachment.description,
                type: data.attachment.type
            }));
            
            const headers = {
                "Authorization": "Bearer " + TokenService.getToken(),
            };

            const oUploader = sap.ui.getCore().byId("idFileUploader");

            fetch(`https://localhost:7187/api/tickets/${data.ticketId}/uploadAttachment`, {
                method: "POST",
                headers: {
                    "Authorization": "Bearer " + TokenService.getToken()
                },
                body: formData
            })
            .then(r => r.ok ? r.json() : Promise.reject(r))
            .then(() => {
                MessageToast.show("Attachment uploaded");
                this._resetAttachmentModel();
            })
            .catch(() => {
                oModel.setProperty("/state/error", "Upload failed");
            })
            .finally(() => {
                oModel.setProperty("/attachment/file", null);
                oModel.setProperty("/attachment/fileName", "");
                oModel.setProperty("/attachment/size", 0);                
                oModel.setProperty("/state/uploading", false);                
                this.onDialogAfterClose();
            });            
            
        },

        onButtonDeletePress: async function (oEvent) {
            const oBundle = this.getView().getModel("i18n").getResourceBundle();
            
            const oItemAttachement = oEvent.getSource().getBindingContext("ticketsModel");
            
            const oAttachment = oItemAttachement.getObject();
            const sAttachmentPath = oItemAttachement.sPath;
            const attachmentId = oAttachment.id;
            const sTicketPath = sAttachmentPath.split("/attachments")[0]; // "/tickets/0"
            const oTicket = this.getView().getModel("ticketsModel").getProperty(sTicketPath);
            const ticketId = oTicket.id;

            try {
                const sToken = TokenService.getToken();

                if(!TokenService.isTokenValid(sToken))
                {
                    MessageToast.show("Session expired");
                    this.getOwnerComponent().getRouter().navTo("home", {}, true);
                    return;
                }

                const response = await fetch(`https://localhost:7187/api/tickets/${ticketId}/deleteAttachment/${attachmentId}`, {
                    method: "DELETE",
                    headers: {
                        "Authorization": "Bearer " + sToken
                    }
                });

                if (!response.ok) {
                    MessageToast.show("Issue during deletion");
                    return;
                }

                // Mise à jour du modèle local
                const sPath = this.getView().getBindingContext("ticketsModel").getPath();
                const oModel = this.getView().getModel("ticketsModel");

                let aAttachments = oModel.getProperty(sPath + "/attachments") || [];
                aAttachments = aAttachments.filter(a => a.id !== attachmentId);
                oModel.setProperty(sPath + "/attachments", aAttachments);

                MessageToast.show("Attachment deleted");

            } catch (err) {
                console.error(err);
                MessageToast.show("Issue during Deletion");
            }
        },
        
        onSendButtonPress: async function(oEvent)
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

            // -- Update API
            await TicketService.addComment(ticketId, comment);

                // -- Refresh UI bindings
            this.getView().getModel("ticketsModel").refresh(true);

            oCommentModel.setData({ author: "", text: "", createAt: "" });

            MessageToast.show("Comment sent!");
            
            this._refreshCommentList();
            this._refreshHistory();
        },

        _refreshAttachmentList: function() {
            const sPath = this.getView().getBindingContext("ticketsModel").getPath();
            const oTicket = this.getView().getModel("ticketsModel").getProperty(sPath);

            fetch(`https://localhost:7187/api/tickets/${oTicket.id}/attachments`, {
                headers: {
                    "Authorization": "Bearer " + TokenService.getToken()
                }
            })
            .then(res => res.json())
            .then(data => {
                // Mettre à jour le modèle ticketsModel>attachments
                const oModel = this.getView().getModel("ticketsModel");
                oModel.setProperty(sPath + "/attachments", data);
            })
            .catch(err => console.error(err));
        },

        _refreshCommentList: function() {
            const sPath = this.getView().getBindingContext("ticketsModel").getPath();
            const oTicket = this.getView().getModel("ticketsModel").getProperty(sPath);

            fetch(`https://localhost:7187/api/tickets/${oTicket.id}/comments`, {
                headers: {
                    "Authorization": "Bearer " + TokenService.getToken()
                }
            })
            .then(res => res.json())
            .then(data => {
                
                const oModel = this.getView().getModel("ticketsModel");
                oModel.setProperty(sPath + "/comments", data);
            })
            .catch(err => console.error(err));
        },

        _refreshHistory: function() {
            const sPath = this.getView().getBindingContext("ticketsModel").getPath();
            const oTicket = this.getView().getModel("ticketsModel").getProperty(sPath);

            fetch(`https://localhost:7187/api/tickets/${oTicket.id}/histories`, {
                headers: {
                    "Authorization": "Bearer " + TokenService.getToken()
                }
            })
            .then(res => res.json())
            .then(data => {
                
                const oModel = this.getView().getModel("ticketsModel");
                oModel.setProperty(sPath + "/histories", data);
            })
            .catch(err => console.error(err));
        }
           
    });
});