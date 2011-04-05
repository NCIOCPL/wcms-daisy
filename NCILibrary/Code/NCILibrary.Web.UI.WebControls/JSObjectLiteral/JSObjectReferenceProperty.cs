using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.UI.WebControls
{
    public class JSObjectReferenceProperty : JSObjectItem
    {
                private string _value = string.Empty;

        /// <summary>
        /// Gets or sets the value of this JSObjectReferenceProperty
        /// </summary>
        /// <value>The value of the property</value>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Gets and sets the value of this JSObjectReferenceProperty with a string.
        /// </summary>
        /// <value></value>
        public override string StringValue
        {
            get
            {
                return Value;
            }
            set
            {
                Value = value;
            }
        }

        /// <summary>
        /// Gets the Value Type of this JSObjectProperty
        /// </summary>
        /// <value></value>
        public override JSObjectItemType ValueType
        {
            get { return JSObjectItemType.Reference; }
        }

        public JSObjectReferenceProperty() : base() { }

        public JSObjectReferenceProperty(string propertyName) : base(propertyName) { }

        public JSObjectReferenceProperty(string propertyName, string value)
            : base(propertyName)
        {
            _value = value;
        }

    }
}
