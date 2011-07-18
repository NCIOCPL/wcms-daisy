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
    [ToolboxData("<{0}:ServerControl1 runat=server></{0}:ServerControl1>")]
    public class PostBackButtonPager : WebControl, IPostBackDataHandler
    {
        private static readonly object PageChangedEvent = new object();
        private PagerStyleSettings _pagerStyleSettings = null;

        #region Properties

        /// <summary>
        /// Gets a reference to the PagerStyleSettings object that allows you to set the properties of the pager buttons in a Pager control.
        /// <br />
        /// Return Value <br/>
        /// A reference to the PagerStyleSettings that allows you to set the properties of the pager buttons in a Pager control.
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public PagerStyleSettings PagerStyleSettings
        {
            get
            {
                if (this._pagerStyleSettings == null)
                {
                    this._pagerStyleSettings = new PagerStyleSettings();
                    if (base.IsTrackingViewState)
                    {
                        ((IStateManager)this._pagerStyleSettings).TrackViewState();
                    }
                }
                return this._pagerStyleSettings;
            }
        }


        /// <summary>
        /// Gets and sets the current page
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(1)]
        [Localizable(true)]
        public int CurrentPage
        {
            get
            {
                object i = ViewState["CurrentPage"];
                return (i == null) ? 1 : (int)i;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("CurrentPage");
                ViewState["CurrentPage"] = value;
            }
        }

        /// <summary>
        /// Gets and sets the number of records per page view
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(10)]
        [Localizable(true)]
        public int RecordsPerPage
        {
            get
            {
                object i = ViewState["RecordsPerPage"];
                return (i == null) ? 1 : (int)i;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("RecordsPerPage");
                ViewState["RecordsPerPage"] = value;
            }
        }

        /// <summary>
        /// Gets and sets the total number of records
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(0)]
        [Localizable(true)]
        public int RecordCount
        {
            get
            {
                object i = ViewState["RecordCount"];
                return (i == null) ? 1 : (int)i;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("RecordCount");
                ViewState["RecordCount"] = value;
            }
        }

        /// <summary>
        /// Gets and Sets the number of page links to show.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(10)]
        [Localizable(true)]
        public int ShowNumPages
        {
            get
            {
                object i = ViewState["ShowNumPages"];
                return (i == null) ? 10 : (int)i;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("ShowNumPages");
                ViewState["ShowNumPages"] = value;
            }
        }

        /// <summary>
        /// The HtmlTextWriterTag (Div) that this control renders as...
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler PageChanged
        {
            add { this.Events.AddHandler(PageChangedEvent, value); }
            remove { this.Events.RemoveHandler(PageChangedEvent, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPageChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler)Events[PageChangedEvent];
            if (handler != null)
                handler(this, e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            Page.RegisterRequiresPostBack(this);
        }

        #region Render Stuff

        /// <summary>
        /// Renders the contents of this control.
        /// </summary>
        /// <param name="output">A HtmlTextWriter to render the contents to.</param>
        protected override void RenderContents(HtmlTextWriter output)
        {
            int startIndex = 0;
            int endIndex = 0;
            int pages = 0;

            //Get number of pages
            if (this.RecordsPerPage > 0)
            {
                pages = (this.RecordCount / this.RecordsPerPage) + ((this.RecordCount % this.RecordsPerPage > 0) ? 1 : 0);
            }

            if (pages > 1)
            {
                startIndex = this.CurrentPage - this.ShowNumPages > 0 ? this.CurrentPage - this.ShowNumPages : 1;
                endIndex = this.CurrentPage + this.ShowNumPages > pages ? pages : this.CurrentPage + this.ShowNumPages;

                //Previous Page Link
                if (this.CurrentPage > 1)
                {
                    RenderPrevLink(output);
                }

                RenderPageNumbers(output, startIndex, endIndex);

                //Next Page Link
                if (this.CurrentPage < pages)
                {
                    RenderNextLink(output);
                }
            }
        }

        protected virtual void RenderPrevLink(HtmlTextWriter output)
        {
            //If there is no image, and there is not text, then there is nothing to write.
            if (string.IsNullOrEmpty(this.PagerStyleSettings.PrevPageText) && string.IsNullOrEmpty(this.PagerStyleSettings.PrevPageImageUrl))
                return;

            if (!string.IsNullOrEmpty(this.PagerStyleSettings.PrevPageImageUrl))
            {
                RenderImageButton(
                    output,
                    this.PagerStyleSettings.PrevPageImageUrl,
                    this.PagerStyleSettings.PrevPageText,
                    CurrentPage - 1,
                    "prevBtn",
                    this.PagerStyleSettings.PrevPageCssClass);
            }
            else
            {
                RenderTextButton(
                    output,
                    this.PagerStyleSettings.PrevPageText,
                    CurrentPage - 1,
                    "prevBtn",
                    this.PagerStyleSettings.PrevPageCssClass);
            }

            if (this.PagerStyleSettings.PrevPageSeparatorImageUrl != string.Empty)
            {
                //If there is a class, draw a span tag, otherwise, just spit it out.
                if (this.PagerStyleSettings.PrevPageSeparatorCssClass != string.Empty)
                {
                    output.AddAttribute(HtmlTextWriterAttribute.Class, this.PagerStyleSettings.PrevPageSeparatorCssClass);
                    output.RenderBeginTag(HtmlTextWriterTag.Span);
                }

                //Draw separator image
                if (this.PagerStyleSettings.PrevPageSeparatorText != string.Empty)
                    output.AddAttribute(HtmlTextWriterAttribute.Alt, this.PagerStyleSettings.PrevPageSeparatorText);
                else
                    output.AddAttribute(HtmlTextWriterAttribute.Alt, "");
                output.AddAttribute(HtmlTextWriterAttribute.Src, this.PagerStyleSettings.PrevPageSeparatorImageUrl);
                output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                output.RenderBeginTag(HtmlTextWriterTag.Img);
                output.RenderEndTag();

                if (this.PagerStyleSettings.PrevPageSeparatorCssClass != string.Empty)
                    output.RenderEndTag();
            }
            else
            {
                //The prev button separator is only text
                if (this.PagerStyleSettings.PrevPageSeparatorText != string.Empty)
                {
                    //If there is a class, draw a span tag, otherwise, just spit it out.
                    if (this.PagerStyleSettings.PrevPageSeparatorCssClass != string.Empty)
                    {
                        output.AddAttribute(HtmlTextWriterAttribute.Class, this.PagerStyleSettings.PrevPageSeparatorCssClass);
                        output.RenderBeginTag(HtmlTextWriterTag.Span);
                    }

                    output.Write(this.PagerStyleSettings.PrevPageSeparatorText);

                    if (this.PagerStyleSettings.PrevPageSeparatorCssClass != string.Empty)
                        output.RenderEndTag();
                }
            }
        }

        protected virtual void RenderNextLink(HtmlTextWriter output)
        {
            //If there is no image, and there is not text, then there is nothing to write.
            if (String.IsNullOrEmpty(this.PagerStyleSettings.NextPageText) && String.IsNullOrEmpty(this.PagerStyleSettings.NextPageImageUrl))
                return;

            if (this.PagerStyleSettings.NextPageSeparatorImageUrl != string.Empty)
            {
                //If there is a class, draw a span tag, otherwise, just spit it out.
                if (this.PagerStyleSettings.NextPageSeparatorCssClass != string.Empty)
                {
                    output.AddAttribute(HtmlTextWriterAttribute.Class, this.PagerStyleSettings.NextPageSeparatorCssClass);
                    output.RenderBeginTag(HtmlTextWriterTag.Span);
                }

                //Draw separator image
                if (this.PagerStyleSettings.NextPageSeparatorText != string.Empty)
                    output.AddAttribute(HtmlTextWriterAttribute.Alt, this.PagerStyleSettings.NextPageSeparatorText);
                else
                    output.AddAttribute(HtmlTextWriterAttribute.Alt, "");
                output.AddAttribute(HtmlTextWriterAttribute.Src, this.PagerStyleSettings.NextPageSeparatorImageUrl);
                output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                output.RenderBeginTag(HtmlTextWriterTag.Img);
                output.RenderEndTag();

                if (this.PagerStyleSettings.NextPageSeparatorCssClass != string.Empty)
                    output.RenderEndTag();
            }
            else
            {
                //The next button separator is only text
                if (this.PagerStyleSettings.NextPageSeparatorText != string.Empty)
                {
                    //If there is a class, draw a span tag, otherwise, just spit it out.
                    if (this.PagerStyleSettings.NextPageSeparatorCssClass != string.Empty)
                    {
                        output.AddAttribute(HtmlTextWriterAttribute.Class, this.PagerStyleSettings.NextPageSeparatorCssClass);
                        output.RenderBeginTag(HtmlTextWriterTag.Span);
                    }

                    output.Write(this.PagerStyleSettings.NextPageSeparatorText);

                    if (this.PagerStyleSettings.NextPageSeparatorCssClass != string.Empty)
                        output.RenderEndTag();
                }
            }

            if (!string.IsNullOrEmpty(this.PagerStyleSettings.NextPageImageUrl))
            {
                RenderImageButton(
                    output,
                    this.PagerStyleSettings.NextPageImageUrl,
                    this.PagerStyleSettings.NextPageText,
                    CurrentPage + 1,
                    "nextBtn",
                    this.PagerStyleSettings.NextPageCssClass);
            }
            else
            {
                RenderTextButton(
                    output,
                    this.PagerStyleSettings.NextPageText,
                    CurrentPage + 1,
                    "nextBtn",
                    this.PagerStyleSettings.NextPageCssClass);
            }
        }

        private void RenderPageNumbers(HtmlTextWriter output, int startIndex, int endIndex)
        {
            //Loop through page numbers and draw page links
            for (int pageNumber = startIndex; pageNumber <= endIndex; pageNumber++)
            {
                if (pageNumber > startIndex)
                {
                    //Draw the separator between items
                    if (this.PagerStyleSettings.PageSeparatorImageUrl != string.Empty)
                    {
                        //If there is a class, draw a span tag, otherwise, just spit it out.
                        if (this.PagerStyleSettings.PageSeparatorCssClass != string.Empty)
                        {
                            output.AddAttribute(HtmlTextWriterAttribute.Class, this.PagerStyleSettings.PageSeparatorCssClass);
                            output.RenderBeginTag(HtmlTextWriterTag.Span);
                        }

                        //Draw separator image
                        if (this.PagerStyleSettings.PageSeparatorText != string.Empty)
                            output.AddAttribute(HtmlTextWriterAttribute.Alt, this.PagerStyleSettings.PageSeparatorText);
                        else
                            output.AddAttribute(HtmlTextWriterAttribute.Alt, "");
                        output.AddAttribute(HtmlTextWriterAttribute.Src, this.PagerStyleSettings.PageSeparatorImageUrl);
                        output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                        output.RenderBeginTag(HtmlTextWriterTag.Img);
                        output.RenderEndTag();

                        if (this.PagerStyleSettings.PageSeparatorCssClass != string.Empty)
                            output.RenderEndTag();
                    }
                    else
                    {
                        //The next button separator is only text
                        if (this.PagerStyleSettings.PageSeparatorText != string.Empty)
                        {
                            //If there is a class, draw a span tag, otherwise, just spit it out.
                            if (this.PagerStyleSettings.PageSeparatorCssClass != string.Empty)
                            {
                                output.AddAttribute(HtmlTextWriterAttribute.Class, this.PagerStyleSettings.PageSeparatorCssClass);
                                output.RenderBeginTag(HtmlTextWriterTag.Span);
                            }

                            output.Write(this.PagerStyleSettings.PageSeparatorText);

                            if (this.PagerStyleSettings.PageSeparatorCssClass != string.Empty)
                                output.RenderEndTag();
                        }
                    }
                }

                if (this.CurrentPage != pageNumber)
                {
                    RenderTextButton(
                        output,
                        pageNumber.ToString(),
                        pageNumber,
                        pageNumber.ToString(),
                        GetItemCssClass(this.PagerStyleSettings.IndexLinkCssClass, startIndex, endIndex, pageNumber));
                }
                else
                {
                    string cssClass = GetItemCssClass(this.PagerStyleSettings.SelectedIndexCssClass, startIndex, endIndex, pageNumber);
                    //CurrentPageNumber
                    if (cssClass != string.Empty)
                        output.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
                    output.RenderBeginTag(HtmlTextWriterTag.Span);
                    output.Write(pageNumber.ToString());
                    output.RenderEndTag();
                }
            }
        }

        private void RenderTextButton(HtmlTextWriter output, string buttonText, int pageNum, string buttonID, string className)
        {
            if (!string.IsNullOrEmpty(className))
                output.AddAttribute(HtmlTextWriterAttribute.Class, className);
            output.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + this.ClientIDSeparator + buttonID);
            output.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID + this.IdSeparator + pageNum.ToString());
            output.AddAttribute(HtmlTextWriterAttribute.Value, buttonText);
            output.AddAttribute(HtmlTextWriterAttribute.Type, "submit");
            output.RenderBeginTag(HtmlTextWriterTag.Input);
            output.RenderEndTag();
        }

        private void RenderImageButton(HtmlTextWriter output, string imgSrc, string altText, int pageNum, string buttonID, string className)
        {
            if (!string.IsNullOrEmpty(className))
                output.AddAttribute(HtmlTextWriterAttribute.Class, className);

            output.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID + this.ClientIDSeparator + buttonID);
            output.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID + this.IdSeparator + pageNum.ToString());
            output.AddAttribute(HtmlTextWriterAttribute.Alt, altText);
            output.AddAttribute(HtmlTextWriterAttribute.Src, imgSrc);
            output.AddAttribute(HtmlTextWriterAttribute.Type, "image");
            output.RenderBeginTag(HtmlTextWriterTag.Input);
            output.RenderEndTag();
        }

        private string GetItemCssClass(string itemClass, int startIndex, int endIndex, int pageNumber)
        {
            //Draw classes
            string className = string.Empty;

            if (itemClass != string.Empty)
                className = itemClass;

            if ((pageNumber == startIndex) && (this.PagerStyleSettings.FirstIndexCssClass != string.Empty))
                if (className != string.Empty)
                    className += " " + this.PagerStyleSettings.FirstIndexCssClass;
                else
                    className = this.PagerStyleSettings.FirstIndexCssClass;

            if ((pageNumber == endIndex) && (this.PagerStyleSettings.LastIndexCssClass != string.Empty))
                if (className != string.Empty)
                    className += " " + this.PagerStyleSettings.LastIndexCssClass;
                else
                    className = this.PagerStyleSettings.LastIndexCssClass;

            return className;
        }


        #endregion


        #region Static Helper Methods

        /// <summary>
        /// Calculates the first record number and the last record number.
        /// </summary>
        /// <param name="pageNum">The current page number.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="totalNumResults">The total number of results.</param>
        /// <param name="firstItem">Out parameter of the first item index.</param>
        /// <param name="lastItem">Out parameter of the last item index.</param>
        public static void GetFirstItemLastItem(int pageNum, int pageSize, int totalNumResults, out int firstItem, out int lastItem)
        {
            firstItem = 0;
            lastItem = 0;

            if (totalNumResults != 0)
            {
                int offSet = CalculateOffset(pageNum, pageSize);

                firstItem = offSet + 1;
                if (totalNumResults < (pageSize * pageNum))
                    lastItem = totalNumResults;
                else
                    lastItem = pageSize * pageNum;
            }
        }

        /// <summary>
        /// Calculates the offset for the first record on the specified page number.
        /// </summary>
        /// <param name="pageNumber">Page number to calculate the offset for (one-based)</param>
        /// <param name="recordsPerPage">The number of records to display on each page</param>
        /// <returns></returns>
        public static int CalculateOffset(int pageNumber, int recordsPerPage)
        {
            int offset = 0;
            if (pageNumber > 0)
                offset = (pageNumber - 1) * recordsPerPage;
            return offset;
        }

        #endregion

        protected override object SaveViewState()
        {
            object obj1 = base.SaveViewState();
            object obj2 = (this._pagerStyleSettings != null) ? ((IStateManager)this._pagerStyleSettings).SaveViewState() : null;

            return new object[] { obj1, obj2 };
        }

        /// <summary>
        /// Loads the previously saved view state of the PostBackButtonPager control.
        /// </summary>
        /// <param name="savedState">An Object that contains the saved view state values for the control.</param>
        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] objArray = (object[])savedState;
                base.LoadViewState(objArray[0]);
                if (objArray[1] != null)
                {
                    ((IStateManager)this.PagerStyleSettings).LoadViewState(objArray[1]);
                }
            }
        }

        /// <summary>
        /// Tracks view-state changes to the PostBackButtonPager control so they can be stored in the control's StateBag object. This object is accessible through the ViewState property.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();
            if (this._pagerStyleSettings != null)
            {
                ((IStateManager)this._pagerStyleSettings).TrackViewState();
            }
        }

        #region IPostBackDataHandler Members

        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            string buttonID = string.Empty;

            foreach (string key in postCollection.Keys)
            {
                if (key.IndexOf(postDataKey + IdSeparator) != -1)
                {
                    //There should be no key that ever matches uniqueid + IdSeparator, otherwise uniqueid would not be unique
                    buttonID = key;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(buttonID))
            {
                string strPageNum = buttonID.Replace(postDataKey + IdSeparator, string.Empty);

                if (strPageNum.IndexOf(".x") != -1)
                    strPageNum = strPageNum.Replace(".x", string.Empty);

                int pageNum = Int32.Parse(strPageNum);

                if (CurrentPage != pageNum)
                {
                    CurrentPage = pageNum;
                    return true;
                }
            }

            return false;
        }

        public void RaisePostDataChangedEvent()
        {
            OnPageChanged(EventArgs.Empty);
        }

        #endregion
    }
}
