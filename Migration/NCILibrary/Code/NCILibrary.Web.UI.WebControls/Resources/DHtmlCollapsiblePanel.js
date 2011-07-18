DHtmlCollapsiblePanel = Class.create({
	panelElem: false,
	panelBodyElem: false,
	headerTextElem: false,
	hiddenFieldElem: false,
	iconLinkElem: false,
	iconElem: false,
    initialize: function(element, options) {
        this.panelElem = $(element);
        this.panelBodyElem = $(element + '-body');
        this.headerTextElem = $(element + '-header-text');

   		this.options = {
   		    isCollapsed: false, 
   		    title: '', 
   		    effectDuration: 1.0, 
   		    expandIconUrl: false, 
   		    collapseIconUrl: false, 
   		    expandIconAltTag: '', 
   		    collapseIconAltTag: '', 
   		    iconHeight: '13px', 
   		    iconWidth: '13px'
		};
		Object.extend(this.options,options || {});

	    //Setup and rebuild the control
        //Create the expand/collapse icon, but only when they exist.                
        if (this.options.expandIconUrl && this.options.collapseIconUrl) 
        {              
            //This is the tricky part.  Basically if there are icons the html should now be
            //<div><img><h3></div> instead of <h3> .. so we need to build it up.
    		
            var iconUrl = this.options.collapseIconUrl;
            var iconAlt = this.options.collapseIconAltTag;                    
            if (this.options.isCollapsed) 
            {
                iconUrl = this.options.expandIconUrl;
                iconAlt = this.options.expandIconAltTag;    
            }
            
            var newHeader = Builder.node('div', {className: "panelHeader"});
            this.iconLinkElem = Builder.node('a', {id: this.panelElem.id + '-iconLink', className: "panelIconLink", href: "#"});
            this.iconElem = Builder.node('img', {id: this.panelElem.id + '-icon', className: 'panelIcon', border: '0', height: this.options.iconHeight, width: this.options.iconWidth, src: iconUrl, alt: iconAlt, title: iconAlt}); 
            	                    
            newHeader.appendChild(this.iconLinkElem);
            this.iconLinkElem.appendChild(this.iconElem);
            newHeader.appendChild(this.headerTextElem); //Does this move it?
            
            this.panelElem.appendChild(newHeader);
            this.panelElem.appendChild(this.panelBodyElem);
        }
        
        //Setup onclick of header text
        if (this.headerTextElem)
            this.headerTextElem.observe('click', this.toggle.bindAsEventListener(this));

        if (this.iconLinkElem) {
            Element.extend(this.iconLinkElem); //For some reason the anchor is not extended.  The Builder must not extend elements. (Extend adds the cool prototype functions)    
            this.iconLinkElem.observe('click', this.toggle.bindAsEventListener(this));
        }

        //Collapse the body if it is supposed to be.                           
        if (this.options.isCollapsed) {                    
            if (this.panelBodyElem)
                this.panelBodyElem.hide();
        }

	    //Hookup hiddenField...
        this.hiddenFieldElem = $(this.panelElem.id + '-collapsedStatus');
    },
    toggle: function(e) {
	    Effect.toggle(this.panelBodyElem.id, 'blind', {duration: this.options.effectDuration, afterFinish: this.effectFinished.bind(this)});                    	                               
        Event.stop(e);    
    },
    effectFinished: function(effect) {
    
        //Now somehow we need to swap out the icon image.
        if (this.options.expandIconUrl && this.options.collapseIconUrl) 
        {              
            if (this.iconElem.src.endsWith(this.options.expandIconUrl)) {
                this.iconElem.src = this.options.collapseIconUrl;
                this.iconElem.alt = this.options.collapseIconAltTag;    
                this.iconElem.title = this.options.collapseIconAltTag;    
            } else { 
                this.iconElem.src = this.options.expandIconUrl;
                this.iconElem.alt = this.options.expandIconAltTag;    
                this.iconElem.title = this.options.expandIconAltTag;    
            }
        }

        if (this.hiddenFieldElem)
            if ($F(this.hiddenFieldElem).toLowerCase() == "true")
                this.hiddenFieldElem.value = 'false';
            else
                this.hiddenFieldElem.value = 'true';
    }

});
