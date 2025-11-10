sap.ui.define([
    "sap/ui/core/ComponentContainer"

], (ComponentContainer) => {
	"use strict";
    
   new ComponentContainer({
		name: "ui5.ticket-ui",
		settings : {
			id : "ticket-ui"
		},
		async: true
	}).placeAt("content");
});