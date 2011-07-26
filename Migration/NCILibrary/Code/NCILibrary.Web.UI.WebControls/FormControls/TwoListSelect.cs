// TwoListSelect.cs
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using NCI.Util;
using NCI.Web.UI.WebControls;
using NCI.Web.UI.WebControls.JSLibraries;

namespace NCI.Web.UI.WebControls.FormControls
{
    [
    AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal),
    AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal),
    DefaultProperty("Email"),
    ToolboxData("<{0}:TwoListSelect runat=\"server\"> </{0TwoListSelector>"),
    ValidationProperty("HasSelections")
    ]
    public class TwoListSelect : ListControl, INamingContainer, IPostBackDataHandler
    {
        #region Internal Types

        /// <summary>
        /// Provides named values for determining whether an element should float to the 
        /// left or the right side of the control.
        /// </summary>
        private enum alignDirection
        {
            left = 0,
            right = 1
        }

        /// <summary>
        /// Named values for whether options have added or removed from the selection list.
        /// </summary>
        private enum selectAction
        {
            None = 0,   // Neither Select nor Remove.
            Select = 1,
            Remove = 2
        }

        #endregion

        #region Fields

        private const string _leftSelectName = "LeftSelect";
        private const string _rightSelectName = "RightSelect";
        private const string _addButtonName = "AddButton";
        private const string _removeButtonName = "RemoveButton";
        private const string _hiddenSelectListName = "Selections";

        #endregion

        #region Public Properties

        /// <summary>
        /// The number of rows to display in each of the control's list boxes.
        /// Both lists display with the same number of rows.
        /// </summary>
        [
        Bindable(true),
        Category("Layout"),
        DefaultValue(""),
        Description("The number of rows to display in the two list boxes."),
        Localizable(true),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public virtual int Rows
        {
            get
            {
                int height = (int)(ViewState["Rows"] ?? 10);
                return height;
            }
            set { ViewState["Rows"] = value; }
        }

        /// <summary>
        /// The width of the control's listboxes.  Both listboxes are set to the same width.
        /// If this value is not set, each listbox will occupy 40% of the control's width.
        /// </summary>
        [
        Bindable(true),
        Category("Layout"),
        Description("The width of the two listboxes.  Both listboxes use the same width."),
        Localizable(true),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public virtual Unit ListBoxWidth
        {
            get
            {
                Unit listWidth = (Unit)(ViewState["ListBoxWidth"] ?? Unit.Empty);
                if (listWidth == Unit.Empty && Width != Unit.Empty)
                {
                    listWidth = new Unit(Width.Value * 0.40, Width.Type);
                }
                return listWidth;
            }
            set { ViewState["ListBoxWidth"] = value; }
        }

        /// <summary>
        /// The width of the control's buttons.  Both buttons are set to the same width.
        /// If this value is not set, each button will occupy 15% of the control's width.
        /// </summary>
        [
        Bindable(true),
        Category("Layout"),
        Description("The width of the add and remove buttons.  Both buttons use the same width."),
        Localizable(true),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public virtual Unit ButtonWidth
        {
            get
            {
                Unit listWidth = (Unit)(ViewState["ButtonWidth"] ?? Unit.Empty);
                if (listWidth == Unit.Empty && Width != Unit.Empty)
                {
                    listWidth = new Unit(Width.Value * 0.15, Width.Type);
                }
                return listWidth;
            }
            set { ViewState["ButtonWidth"] = value; }
        }

        /// <summary>
        /// Label for the list of selected items.
        /// If both SelectedItemLabel and UnselectedItemLabel are not specified, the default
        /// behavior is to suppress the label row in its entirety.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("Label for the list of selected items."),
        Localizable(true),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public virtual string SelectedItemLabel
        {
            get { return (string)(ViewState["SelectedItemLabel"] ?? string.Empty); }
            set { ViewState["SelectedItemLabel"] = value; }
        }

        /// <summary>
        /// Label for the list of unselected items.
        /// If both SelectedItemLabel and UnselectedItemLabel are not specified, the default
        /// behavior is to suppress the label row in its entirety.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("Label for the list of unselected items."),
        Localizable(true),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public virtual string UnselectedItemLabel
        {
            get { return (string)(ViewState["UnselectedItemLabel"] ?? string.Empty); }
            set { ViewState["UnselectedItemLabel"] = value; }
        }

        /// <summary>
        /// URL for the Select button image.  If empty, a text button is displayed instead.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("URL for the Select button image."),
        Localizable(true),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public virtual string SelectButtonImageUrl
        {
            get { return (string)(ViewState["SelectButtonImageUrl"] ?? string.Empty); }
            set { ViewState["SelectButtonImageUrl"] = value; }
        }

        /// <summary>
        /// URL for the Unselect button image.  If empty, a text button is displayed instead.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("URL for the Unselect button image."),
        Localizable(true),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public virtual string UnselectButtonImageUrl
        {
            get { return (string)(ViewState["UnselectButtonImageUrl"] ?? string.Empty); }
            set { ViewState["UnselectButtonImageUrl"] = value; }
        }

        /// <summary>
        /// Text for the add button.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("Text for the add button."),
        Localizable(true),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public virtual string SelectButtonText
        {
            get { return (string)(ViewState["SelectButtonText"] ?? "Add"); }
            set { ViewState["SelectButtonText"] = value; }
        }

        /// <summary>
        /// Text for the remove button.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        Description("Text for the remove button."),
        Localizable(true),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public virtual string UnselectButtonText
        {
            get { return (string)(ViewState["UnselectButtonText"] ?? "Remove"); }
            set { ViewState["UnselectButtonText"] = value; }
        }

        /// <summary>
        /// True if one or more items are selected.
        /// </summary>
        [
        Bindable(true),
        Description("True if one or more items are selected."),
        ReadOnly(true)
        ]
        public virtual bool HasSelections
        {
            get
            {
                bool hasSelections = false;
                foreach (ListItem item in Items)
                {
                    if (item.Selected)
                    {
                        hasSelections = true;
                        break;
                    }
                }
                return hasSelections;
            }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Override the control's default outermost tag.
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        #endregion

        #region Methods supporting IPostBackDataHandler

        /// <summary>
        /// Implementation of IPostBackDataHandler.LoadPostData().
        /// This method examines the postback data and updates the control's state.
        /// </summary>
        /// <param name="postDataKey">Provides the dictionary key for the control instance.</param>
        /// <param name="postCollection">A dictionary collection of all incoming form values.</param>
        /// <returns>True if the control's state changes.</returns>
        /// <remarks>If this method returns true, the .NET framework will call RaisePostDataChangedEvent().
        /// 
        /// Because this is effectively a composite control with the potential for multiple fields
        /// being returned, the postDataKey parameter is ignored.</remarks>
        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            selectAction action = GetSelectAction(postCollection);
            bool StateHasChanged;

            if (action != selectAction.None)
            {
                StateHasChanged = ProcessSelectionAction(action, postCollection);
            }
            else
            {
                StateHasChanged = ProcessAsynchronousAction(postCollection);
            }

            return StateHasChanged;
        }

        /// <summary>
        /// Helper function to determine whether items are being selected or removed.
        /// </summary>
        /// <param name="data">A dictionary collection of all incoming form values.</param>
        /// <returns>A value specifying that items are being selected, removed or no change.</returns>
        private selectAction GetSelectAction(NameValueCollection postData)
        {
            selectAction action;

            if (postData[BuildUniqueControlName(_addButtonName)] != null ||
                postData[BuildUniqueControlName(_addButtonName) + ".x"] != null)
            {
                action = selectAction.Select;
            }
            else if (postData[BuildUniqueControlName(_removeButtonName)] != null ||
                    postData[BuildUniqueControlName(_removeButtonName) + ".x"] != null)
            {
                action = selectAction.Remove;
            }
            else
                action = selectAction.None;

            return action;
        }

        /// <summary>
        /// Helper function to process the select/remove operation and update the control's state.
        /// </summary>
        /// <param name="action">Specifies whether items are being selected or removed.</param>
        /// <param name="postData">A dictionary collection of all incoming form values.</param>
        /// <returns>True if the action results in a state change, false if no change has occured.</returns>
        /// <remarks>It is possible for this method to return false if the impact of a selection
        /// action is that no change has occured.  For example, if the user clicks the "Remove" button, but
        /// the only items selected are in the Additions list.</remarks>
        private bool ProcessSelectionAction(selectAction action, NameValueCollection postData)
        {
            bool selectionHasChanged = false;

            string[] selections = null;
            string selectKey =
                BuildUniqueControlName((action == selectAction.Select) ? _leftSelectName : _rightSelectName);

            if ((selections = postData.GetValues(selectKey)) != null)
            {
                foreach (ListItem item in Items)
                {
                    if (Array.Find(selections, delegate(string s) { return s == item.Text; }) != null)
                    {
                        // Mark the item selected or unselected based on the current action.
                        item.Selected = (action == selectAction.Select);
                        selectionHasChanged = true;
                    }
                }
            }

            return selectionHasChanged;
        }

        /// <summary>
        /// Helper function to check for select/remove operations which occured without a postback.
        /// (i.e. The updates occurred via JavaScript.)
        /// </summary>
        /// <param name="postData">A dictionary collection of all incoming form values.</param>
        /// <returns>True if the action results in a state change, false if no change has occured.</returns>
        /// <remarks>It is possible for this method to return false if the postback event was
        /// unrelated to any changes in the control</remarks>
        private bool ProcessAsynchronousAction(NameValueCollection postData)
        {
            bool selectionHasChanged = false;

            string[] selections = null;
            string selectKey =
                BuildUniqueControlName(_hiddenSelectListName);

            if ((selections = postData.GetValues(selectKey)) != null)
            {
                /// The selections array is expected to only have one element, made of pipe-separated
                /// values.  Here we split the values into an array.
                selections = selections[0].Split(new char[]{'|'}, StringSplitOptions.RemoveEmptyEntries);
                foreach (ListItem item in Items)
                {
                    if (Array.Find(selections, delegate(string s) { return s == item.Value; }) != null)
                    {
                        /// Only update the item if its selection state changed.
                        if (!item.Selected)
                        {
                            item.Selected = true;
                            selectionHasChanged = true;
                        }
                    }
                    else
                    {
                        /// Only update the item if its selection state changed.
                        if (item.Selected)
                        {
                            item.Selected = false;
                            selectionHasChanged = true;
                        }
                    }
                }
            }

            return selectionHasChanged;
        }


        /// <summary>
        /// Implementation of IPostBackDataHandler.RaisePostDataChangedEvent().
        /// 
        /// The .NET framework calls this method in response to LoadPostData() returning true,
        /// allowing other controls to be notified that this one's selection has changed.
        /// </summary>
        public void RaisePostDataChangedEvent()
        {
            base.OnSelectedIndexChanged(EventArgs.Empty);
        }

        #endregion

        #region Rendering Methods

        /// <summary>
        /// Overrides the control default set of attributes on the control's outermost tag.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object associated with the
        /// current output stream.</param>
        protected override void AddAttributesToRender(
            HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
        }

        /// <summary>
        /// Register the control to be notified of postback events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            ClientScriptManager cs = Page.ClientScript;
            Type myType = typeof(TwoListSelect);

            // Order is important.
            // The control's JavaScript component relies on Prototype being present first.
            PrototypeManager.Load(this.Page);
            JSManager.AddResource(this.Page, typeof(TwoListSelect), "NCI.Web.UI.WebControls.FormControls.Resources.TwoListSelect.js");

            Page.RegisterRequiresPostBack(this);
            base.OnPreRender(e);
        }

        /// <summary>
        /// Overrides the control's rendering to allow customized output.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object associated with the
        /// current output stream.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            string addButtonName = BuildUniqueControlName(_addButtonName);
            string removeButtonName = BuildUniqueControlName(_removeButtonName);
            string leftSelectName = BuildUniqueControlName(_leftSelectName);
            string rightSelectName = BuildUniqueControlName(_rightSelectName);
            string selectListName = BuildUniqueControlName(_hiddenSelectListName);

            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            // Label row
            if (!string.IsNullOrEmpty(SelectedItemLabel) || !string.IsNullOrEmpty(UnselectedItemLabel))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                if (!string.IsNullOrEmpty(UnselectedItemLabel))
                    writer.WriteEncodedText(UnselectedItemLabel);
                writer.RenderEndTag();  // TD

                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.RenderEndTag();  // TD

                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                if (!string.IsNullOrEmpty(SelectedItemLabel))
                    writer.WriteEncodedText(SelectedItemLabel);
                writer.RenderEndTag();  // TD

                writer.RenderEndTag();  // TR
            }

            // Selections
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            // Left Cell
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            DrawSelectBox(writer, leftSelectName, false, alignDirection.left);
            DrawHiddenSelectionField(writer, selectListName);
            writer.RenderEndTag();  // TD -- Left Cell


            // Middle Cell
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            DrawSelectButton(writer, addButtonName, SelectButtonText, SelectButtonImageUrl);
            writer.WriteBreak();
            DrawSelectButton(writer, removeButtonName, UnselectButtonText, UnselectButtonImageUrl);
            writer.RenderEndTag();  // TD -- Middle Cell


            // Right Cell
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            DrawSelectBox(writer, rightSelectName, true, alignDirection.right);
            writer.RenderEndTag();  // TD -- Right Cell

            writer.RenderEndTag();  // TR

            writer.RenderEndTag();  // Table

            // Setup Javascript.
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
            writer.Write("var " + this.ClientID + this.ClientIDSeparator + "obj = new TwoListSelect('" + this.ClientID + "');" );
            writer.RenderEndTag();

        }

        /// <summary>
        /// Renders a list box containing only the list items which are selected or unselected
        /// (as determined by the isSelectedSide parameter).
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object associated with the
        /// current output stream.</param>
        /// <param name="controlId">The UI element Id.</param>
        /// <param name="isSelectedSide">Boolean value to control whether the listbox is to be
        /// drawn with the values which have been selected (True) or the values which have not
        /// been selected (False).</param>
        /// <param name="align">Enumerated value of type alignDirection controlling whether
        /// the list box should float to the left or right side of the control.</param>
        private void DrawSelectBox(HtmlTextWriter writer, string controlId, bool isSelectedSide, alignDirection align)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, controlId);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, controlId);
            writer.AddAttribute(HtmlTextWriterAttribute.Multiple, string.Empty);
            writer.AddAttribute(HtmlTextWriterAttribute.Size, Rows.ToString());
            writer.AddStyleAttribute("float", align.ToString());
            Unit width = ListBoxWidth;
            if (width != Unit.Empty)    // Don't style an empty width.
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, ListBoxWidth.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Select);

            foreach (ListItem option in Items)
            {
                if (option.Selected == isSelectedSide)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, option.Value);
                    writer.RenderBeginTag(HtmlTextWriterTag.Option);
                    writer.WriteEncodedText(option.Text);
                    writer.RenderEndTag();
                }
            }

            writer.RenderEndTag();
        }

        /// <summary>
        /// Helper function to encapsulate the logic for rendering a UI button as either a text
        /// button, or as an Image.
        /// </summary>
        /// <param name="writer">The HtmlTextWriter object associated with the
        /// current output stream.</param>
        /// <param name="controlId">The UI element Id.</param>
        /// <param name="text">The label to be displayed on the button or, in the case of an image button
        /// the associated tooltip text.</param>
        /// <param name="imgUrl">URL of an image to be displayed on the button.</param>
        /// <remarks>DrawSelectButton may be used to display either a text button, or an image button.
        /// If an image button is rendered, the text value is used as the tool tip.  If the image URL is
        /// null or the empty string, a text button will be displayed.</remarks>
        private void DrawSelectButton(HtmlTextWriter writer, string controlId, string text, string imgUrl)
        {
            if (string.IsNullOrEmpty(imgUrl))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "submit");
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "image");
                writer.AddAttribute(HtmlTextWriterAttribute.Src, base.ResolveClientUrl(imgUrl));
                writer.AddAttribute(HtmlTextWriterAttribute.Title, text);
                writer.AddAttribute(HtmlTextWriterAttribute.Alt, text);
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Id, controlId);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, controlId);
            writer.AddAttribute(HtmlTextWriterAttribute.Value, text);
            if (ButtonWidth != Unit.Empty)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, ButtonWidth.ToString());

            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        /// <summary>
        /// Creates a hidden field containing a list of the current selections for use when the control
        /// is being updated on the client-side via JavaScript.
        /// </summary>
        /// <param name="writer"></param>
        private void DrawHiddenSelectionField(HtmlTextWriter writer, string name)
        {
            // Gather the list of values.
            StringBuilder selectedValues = new StringBuilder();
            foreach (ListItem item in Items)
            {
                if (item.Selected)
                {
                    if (selectedValues.Length > 0)
                        selectedValues.Append('|');
                    selectedValues.Append(item.Value);
                }
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, selectedValues.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Id, name);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, name);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        /// <summary>
        /// Helper function to create a unique name for a UI item based on the
        /// unique Id of the overall control.
        /// </summary>
        /// <param name="controlId">String containing the name of the UI element.</param>
        /// <returns>String value containing a unique Identifier for the UI element.</returns>
        private string BuildUniqueControlName(string controlId)
        {
            return this.ClientID + this.ClientIDSeparator + controlId;
        }

        #endregion
    }
}