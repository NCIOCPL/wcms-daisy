using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web.UI;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("NCILibrary.Web.UI.WebControls")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("OTSA")]
[assembly: AssemblyProduct("NCILibrary.Web.UI.WebControls")]
[assembly: AssemblyCopyright("Copyright © OTSA 2007")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

/**************** A spacer for controls to use ************/
[assembly: WebResourceAttribute("NCI.Web.UI.WebControls.Resources.spacer.gif", "image/gif")]

/**************** Templated Tree View ********************/
[assembly: WebResource("NCI.Web.UI.WebControls.Resources.TemplatedTreeView.js", "text/javascript")]

/**************** DHtml Collapsible Panel ********************/
[assembly: WebResource("NCI.Web.UI.WebControls.Resources.DHtmlCollapsiblePanel.js", "text/javascript")]

/**************** DHtml Modal Popup Control ************************/
[assembly: WebResourceAttribute("NCI.Web.UI.WebControls.Resources.DHtmlModalPopup.js", "text/javascript")]

/**************** Event View Control Resources *********************************/
[assembly: WebResourceAttribute("NCI.Web.UI.WebControls.Resources.EventLogViewer.error.gif", "image/gif")]
[assembly: WebResourceAttribute("NCI.Web.UI.WebControls.Resources.EventLogViewer.warning.gif", "image/gif")]
[assembly: WebResourceAttribute("NCI.Web.UI.WebControls.Resources.EventLogViewer.information.gif", "image/gif")]
[assembly: WebResourceAttribute("NCI.Web.UI.WebControls.Resources.EventLogViewer.help.gif", "image/gif")]

/**************** EditOnPro HTML editor Control Resources *********************************/
[assembly: WebResourceAttribute("NCI.Web.UI.WebControls.Resources.EditOnPro.NCILibraryEOP.js", "text/javascript")]



/**************** CollapsiblePanel ******************/
[assembly: WebResourceAttribute("NCI.Web.UI.WebControls.Resources.CollapsiblePanel.bkg-menu-bar.gif", "image/jpg")]
[assembly: WebResourceAttribute("NCI.Web.UI.WebControls.Resources.CollapsiblePanel.menu-btn-left.gif", "image/jpg")]
[assembly: WebResourceAttribute("NCI.Web.UI.WebControls.Resources.CollapsiblePanel.menu-btn-right.gif", "image/gif")]
[assembly: WebResourceAttribute("NCI.Web.UI.WebControls.Resources.CollapsiblePanel.icon-collapse.jpg", "image/jpg")]
[assembly: WebResourceAttribute("NCI.Web.UI.WebControls.Resources.CollapsiblePanel.icon-expand.jpg", "image/jpg")]
[assembly: WebResourceAttribute("NCI.Web.UI.WebControls.Resources.CollapsiblePanel.CollapsiblePanel.js", "text/javascript")]
[assembly: WebResourceAttribute("NCI.Web.UI.WebControls.Resources.CollapsiblePanel.CollapsiblePanel.css", "text/css", PerformSubstitution = true)]


[assembly: WebResourceAttribute("NCI.Web.UI.WebControls.Resources.CustomSortingGridView.CustomSortingGridView.css", "text/css", PerformSubstitution = true)]

/*********************** CSS Browser Selector (Adds styles to Html element for browser) *********************************/
[assembly: WebResourceAttribute("NCI.Web.UI.WebControls.Resources.CssBrowserSelector.js", "text/javascript")]

/********** Files for the context menu ****************/
[assembly: WebResource("NCI.Web.UI.WebControls.Menus.Resources.ContextMenu.ContextMenu.js", "text/javascript")]
[assembly: WebResource("NCI.Web.UI.WebControls.Menus.Resources.ContextMenu.CTXMenuBase.css", "text/css", PerformSubstitution = true)]

// Silver Style
[assembly: WebResource("NCI.Web.UI.WebControls.Menus.Resources.ContextMenu.Silver.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("NCI.Web.UI.WebControls.Menus.Resources.ContextMenu.Silver-DownArrow-off.gif", "image/gif")]
[assembly: WebResource("NCI.Web.UI.WebControls.Menus.Resources.ContextMenu.Silver-DownArrow-on.gif", "image/gif")]
[assembly: WebResource("NCI.Web.UI.WebControls.Menus.Resources.ContextMenu.Silver-menu-bckgrnd.gif", "image/gif")]

//Blue Style
[assembly: WebResource("NCI.Web.UI.WebControls.Menus.Resources.ContextMenu.Blue.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("NCI.Web.UI.WebControls.Menus.Resources.ContextMenu.Blue-menu-bckgrnd.gif", "image/gif")]
[assembly: WebResource("NCI.Web.UI.WebControls.Menus.Resources.ContextMenu.Blue-DownArrow-off.gif", "image/gif")]
[assembly: WebResource("NCI.Web.UI.WebControls.Menus.Resources.ContextMenu.Blue-DownArrow-on.gif", "image/gif")]
/********** End context menu files ********************/

/********** Files for the menu bar ********************/
[assembly: WebResource("NCI.Web.UI.WebControls.Menus.Resources.MenuBar.MBMenuBase.css", "text/css", PerformSubstitution = true)]

//Normal Style
[assembly: WebResource("NCI.Web.UI.WebControls.Menus.Resources.MenuBar.Silver.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("NCI.Web.UI.WebControls.Menus.Resources.MenuBar.Silver-btn-left.gif", "image/gif")]
[assembly: WebResource("NCI.Web.UI.WebControls.Menus.Resources.MenuBar.Silver-btn-right.gif", "image/gif")]
[assembly: WebResource("NCI.Web.UI.WebControls.Menus.Resources.MenuBar.Silver-bkg-bar.gif", "image/gif")]
/******************************************************/

/*********************** JS Libraries *************************/
[assembly: WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.HtmlBuilder.js", "text/javascript")]
[assembly: WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Prototype.v1_5_1.prototype.js", "text/javascript")]
[assembly: WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Prototype.v1_6_0.prototype.js", "text/javascript")]
[assembly: WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_7_1.scriptaculous.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_7_1.builder.js", "text/javascript")]
[assembly: WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_7_1.controls.js", "text/javascript")]
[assembly: WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_7_1.dragdrop.js", "text/javascript")]
[assembly: WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_7_1.effects.js", "text/javascript")]
[assembly: WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_7_1.slider.js", "text/javascript")]
[assembly: WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_7_1.sound.js", "text/javascript")]

[assembly: WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_8_0.scriptaculous.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_8_0.builder.js", "text/javascript")]
[assembly: WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_8_0.controls.js", "text/javascript")]
[assembly: WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_8_0.dragdrop.js", "text/javascript")]
[assembly: WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_8_0.effects.js", "text/javascript")]
[assembly: WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_8_0.slider.js", "text/javascript")]
[assembly: WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_8_0.sound.js", "text/javascript")]
[assembly: WebResource("NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v1_8_0.unittest.js", "text/javascript")]


//Modal Control
[assembly: WebResource("NCI.Web.UI.WebControls.Resources.ModalControl.ModalControl.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("NCI.Web.UI.WebControls.Resources.ModalControl.ModalControl.js", "text/javascript")]

[assembly: WebResource("NCI.Web.UI.WebControls.Resources.ModalControl.bkg-menu-bar.gif", "image/gif")]
[assembly: WebResource("NCI.Web.UI.WebControls.Resources.ModalControl.menu-btn-left.gif", "image/gif")]
[assembly: WebResource("NCI.Web.UI.WebControls.Resources.ModalControl.menu-btn-right.gif", "image/gif")]
/**************************************************************/


/********************* Two List Select ************************/
[assembly: WebResource("NCI.Web.UI.WebControls.FormControls.Resources.TwoListSelect.js", "text/javascript")]
/**************************************************************/

/* bof WebResources for phone control*/
[assembly: WebResource("NCI.Web.UI.WebControls.FormControls.Resources.PhoneNumberValidator.js", "text/javascript")]
/* eof WebResources for phone control*/

/********************* Two List Select ************************/
[assembly: WebResource("NCI.Web.UI.WebControls.FormControls.Resources.TextAreaMaxLengthValidator.js", "text/javascript")]
/**************************************************************/

/********************* Autocomplete Box ***********************/
[assembly: WebResource("NCI.Web.UI.WebControls.FormControls.Resources.AutoComplete.js", "text/javascript")]
[assembly: WebResource("NCI.Web.UI.WebControls.FormControls.Resources.AutoComplete.css", "text/css")]
/**************************************************************/

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("66246e9f-b2d2-4804-a735-cb9e496061ec")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
