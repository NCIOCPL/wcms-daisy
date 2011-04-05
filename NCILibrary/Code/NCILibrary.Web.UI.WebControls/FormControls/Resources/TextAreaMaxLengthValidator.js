
/**************************************************************************************************
* Name		: TextAreaMaxLengthValidator.js
* Purpose	: has client side functions to validate the TextArea's MaxLength
                Modified from tmobile.com Effie.Nadiv@comverse.com
* Author	: Jay 
* Date		: 02/11/2008
* Changes	:
**************************************************************************************************/

TextMaxLenValidator = function(){

    var MAXIMUM_MESSAGE_LENGTH_REACHED = "Maximum message length reached. " +
        "Your message has been truncated at %s characters";
		
    function doCountChars(e, ctrl, target){
		if (ctrl)
		{
			var maxSmsMessageLength = ctrl.attributes["maxLength"].value;
    	    var total = 0,
            	toLength = 0;
        	e = e || event;

	        if (e){
	            if (ctrl.value){
	                total +=  ctrl.value.length;
	            }
	            if (total <= maxSmsMessageLength){
					document.getElementById(target).innerHTML = createInput(target+"count1", maxSmsMessageLength - total);
	            }else{
	                var input = e.target || e.srcElement;
	                var oldHandler = ctrl.onpropertychange;
	                ctrl.onpropertychange = null;
	                input.value = input.value.substring(0, input.value.length + (maxSmsMessageLength - total));
	                ctrl.onpropertychange = oldHandler;
	                alert(MAXIMUM_MESSAGE_LENGTH_REACHED.replace(/%s/, maxSmsMessageLength));
	            }
	        }
	    }
	}
	
	function createInput(id,value) {
  		return "<input type='text' size='2' disabled='disabled' id='"+ id +"' value='"+ value +"'><br>";
	}

    return {

		countChars: function(e, ctrl, target){
            doCountChars(e, ctrl, target);
        },

        setMaxLength: function(value, target){
            var f = document.getElementById(target);

            maxSmsMessageLength = value;
            if (f){
				f.innerHTML += createInput(target+"count1", maxSmsMessageLength);
            }
        }
    }
}();



/*
function textAreaMaxLengthDoKeypress(control){
    maxLength = control.attributes["maxLength"].value;
    value = control.value;
     if(maxLength && value.length > maxLength-1){
        control.value = control.value.substring(0,control.attributes["maxLength"].value);
        alert('Maximum messge length reached. Your message has been truncated at ' +maxLength + ' characters');
          //maxLength = parseInt(maxLength);
     }
}
// Cancel default behavior

function textAreaMaxLengthDoBeforePaste(control){
    maxLength = control.attributes["maxLength"].value;
     if(maxLength)
     {
          event.returnValue = false;
     }
}
// Cancel default behavior and create a new paste routine

function textAreaMaxLengthDoPaste(control){
    maxLength = control.attributes["maxLength"].value;
    value = control.value;
     if(maxLength){
          event.returnValue = false;
          maxLength = parseInt(maxLength);
          var oTR = control.document.selection.createRange();
          var iInsertLength = maxLength - value.length + oTR.text.length;
          var sData = window.clipboardData.getData("Text").substr(0,iInsertLength);
          oTR.text = sData;    
          alert('Maximum messge length reached. Your message has been truncated at ' +maxLength + ' characters');
      
     }
}
function ValidateTextAreaMaxLength(source, args)
{     
    var controlID = source.controltovalidate;
    var control = document.getElementById(controlID);
    var maxLength = control.attributes["maxLength"].value;
    var value = args.Value;
    
    if (ValidatorTrim(value).lenght ==0) return true;
    
    if(maxLength){    
        event.returnValue = false;
        maxLength = parseInt(maxLength);
        
        args.IsValid = (value.Length <=maxLength);
        
        if (!args.IsValid )
            control.Value = value.substr(0,maxLength -1);
    }
}
*/