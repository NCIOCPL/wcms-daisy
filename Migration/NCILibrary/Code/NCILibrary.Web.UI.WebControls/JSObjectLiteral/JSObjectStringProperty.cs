using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.UI.WebControls
{
    public class JSObjectStringProperty : JSObjectItem
    {
        private string _value = string.Empty;

        /// <summary>
        /// Gets or sets the value of this JSObjectStringProperty
        /// </summary>
        /// <value>The value of the property</value>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Determines if the StringValue should be quoted when the ToString() method is called.
        /// </summary>
        /// <value></value>
        protected override bool ShouldValueBeQuoted
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets and sets the value of this JSObjectStringProperty with a string.
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
            get { return JSObjectItemType.String; }
        }

        public JSObjectStringProperty() : base() { }

        public JSObjectStringProperty(string propertyName) : base(propertyName) { }

        public JSObjectStringProperty(string propertyName, string value)
            : base(propertyName)
        {
            _value = value;
        }

    }
}
