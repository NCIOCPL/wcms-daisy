using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// This class is the container in which the template will be instantiated. It essentially replaces the
    /// TreeNode if compared the the ASP.NET TreeView control. So it's a "tree node" control that 
    /// supports templates. This is why it implements INamingContainer - so that any child controls that are
    /// in the template will get unique names.
    /// </summary>
    public class TemplatedTreeViewNodeContainer : TemplateItemContainer, IDataItemContainer, INamingContainer
    {
        #region fields
        private object _data;
        private string _templateType = string.Empty;

        #endregion

        #region public properties

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue("")]
        public string TreeNodeId
        {
            get
            {
                string str = (string)this.ViewState["TreeNodeId"];
                if (str == null)
                {
                    return string.Empty;
                }
                return str;
            }
            set
            {
                this.ViewState["TreeNodeId"] = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue("")]
        public string Path
        {
            get
            {
                string str = (string)this.ViewState["Path"];
                if (str == null)
                {
                    return string.Empty;
                }
                return str;
            }
            set
            {
                this.ViewState["Path"] = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue("")]
        public int Depth
        {
            get
            {
                string[] starr = Path.Split('|');
                if (starr != null)
                    return starr.Length;
                else
                    return -1;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool IsSelected
        {
            get
            {
                bool bl = (bool)this.ViewState["IsSelected"];
                return bl;
            }
            set
            {
                this.ViewState["IsSelected"] = value;
                if (value)
                    this.CssClass = "selected";
                else
                    this.CssClass = "";
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool IsExpanded
        {
            get
            {
                bool bl = (bool)this.ViewState["IsExpanded"];
                return bl;
            }
            set
            {
                this.ViewState["IsExpanded"] = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(false)]
        public bool HasChildren
        {
            get
            {
                bool bl = (bool)this.ViewState["HasChildren"];
                return bl;
            }
            set
            {
                this.ViewState["HasChildren"] = value;
            }
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content.</param>
        /// <remarks>This implements the normal render method because we want the contents to be wrapped with
        /// a span tag.</remarks>
        protected override void Render(HtmlTextWriter writer)
        {
            this.RenderBeginTag(writer);
            this.RenderContents(writer);
            this.RenderEndTag(writer);
        }

        #endregion

        #region construction

        public TemplatedTreeViewNodeContainer(string templateType) : base(templateType) { }

        public TemplatedTreeViewNodeContainer(string templateType, object data) : base(templateType, data) { }

        #endregion


    }
}
