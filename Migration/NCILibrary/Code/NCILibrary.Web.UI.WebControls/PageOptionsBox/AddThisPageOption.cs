using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.ComponentModel;

namespace NCI.Web.UI.WebControls
{
    public class AddThisPageOption : PageOption
    {
        private AddThisPageOptionSettings _settings;

        public virtual string PageTitle
        {
            get
            {
                return (string)this.ViewState["PageTitle"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["PageTitle"]))
                {
                    this.ViewState["PageTitle"] = value;
                    this.OnOptionChanged();
                }
            }
        }

        public virtual AddThisPageOptionSettings Settings
        {
            get
            {
                if (this._settings == null)
                {
                    this._settings = new AddThisPageOptionSettings();
                    if (this.IsTrackingViewState)
                    {
                        ((IStateManager)this._settings).TrackViewState();
                    }
                }
                return this._settings;
            }
        }

        protected override void CopyProperties(PageOption newOption)
        {
            ((AddThisPageOption)newOption).PageTitle = this.PageTitle;
            ((AddThisPageOption)newOption).Settings.CopyProperties(this.Settings);
            base.CopyProperties(newOption);
        }

        protected override PageOption CreateOption()
        {
            return new AddThisPageOption();
        }

        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] objArray = (object[])savedState;

                if (objArray[0] != null)
                    ((IStateManager)this.ViewState).LoadViewState(objArray[0]);

                if ((objArray.Length > 1) && (objArray[1] != null))
                {
                    ((IStateManager)Settings).LoadViewState(objArray[1]);
                }
            }
        }

        protected override object SaveViewState()
        {
            object obj1 = ((IStateManager)this.ViewState).SaveViewState();
            object obj2 = ((IStateManager)this.Settings).SaveViewState();

            if (obj1 == null)
                return null;

            return new object[] { obj1, obj2 };
        }

        protected override void TrackViewState()
        {
            base.TrackViewState();
            ((IStateManager)_settings).TrackViewState();
        }

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
    }
    
}
