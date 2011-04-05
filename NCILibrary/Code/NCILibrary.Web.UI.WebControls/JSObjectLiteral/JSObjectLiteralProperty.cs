using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// This is used when a JSObjectProperty is a ObjectLiteral.
    /// </summary>
    public class JSObjectLiteralProperty : JSObjectItem
    {        
        private JSObjectLiteral _value = new JSObjectLiteral();

        /// <summary>
        /// Gets or sets the value of this JSObjectLiteralProperty
        /// </summary>
        /// <value>The value of the property</value>
        public JSObjectLiteral Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Gets and sets the value of this JSObjectLiteralProperty with a string.
        /// </summary>
        /// <value></value>
        /// <exception cref="System.NotImplementedException">Currently you cannot set 
        /// a JSObjectLiteralProperty with a string.
        /// </exception>
        public override string StringValue
        {
            get
            {
                return Value.ToString();
            }
            set
            {
                throw new NotImplementedException("You cannot set a JSObjectLiteralProperty via a string.");
            }
        }

        /// <summary>
        /// Gets the Value Type of this JSObjectLiteralProperty
        /// </summary>
        /// <value></value>
        public override JSObjectItemType ValueType
        {
            get { return JSObjectItemType.ObjectLiteral; }
        }

        public JSObjectLiteralProperty() : base() { }

        public JSObjectLiteralProperty(string propertyName) : base(propertyName) { }

        public JSObjectLiteralProperty(string propertyName, JSObjectLiteral value)
            : this(propertyName)
        {
            this._value = value;
        }

        /// <summary>
        /// This works like the Prototype.js Object.Extend method.  What it does is it takes the properties
        /// and methods in <em>literal</em> and overrides any methods and properties in this 
        /// JSObjectLiteralProperty with the same name, and adds any new properties and methods that currently
        /// are not in this JSObjectLiteral.
        /// </summary>
        /// <param name="literal"></param>
        public void Extend(JSObjectLiteral literal)
        {
            this._value.Extend(literal);
        }
    }
}
