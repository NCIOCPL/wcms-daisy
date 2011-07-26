
function ItemMenu() {
	this._openMenu = null; //This is a collection of open menus for different groups				
	
	if (typeof(_ItemMenu_prototype_called) == 'undefined')
	{
		_ItemMenu_prototype_called = true;					
		
		ItemMenu.prototype.showMenu = showMenu;
		ItemMenu.prototype.hideMenu = hideMenu;
		ItemMenu.prototype.startCountdownToHide = startCountdownToHide;
		ItemMenu.prototype.checkOpenHover = checkOpenHover;
	}
					
	function showMenu(menuParent) {
		//Close the current open menu
		if (this._openMenu != null)
		{
			if (this._openMenu.hasOwnProperty("timeoutHandle") && this._openMenu.timeoutHandle != null)
			{		
				clearTimeout(this._openMenu.timeoutHandle);
				this._openMenu.timeoutHandle = null;
			}
			this.hideMenu();
		}

		menuParent.className = 'show';
		
		if (this._openMenu == null)
			this._openMenu = {};
		
		this._openMenu.timeoutHandle = null;	
		this._openMenu.menuInfo = menuParent;			
	}

	function hideMenu() {
		if (this._openMenu != null && 
			this._openMenu.hasOwnProperty("menuInfo") && 
			this._openMenu.menuInfo != null) 
		{
			this._openMenu.menuInfo.className = '';
			this._openMenu.menuInfo = null;
			this._openMenu = null;
		}
	}

	function startCountdownToHide(e, menuParent, menuId)
	{
		e = (e) ? e : ((window.event) ? window.event : "")
		if (e) 
		{
			//Loop though parents of onmouseout element
			//and see if the parent is the menuParent, if it is,
			//do nothing.

			var tg = (window.event) ? e.srcElement : e.target;
		
			//If the target of the event == the parentObj, then return
			if (tg == menuParent) return;
		
			var reltg = (e.relatedTarget) ? e.relatedTarget : e.toElement;
		
			try 
			{
				while (reltg != menuParent && reltg.nodeName != 'BODY') 
				{
					reltg = reltg.parentNode;
				}
			
				if (reltg == menuParent) return;
			} catch(e) { }					

			if (this._openMenu != null) 
			{
				//There is a menu open, and we are not in it
				//soo start the timeout
				var self = this;
				this._openMenu.timeoutHandle = setTimeout(function() { self.hideMenu();}, 500); 
			}
			else
			{
				menuParent.className = '';
			}
		}					
	}
	
	function checkOpenHover(menuParent) 
	{
		if (this._openMenu == null || 
			(this._openMenu.hasOwnProperty("menuInfo") && this._openMenu.menuInfo == null ) ||
			!this._openMenu.hasOwnProperty("menuInfo")) 
		{
			//There is not an open menu for this group, so hover
			menuParent.className='hover';
		} 
		else 
		{
			//There is an open menu, so we need to see if it is ours						
			if (this._openMenu != null &&
				this._openMenu.hasOwnProperty("menuInfo") &&
				this._openMenu.menuInfo.id == menuParent.id) 
			{
				//Clear Timeout
				if (this._openMenu.hasOwnProperty("timeoutHandle") &&
					this._openMenu.timeoutHandle != null) 
				{
					clearTimeout(this._openMenu.timeoutHandle);
					this._openMenu.timeoutHandle = null;
				}
			}
		}
	}
}

