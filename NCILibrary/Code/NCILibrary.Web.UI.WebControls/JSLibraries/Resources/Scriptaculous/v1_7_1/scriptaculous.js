// script.aculo.us scriptaculous.js v1.7.1_beta3, Fri May 25 17:19:41 +0200 2007

// Copyright (c) 2005-2007 Thomas Fuchs (http://script.aculo.us, http://mir.aculo.us)
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// For details, see the script.aculo.us web site: http://script.aculo.us/

//Modified to use .Net 2.0 embedded resources
//7/23/07 --BP

var Scriptaculous = {
  Version: '1.7.1_beta3',
  //Defining the web resources as a hashtable
  embeddedJSFiles: {
      builder: '<%= WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_7_1.builder.js") %>',
      controls: '<%= WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_7_1.controls.js") %>',
      dragdrop: '<%= WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_7_1.dragdrop.js") %>',      
      effects: '<%= WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_7_1.effects.js") %>',
      slider: '<%= WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_7_1.slider.js") %>',
      sound: '<%= WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_7_1.sound.js") %>'  },
  require: function(libraryName) {
    // inserting via DOM fails in Safari 2.0, so brute force approach    
    document.write('<script type="text/javascript" src="'+ Scriptaculous.embeddedJSFiles[libraryName] +'"></script>');
  },
  REQUIRED_PROTOTYPE: '1.5.1',
  load: function() {
    function convertVersionString(versionString){
      var r = versionString.split('.');
      return parseInt(r[0])*100000 + parseInt(r[1])*1000 + parseInt(r[2]);
    }
 
    if((typeof Prototype=='undefined') || 
       (typeof Element == 'undefined') || 
       (typeof Element.Methods=='undefined') ||
       (convertVersionString(Prototype.Version) < 
        convertVersionString(Scriptaculous.REQUIRED_PROTOTYPE)))
       throw("script.aculo.us requires the Prototype JavaScript framework >= " +
        Scriptaculous.REQUIRED_PROTOTYPE);
    
    //The old code allowed the different required scripts to be loaded through the
    //command line. Since it is a web resource this will not work correctly anymore,
    //so we are going to load all.
    Scriptaculous.require('builder');
    Scriptaculous.require('effects');
    Scriptaculous.require('dragdrop');
    Scriptaculous.require('controls');
    Scriptaculous.require('slider');
    Scriptaculous.require('sound');    
  }
}

Scriptaculous.load();