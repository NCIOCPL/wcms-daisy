using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.Util;

namespace NCI.Web.UI.WebControls
{
    public class JSObjectIntProperty : JSObjectItem
    {
        private int _value = -1;

        /// <summary>
        /// Gets or sets the value of this JSObjectIntProperty
        /// </summary>
        /// <value>The value of the property</value>
        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Gets and sets the value of this JSObjectIntProperty with a string.
        /// </summary>
        /// <value></value>
        public override string StringValue
        {
            get
            {
                return Value.ToString();
            }
            set
            {
                Value = Strings.ToInt(value);
            }
        }

        /// <summary>
        /// Gets the Value Type of this JSObjectProperty
        /// </summary>
        /// <value></value>
        public override JSObjectItemType ValueType
        {
            get { return JSObjectItemType.Int; }
        }

        public JSObjectIntProperty() : base() { }

        public JSObjectIntProperty(string propertyName) : base(propertyName) { }

        public JSObjectIntProperty(string propertyName, int value)
            : base(propertyName)
        {
            _value = value;
        }       

    }
}
