sap.ui.define(["sap/ui/model/json/JSONModel"], function(JSONModel) {
    "use strict";

    return {
        create: function() {
            return new JSONModel(
                {   
                    "title": "",
                    "description": "",
                    "author": "",
                    "createdAt": "",
                    "updatedAt": "",
                    "status": "",
                    "priority": "",
                    "category": "",
                    "location": "",
                    "costCenter": "",
                    "assignedTo": null,
                    "orderNumber": "",
                    "userId": null,
                    "user": null,
                    "attachments": [],
                    "comments": [],
                    "histories": []
                }
            );
        }
    }
});
