using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.Util;

namespace NCI.Web.UI.WebControls
{
    public class JSObjectFloatProperty : JSObjectItem
    {
        private float _value = 0;

        /// <summary>
        /// Gets or sets the value of this JSObjectFloatProperty
        /// </summary>
        /// <value>The value of the property</value>
        public float Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Gets and sets the value of this JSObjectStringProperty with a string.
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
                Value = Strings.ToFloat(value, 0);
            }
        }

        /// <summary>
        /// Gets the Value Type of this JSObjectProperty
        /// </summary>
        /// <value></value>
        public override JSObjectItemType ValueType
        {
            get { return JSObjectItemType.Float; }
        }

        public JSObjectFloatProperty() : base() { }

        public JSObjectFloatProperty(string propertyName) : base(propertyName) { }

        public JSObjectFloatProperty(string propertyName, float value)
            : base(propertyName)
        {
            _value = value;
        }

    }
}
