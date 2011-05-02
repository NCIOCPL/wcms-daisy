using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.UI.WebControls
{
    public class JSObjectMethod : JSObjectItem
    {
        private List<string> _parameters = new List<string>();
        private string _methodBody;

        /// <summary>
        /// Gets the collection of input parameters for the method.
        /// </summary>
        public List<string> Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Gets or sets the method body.
        /// </summary>
        public string Body
        {
            get { return _methodBody; }
            set { _methodBody = value; }
        }

        #region Constructors

        public JSObjectMethod() { }

        public JSObjectMethod(string methodName) : base(methodName) { }

        public JSObjectMethod(string methodName, string methodBody)
            : this(methodName)
        {
            _methodBody = methodBody;
        }

        public JSObjectMethod(string methodName, string methodBody, string[] parameters)
            : this(methodName, methodBody)
        {
            _parameters.AddRange(parameters);
        }

        #endregion

        /// <summary>
        /// Gets and sets the value of this JSObjectMethod with a string.
        /// </summary>
        /// <value></value>
        public override string StringValue
        {
            get
            {
                return Body;
            }
            set
            {
                Body = value;
            }
        }

        /// <summary>
        /// Gets the Value Type of this JSObjectMethod
        /// </summary>
        /// <value></value>
        public override JSObjectItemType ValueType
        {
            get { return JSObjectItemType.Method; }
        }

        public override string ToString()
        {
            string parameters = "";
            bool isFirst = true;
            Parameters.ForEach(param =>
            {
                if (isFirst)
                    isFirst = false;
                else
                    parameters += ", ";
                parameters += param;
            });


            return Name + " : function (" + parameters + ") {" + Body + "}"; 
        }
    }
}
