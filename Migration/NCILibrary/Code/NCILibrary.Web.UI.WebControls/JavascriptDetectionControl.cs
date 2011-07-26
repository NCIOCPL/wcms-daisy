using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Text;


namespace NCI.Web.UI.WebControls
{
	/// <summary>
	/// The JavascriptTest control is designed to provide a quick and
	/// reliable way of determining if the client browser supports
	/// javascript and if the javascript is enabled (on) in the client
	/// browser.
    /// Code borrowed from http://www.15seconds.com/issue/030303.htm
    /// Creating a Server Control for JavaScript Testing 
    /// By George Masselli
	/// </summary>
    [DefaultProperty("Enabled"), ToolboxData("<{0}:JavascriptDetection runat=server></{0}:JavascriptDetection>")]
	public class JavascriptDetectionControl : Control, IPostBackDataHandler
	{
		/// <summary>
		/// Event which is raised when state changes
		/// </summary>
		public event EventHandler EnabledChanged;

		#region Private Helper Methods

		/// <summary>
		/// A recursive function that unwinds the control tree to find
		/// the first HtmlForm control's client ID value.
		/// </summary>
		/// <remarks>
		/// This method is hard-coded to return a HtmlForm. It also 
		/// assumes that it is getting a top level control, such as the 
		/// Page control, and that this control contains the HtmlForm.
		/// </remarks>
		/// <param name="parentPage">Control</param>
		/// <returns>String</returns>
		private string GetFormName(Control parentControl)
		{
			// Init the name of the control
			string Name = "";

			// Loop through the control collection of the parentControl
			foreach(Control childControl in parentControl.Controls)
			{
				// Check if the type of control is a HtmlForm
				if (childControl.GetType().ToString() == "System.Web.UI.HtmlControls.HtmlForm")
				{
					// Set the Name to the ClientID
					Name = childControl.ClientID;
					
					// Exit from the loop
					break;
				}
				else
				{
					// Check for any child controls of the child
					if (childControl.HasControls())
					{
						// Make a recursive call to loop through the child's children
						Name = GetFormName(childControl);
					}
				}
			}

			// Return the name of the HtmlForm
			return Name;
		}


		#endregion Private Helper Methods

		#region Public Attributes

		/// <summary>
		/// Public attribute that returns the name of the hidden
		/// html element that is updated by javascript to test
		/// if javascript is enabled.
		/// </summary>
		/// <remarks>
		/// The ClientID is the ClientID of this control (JavascriptTest).
		/// </remarks>
		protected string HelperID
		{
			get
			{
				return "__" + ClientID + "_State";
			}
		}

		
		/// <summary>
		/// Public attribute indicating if javascript is enabled
		/// on the clients' browsers.
		/// </summary>
		public bool Enabled
		{
			get
			{
				// Get a base objec to hold the viewstate value
				object obj = ViewState[ClientID + "_Enabled"];
				
				// Check that obj is not null
				if (obj == null)
				{	// object is null
					return false;
				}
				else
				{	// return the converted value
					return (bool)obj;
				}
			}

			set
			{	// Save the value in viewstate
				ViewState[ClientID + "_Enabled"] = value;
			}
		}

		
		#endregion Public Attributes
		
		#region Control Events

		/// <summary>
		/// Initialize settings needed during the lifetime of the 
		/// incoming web request.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			if (Page != null)
			{
				Page.RegisterRequiresPostBack(this);
			}
		}

		
		/// <summary>
		/// Perform any updates before the output is rendered.
		/// </summary>
		/// <param name="e">EventArgs</param>
		protected override void OnPreRender(EventArgs e)
		{
			// Call the base classes' OnPreRender event
			base.OnPreRender(e);
			
			// verify that the page object is valid
			if (Page != null)
			{
				// Register the hidden form element
				Page.RegisterHiddenField(HelperID, Enabled.ToString());
				
				// Verify that the startup script isn't already registered (postbacks)
				if(!Page.IsStartupScriptRegistered("JavascriptTest_Startup"))
				{
					// Form the script to be registered at client side.
					StringBuilder sb = new StringBuilder();
					string sFormName = GetFormName(this.Page);

					sb.Append("<script lang='javascript'>");
					sb.Append("if (document." + sFormName + "." + HelperID + ".value == 'False')");
					sb.Append("{");
					sb.Append("document." + sFormName + "." + HelperID + ".value = 'True';");
					sb.Append("document." + sFormName + ".submit();");
					sb.Append("}");
					sb.Append("</script>");
					// Register the startup script
					Page.RegisterStartupScript("JavascriptTest_Startup", sb.ToString());
				}
			}
		}

		
		/// <summary>
		/// Process incoming form data and update properties accordingly,
		/// </summary>
		/// <param name="postDataKey">String</param>
		/// <param name="postCollection">NameValueCollection</param>
		/// <returns></returns>
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			// Pull out the hidden form value from the collection of values with the name of the control
			string value = postCollection[HelperID];
			
			// Verify that the value isn't null
			if (value != null)
			{
				// Grab the new value and compare it to 'true'
				bool newValue = (String.Compare(value, "true", true,System.Globalization.CultureInfo.CreateSpecificCulture("en-US")) == 0);
				// Set the old value to the enabled property value
				bool oldValue = Enabled;

				Enabled = newValue;

				// Raise the change event if there was a change
				return (newValue != oldValue);
			}

			// value didn't change
			return false;
		}

		
		/// <summary>
		/// Raise change events in response to state changes between
		/// the current and previous postbacks.
		/// </summary>
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{

			if (EnabledChanged != null)
			{	// There was a change,  so raise any events.
				EnabledChanged(this, EventArgs.Empty);
			}
		}
		
		#endregion "Control Events"

	}
}
