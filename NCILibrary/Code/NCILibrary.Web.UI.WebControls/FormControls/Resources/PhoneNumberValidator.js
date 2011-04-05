/**************************************************************************************************
* Name		: PhoneNumberValidator.js
* Purpose	: has client side functions to validate the phone number
* Author	: SR
* Date		: 02/05/2007
* Changes	:
**************************************************************************************************/

//checks if the entered phone number is valid
function ValidatePhoneNumber(source, args)
{   
    //get the control and value to be validated   
    var controlID = source.controltovalidate;	
    var control = document.getElementById(controlID);

    var phone 
            = document.getElementById(controlID + '_AreaCode').value 
            + document.getElementById(controlID + '_Exchange').value 
            + document.getElementById(controlID + '_LocalNumber').value;
    
    //get the source element  
	var e = (e) ? e : ((window.event) ? window.event : "")
    var srcElmnt;  
	if (e.target) 
	    srcElmnt = e.target;  
	else if (e.srcElement) 
	    srcElmnt = e.srcElement;
	    
	if (srcElmnt && srcElmnt.nodeType == 3)// to fix safari issue
		srcElmnt = srcElmnt.parentNode;
	
    //do not validate yet if the user is entering the phone number
    if(srcElmnt && (srcElmnt.id == controlID + '_AreaCode' || srcElmnt.id == controlID + '_Exchange') )
    {
        args.IsValid = true;
    }    
    else if(source.isRequired)
    {
        if(source.isRequired.toLowerCase() =='true' && phone.length != source.maxLength)
        {
            args.IsValid = false;
        }
    }
    else 
    {
        var pattern=/^(\d{3})(\d{3})(\d{4})$/                    
        if(phone.search(pattern) == -1)
            args.IsValid = false;
        else
            args.IsValid = true;
    }    
}

//checks and jumps to next field
//it allows only numeric to be entered
function CheckAndJump(e, currelmnt, nextelmntid) 
{ 
    var key = e.keyCode || e.which;    
    if(key == 16)
    {
         ; //don't do anything if it is a shift+tab key         
    }    
    else if (!/^\d*$/.test(currelmnt.value)) 
    {
        currelmnt.value = currelmnt.value.replace(/[^\d]/g,'');
    }                    
    else if(currelmnt.value.length==currelmnt.maxLength) 
    {
        
        if(nextelmntid)
            document.getElementById(nextelmntid).focus();
    }
}