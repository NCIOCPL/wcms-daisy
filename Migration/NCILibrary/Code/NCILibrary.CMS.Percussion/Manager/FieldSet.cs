using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.CMS.Percussion.Manager.CMS
{
    /// <summary>
    /// Represents an optionally named set of Key/Value pairs.
    /// </summary>
    public class FieldSet : Dictionary<string, string>
    {
        public FieldSet() :
            base()
        {
        }

        public FieldSet(IDictionary<string, string> fields) :
            base(fields)
        {
        }
    }
}
