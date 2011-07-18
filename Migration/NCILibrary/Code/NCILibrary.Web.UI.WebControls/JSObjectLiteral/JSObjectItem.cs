using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.UI.WebControls
{
    public enum JSObjectItemType
    {
        String = 1,
        Int = 2,
        Bool = 3,
        Reference = 4,
        ObjectLiteral = 5,
        Method = 6,
        Float = 7   
    }

    public abstract class JSObjectItem
    {
        private string _name = string.Empty;

        /// <summary>
        /// Gets or sets the name of this property or method
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Determines if the StringValue should be quoted when the ToString() method is called.
        /// </summary>
        protected virtual bool ShouldValueBeQuoted
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets and sets the value of this JSObjectProperty with a string.
        /// </summary>
        public abstract string StringValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the Value Type of this JSObjectProperty
        /// </summary>
        public abstract JSObjectItemType ValueType { get; }

        public JSObjectItem() { }

        public JSObjectItem(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Outputs this property or method to a string for embedding in client script.
        /// </summary>
        /// <returns></returns>
        public virtual string ToString()
        {
            if (ShouldValueBeQuoted)
                return _name + " : '" + StringValue + "'";
            else
                return _name + " : " + StringValue;
        }

        /// <summary>
        /// Creates a new JSObjectItem of the type specified by <em>type</em>.
        /// </summary>
        /// <param name="propertyName">The name of the property to be created.</param>
        /// <param name="value">The value of the property.</param>
        /// <param name="type">The type of the property.</param>
        /// <returns>
        /// A JSObjectItem of the type specified by <em>type</em>.
        /// </returns>
        /// <remarks>
        /// This method cannot be used to create methods or ObjectLiteral properties.
        /// </remarks>
        /// <exception cref="System.ArgumentException">
        /// This is thrown if the <em>type</em> is Method or ObjectLiteral.
        /// </exception>
        public static JSObjectItem CreateProperty(string propertyName, string value, JSObjectItemType type)
        {
            JSObjectItem item = null;

            switch (type)
            {
                case JSObjectItemType.Bool:
                    item = (JSObjectItem) new JSObjectBoolProperty(propertyName);
                    break;
                case JSObjectItemType.Float :
                    item = (JSObjectItem) new JSObjectFloatProperty(propertyName);
                    break;
                case JSObjectItemType.Reference:
                    item = (JSObjectItem) new JSObjectReferenceProperty(propertyName);
                    break;
                case JSObjectItemType.Int :
                    item = (JSObjectItem) new JSObjectIntProperty(propertyName);
                    break;
                case JSObjectItemType.String :
                    item = (JSObjectItem) new JSObjectStringProperty(propertyName);
                    break;
                case JSObjectItemType.ObjectLiteral :
                    throw new ArgumentException("Property type ObjectLiteral cannot be created by this function.");
                case JSObjectItemType.Method:
                    throw new ArgumentException("Methods cannot be created by this function.");

            }

            item.StringValue = value;
            return item;
        }
    }
}
