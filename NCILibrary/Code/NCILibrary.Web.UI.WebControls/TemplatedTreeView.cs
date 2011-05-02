using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.Collections;
using System.Globalization;
using System.Collections.Specialized;

using NCI.Web.UI.WebControls;
using NCI.Web.UI.WebControls.JSLibraries;

namespace NCI.Web.UI.WebControls
{
    [DefaultProperty("")]
    [ToolboxData("<{0}:TemplatedTreeView runat=server></{0}:TemplatedTreeView>")]
    [ParseChildren(true, "Templates"), PersistChildren(false)]
    public class TemplatedTreeView : MultiTemplatedDataBoundControl, IPostBackDataHandler
    {
        #region fields
        private TemplateItemCollection _templates = new TemplateItemCollection();
        private string _hiddenFieldId = "hiddenField";

        #endregion

        #region public properties

        /// <summary>
        /// Holds the collection of templates to be used for the nodes of the tree view.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public TemplateItemCollection Templates
        {
            get
            {
                return _templates;
            }
        }

        /// <summary>
        /// The location of the image (usually spacer.gif or another transparent gif) that is created by
        /// javascript to reside inside the expand/collapse link associated with nodes that have children.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue(""),
        Description("The url to the image that is clicked on to expand the tree node."),
        Localizable(true),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public virtual string ExpanderLinkImageUrl
        {
            get
            {
                string str = (string)this.ViewState["ExpanderLinkImageUrl"];
                if (str == null)
                {
                    return Page.ClientScript.GetWebResourceUrl(
                        typeof(TemplatedTreeView), 
                        "NCI.Web.UI.WebControls.Resources.spacer.gif");
                }
                return str;
            }
            set
            {
                this.ViewState["ExpanderLinkImageUrl"] = value;
            }
        }

        /// <summary>
        /// The css class that styles the image (see ExpanderLinkImageUrl) that is created by
        /// javascript to reside inside the expand/collapse link associated with nodes that have children.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("expander-image"),
        Description("The css class that styles ExpanderLinkImageUrl."),
        Localizable(true),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public virtual string ExpanderLinkImageCssClass
        {
            get
            {
                string str = (string)this.ViewState["ExpanderLinkImageCssClass"];
                if (str == null)
                {
                    return "expander-image";
                }
                return str;
            }
            set
            {
                this.ViewState["ExpanderLinkImageCssClass"] = value;
            }
        }

        /// <summary>
        /// The css class that styles the li of a collapsed node.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("collapsed"),
        Description("The css class that styles the li of a collapsed node."),
        Localizable(true),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public virtual string NodeCollapsedCssClass
        {
            get
            {
                string str = (string)this.ViewState["NodeCollapsedCssClass"];
                if (str == null)
                {
                    return "collapsed";
                }
                return str;
            }
            set
            {
                this.ViewState["NodeCollapsedCssClass"] = value;
            }
        }

        /// <summary>
        /// The css class that styles the li of an expanded node.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("expanded"),
        Description("The css class that styles the li of an expanded node."),
        Localizable(true),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public virtual string NodeExpandedCssClass
        {
            get
            {
                string str = (string)this.ViewState["NodeExpandedCssClass"];
                if (str == null)
                {
                    return "expanded";
                }
                return str;
            }
            set
            {
                this.ViewState["NodeExpandedCssClass"] = value;
            }
        }

        /// <summary>
        /// The css class that styles the li of a collapsed node through javascript.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("collapsed-js"),
        Description("The css class that styles the li of a collapsed node through javascript."),
        Localizable(true),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public virtual string NodeCollapsedJSCssClass
        {
            get
            {
                string str = (string)this.ViewState["NodeCollapsedJSCssClass"];
                if (str == null)
                {
                    return "collapsed-js";
                }
                return str;
            }
            set
            {
                this.ViewState["NodeCollapsedJSCssClass"] = value;
            }
        }

        /// <summary>
        /// The css class that styles the li of an expanded node through javascript.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("expanded-js"),
        Description("The css class that styles the li of an expanded node through javascript."),
        Localizable(true),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public virtual string NodeExpandedJSCssClass
        {
            get
            {
                string str = (string)this.ViewState["NodeExpandedJSCssClass"];
                if (str == null)
                {
                    return "expanded-js";
                }
                return str;
            }
            set
            {
                this.ViewState["NodeExpandedJSCssClass"] = value;
            }
        }

        /// <summary>
        /// The css class that styles the li of a non-appendable leaf node.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("leaf"),
        Description("The css class that styles the li of a non-appendable leaf node."),
        Localizable(true),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public virtual string NodeLeafCssClass
        {
            get
            {
                string str = (string)this.ViewState["NodeLeafCssClass"];
                if (str == null)
                {
                    return "leaf";
                }
                return str;
            }
            set
            {
                this.ViewState["NodeLeafCssClass"] = value;
            }
        }

        /// <summary>
        /// The css class that styles the li of a non-appendable leaf node through javascript.
        /// </summary>
        [
        Bindable(true),
        Category("Appearance"),
        DefaultValue("leaf-js"),
        Description("The css class that styles the li of a non-appendable leaf node through javascript."),
        Localizable(true),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public virtual string NodeLeafJSCssClass
        {
            get
            {
                string str = (string)this.ViewState["NodeLeafJSCssClass"];
                if (str == null)
                {
                    return "leaf-js";
                }
                return str;
            }
            set
            {
                this.ViewState["NodeLeafJSCssClass"] = value;
            }
        }

        /// <summary>
        /// Gets or Sets the selected node of this tree.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedNodeID
        {
            get
            {
                TemplatedTreeViewNodeContainer selected = GetSelectedTreeNode();
                if (selected != null)
                    return selected.TreeNodeId;
                return string.Empty;
            }
            set
            {
                if (SelectedNodeID != string.Empty)
                {
                    TemplatedTreeViewNodeContainer selected = GetSelectedTreeNode();
                    selected.IsSelected = false;
                }

                TemplatedTreeViewNodeContainer nodeToSelect = GetTreeNodeByID(value);
                if (nodeToSelect != null)
                {
                    nodeToSelect.IsSelected = true;
                    ExpandNodesToParent(nodeToSelect);
                }
            }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        #endregion

        #region control creation

        /// <summary>
        /// Creates the controls based on the list of nodes (if databinding) or from viewstate (if postback).
        /// The child controls are of type TemplatedTreeViewNodeContainer.
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataBinding"></param>
        /// <returns>Number of child controls created.</returns>
        protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
        {
            if (dataSource != null)
            {
                int count = 0;
                foreach (ITemplatedTreeNode item in dataSource)
                {
                    if (item != null)
                    {
                        CreateControlFromNode(item);
                    }
                    else
                    {
                        CreateControl(count);
                    }
                    count++;
                }

                return count;
            }
            else return 0;

        }

        /// <summary>
        /// Called from CreateChildControls if this is not a postback. Creates an instance of 
        /// TemplatedTreeViewNodeContainer and sets some of its properties based on the node passed in.
        /// This is where the template is created and instantiated in the container control.
        /// When we post back, we will need to recreate these controls, but will not have the nodes
        /// available and therefore, will not have the NodeType (or TemplateType) available either.
        /// So for each of these controls, we store the string of the NodeType into a list 
        /// (_templateTypeList) that will be restored when the ViewState of the tree view gets restored.
        /// </summary>
        /// <param name="node"></param>
        private void CreateControlFromNode(ITemplatedTreeNode node)
        {
            TemplatedTreeViewNodeContainer container = new TemplatedTreeViewNodeContainer(node.ItemType, node.Data);

            this.Controls.Add(container);
            container.TreeNodeId = node.TemplatedTreeNodeID;
            container.Path = node.Path;

            container.IsExpanded = node.IsExpanded;
            container.IsSelected = node.IsSelected;
            container.HasChildren = node.HasChildren;

            
            ITemplate nodeTemplate = Templates[node.ItemType];

            if (nodeTemplate != null)
            {
                nodeTemplate.InstantiateIn(container);
                container.DataBind();
            }
        }

        /// <summary>
        /// If this is not a postback, then the control properties have been loaded from viewstate
        /// and the CreateChildControls uses this method to create each child control of 
        /// type TemplatedTreeViewNodeContainer. There is no node here to copy the properties from, but at this
        /// point, the properties are repopulated from the viewstate of each control. The template is 
        /// recreated here for each control and instantiated in the container control.
        /// The TemplateType list has been restored from the ViewState of the tree view and we can access
        /// the template types by index (passed in controlNumber). We have to also save off this list so it
        /// can later be stored in ViewState again.
        /// </summary>
        /// <param name="controlNumber"></param>
        private void CreateControl(int controlNumber)
        {
            TemplatedTreeViewNodeContainer container = new TemplatedTreeViewNodeContainer(TemplateTypes[controlNumber]);
            this.Controls.Add(container);
            ITemplate nodeTemplate = Templates[TemplateTypes[controlNumber]];
            nodeTemplate.InstantiateIn(container);
        }

        #endregion

        #region rendering

        /// <summary>
        /// Sets up the prototype library and the javascript on the page.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            //Do not render this stuff if there are no controls.  This avoids unnessasary JS includes
            //and possible broken JS.
            if (this.Controls.Count < 1)
            {
                this.Visible = false;
                return;
            }

            // Order is important.
            // The control's JavaScript component relies on Prototype being present first.
            PrototypeManager.Load(this.Page);
            JSManager.AddResource(this.Page, typeof(TemplatedTreeView), "NCI.Web.UI.WebControls.Resources.TemplatedTreeView.js");

            //string hiddenFieldClientID = this.ClientID + this.ClientIDSeparator + _hiddenFieldId;
            //string script = "document.getElementById('" + hiddenFieldClientID + "').value = " + this.ClientID + "_obj.getState('" + this.ClientID + "');";
            //Page.ClientScript.RegisterOnSubmitStatement(typeof(TemplatedTreeView), hiddenFieldClientID, script);

            Page.RegisterRequiresPostBack(this);

            base.OnPreRender(e);
        }

        /// <summary>
        /// Renders a closing div tag around the tree view.
        /// Optionaly, renders a wrapping closing div tag if the css class for the wrapper div is defined.
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            //render hidden field
            string hiddenFieldClientID = this.ClientID + this.ClientIDSeparator + _hiddenFieldId;
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, hiddenFieldClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID + this.IdSeparator + _hiddenFieldId);
            writer.AddAttribute(HtmlTextWriterAttribute.Value, GetExpansionState());
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            //Setup JavaScript
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            writer.RenderBeginTag(HtmlTextWriterTag.Script);

            writer.Write("var " + this.ClientID + "_obj = new NCITreeView('" + this.ClientID + "');");

            writer.RenderEndTag(); //End script tag

            base.RenderEndTag(writer);
        }

        /// <summary>
        /// Renders the tree view control, the hidden field to store the expansion state,
        /// and the associated javascript
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            int pos = 0;
            int endingDepth = 1;

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "rootlist");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + this.ClientIDSeparator + "rootlist");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);

            while (pos < this.Controls.Count)
            {
                TemplatedTreeViewNodeContainer ctrl = (TemplatedTreeViewNodeContainer)this.Controls[pos];

                //If the current depth is less than the previous depth then close all of the
                //ul li elements until we are at the correct depth.
                if (pos != 0)
                {
                    int prevDepth = ((TemplatedTreeViewNodeContainer)this.Controls[pos - 1]).Depth;
                    if (ctrl.Depth < prevDepth)
                    {
                        ChangeDepth(writer, prevDepth, ctrl.Depth);
                    }
                }

                //Draw the opening li
                writer.Indent++;
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + this.ClientIDSeparator + "ttvnode" + this.ClientIDSeparator + pos.ToString());
                writer.AddAttribute(HtmlTextWriterAttribute.Class, GetClassesForNode(ctrl));
                writer.RenderBeginTag(HtmlTextWriterTag.Li);

                //Render the actual contents of the Node
                ctrl.RenderControl(writer);

                if (!ctrl.HasChildren)
                {
                    //Close out the li of this node since there are no children.
                    writer.RenderEndTag();
                    writer.Indent--;
                    writer.WriteLine();
                }
                else
                {
                    //This has children so we need to draw an opening ul
                    writer.Indent++;
                    writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                }

                endingDepth = ctrl.Depth;
                pos++;
            }

            if (endingDepth != 1)
            {
                ChangeDepth(writer, endingDepth, 1);               
            }

            writer.RenderEndTag();
        }

        /// <summary>
        /// Closes the ULs LIs to move to a different depth.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="fromDepth"></param>
        /// <param name="toDepth"></param>
        private void ChangeDepth(HtmlTextWriter writer, int fromDepth, int toDepth)
        {
            for (int i = fromDepth; i > toDepth; i--)
            {
                //close li
                writer.RenderEndTag();
                writer.Indent--;
                writer.WriteLine();

                //close ul
                writer.RenderEndTag();
                writer.Indent--;
                writer.WriteLine();
            }
        }

        private string GetClassesForNode(TemplatedTreeViewNodeContainer ctrl)
        {
            string classes = "ttvnode";

            if (!string.IsNullOrEmpty(ctrl.TemplateType))
                classes += " " + ctrl.TemplateType;

            if (!ctrl.HasChildren)
                classes += " ttvleaf";

            if (ctrl.IsSelected)
                classes += " selected";

            return classes;
        }

        #endregion

        #region private utility methods

        /// <summary>
        /// Builds a string of characters representing the expansion state of the nodes.
        /// </summary>
        /// <returns></returns>
        private string GetExpansionState()
        {
            string s = string.Empty;
            foreach (TemplatedTreeViewNodeContainer c in this.Controls)
            {
                if (c.IsExpanded)
                    s += "e";
                else if (!c.HasChildren)
                    s += "l";
                else
                    s += "c";
            }
            return s;
        }

        private void ExpandNodesToParent(TemplatedTreeViewNodeContainer node)
        {
            int currentDepth = node.Depth;

            int indexOfNode = this.Controls.IndexOf(node);
            if (indexOfNode >= 0)
            {
                for (int i = indexOfNode; i >= 0; i--)
                {
                    if (currentDepth == 0)
                        break;

                    TemplatedTreeViewNodeContainer checkNode = (TemplatedTreeViewNodeContainer)this.Controls[i];
                    if (checkNode.Depth == currentDepth)
                    {
                        if (checkNode.HasChildren)
                            checkNode.IsExpanded = true;
                        currentDepth--;
                    }
                }
            }
        }

        private TemplatedTreeViewNodeContainer GetTreeNodeByID(string nodeID)
        {
            foreach (TemplatedTreeViewNodeContainer node in this.Controls)
                if (node.TreeNodeId == nodeID)
                    return node;
            return null;
        }

        private TemplatedTreeViewNodeContainer GetSelectedTreeNode()
        {
            foreach (TemplatedTreeViewNodeContainer node in this.Controls)
                if (node.IsSelected)
                    return node;
            return null;
        }

        #endregion

        #region IPostBackDataHandler Members

        /// <summary>
        /// Extracts the value of the hidden field, which is the expansion state of all the nodes written
        /// as a sequence of characters that are one of the following three: 
        /// 'e' - expanded, 'c' - collapsed, 'l' - leaf
        /// At this point the controls collection has been rebuilt so we go through the expansion state
        /// string, one character at a time and assign the container controls IsExpanded property based on
        /// the character.
        /// </summary>
        /// <param name="postDataKey">Unique ID of this tree</param>
        /// <param name="postCollection">Collection that contains the posted fields, including the hidden field we need.</param>
        /// <returns></returns>
        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            //this is the value of the hidden field that is written by javascript 
            //and returned in the postCollection
            string postedExpansionState = postCollection[postDataKey + this.IdSeparator + this._hiddenFieldId];
            
            int count = 0;
            foreach (char c in postedExpansionState)
            {
                if (this.Controls[count] is TemplatedTreeViewNodeContainer)
                {
                    TemplatedTreeViewNodeContainer container = (TemplatedTreeViewNodeContainer)this.Controls[count];
                    switch (c)
                    {
                        case 'e':
                            {
                                container.IsExpanded = true;
                                break;
                            }
                        case 'c':
                            {
                                container.IsExpanded = false;
                                break;
                            }
                    }
                }
                count++;
            }
            return false;
        }

        /// <summary>
        /// This is needed as part of implementing IPostBackDataHandler, but we don't need it.
        /// </summary>
        public void RaisePostDataChangedEvent()
        {
            return;
        }

        #endregion
    }
}
