using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace NCI.Web.UI.WebControls
{
    public class PagerStyleSettings : IStateManager
    {
        private bool _isTracking;
        private StateBag _viewState = new StateBag();

        /// <summary>
        /// Gets and Sets the additional Css class name of the first page item.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string FirstIndexCssClass
        {
            get
            {
                string s = (string)ViewState["FirstIndexCssClass"];
                return (s == null) ? string.Empty : s;
            }
            set { ViewState["FirstIndexCssClass"] = value; }
        }

        /// <summary>
        /// Gets and Sets the additional Css class name of the last page item.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string LastIndexCssClass
        {
            get
            {
                string s = (string)ViewState["LastIndexCssClass"];
                return (s == null) ? string.Empty : s;
            }
            set { ViewState["LastIndexCssClass"] = value; }
        }

        /// <summary>
        /// Gets and Sets the Css class name of the selected index.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string SelectedIndexCssClass
        {
            get
            {
                string s = (string)ViewState["SelectedIndexCssClass"];
                return (s == null) ? string.Empty : s;
            }
            set { ViewState["SelectedIndexCssClass"] = value; }
        }

        /// <summary>
        /// Gets and Sets the Css class name of the the index links (1, 2, ..., n).
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string IndexLinkCssClass
        {
            get
            {
                string s = (string)ViewState["SelectedIndexCssClass"];
                return (s == null) ? string.Empty : s;
            }
            set { ViewState["SelectedIndexCssClass"] = value; }
        }

        #region PreviousLink

        /// <summary>
        /// Gets and Sets the Css class name of the the Previous link.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string PrevPageCssClass
        {
            get
            {
                string s = (string)ViewState["PrevPageCssClass"];
                return (s == null) ? "" : s;
            }
            set { ViewState["PrevPageCssClass"] = value; }
        }

        /// <summary>
        /// Gets and Sets the text of the the Previous link. (Can be html)
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("&lt;&nbsp;Previous")]
        [Localizable(true)]
        public string PrevPageText
        {
            get
            {
                string s = (string)ViewState["PrevPageText"];
                return (s == null) ? "&lt;&nbsp;Previous" : s;
            }
            set { ViewState["PrevPageText"] = value; }
        }

        /// <summary>
        /// Gets and Sets the url of the image to use for the Previous link.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string PrevPageImageUrl
        {
            get
            {
                string s = (string)ViewState["PrevPageImageUrl"];
                return (s == null) ? "" : s;
            }
            set { ViewState["PrevPageImageUrl"] = value; }
        }

        /// <summary>
        /// Gets and Sets the css class to use for the separator between the Previous link and the items list.
        /// <remarks>If this is not set not markup will be added to the separator</remarks>
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string PrevPageSeparatorCssClass
        {
            get
            {
                string s = (string)ViewState["PrevPageSeparatorCssClass"];
                return (s == null) ? string.Empty : s;
            }
            set { ViewState["PrevPageSeparatorCssClass"] = value; }
        }

        /// <summary>
        /// Gets and Sets the text to use for the separator between the Previous link and the items list.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("&nbsp;&nbsp;")]
        [Localizable(true)]
        public string PrevPageSeparatorText
        {
            get
            {
                string s = (string)ViewState["PrevPageSeparatorText"];
                return (s == null) ? "&nbsp;&nbsp;" : s;
            }
            set { ViewState["PrevPageSeparatorText"] = value; }
        }

        /// <summary>
        /// Gets and Sets the text to use for the separator between the Previous link and the items list.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string PrevPageSeparatorImageUrl
        {
            get
            {
                string s = (string)ViewState["PrevPageSeparatorImageUrl"];
                return (s == null) ? "" : s;
            }
            set { ViewState["PrevPageSeparatorImageUrl"] = value; }
        }

        #endregion

        #region NextLink

        /// <summary>
        /// Gets and Sets the Css class name of the the Next link.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string NextPageCssClass
        {
            get
            {
                string s = (string)ViewState["NextPageCssClass"];
                return (s == null) ? "" : s;
            }
            set { ViewState["NextPageCssClass"] = value; }
        }

        /// <summary>
        /// Gets and Sets the text of the the Next link. (Can be html)
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("Next&nbsp;&gt;")]
        [Localizable(true)]
        public string NextPageText
        {
            get
            {
                string s = (string)ViewState["NextPageText"];
                return (s == null) ? "Next&nbsp;&gt;" : s;
            }
            set { ViewState["NextPageText"] = value; }
        }

        /// <summary>
        /// Gets and Sets the url of the icon to use for the Next link.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string NextPageImageUrl
        {
            get
            {
                string s = (string)ViewState["NextPageImageUrl"];
                return (s == null) ? "" : s;
            }
            set { ViewState["NextPageImageUrl"] = value; }
        }

        /// <summary>
        /// Gets and Sets the css class to use for the separator between the next link and the items list.
        /// <remarks>If this is not set not markup will be added to the separator</remarks>
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string NextPageSeparatorCssClass
        {
            get
            {
                string s = (string)ViewState["NextPageSeparatorCssClass"];
                return (s == null) ? string.Empty : s;
            }
            set { ViewState["NextPageSeparatorCssClass"] = value; }
        }

        /// <summary>
        /// Gets and Sets the text to use for the separator between the Next link and the items list.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("&nbsp;&nbsp;")]
        [Localizable(true)]
        public string NextPageSeparatorText
        {
            get
            {
                string s = (string)ViewState["NextPageSeparatorText"];
                return (s == null) ? "&nbsp;&nbsp;" : s;
            }
            set { ViewState["NextPageSeparatorText"] = value; }
        }

        /// <summary>
        /// Gets and Sets the text to use for the separator between the Next link and the items list.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string NextPageSeparatorImageUrl
        {
            get
            {
                string s = (string)ViewState["NextPageSeparatorImageUrl"];
                return (s == null) ? "" : s;
            }
            set { ViewState["NextPageSeparatorImageUrl"] = value; }
        }

        #endregion

        #region PageSeparator

        /// <summary>
        /// Gets and Sets the text of the the separator that goes between page numbers. (Can be html)
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("&nbsp;")]
        [Localizable(true)]
        public string PageSeparatorText
        {
            get
            {
                string s = (string)ViewState["PrevLinkText"];
                return (s == null) ? "&nbsp;" : s;
            }
            set { ViewState["PrevLinkText"] = value; }
        }

        /// <summary>
        /// Gets and Sets the CssClass of the the separator that goes between page numbers. (Can be html)
        /// <remarks>If this is not set not markup will be added to the separator</remarks>
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string PageSeparatorCssClass
        {
            get
            {
                string s = (string)ViewState["PageSeparatorCssClass"];
                return (s == null) ? string.Empty : s;
            }
            set { ViewState["PageSeparatorCssClass"] = value; }
        }

        /// <summary>
        /// Gets and Sets the url of the image of the the separator that goes between page numbers. (Can be html)
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string PageSeparatorImageUrl
        {
            get
            {
                string s = (string)ViewState["PageSeparatorImageUrl"];
                return (s == null) ? string.Empty : s;
            }
            set { ViewState["PageSeparatorImageUrl"] = value; }
        }
        #endregion


        #region IStateManager Members

        private StateBag ViewState
        {
            get
            {
                return this._viewState;
            }
        }

        bool IStateManager.IsTrackingViewState
        {
            get
            {
                return this._isTracking;
            }
        }

        void IStateManager.LoadViewState(object state)
        {
            if (state != null)
            {
                ((IStateManager)this.ViewState).LoadViewState(state);
            }
        }

        object IStateManager.SaveViewState()
        {
            return ((IStateManager)this.ViewState).SaveViewState();
        }

        void IStateManager.TrackViewState()
        {
            this._isTracking = true;
            ((IStateManager)this.ViewState).TrackViewState();
        }


        #endregion
    }
}
