/*
Autosuggest allows an input tag to retrieve and show a list of 
possible values.
*/
var AutoComplete = new Object();

Object.extend(AutoComplete,
{
    // Variable that tracks all the autocomplete textboxes on the page
    elemArr: new Array(),

    // Register the textbox
    registerElement: function(elem) {
        if (AutoComplete.elemArr.length == 0) {
            window.onresize = AutoComplete.onBrowserResize;
        }

        AutoComplete.elemArr.push(elem);
    },

    // This function repositions the listbox when the browser is resized
    onBrowserResize: function() {
        for (var i = 0; i < AutoComplete.elemArr.length; i++) {
            if (AutoComplete.elemArr[i].showing) {
                AutoComplete.elemArr[i].reposition();
            }
        }
    },

    // This functions adds the Autocomplete behavior to the textbox
    extendTextBox: function(autoCompleteElementID, options) {
        // Get the element we want to work on
        var elem = $(autoCompleteElementID);

        Object.extend(elem, {
            onKeyPress: function(e) { // {{{
                if (!e) e = window.event;
                var key = e.keyCode || e.which;

                switch (key) {
                    case Event.KEY_RETURN:
                        this.setHighlightedValue();
                        Event.stop(e);
                        break;
                    //case Event.KEY_TAB:     
                    //    this.clearSuggestions();     
                    //    this.focus();     
                    //if (this.options.nextElementId.length > 0) {     
                    //    Event.stop(e);     
                    //    var elem = $(this.options.nextElementId);     
                    //    elem.focus();     
                    //}     
                    //    break;     
                    case Event.KEY_ESC:
                        this.clearSuggestions();
                        this.value = this.sInp;
                        this.iHigh = -1;
                        break;
                    default:
                        for (var i = 0; i < this.value.length && this.value.charAt(i) == ' '; i++);
                        if (i > 0) {
                            this.value = this.value.substring(i);
                            Event.stop(e);
                            return false;
                        }
                        //var index = (" :+\\?/<>&*%").indexOf(String.fromCharCode(key));
                        //if ((this.value.length == 0 && index == 0) || index > 0) {
                        //    Event.stop(e);
                        //    return false;
                        //}
                }
                return true;
            }, //}}}

            onKeyDown: function(e) {
                if (!e) e = window.event;
                var key = e.keyCode || e.which;

                if (key == Event.KEY_TAB) {
                    this.clearSuggestions();
                }

                return true;
            },

            onKeyUp: function(e) { // {{{
                if (!e) e = window.event;

                var key = e.keyCode || e.which;

                if (key == 37 || key == 39)
                    return false;

                if (key == Event.KEY_UP || key == Event.KEY_DOWN) {
                    this.changeHighlight(e, key);
                    Event.stop(e);
                }
                else {
                    // Strip any spaces
                    for (var i = 0; i < this.value.length && this.value.charAt(i) == ' '; i++);
                    if (i > 0) {
                        this.value = this.value.substring(i);
                    }
                    this.getSuggestions(this.value, false);
                }

                return true;
            }, //}}}

            getSuggestions: function(val, override) { // {{{
                // input the same? do nothing
                if (!override && val.toLowerCase() == this.sInp.toLowerCase()) {
                    return false;
                }

                this.sInp = val;
                //	this.setAttribute("sInp", val);

                // input length is less than the min required to trigger a request
                // do nothing
                if (val.length < this.options.minchars) {
                    this.closeSelected = false;
                    this.aSug = new Array();
                    this.nInpC = val.length;
                    this.clearSuggestions();
                    return false;
                }

                // Let's see if we should even be here
                if (this.closeSelected)
                    return false;

                // Here we will detect if there is a comma and the splitted value has a value to check
                // comma stars a new search and val is converted to the new value after the comma
                var ol = this.nInpC; // old length
                this.nInpC = val.length ? val.length : 0;

                // if caching enabled, and we didn't receive the maxentries value
                // and user is typing (ie. length of input is increasing)
                // filter results out of suggestions from last request
                var look = true;
                var l = this.aSug.length;
                if (this.options.cache && (this.nInpC > ol) && l) {
                    var arr = new Array();
                    var offset = 0;
                    for (var i = 0; i < l; i++) {
                        offset = this.aSug[i].item.toLowerCase().indexOf(val.toLowerCase());
                        if ((this.options.contains && offset != -1) || (!this.options.contains && offset == 0)) {
                            arr.push(this.aSug[i]);
                        }
                    }

                    // see if we added anything to our new array
                    if (arr.length > 0) {
                        look = false;
                        this.aSug = arr;
                        this.createList(this.aSug);
                    }
                }
                // See if we need to call the web
                // do new request
                var p = this;
                clearTimeout(this.ajID); // ajax id timer
                this.ajID = setTimeout(function() { p.doAjaxRequest(p.sInp) }, this.options.delay);
                //}

                document.helper = this;
                return false;
            }, // }}}

            getLastInput: function(str) { // {{{
                var ret = str;
                if (undefined != this.options.valueSep) {
                    var idx = ret.lastIndexOf(this.options.valueSep);
                    ret = idx == -1 ? ret : ret.substring(idx + 1, ret.length);
                }

                return ret;
            }, // }}}

            doAjaxRequest: function(input) { // {{{
                // we have to check here if there is a new splitted value (, or ;)
                // always check against the last part of the comma and then check
                // saved input is still the value of the field
                if (input != this.value)
                    return false;

                // Gmail like : get only the last user's input
                //this.sInp = this.getLastInput(this.sInp);
                //	this.setAttribute("sInp", this.sInp);

                // Some special characters won't work with AJAX so let's make them spaces
                //var newInp = this.sInp; ////.replace(/[\:\+\\\?\/\<\>\&\*\%]/g, " ");

                // Replace [ to be a literal for search
                var newInp = this.sInp.replace(/[\[]/g, "[[]");

                // create ajax request
                // do we need to call a function to recreate the url?
                var url;
                if (typeof this.options.script == 'function')
                    url = this.options.script(encodeURIComponent(newInp));
                else
                    url = this.options.script + encodeURIComponent(newInp) + "&maxRows=" + this.options.maxentries + "&contains=" + this.options.contains;

                if (!url) return false;

                var p = this;
                var m = this.options.meth;  // get or post?
                if (this.options.useNotifier) {
                    this.removeClassName('ac_field');
                    this.addClassName('ac_field_busy');
                };

                // Check if this is a gadget or regular XMLRequest
                if (this.options.gadget) {
                    _IG_FetchXmlContent(url, function(response) {
                        if (p.options.useNotifier) {
                            p.removeClassName('ac_field_busy');
                            p.addClassName('ac_field');
                        };
                        p.setSuggestions(response, p.value);
                    });
                }
                else {
                    var options =
                    {
                        method: m,
                        onSuccess: function(req) { // {{{
                            if (p.options.useNotifier) {
                                p.removeClassName('ac_field_busy');
                                p.addClassName('ac_field');
                            };
                            p.setSuggestions(req, input);
                        }, // }}}

                        onFailure: (typeof p.options.onAjaxError == 'function') ? function(status) {
                            // Added 12/15/200
                            if ($(this.acID))
                                $(this.acID).remove();

                            if (p.options.useNotifier) {
                                p.removeClassName('ac_field_busy');
                                p.addClassName('ac_field');
                            }
                            p.options.onAjaxError(status)
                        } : // }}}

                        function(status) { // {{{
                            if (p.options.useNotifier) {
                                p.removeClassName('ac_field_busy');
                                p.addClassName('ac_field');
                            }
                            alert("AJAX error: " + status.statusText);
                        } // }}}
                    }

                    // make new ajax request
                    new Ajax.Request(url, options);
                }
            }, // }}}

            setSuggestions: function(req, input) { // {{{
                // if field input no longer matches what was passed to the request
                // don't show the suggestions
                // here we need to check against the splitted values if any (, or ;)
                if (input != this.value)
                    return false;

                // When we do the search convert to lower case and
                // replace special characters to spaces
                var newInp = this.sInp.toLowerCase(); ////.replace(/[\:\+\\\?\/\<\>\&\*\%]/g, " ");

                this.aSug = new Array();
                if (this.options.json) { // response in json format?
                    if (req.responseText.length == 0) {
                        this.nLastFailedChar = this.nInpC;
                        this.clearSuggestions();
                        return false;
                    }

                    var jsondata = eval('(' + req.responseText + ')');

                    // Do some filtering
                    if (jsondata.length > 0) {
                        for (var i = 0; i < jsondata.length; i++) {
                            if (this.matchValues(newInp, jsondata[i].item.toLowerCase()))
                                this.aSug.push(jsondata[i]);
                        }
                    }
                } else { // response in xml format?
                    // See if we have anything
                    var results;
                    if (this.options.gadget)
                        results = req.childNodes;
                    else
                        var results = req.responseXML.childNodes;

                    // See what we have to do
                    if (results != null) {

                        // figure out which variable to use
                        var hasTextContent = (results[0].textContent != undefined) ? true : false;

                        // Now get our children
                        results = results[0].childNodes;

                        var stuff = new Array();
                        stuff.push(''); stuff.push(''); stuff.push('');

                        // iterate through the children
                        for (var i = 0; i < results.length; i++) {
                            if (results[i].hasChildNodes()) {
                                // Get our elements
                                if (hasTextContent) {
                                    stuff[0] = results[i].childNodes[0].textContent;
                                    stuff[1] = results[i].childNodes[1].textContent;
                                    stuff[2] = results[i].childNodes[2].textContent;
                                }
                                else {
                                    stuff[0] = results[i].childNodes[0].text;
                                    stuff[1] = results[i].childNodes[1].text;
                                    stuff[2] = results[i].childNodes[2].text;
                                }

                                // Check to see if we should add this bad boy
                                if (this.matchValues(newInp, stuff[2].toLowerCase())) {
                                    this.aSug.push({ 'id': stuff[0], 'info': stuff[1], 'item': stuff[2] });
                                }
                            }
                        }
                    }
                }

                //this.aSug = jsondata;
                if (this.aSug.length == 0) {
                    this.nLastFailedChar = this.nInpC;
                    this.clearSuggestions();
                    return false;
                }
                this.nLastFailedChar = 0;

                this.acID = 'ac_' + this.id;
                this.createList(this.aSug);
            }, // }}}

            // Check to see if the string the user entered matches the item
            matchValues: function(input, item) {
                var retVal = true;

                // check for matching values
                if (this.options.contains == true) {
                    if (item.indexOf(input) == -1) {
                        retVal = false;
                    }
                }
                else {
                    if (item.substr(0, this.nInpC) != input) {
                        retVal = false;
                    }
                }

                // return a new array
                return retVal;
            },

            createDOMElement: function(type, attr, cont, html) { // {{{
                var ne = document.createElement(type);

                if (!ne)
                    return 0;

                for (var a in attr)
                    ne[a] = attr[a];

                var t = typeof (cont);

                if (t == "string" && !html)
                    ne.appendChild(document.createTextNode(cont));
                else if (t == "string" && html)
                    ne.innerHTML = cont;
                else if (t == "object")
                    ne.appendChild(cont);

                return ne;
            }, // }}}

            createList: function(arr) { // {{{
                var ul = $('ac_ul');
                if (ul) {
                    this.addLineItem(ul, arr);
                }
                else {
                    // get rid of the old list if any  
                    if ($(this.acID)) $(this.acID).remove();

                    // if no results, and showNoResults is false, do nothing
                    if (arr.length == 0 && !this.options.shownoresults) return false;

                    // create holding div
                    var div = this.createDOMElement('div',
  	                {
  	                    id: this.acID,
  	                    className: this.options.className
  	                });

                    if (this.options.isIE) {
                        div.style.position = "absolute";
                        document.body.appendChild(div);
                    }
                    else {
                        div.style.position = "relative";
                        this.parentNode.appendChild(div);
                    }

                    Element.extend(div);


                    // we need to handle the onclick to make sure we set the focus back on the textbox
                    div.observe('click', this.ignoreEvents.bindAsEventListener(this));
                    div.observe('mousedown', this.ignoreEvents.bindAsEventListener(this));
                    div.observe('mouseover', this.keepListBox.bindAsEventListener(this, true));
                    div.observe('mouseout', this.keepListBox.bindAsEventListener(this, false));

                    // create div header
                    var header = null;
                    if (!this.options.boxOutline) {
                        var hcorner = this.createDOMElement('div', { className: 'ac_corner' });
                        var hbar = this.createDOMElement('div', { className: 'ac_bar' });
                        header = this.createDOMElement('div', { className: 'ac_header' });
                        header.appendChild(hcorner);
                        header.appendChild(hbar);
                        div.appendChild(header);
                    }

                    // create and populate ul
                    /**/
                    var ul = this.createDOMElement('ul', { id: 'ac_ul', className: 'ac_listbox' });
                    div.appendChild(ul); // add the newly created list to the div element
                    this.addLineItem(ul, arr);
                    /**/

                    /**/
                    // Add the close link - Added 12/23/2008
                    //var navTag = this.createDOMElement('div', {className:'ac_navtag'});
                    var closeTag = this.createDOMElement('a', { id: 'ac_close', className: 'ac_close', href: '#' }, this.options.closeText, true);
                    div.appendChild(closeTag);
                    Element.extend(closeTag);
                    closeTag.observe('click', this.closeListBox.bindAsEventListener(this));
                    /**/

                    // create div footer
                    var footer = null;
                    if (!this.options.boxOutline) {
                        var fcorner = this.createDOMElement('div', { className: 'ac_corner' });
                        var fbar = this.createDOMElement('div', { className: 'ac_bar' });
                        footer = this.createDOMElement('div', { className: 'ac_footer' });
                        footer.appendChild(fcorner);
                        footer.appendChild(fbar);
                        div.appendChild(footer);
                    }

                    // get position of target textfield
                    // position holding div below it
                    // set width of holding div to width of field 

                    this.reposition();
                    //var pos = this.cumulativeOffset();
                    //div.style.left = this.offsetLeft + "px"; //pos[0] + "px";
                    //div.style.top = 0;  //pos[1] + this.offsetHeight + "px";


                    // if setwidth set and the offsetwidth if less than the min width
                    // set the min width. If the offset width > maxwidth
                    // set the mx width.

                    var w =
                    (
                      this.options.setWidth && this.offsetWidth < this.options.minWidth
                    )
                    ? this.options.minWidth :
                    (
                      this.options.setWidth && this.offsetWidth > this.options.maxWidth
                    )
                    ? this.options.maxWidth : this.offsetWidth;

                    // Set the size of the listbox
                    div.style.width = (w + this.options.addWidth) + "px";

                }

                // put is just before first item
                this.iHigh = -1;
                this.topRow = 0;
                //	this.setAttribute("iHigh", this.iHigh);

                this.showing = true;

                /*
                if (this.options.gadget)
                _IG_AdjustIFrameHeight();
                */

            }, // }}}

            addLineItem: function(ul, arr) {
                var p = this; // pointer that we will need later on
                var nav = false;

                // Get rid of anything that is in the UL
                while (ul.childNodes.length) {
                    var aChild = ul.childNodes[0];
                    $(aChild).remove();
                }

                // no results?
                if (arr.length == 0 && this.options.shownoresults) {
                    var li = this.createDOMElement('li', { className: 'ac_warning' }, this.options.noresults);
                    ul.appendChild(li);
                } else {
                    // do out special thing here
                    var newInp = this.sInp; ////.replace(/[\:\+\\\?\/\<\>\&\*\%]/g, " ").toLowerCase();

                    // loop through arr of suggestions creating an LI element for each of them
                    for (var i = 0, l = arr.length; i < l; i++) {
                        // format output with the input enclosed in a EM elementFromPoint
                        // (as HTML not DOM)
                        var val = arr[i].item;
                        var st = val.toLowerCase().indexOf(newInp); // HERE WE CHECK AGAINST THE SPLITTED VALUE IF ANY***
                        var output = val.substring(0, st) + '<em>' + val.substring(st, st + newInp.length) + '</em>' + val.substring(st + newInp.length);
                        var span = this.createDOMElement('span', {}, output, true); // type of, properties, output, isHTML?

                        if (arr[i].info != '') // do we need to add extra info?
                        {
                            var br = this.createDOMElement('br', {});
                            span.appendChild(br);

                            var small = this.createDOMElement('small', {}, arr[i].info);
                            span.appendChild(small);
                        }
                        var a = this.createDOMElement('a', { href: '#' });

                        a.name = i;
                        a.appendChild(span); // add the object span into the link                        
                        Element.extend(a);

                        // Let's show the rows we want to see on the page and
                        // hide those rows we don't want to show at this moment
                        var li;
                        if (this.options.rowsPerPage == 0 || i < this.options.rowsPerPage) {
                            li = this.createDOMElement('li', {}, a); // add the link element to a li element
                        }
                        else {
                            li = this.createDOMElement('li', { className: 'ac_hide' }, a);
                            nav = true;
                        }
                        // finally add the newly created li element to the ul element
                        ul.appendChild(li);

                        a.textForSearch = val;

                        a.observe('click', this.setHighlightedValue.bindAsEventListener(this));
                        a.observe('mouseover', this.changeHighlight.bindAsEventListener(this, 'mouse', a.name));
                    }
                }

            },

            moveUp: function() {
                var key = Event.KEY_UP;
                var amount = this.rowPerPage
                //this.keepListBox(false);
                if (this.iHigh > 0) {
                    this.changeHighlight(key);
                }
            },

            moveDown: function() {
                var list = $("ac_ul");
                if (!list)
                    return false;

                var key = Event.KEY_DOWN;
                //this.keepListBox(false);
                if (this.iHigh < list.childNodes.length) {
                    this.changeHighlight(key);
                }
            },

            getListValues: function(list) {
                var count = list.childNodes.length;

                // Reset this value so we can find out where things are
                this.topRow = -1;

                for (var i = 0; i < count; i++) {
                    // find the topRow - may be equal to highlighted row
                    if (this.topRow == -1 && list.childNodes[i].className != 'ac_hide') {
                        this.topRow = i;
                    }

                    // See if we now which row was highlighed.
                    if (list.childNodes[i].className == 'ac_highlight') {
                        this.iHigh = i;
                        break;
                    }
                }

                // Check to see if we were set
                if (-1 == this.topRow)
                    this.topRow = 0;

            },

            changeHighlight: function(evt, key, itemNo) { // {{{
                var list = $("ac_ul");
                if (!list)
                    return false;

                // We need to do this because sometimes when we go out of focus
                // our counters are incorrect
                this.getListValues(list);

                // Find out what next row is. Really a check for boundaries
                var row;
                if (itemNo) {
                    row = itemNo;
                }
                else {
                    row = (key == Event.KEY_DOWN) ? this.iHigh + 1 : this.iHigh - 1;
                    row = ((row < list.childNodes.length) ? ((row < -1) ? -1 : row) : (list.childNodes.length));
                }

                // Figure out if we need to hide a row
                if (row >= 0 && row < list.childNodes.length) {
                    if (this.options.rowsPerPage > 0)
                        this.adjustDisplay(list, row);
                }
                else if (row > 0) { // this takes care 
                    row--;
                }

                // display text in textbox
                if (!itemNo && row >= 0) {
                    this.value = list.childNodes[row].firstChild["textForSearch"];
                }
                else if (!itemNo || Number(itemNo) == 'NaN') {
                    this.value = this.sInp;
                }

                this.setHighlight(list, row);
            }, // }}}

            setHighlight: function(list, n) { // {{{
                // Make sure we didn't loose our way  	
                if (this.topRow == -1)
                    this.getListValues();

                if (this.iHigh >= 0) this.clearHighlight(list);

                this.iHigh = Number(n);

                // Save value
                //	this.setAttribute("iHigh", this.iHigh);

                if (this.iHigh >= 0) {
                    list.childNodes[this.iHigh].className = 'ac_highlight';
                }
            }, // }}}

            clearHighlight: function(list) { // {{{
                if (this.iHigh >= 0) {
                    list.childNodes[this.iHigh].className = '';
                    this.iHigh = -1;
                }
            }, // }}}

            adjustDisplay: function(list, row) {
                // Flag to do something
                var doWork = false;

                // Find our upper bound
                var count = list.childNodes.length;
                //var top = (this.topRow > row) ? row : this.topRow;
                var rowToHide = this.topRow + this.options.rowsPerPage - 1;
                rowToHide = (rowToHide < count) ? rowToHide : count;

                // Do we need to do anything?
                if (row < this.topRow) {
                    this.topRow = row;
                    //rowToHide -= 1;
                    doWork = true;
                }
                else if (row > rowToHide) {
                    rowToHide = this.topRow;
                    this.topRow += 1;
                    doWork = true;
                }

                // Do we need to do anything?
                if (doWork) {
                    //		this.setAttribute("topRow", this.topRow);
                    list.childNodes[rowToHide].className = "ac_hide"; // We hide the row
                }
            },

            setHighlightedValue: function() { // {{{                
                if (this.iHigh > -1) {
                    // HERE WE NEED TO IMPLEMENT THE GMAIL LIKE SPLITTED VALUE
                    if (!this.aSug[this.iHigh]) return;

                    // Gmail like
                    if (undefined != this.options.valueSep) {
                        var str = this.getLastInput(this.value);
                        var idx = this.value.lastIndexOf(str);
                        str = this.aSug[this.iHigh].item + this.options.valueSep;
                        this.sInp = this.value = idx == -1 ? str : this.value.substring(0, idx) + str;
                    } else {
                        var str = this.getLastInput(this.value);
                        var idx = this.value.lastIndexOf(str);
                        str = this.aSug[this.iHigh].item;
                        this.sInp = this.value = idx == -1 ? str : this.value.substring(0, idx) + str;
                    }

                    // move cursor to end of input (safari)
                    this.focus();
                    if (this.selectionStart)
                        this.setSelectionRange(this.sInp.length, this.sInp.length);

                }

                this.clearSuggestions();

                // pass selected object to callback function, if exists
                if (typeof this.options.callback == 'function')
                    this.options.callback(this.sInp); // the object has the properties we want, it will depend of
            }, // }}}

            clearSuggestions: function() { // {{{
                if ($(this.acID)) {
                    $(this.acID).remove();

                    /*
                    if (this.options.gadget)
                    _IG_AdjustIFreameHeight();
                    */
                }

                this.showing = false;
            }, // }}}

            // These methods control external input
            changeLookup: function(evt, contains) {
                if (this.options.contains != contains) {
                    this.options.contains = contains;
                    this.getSuggestions(this.value, true);
                }
                //else {
                //    this.createList(this.aSug);
                //}
                this.focus();
                return false;
            },

            // Close the listbox unless someone told you not to
            closeAutoComplete: function(evt) {
                if (!this.insideListBox) {
                    this.clearSuggestions();
                }
                return false;
            },

            // Set flag to keep or get rid of list box when closeAutoComplete is called
            keepListBox: function(evt, setting) {
                this.insideListBox = setting;
                return false;
            },

            // Set the focux on the text box
            ignoreEvents: function(evt) {
                Event.stop(evt);
                this.focus();
                return false;
            },

            setTextboxFocus: function(evt) {
                //Event.stop(evt);
                var elem = $(this.acID);
                elem.className = this.options.className;
                this.focus();
                return false;
            },

            closeListBox: function(evt) {
                this.clearSuggestions();
                this.focus();
                this.closeSelected = true;
                return false;
            },

            // Determine if the listbox has data
            display: function(key) {
                if (this.showing == key)
                    return true;
                else
                    return false;
            },

            reposition: function() {
                var div = $(this.acID);
                if (div) {
                    if (this.options.isIE) {
                        var pos = this.cumulativeOffset();
                        div.style.left = pos[0] + "px"; //this.offsetLeft;  //
                        div.style.top = pos[1] + this.offsetHeight + 'px'; // 0
                    }
                    else {
                        div.style.left = this.offsetLeft + 'px';
                        div.style.top = 0;
                    }
                }

                return true;
            }
        });    // end of extendTexBox functions


        // Init variables
        elem.aSug = []; // suggestions array 
        elem.sInp = "";
        elem.nInpC = 0;
        elem.nLastFailedChar = 0;
        elem.iHigh = -1;
        elem.topRow = -1;
        elem.insideListBox = false;
        elem.showing = false;
        elem.closeSelected = false;

        // These are the default settings {{{
        elem.options = {
            valueSep: null,
            isIE: false,
            gadget: false,
            minchars: 1,
            json: true,
            meth: "get",
            varname: "criteria",
            className: "autocomplete",
            closeText: "close",
            timeout: 3000,
            delay: 0,
            shownoresults: false,
            noresults: "No results were found.",
            rowsPerPage: 0,
            cache: true,
            maxentries: 10,
            contains: false,
            onAjaxError: null,
            setWidth: false,
            minWidth: 200,
            maxWidth: 200,
            addWidth: 0,
            boxOutline: true,
            useNotifier: true
        };

        Object.extend(elem.options, options || {});

        // Not everyone wants to use the Notifier. Give them the option	
        if (elem.options.useNotifier) {
            elem.addClassName('ac_field');
        }

        // set keyup handler for field
        // and prevent AutoComplete from client
        try {
            elem.observe('keypress', elem.onKeyPress.bindAsEventListener(elem));
            elem.observe('keyup', elem.onKeyUp.bindAsEventListener(elem));
            elem.observe('keydown', elem.onKeyDown.bindAsEventListener(elem));
            elem.observe('blur', elem.closeAutoComplete.bindAsEventListener(elem));
        }
        catch (e) {
            alert(e);
        }

        // We need to set the listener for the browser resizing
        AutoComplete.registerElement(elem);
    }   // End of extendTextBox
});                                                                                                                                   // End of Autocomplete

// support for radio buttons

// This functions allows a user to set the autocomplete control
// and have it search either at the start of the text or within
// The text. Set contains to:
// false - to start search at beginning of text
// true  - to search anywhere within the text
function toggleSearchMode(evt, acID, contains) {
    var elem = $(acID);
    if (elem) {
        elem.changeLookup(evt, contains);
    }

    return false;
}

// This function allows a user to keep the listbox from
// closing. Used when you want to mouse over a control and
// set the insideListBox which determines if the list box
// should close when the textbox loses focus.
// Use this method with the mouseover and mouseout events
// keep(true) on mouseover
// keep(false) on mouseout
function keepListBox(evt, acID, keep) {
    var elem = $(acID);
    if (elem)
        elem.keepListBox(evt, keep);

    return false;
}

