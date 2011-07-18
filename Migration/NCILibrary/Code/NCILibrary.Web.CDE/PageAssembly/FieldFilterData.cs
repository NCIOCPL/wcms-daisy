using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE
{
    /// <summary>
    /// Field Fiter Data is the actual value/data for the field filter.
    /// 
    /// </summary>
    public class FieldFilterData
    {
        private string _value = string.Empty;

        /// <summary>
        /// Gets or sets the value for the field filter.
        /// </summary>
        /// <value>The value.</value>
        public string Value {
            get { return _value; }
            set { _value = value; } 
        }
        
    }
}
