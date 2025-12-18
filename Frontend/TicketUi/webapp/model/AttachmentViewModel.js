sap.ui.define([
    "sap/ui/model/json/JSONModel"],
    function(JSONModel){
	"use strict";

	return {
        create: function() {
            return new JSONModel(
                {   
                    ticketId:"",
                    attachment:{
                        file:null,
                        fileName:"",                        
                    },
                    state:{
                        uploading:false,
                        error:null
                    }
                }
            );
        }
    }
});
