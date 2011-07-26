   
function CollapsiblePanel(panelID,buttonID,expandImage,collapseImage) 
{	
	this.panelID = panelID;
	this.buttonID = buttonID;
	this.expandImage =  expandImage;
	this.collapseImage =  collapseImage;
			
    this.ShowHideGridView = function () 
    {
        if ( document.getElementById(this.panelID)==null ) return;
        if ( document.getElementById(this.panelID).style.display!='none' ) 
        {
	        document.getElementById(this.panelID).style.display='none';
	        document.getElementById(this.buttonID).src= this.expandImage;
	        
	        if ( document.forms[0].elements[this.panelID+'_State']!=null )
	            document.forms[0].elements[this.panelID+'_State'].value='Collapsed';
        } else 
        {
	        document.getElementById(this.panelID).style.display='';
	        document.getElementById(this.buttonID).src= this.collapseImage;	
	        
	        if ( document.forms[0].elements[this.panelID+'_State']!=null )    
                document.forms[0].elements[this.panelID+'_State'].value='Expanded';	    	    
	    }
    }
}


