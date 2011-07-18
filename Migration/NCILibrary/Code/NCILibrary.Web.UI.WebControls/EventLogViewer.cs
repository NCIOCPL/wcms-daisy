using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:EventLogViewer runat=server></{0}:EventLogViewer>")]
    public class EventLogViewer : WebControl
    {
        string _logSource = string.Empty;
        string _logFile = string.Empty;
        string _machineName = ".";

        public string Source
        {
            get
            {
                String s = _logSource;
                return ((s == null) ? String.Empty : s);

            }

            set
            {
                _logSource = value;

            }
        }

        public string MachineName
        {
            get
            {
                String s = _machineName;
                return ((s == null) ? String.Empty : s);

            }

            set
            {
                _machineName = value;

            }
        }

        public string Log
        {
            get
            {
                String s = _logFile;
                return ((s == null) ? String.Empty : s);

            }

            set
            {
                _logFile = value;

            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            string errorMessage = null;
            int rowCounter = -1;

            //render the header of data grid
            DrawGridHeader(output);

            try
            {
                using (EventLog log = new EventLog())
                {
                    if (string.IsNullOrEmpty(_logSource))
                    {
                        //errorMessage = "Log Source is not Set!";
                        _logSource = "AllEntries";
                    }
                    if (string.IsNullOrEmpty(_logFile))
                    {
                        errorMessage = "The event log name is not set!";
                    }
                    else
                    {
                        log.Log = _logFile;
                        log.MachineName = _machineName;
                        if (log.Entries.Count > 0)
                        {
                            EventLogEntry[] arrEntries = new EventLogEntry [log.Entries.Count];
                            log.Entries.CopyTo(arrEntries, 0);
                            //Array.Reverse(arrEntries);
                            //Sort func with IComparer interface is more general method
                            Array.Sort(arrEntries, SortByDateDesc);
                            if (_logSource == "AllEntries")
                            {
                                foreach (EventLogEntry entry in arrEntries)
                                {
                                    DrawEntryRow(output, entry, ref rowCounter);
                                }
                            }
                            else
                            {
                                foreach (EventLogEntry entry in arrEntries)
                                {
                                    if (entry.Source.ToLower() == _logSource.ToLower())
                                    {
                                        DrawEntryRow(output, entry, ref rowCounter);
                                    }
                                }
                            }
                            output.RenderEndTag();
                        }
                        else
                        {
                            errorMessage = "No Log Entries!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            if (errorMessage != null)
            {
                DrawErrorMessage(output, errorMessage);
            }
        }

        private static void DrawGridHeader(HtmlTextWriter output)
        {
            output.AddAttribute("cellspacing", "0");
            output.AddAttribute("border", "1");
            output.AddAttribute("rules", "cols");
            output.AddAttribute("id", "GridView1");
            output.AddAttribute("style", "border-color:White;border-style:None;width:100%;border-collapse:collapse;font-size:8pt;fontfamily=arial");
            output.RenderBeginTag(HtmlTextWriterTag.Table);

            output.AddAttribute("style", "background-color:#E9E9E9;");
            output.RenderBeginTag(HtmlTextWriterTag.Tr);
            output.AddAttribute("scope", "col");
            output.AddAttribute("align", "center");
            output.AddAttribute("width", "30px;");
            output.RenderBeginTag(HtmlTextWriterTag.Th);
            output.Write("Type");
            output.RenderEndTag();
            output.AddAttribute("scope", "col");
            output.AddAttribute("align", "center");
            output.AddAttribute("width", "70px;");
            output.RenderBeginTag(HtmlTextWriterTag.Th);
            output.Write("Date");
            output.RenderEndTag();
            output.AddAttribute("scope", "col");
            output.AddAttribute("align", "center");
            output.AddAttribute("width", "60px;");
            output.RenderBeginTag(HtmlTextWriterTag.Th);
            output.Write("Time");
            output.RenderEndTag();
            output.AddAttribute("scope", "col");
            output.AddAttribute("align", "center");
            output.AddAttribute("width", "100px;");
            output.RenderBeginTag(HtmlTextWriterTag.Th);
            output.Write("Computer");
            output.RenderEndTag();
            output.AddAttribute("scope", "col");
            output.AddAttribute("align", "center");
            output.AddAttribute("width", "90px;");
            output.RenderBeginTag(HtmlTextWriterTag.Th);
            output.Write("Source");
            output.RenderEndTag();
            output.AddAttribute("scope", "col");
            output.AddAttribute("align", "center");
            output.RenderBeginTag(HtmlTextWriterTag.Th);
            output.Write("Description");
            output.RenderEndTag();
            output.RenderEndTag();
        }

        private void DrawEntryRow(HtmlTextWriter output, EventLogEntry entry, ref int rowCounter)
        {
            rowCounter++;
            if (rowCounter % 2 != 0)
                output.AddAttribute("style", "background-color:#E8E8E7;");
            //output.AddStyleAttribute(HtmlTextWriterStyle.FontWeight,"lighter");
            output.RenderBeginTag(HtmlTextWriterTag.Tr);
            output.AddAttribute("align", "center");
            output.AddAttribute("style", "width:30px;");
            output.RenderBeginTag(HtmlTextWriterTag.Td);

            DrawIconForEntry(output, entry);

            output.RenderEndTag();
            output.AddAttribute("style", "width:70px;");
            output.RenderBeginTag(HtmlTextWriterTag.Td);
            output.Write(entry.TimeGenerated.Month + "/" + entry.TimeGenerated.Day + "/" + entry.TimeGenerated.Year);
            output.RenderEndTag();
            output.AddAttribute("style", "width:60px;");
            output.RenderBeginTag(HtmlTextWriterTag.Td);
            output.Write(entry.TimeGenerated.TimeOfDay.ToString());
            output.RenderEndTag();
            output.AddAttribute("style", "width:100px;");
            output.RenderBeginTag(HtmlTextWriterTag.Td);
            output.Write(entry.MachineName.ToString());
            output.RenderEndTag();
            output.AddAttribute("source", "width:90px;");
            output.RenderBeginTag(HtmlTextWriterTag.Td);
            output.Write(entry.Source.ToString());
            output.RenderEndTag();
            output.RenderBeginTag(HtmlTextWriterTag.Td);
            output.Write(entry.Message.ToString().Replace("\n", "<br \\>"));
            output.RenderEndTag();
            output.RenderEndTag();
        }

        private void DrawErrorMessage(HtmlTextWriter output, string errorMessage)
        {
            output.WriteBreak();
            output.WriteBreak();
            output.AddAttribute("width", "100%");
            output.AddAttribute("height", "100px");
            output.AddAttribute("style", "border-color:#BDBDBD;border-style:Solid;border=1px;height=50px;border-collapse:collapse;");
            output.RenderBeginTag(HtmlTextWriterTag.Table);
            output.RenderBeginTag(HtmlTextWriterTag.Tr);
            output.AddAttribute("align", "center");
            output.AddAttribute("valign", "center");
            output.RenderBeginTag(HtmlTextWriterTag.Td);

            //output.AddAttribute("hspace", "30px");
            output.AddAttribute("width", "350px");
            output.RenderBeginTag(HtmlTextWriterTag.Table);
            output.RenderBeginTag(HtmlTextWriterTag.Tr);
            output.AddAttribute("align", "left");
            output.RenderBeginTag(HtmlTextWriterTag.Td);
            output.Write(errorMessage);

            output.RenderEndTag();
            output.RenderEndTag();
            output.RenderEndTag();
            output.RenderEndTag();
            output.RenderEndTag();
            output.RenderEndTag();
        }

        private void DrawIconForEntry(HtmlTextWriter output, EventLogEntry entry)
        {
            string src = string.Empty;
            string id = string.Empty;
            string alt = string.Empty;

            switch (entry.EntryType)
            {
                case (EventLogEntryType.Information):
                    {
                        id = "infromationIcon";
                        src = Page.ClientScript.GetWebResourceUrl(
                            typeof(EventLogViewer),
                            "NCI.Web.UI.WebControls.Resources.EventLogViewer.information.gif"
                            );
                        alt = "Information";
                        break;
                    }
                case (EventLogEntryType.Error):
                    {
                        id = "errorIcon";
                        src = Page.ClientScript.GetWebResourceUrl(
                            typeof(EventLogViewer),
                            "NCI.Web.UI.WebControls.Resources.EventLogViewer.error.gif"
                            );
                        alt = "Error";
                        break;
                    }
                case (EventLogEntryType.Warning):
                    {
                        id = "warningIcon";
                        src = Page.ClientScript.GetWebResourceUrl(
                            typeof(EventLogViewer),
                            "NCI.Web.UI.WebControls.Resources.EventLogViewer.warning.gif"
                            );
                        alt = "Warning";
                        break;
                    }
                default:
                    {
                        id = "unknownIcon";
                        src = Page.ClientScript.GetWebResourceUrl(
                            typeof(EventLogViewer),
                            "NCI.Web.UI.WebControls.Resources.EventLogViewer.help.gif"
                            );
                        alt = "Unknown";
                        break;
                    }
            }

            output.AddAttribute("id", id);
            output.AddAttribute("src", src);
            output.AddAttribute("alt", alt);
            output.AddAttribute("height", "16");
            output.AddAttribute("width", "16");
            output.AddAttribute("style", "border-width:0px;");
            output.RenderBeginTag(HtmlTextWriterTag.Img);
            output.RenderEndTag();

        }

        #region Properties
        private IComparer<EventLogEntry> SortByDateDesc
        {
            get { return (IComparer<EventLogEntry>)new SortEntriesByDateDesc(); }
        }
        
        #endregion
    }

    public class SortEntriesByDateDesc : IComparer<EventLogEntry>
    {
        public SortEntriesByDateDesc() { }

        int IComparer<EventLogEntry>.Compare(EventLogEntry entry1, EventLogEntry entry2)
        {
            return DateTime.Compare(entry2.TimeGenerated, entry1.TimeGenerated);
        }
    }
}
