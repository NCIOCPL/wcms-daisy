using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.Util;

namespace NCI.Web.UI.WebControls
{
    public class JSObjectBoolProperty : JSObjectItem
    {
        private bool _value = false;

        /// <summary>
        /// Gets or sets the value of this JSObjectBoolProperty
        /// </summary>
        /// <value>The value of the property</value>
        public bool Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Gets and sets the value of this JSObjectBoolProperty with a string.
        /// </summary>
        /// <value></value>
        public override string StringValue
        {
            get
            {
                if (_value)
                    return "true";
                else
                    return "false";
            }
            set
            {
                Value = Strings.ToBoolean(value);
            }
        }

        /// <summary>
        /// Gets the Value Type of this JSObjectProperty
        /// </summary>
        /// <value></value>
        public override JSObjectItemType ValueType
        {
            get { return JSObjectItemType.Bool; }
        }

        public JSObjectBoolProperty() : base() { }

        public JSObjectBoolProperty(string propertyName) : base(propertyName) { }

        public JSObjectBoolProperty(string propertyName, bool value)
            : base(propertyName)
        {
            _value = value;
        }

    }
}
