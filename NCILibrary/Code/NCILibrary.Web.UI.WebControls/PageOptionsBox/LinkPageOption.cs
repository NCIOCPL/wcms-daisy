using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Web.UI.WebControls
{
    public class LinkPageOption : PageOption
    {
        public virtual string OnClick
        {
            get
            {
                return (string)this.ViewState["OnClick"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["OnClick"]))
                {
                    this.ViewState["OnClick"] = value;
                    this.OnOptionChanged();
                }
            }
        }

        public virtual string Href
        {
            get
            {
                return (string)this.ViewState["Href"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["Href"]))
                {
                    this.ViewState["Href"] = value;
                    this.OnOptionChanged();
                }
            }
        }

        protected override void CopyProperties(PageOption newOption)
        {
            ((LinkPageOption)newOption).OnClick = this.OnClick;
            ((LinkPageOption)newOption).Href = this.Href;
            base.CopyProperties(newOption);
        }

        protected override PageOption CreateOption()
        {
            return new LinkPageOption();
        }
    }
}
