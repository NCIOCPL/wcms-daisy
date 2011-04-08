using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.CMS.Percussion.Manager.CMS;

namespace Munger
{
    class NciFile
    {
        public string ContentType { get { return "nciFile"; } }

        private FieldSet _fieldSet = new FieldSet();
        public FieldSet FieldSet { get { return _fieldSet; } }

        public NciFile(NciFileInfo info)
        {
            _fieldSet.Add("long_title", info.Title);
            _fieldSet.Add("short_title", info.Title.Substring(0, Math.Min(100, info.Title.Length)));
            _fieldSet.Add("item_file_attachment", info.Data);
            _fieldSet.Add("item_file_attachment_ext", info.Extension);
            _fieldSet.Add("item_file_attachment_filename", info.FileName);
            _fieldSet.Add("item_file_attachment_size", info.FileSize.ToString());
            _fieldSet.Add("item_file_attachment_type", info.MimeType);
            _fieldSet.Add("pretty_url_name", info.Title.Substring(0, Math.Min(100, info.Title.Length)));
            _fieldSet.Add("filename", info.FileName);
            _fieldSet.Add("sys_suffix", info.Extension);
        }
    }
}
