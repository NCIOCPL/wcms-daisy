using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls
{    
    [ToolboxData("<{0}:SimplePager runat=server></{0}:SimplePager>")]
    public class SimplePager : WebControl
    {
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
        /// Gets and Sets the url to append to.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("/")]
        [Localizable(true)]
        public string BaseUrl
        {
            get {
                string s = (string)ViewState["BaseUrl"];
                return (s == null) ? "/" : s; 
            }
            set { ViewState["BaseUrl"] = value; }
        }

        /// <summary>
        /// Gets and Sets the query parameter name for the OffSet Parameter.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("Offset")]
        [Localizable(true)]
        public string OffSetParamName
        {
            get
            {
                string s = (string)ViewState["OffSetParamName"];
                return (s == null) ? "Offset" : s;
            }
            set { ViewState["OffSetParamName"] = value; }
        }

        /// <summary>
        /// Gets and Sets the query parameter name for the Page Parameter.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("PageNum")]
        [Localizable(true)]
        public string PageParamName
        {
            get
            {
                string s = (string)ViewState["PageParamName"];
                return (s == null) ? "PageNum" : s;
            }
            set { ViewState["PageParamName"] = value; }
        }

        /// <summary>
        /// Gets and Sets the query parameter name for the Page Parameter.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("RecordsPerPage")]
        [Localizable(true)]
        public string RecordsPerPageParamName
        {
            get
            {
                string s = (string)ViewState["RecordsPerPageParamName"];
                return (s == null) ? "RecordsPerPage" : s;
            }
            set { ViewState["RecordsPerPageParamName"] = value; }
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

        protected virtual string GetItemUrl(int offSet, int pageNumber)
        {
            //So yea, in the future we could pass in List<Pair>
            //where Pair.x = Param Name
            //and Pair.y = Param value
            //and then iterate through the list.

            StringBuilder url = new StringBuilder();

            url.Append(this.BaseUrl);

            if (this.BaseUrl.IndexOf("?") == -1)
            {
                url.Append("?");
            }
            else
            {
                url.Append("&");
            }

            url.Append(this.PageParamName);
            url.Append("=");
            url.Append(pageNumber);
            url.Append("&");
            url.Append(this.RecordsPerPageParamName);
            url.Append("=");
            url.Append(this.RecordsPerPage);
            url.Append("&");
            url.Append(this.OffSetParamName);
            url.Append("=");
            url.Append(offSet);

            return url.ToString();
        }

        protected virtual void RenderPrevLink(HtmlTextWriter output)
        {
            //If there is no image, and there is not text, then there is nothing to write.
            if (this.PagerStyleSettings.PrevPageText != string.Empty || this.PagerStyleSettings.PrevPageImageUrl != string.Empty)
            {
                //Get the url for the link
                int offSet = ((this.CurrentPage - 2) * this.RecordsPerPage); //Prettymuch everything is 0 based offset
                string url = GetItemUrl(offSet, this.CurrentPage - 1);

                //Begin writing the previous link
                if (this.PagerStyleSettings.PrevPageCssClass != string.Empty)
                    output.AddAttribute(HtmlTextWriterAttribute.Class, this.PagerStyleSettings.PrevPageCssClass);
                output.AddAttribute(HtmlTextWriterAttribute.Href, url);
                output.RenderBeginTag(HtmlTextWriterTag.A);

                //Draw either the text or the image
                if (this.PagerStyleSettings.PrevPageImageUrl != string.Empty)
                {
                    if (this.PagerStyleSettings.PrevPageText != string.Empty)
                        output.AddAttribute(HtmlTextWriterAttribute.Alt, this.PagerStyleSettings.PrevPageText);
                    else
                        output.AddAttribute(HtmlTextWriterAttribute.Alt, "");
                    output.AddAttribute(HtmlTextWriterAttribute.Src, this.PagerStyleSettings.PrevPageImageUrl);
                    output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                    output.RenderBeginTag(HtmlTextWriterTag.Img);
                    output.RenderEndTag();
                }
                else
                {
                    output.Write(this.PagerStyleSettings.PrevPageText);
                }
                output.RenderEndTag();

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
        }

        protected virtual void RenderNextLink(HtmlTextWriter output)
        {
            //If there is no image, and there is not text, then there is nothing to write.
            if (this.PagerStyleSettings.NextPageText != string.Empty || this.PagerStyleSettings.NextPageImageUrl != string.Empty)
            {
                //Get the url for the link
                int offSet = ((this.CurrentPage) * this.RecordsPerPage);
                string url = GetItemUrl(offSet, this.CurrentPage + 1);

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

                //Begin writing the next link
                if (this.PagerStyleSettings.NextPageCssClass != string.Empty)
                    output.AddAttribute(HtmlTextWriterAttribute.Class, this.PagerStyleSettings.NextPageCssClass);
                output.AddAttribute(HtmlTextWriterAttribute.Href, url);
                output.RenderBeginTag(HtmlTextWriterTag.A);

                //Draw either the text or the image
                if (this.PagerStyleSettings.NextPageImageUrl != string.Empty)
                {
                    if (this.PagerStyleSettings.NextPageText != string.Empty)
                        output.AddAttribute(HtmlTextWriterAttribute.Alt, this.PagerStyleSettings.NextPageText);
                    else
                        output.AddAttribute(HtmlTextWriterAttribute.Alt, "");
                    output.AddAttribute(HtmlTextWriterAttribute.Src, this.PagerStyleSettings.NextPageImageUrl);
                    output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                    output.RenderBeginTag(HtmlTextWriterTag.Img);
                    output.RenderEndTag();
                }
                else
                {
                    output.Write(this.PagerStyleSettings.NextPageText);
                }
                output.RenderEndTag();

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
                    int offSet = ((pageNumber - 1) * this.RecordsPerPage);
                    string url = GetItemUrl(offSet, pageNumber);

                    string cssClass = GetItemCssClass(this.PagerStyleSettings.IndexLinkCssClass, startIndex, endIndex, pageNumber);

                    if (cssClass != string.Empty)
                        output.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
                    output.AddAttribute(HtmlTextWriterAttribute.Href, url);
                    output.RenderBeginTag(HtmlTextWriterTag.A);
                    output.Write(pageNumber.ToString());
                    output.RenderEndTag();                    
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

        protected override object SaveViewState()
        {
            object obj1 = base.SaveViewState();
            object obj2 = (this._pagerStyleSettings != null) ? ((IStateManager)this._pagerStyleSettings).SaveViewState() : null;

            return new object[] { obj1, obj2};
        }

        /// <summary>
        /// Loads the previously saved view state of the SimplePager control.
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
        /// Tracks view-state changes to the SimplePager control so they can be stored in the control's StateBag object. This object is accessible through the ViewState property.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();
            if (this._pagerStyleSettings != null)
            {
                ((IStateManager)this._pagerStyleSettings).TrackViewState();
            }
        }
    }
}

