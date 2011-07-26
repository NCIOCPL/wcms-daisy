NCITreeView = Class.create({

	initialize : function(ctrlId, options)
	{
	    this._treeId = ctrlId + '_rootlist';
	    this._tree = $(this._treeId);
	    this._hiddenFieldID = ctrlId + '_hiddenField';
	    this._hiddenField = $(this._hiddenFieldID);
	    this._state = [];
	    
	    if (this._hiddenField) {
	        this._state = this._hiddenField.getValue().toArray();
	    }
	    
	    this._nodeIDPrefix = ctrlId + "_ttvnode_";
	    
		this._options = {
		    expandImageSrc: '/images/spacer.gif',
   		    expandText: 'Expand node', 
   		    collapseText: 'Collapse node',
   		    expandLinkClass: 'expand-link',
   		    expandedLiClass: 'expanded',
   		    collapsedLiClass: 'collapsed'
		};
		Object.extend(this.options,options || {});
		
		this.setupTreeView();
	},
	
	//Adds the relevant onclick handlers for the nodes in the tree view 
	setupTreeView : function ()
	{ 
		//var treeElements = this._tree.select('li[class~="ttvnode"]');
		var treeElements = this._tree.select('li.ttvnode');
		
		//Loop through all of the LI tags in the tree and add the expand/collapse link
		//and set the current class.
		var treeElementsSize = treeElements.length;
		for (var i=0; i < treeElementsSize; i++)
		{
		    //The id contains the index of this node's state
		    var pos = treeElements[i].id.substr(this._nodeIDPrefix.length, treeElements[i].id.length - this._nodeIDPrefix.length);
		    
		    var nodeState = this._state[pos];
		    if (nodeState != 'l') {
			    var expandLink = Element.extend(document.createElement('a'));
				var expandImg = Element.extend(document.createElement('img'));
				expandLink.href = "#";
				expandLink.addClassName(this._options.expandLinkClass);
				expandImg.src = this._options.expandImageSrc;
				expandLink.id = treeElements[i].id + '_expander';
				expandLink.insert( { top: expandImg } );
				treeElements[i].insert( { top: expandLink } );
				
				expandLink.observe('click', this.toggleNodeStateHandler.bindAsEventListener(this));
								
				if (nodeState == 'e') {
				    this.expandNode(treeElements[i], expandLink, expandImg);
				} else if (nodeState == 'c') {
				    this.collapseNode(treeElements[i], expandLink, expandImg);
				}		        
		    }
		}
	},
	expandNode : function (liElem, linkElem, imgElem){
	    liElem.addClassName(this._options.expandedLiClass);	    
	    linkElem.setAttribute('title', this._options.collapseText);
	    imgElem.setAttribute('alt', this._options.collapseText);
	    this.updateNodeState(liElem, 'e');	
	},
	collapseNode : function (liElem, linkElem, imgElem){
	    liElem.addClassName(this._options.collapsedLiClass);	    
	    linkElem.setAttribute('title', this._options.expandText);
	    imgElem.setAttribute('alt', this._options.expandText);
	    this.updateNodeState(liElem, 'c');
	},
	updateNodeState : function(liElem, state) {
	    var pos = liElem.id.substr(this._nodeIDPrefix.length, liElem.id.length - this._nodeIDPrefix.length);
	    this._state[pos] = state;
	    this._hiddenField.value = this._state.join('');
	},
		
	//The toggle event handler for each expandable/collapsable node 
	//- Note that this also exists to prevent any IE memory leaks 
	//(due to circular references caused by this) 
	toggleNodeStateHandler : function (e)
	{ 
		//this.toggleClass(event.element(), (event == null)? window.event : event);
		//var lielem = Event.element(e).up('li');
		var liElem = null;
		var eventElem = Event.element(e);
		
		if (eventElem.tagName == "IMG") {
		    liElem = eventElem.up().up();
		} else if (eventElem.tagName == "A") {
		    liElem = eventElem.up();
		}
		
		if (liElem != null) {
    		var linkElem = liElem.down();
    		var imgElem = liElem.down().down();
		
		    if (liElem.hasClassName(this._options.expandedLiClass)) {
		        liElem.removeClassName(this._options.expandedLiClass);
		        this.collapseNode(liElem, linkElem, imgElem);		        
		    } else {
   		        liElem.removeClassName(this._options.collapsedLiClass);
   		        this.expandNode(liElem, linkElem, imgElem);		        
		    }
		}
		
		Event.stop(e);
		return false;
	}
});