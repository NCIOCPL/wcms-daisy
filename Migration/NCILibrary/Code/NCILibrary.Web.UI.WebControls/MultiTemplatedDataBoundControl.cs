using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MultiTemplatedDataBoundControl runat=server></{0}:MultiTemplatedDataBoundControl>")]
    public abstract class MultiTemplatedDataBoundControl : CompositeDataBoundControl
    {
        /// <summary>
        /// Stores the types of template to be used for each tree node container in viewState.
        /// </summary>
        protected string[] TemplateTypes
        {
            get
            {
                object o = this.ViewState["TemplateTypes"];
                if (o != null)
                    return (string[])o;
                else
                    return new string[] { };
            }
            set
            {
                this.ViewState["TemplateTypes"] = value;
            }
        }

        /// <summary>
        /// Saves the temporary list of Template types (that were used to create the templates for each 
        /// child control) into ViewState.
        /// </summary>
        /// <returns></returns>
        protected override object SaveViewState()
        {
            TemplateTypes = GetTemplateList();
            return base.SaveViewState();
        }

        protected virtual string[] GetTemplateList()
        {
            List<string> templates = new List<string>();

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TemplateItemContainer)
                {
                    templates.Add(((TemplateItemContainer)ctrl).TemplateType);
                }
            }

            return templates.ToArray();
        }


    }
}
