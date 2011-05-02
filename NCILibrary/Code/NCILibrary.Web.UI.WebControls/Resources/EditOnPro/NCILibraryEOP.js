
function NCILibraryEOP(clientID) {
	
	this.clientID = clientID;
	this.eop = null;
	this.styleSheetURL = '';
	this.bodyClassName = '';
	this.additionalCSS = '';
	this.codeBase = './';
	this.uiConfigURL = 'uiconfig.xml';
	this.configURL = 'config.xml';
	this.height = 420;
	this.width = 710;

	if (typeof(_NCILibraryEOP_prototype_called) == 'undefined')
	{
		_NCILibraryEOP_prototype_called = true;					
		
		NCILibraryEOP.prototype.load = load;
		NCILibraryEOP.prototype.loadData = loadData;
		NCILibraryEOP.prototype.saveData = saveData;
		
	}
	
	function load() {
		this.eop = new editOnPro(
			this.width, 
			this.height, 
			this.clientID+'_MyEditOnPro', 
			this.clientID+'_myID', 
			this.clientID+'_eopObject.eop'
			);
		this.eop.setCodebase(this.codeBase);
		this.eop.setUIConfigURL(this.uiConfigURL);
		this.eop.setConfigURL(this.configURL);
		this.eop.setBase(document.URL);
		this.eop.setLocaleCode(this.eop.getLocaleFromBrowser());
		this.eop.setOnEditorLoaded(this.clientID + '_eopObject.loadData');
		if (this.styleSheetURL != '')
				this.eop.setStyleSheetURL(this.styleSheetURL);								
			
			if (this.bodyClassName != '') {
				var arr = new Array();
				arr['class'] = this.bodyClassName;
				this.eop.setParentElementByNameAttributes('body',arr);
			}
		this.eop.loadEditor();
	}
	
	function loadData() {		
		if(document.getElementById(this.clientID + "_HTMLText").value!=""){
			this.eop.setHTMLData(document.getElementById(this.clientID + "_HTMLText").value);		
			}
			
		 else {
			this.eop.setHTMLData("");
		}
		
		this.eop.pumpEvents();
	}
	
	function saveData() {
		document.getElementById(this.clientID + "_HTMLText").value = this.eop.getHTMLData();
	}
					
}

