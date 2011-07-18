using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.Modules;
using System.Configuration;

namespace NCI.Web.CDE.UI.Modules
{
    [DefaultProperty("Title")]
    [ToolboxData("<{0}:SearchControl runat=server></{0}:SearchControl>")]
    public class SearchBox : SnippetControl
    {
        #region Public

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Title
        {
            get
            {
                String s = (String)ViewState["Title"];
                return ((s == null) ? "[" + this.ID + "]" : s);
            }

            set
            {
                ViewState["Title"] = value;
            }
        }

        [DefaultValue(false)]
        public string SearchType
        {
            get
            {
                if (ViewState["SearchType"] == null)
                    ViewState["SearchType"] = null;
                return (string)ViewState["SearchType"];
            }
            set
            {
                ViewState["SearchType"] = value;
            }
        }

        /// <summary>
        /// The url that will be set for the action attribute of the html form. The data is posted
        /// back to this url when go button is clicked.
        /// </summary>
        public string ActionURL
        {
            get
            {
                return (String)ViewState["ActionURL"];
            }

            set
            {
                ViewState["ActionURL"] = value;
            }
        }

        /// <summary>
        /// The client side Web Analytics function for OnSubmit event.
        /// </summary>
        public string WebAnalyticsFunction
        {
            get
            {
                return (String)ViewState["wbFunction"];
            }

            set
            {
                ViewState["wbFunction"] = value;
            }
        }


        #endregion

        public void Page_Load(object sender, EventArgs e)
        {
            SearchBoxSettings searchBoxSettings = ModuleObjectFactory<SearchBoxSettings>.GetModuleObject(SnippetInfo.Data);
            if (searchBoxSettings != null)
            {
                this.Title = searchBoxSettings.Title;
                this.ActionURL = searchBoxSettings.ActionUrl;
                this.WebAnalyticsFunction = searchBoxSettings.WebAnalyticsFunction;
                this.SearchType = searchBoxSettings.SearchType;
            }
        }

        public override void RenderControl(HtmlTextWriter output)
        {
            output.Write(@"<table width=""164"" class=""gray-border"" border=""0"" cellSpacing=""0"" cellPadding=""1""><TBODY><TR><TD vAlign=top><TABLE border=0 cellSpacing=0 cellPadding=0 width=162 bgColor=#ffffff><TBODY>
                <TR><TD class=box-title vAlign=top align=left><IMG border=0 alt="""" src=""/images/spacer.gif"" width=7 height=17></TD>");
            output.Write(string.Format(@"<TD class=box-title vAlign=left colSpan=2>{0}</TD></TR>", Title));

            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es" && SearchType.ToLower() == "keyword")
            {
                output.Write(@"<tr><td valign=""top"" colspan=""3"" class=""gray-border""><img src=""/images/spacer.gif"" width=""1"" height=""1"" alt="""" border=""0""></td>
            </tr><tr><td valign=""top"" align=""left"" colspan=""3""><img src=""/images/spacer.gif""  alt="""" width=""162"" height=""8"" alt="""" border=""0""></td>
            </tr><tr><td valign=""top"" align=""left""><img src=""/images/spacer.gif"" width=""7"" height=""1"" alt="""" border=""0""></td>
               <td valign=""top"" class=""leftnav"" align=""left"" width=""148"" >");
            }
            else
            {
                output.Write(@"<TR><TD class=gray-border vAlign=top colSpan=3><IMG border=0 alt="""" src=""/images/spacer.gif"" width=1 height=1></TD></TR>
                <TR><TD vAlign=top colSpan=3 align=left><IMG border=0 alt="""" src=""/images/spacer.gif"" width=162 height=8></TD></TR>
                <TR><TD vAlign=top align=left><IMG border=0 alt="""" src=""/images/spacer.gif"" width=7 height=1></TD>
                <TD class=leftnav vAlign=top width=148 align=left></TR>
                <TR><TD vAlign=top colSpan=3><IMG border=0 alt="""" src=""/images/spacer.gif"" width=1 height=5><BR></TD>");
            }
            if (!string.IsNullOrEmpty(WebAnalyticsFunction))
                WebAnalyticsFunction = string.Format("onsubmit=\"{0}\"", WebAnalyticsFunction);
            else
                WebAnalyticsFunction = "";

            output.Write(string.Format("<FORM {0} method=post name=NCSearchBox action={1}>", WebAnalyticsFunction, ActionURL));

            if (SearchType.ToLower() == "keyword_with_date")
            {

                output.Write(@"<TR><TD vAlign=top>&nbsp;</TD><TD vAlign=top>Search For:</TD><TD vAlign=top>&nbsp;</TD></TR><TR>
                <TD vAlign=top>&nbsp;</TD><TD vAlign=top><LABEL class=hidden for=keyword>keyword</LABEL><INPUT id=keyword class=search-field size=8 name=keyword></TD>
                <TD vAlign=top>&nbsp;</TD></TR><TR><TD vAlign=top colSpan=3><IMG border=0 alt="""" src=""/images/spacer.gif"" width=1 height=8></TD></TR>
                <TR><TD vAlign=top>&nbsp;</TD>");

                output.Write(string.Format(@"<TD vAlign=top>Between these dates:<BR><IMG border=0 alt="""" src=""/images/spacer.gif"" width=1 height=5><BR></TD>
                    <TD vAlign=top>&nbsp;</TD></TR><TR>
                    <TD vAlign=top>&nbsp;</TD><TD vAlign=top><LABEL class=hidden for=startMonth>select start Month</LABEL> <SELECT id=startMonth name=startMonth> <OPTION selected value=1>Jan.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</OPTION> <OPTION value=2>Feb.</OPTION> <OPTION value=3>Mar.</OPTION> <OPTION value=4>Apr.</OPTION> <OPTION value=5>May</OPTION> <OPTION value=6>Jun.</OPTION> <OPTION value=7>Jul.</OPTION> <OPTION value=8>Aug.</OPTION> <OPTION value=9>Sept.</OPTION> <OPTION value=10>Oct.</OPTION> <OPTION value=11>Nov.</OPTION> <OPTION value=12>Dec.</OPTION></SELECT> &nbsp;&nbsp;<LABEL class=hidden for=startYear>select start Year</LABEL> <SELECT id=startYear name=startYear>{0}</SELECT> </TD>
                    <TD vAlign=top>&nbsp;</TD></TR><TR><TD vAlign=top colSpan=3><IMG border=0 alt="""" src=""/images/spacer.gif"" width=1 height=3></TD></TR> 
                    <TR><TD vAlign=top>&nbsp;</TD>
                    <TD vAlign=top><LABEL class=hidden for=endMonth>select end Month</LABEL><SELECT id=endMonth name=endMonth> {1} </SELECT> &nbsp;&nbsp;<LABEL class=hidden for=endYear>select end Year</LABEL> <SELECT id=endYear name=endYear>{2}</SELECT> </TD>
                    <TD vAlign=top>&nbsp;</TD></TR>
                    <TR><TD vAlign=top colSpan=3><IMG border=0 alt=""spacer image"" src=""/images/spacer.gif"" width=1 height=10></TD></TR>", GetYearListItems("startYear"), GetMonthListItems("endMonth"), GetYearListItems("endYear")));

                if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
                {
                    output.Write(@"<TR><TD vAlign=top>&nbsp;</TD>
                    <TD vAlign=top align=left><INPUT alt=Search src=""/images/red_buscar_button.gif"" type=image>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
                    <TD vAlign=top>&nbsp;</TD></TR></FORM></TD>");
                }

                else
                {
                    output.Write(@"<TR><TD vAlign=top>&nbsp;</TD>
                    <TD vAlign=top align=left><INPUT alt=Search src=""/images/red_go_button.gif"" type=image>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
                    <TD vAlign=top>&nbsp;</TD></TR></FORM></TD>");
                }

            }
            else
            {
                if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
                {
                    output.Write(@"<table width=""148"" cellspacing=""0"" cellpadding=""0"" border=""0""><tr>
                    <td valign=""top""><img src=""/images/spacer.gif"" width=""7"" height=""1"" alt="""" border=""0""></td>
                    <td valign=""middle"" width=""87"" align=""left"" height=""24""><label for=""keyword"">Palabra clave</label></td>
                    <td valign=""top""><img src=""/images/spacer.gif"" width=""5"" height=""1"" alt="""" border=""0""></td><td></td>
                    </tr><tr>
                    <td valign=""top""><img src=""/images/spacer.gif"" width=""7"" height=""1"" alt="""" border=""0""></td>
                    <td valign=""middle"" width=""87"" align=""left"" height=""24"">
                    <input type=""text"" id=""keyword"" name=""keyword"" style=""width:87px; height:21px;"" class=""search-field""></td>
                    <td valign=""top""><img src=""/images/spacer.gif"" width=""5"" height=""1"" alt="""" border=""0""></td>
                    <td><input type=""image"" src=""/images/buscar-left-nav.gif"" alt=""Buscar"" width=""50"" height=""15"" /></td></tr></table></Form>");

                    output.Write(@"</td><td valign=""top"" align=""right""><img src=""/images/spacer.gif"" width=""7"" height=""1"" alt="""" border=""0""></td></tr>");
                    output.Write(@"<TR><TD vAlign=top colSpan=3 align=left><IMG border=0 alt="""" src=""/images/spacer.gif"" width=162 height=8></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE>");
                    return;

                }
                else
                {

                    output.Write(@"<tr><td valign=""top"">&nbsp;</td><td valign=""center""><label class=""hidden"" for=""keyword"">keyword</label>
                    <input id=""keyword"" class=""search-field"" size=""10"" name=""keyword"">&nbsp;&nbsp;<input
                    alt=""Search"" src=""/images/red_go_button.gif"" type=""image""></td><td valign=""top"">&nbsp;</td></tr>
                <TR><TD vAlign=top colSpan=3><IMG border=0 alt="""" src=""/images/spacer.gif"" width=1 height=8></TD></TR></Form>");
                }

            }


            output.Write(@"<TR><TD vAlign=top align=right><IMG border=0 alt="""" src=""/images/spacer.gif"" width=7 height=1></TD></TR>
            <TR><TD vAlign=top colSpan=3 align=left><IMG border=0 alt="""" src=""/images/spacer.gif"" width=162 height=8></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE>");
        }

        private string GetMonthListItems(string monthType)
        {
            string html = String.Empty;
            string[] monthNameLookup = { "Jan.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "Feb.", "Mar.", "Apr.", "May", "Jun.", "Jul.", "Aug.", "Sept.", "Oct.", "Nov.", "Dec." };
            for (int i = 1; i <= 12; i++)
                html += string.Format("<option {0} value=\"{1}\">{2}</option>", i == DateTime.Now.Month ? "selected" : String.Empty, i, monthNameLookup[i - 1]);
            return html;
        }

        private string GetYearListItems(string yearType)
        {
            string html = String.Empty;
            int startYear = 1998;
            int yearsAgo = DateTime.Now.Year - startYear;
            while (startYear <= DateTime.Now.Year)
            {
                if ((string.Compare("startYear", yearType, true) == 0 && startYear == DateTime.Now.Year - yearsAgo) ||
                     (string.Compare("endYear", yearType, true) == 0 && startYear == DateTime.Now.Year))
                    html += string.Format("<option selected value=\"{0}\">{0}</option>", startYear.ToString());
                else
                    html += string.Format("<option value=\"{0}\">{0}</option>", startYear.ToString());
                startYear++;
            }

            return html;
        }
    }
}
