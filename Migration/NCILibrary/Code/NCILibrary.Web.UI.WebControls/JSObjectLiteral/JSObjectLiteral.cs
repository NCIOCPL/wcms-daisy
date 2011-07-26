using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// Defines an Javascript object literal.
    /// </summary>
    /// <remarks>
    /// An object literal is the JS syntax: { prop1: 'value', prop2: 'value', prop4: value, meth1: function() {} }
    /// </remarks>
    public class JSObjectLiteral : IList, ICollection, IEnumerable
    {
        List<JSObjectItem> _items = new List<JSObjectItem>();

        #region Public Methods

        #region Add

        /// <summary>
        /// Appends the specified javascript property or method to the JSObjectLiteral.
        /// </summary>
        /// <param name="item">The javascript property or method to append to the collection.</param>
        /// <exception cref="System.ArgumentException">This will be thrown if the collection already contains a JSObjectItem with the same name as <em>item</em>.</exception>
        public void Add(JSObjectItem item)
        {
            if (ContainsName(item.Name))
                throw new ArgumentException("This collection already contains the name " + item.Name);

            _items.Add(item);
        }

        /// <summary>
        /// Appends a javascript property to the end of the collection that represents the specified propery name and value.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">A string that represents the value of the JSObjectItem to add to the end of the collection.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <exception cref="System.ArgumentException">This will be thrown if the collection already contains the key.</exception>
        /// <exception cref="System.ArgumentException">
        /// This is thrown if the <em>type</em> is Method or ObjectLiteral.
        /// </exception>
        public void AddProperty(string propertyName, string value, JSObjectItemType type)
        {
            if (ContainsName(propertyName))
                throw new ArgumentException("This collection already contains the name " + propertyName);

            _items.Add(JSObjectItem.CreateProperty(propertyName, value, type));
        }


        /// <summary>
        /// Appends a javascript property of type ObjectLiteral to the end of the collection that represents the specified propery name and value.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentException">This will be thrown if the collection already contains the key.</exception>
        public void AddObjectLiteralProperty(string propertyName, JSObjectLiteral value)
        {
            if (ContainsName(propertyName))
                throw new ArgumentException("This collection already contains the name " + propertyName);

            _items.Add(new JSObjectLiteralProperty(propertyName, value));
        }        

        /// <summary>
        /// Adds a javascript method to the JSObjectLiteral with an empty signature.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="methodBody">The body of the method.</param>
        /// <exception cref="System.ArgumentException">This will be thrown if the collection already contains the key.</exception>
        public void AddMethod(string methodName, string methodBody)
        {
            if (ContainsName(methodName))
                throw new ArgumentException("This collection already contains the name " + methodName);

            _items.Add(new JSObjectMethod(methodName, methodBody));
        }

        /// <summary>
        /// Adds a javascript method to the JSObjectLiteral with a signature specified by <em>parameters</em>.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="methodBody">The body of the method.</param>
        /// <param name="parameters">The signature of the method.</param>
        public void AddMethod(string methodName, string methodBody, string[] parameters)
        {
            if (ContainsName(methodName))
                throw new ArgumentException("This collection already contains the name " + methodName);

            _items.Add(new JSObjectMethod(methodName, methodBody, parameters));
        }

        #endregion

        /// <summary>
        /// Removes the property or method with the name, <em>name</em>.
        /// </summary>
        /// <param name="name">The name of the property to remove.</param>
        public void RemoveName(string name)
        {
            int index = GetIndexOfName(name);

            if (index >= 0)
                _items.RemoveAt(index);
        }

        /// <summary>
        /// This works like the Prototype.js Object.Extend method.  What it does is it takes the properties
        /// and methods in <em>literal</em> and overrides any methods and properties in this 
        /// JSObjectLiteral with the same name, and adds any new properties and methods that currently
        /// are not in this JSObjectLiteral.
        /// </summary>
        /// <param name="literal">The JSObjectLiteral to extend this with.</param>
        public void Extend(JSObjectLiteral literal)
        {
            literal._items.ForEach(item =>
            {
                this.RemoveName(item.Name);
                this.Add(item);
            });
        }

        /// <summary>
        /// Gets the number of elements contained in the JSObjectLiteral.
        /// </summary>
        /// <value></value>
        /// <returns>The number of elements contained in the JSObjectLiteral.</returns>
        public int Count
        {
            get { return _items.Count; }
        }

        /// <summary>
        /// Gets the property and method names of this JSObjectLiteral.
        /// </summary>
        /// <value>An array of strings representing the property and method names of this JSObjectLiteral</value>
        public string[] Names
        {
            get
            {
                List<string> names = new List<string>();
                _items.ForEach(item =>
                {
                    names.Add(item.Name);
                });

                return names.ToArray();
            }
        }

        /// <summary>
        /// Gets the JSObjectItem in this JSObjectLiteral with the specified name.
        /// </summary>
        /// <value></value>
        public JSObjectItem this[string name]
        {
            get { return GetJSObjectItemByName(name); }
        }

        /// <summary>
        /// Gets or sets the JSObjectItem in this JSObjectLiteral with the specified index.
        /// </summary>
        /// <value></value>
        public JSObjectItem this[int index]
        {
            get { return _items[index]; }
            set { _items[index] = value; }
        }

        /// <summary>
        /// Gets the index of the JSObjectItem with the property or method name, <em>name</em> in this collection.
        /// </summary>
        /// <param name="name">The name of the property or method to get the index for.</param>
        /// <returns>
        /// The position of the JSObjectItem with the property or method name, <em>name</em> in this collection, 
        /// or -1 if the collection does not contain the name.
        /// </returns>
        public int GetIndexOfName(string name)
        {
            return _items.FindIndex(item => item.Name == name); //Linq
        }

        #region Contains

        /// <summary>
        /// Checks to see if this JSObjectItem contains a method or property name with the name, <em>name</em>.
        /// </summary>
        /// <param name="name">The name of the property or method to look for.</param>
        /// <returns><c>true</c> if the collection contains the name, <c>false</c> if it does not.</returns>
        public bool ContainsName(string name)
        {
            return (GetIndexOfName(name) >= 0);
        }

        /// <summary>
        /// Checks to see if this JSObjectItem contains a JSObjectProperty with the property name, <em>propertyName</em>.
        /// </summary>
        /// <param name="propertyName">The property name to look for.</param>
        /// <returns>true if the collection contains a JSObjectProperty with the property name, <em>propertyName</em>, false if it does not.</returns>
        public bool ContainsProperty(string propertyName)
        {
            int index = GetIndexOfName(propertyName);

            if (index == -1)
                return false;

            JSObjectItem item = _items[index];

            return (item.ValueType != JSObjectItemType.Method);
        }

        /// <summary>
        /// Checks to see if this JSObjectItem contains a JSObjectItem with the property name, <em>propertyName</em>
        /// and is of JSObjectItemType, <em>type</em>.
        /// </summary>
        /// <param name="propertyName">The property name to look for.</param>
        /// <param name="type">The type of the property.</param>
        /// <returns>true if the collection contains a JSObjectProperty with the property name, <em>propertyName</em>
        /// and is of JSObjectItemType, <em>type</em>, false if it does not.</returns>
        public bool ContainsProperty(string propertyName, JSObjectItemType type)
        {
            int index = GetIndexOfName(propertyName);

            if (index == -1)
                return false;

            JSObjectItem item = _items[index];

            return (item.ValueType == type);
        }

        /// <summary>
        /// Checks to see if this JSObjectItem contains a JSObjectMethod with the method name, <em>methodName</em>.
        /// </summary>
        /// <param name="methodName">The method name to look for.</param>
        /// <returns>true if the collection contains a JSObjectMethod with the method name, <em>methodName</em>, false if it does not.</returns>
        public bool ContainsMethod(string methodName)
        {
            int index = GetIndexOfName(methodName);

            if (index == -1)
                return false;

            JSObjectItem item = _items[index];

            return (item is JSObjectMethod);
        }

        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");

            bool isFirst = true;
            this._items.ForEach(item =>
            {
                if (isFirst)
                    isFirst = false;
                else
                    sb.Append(", ");

                sb.Append(item.ToString());
            });

            sb.Append(" }");

            return sb.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the value or method body of an JSObjectItem with the property or method name, <em>name</em>.
        /// </summary>
        /// <remarks>
        /// Note, a JSObjectItem of type ObjectLiteral will return the complete string of the object
        /// literal.  This might not be what you want.
        /// </remarks>
        /// <param name="name">The name to find the property value or method body by.</param>
        /// <returns>The value of the JSObjectItem with the property or method name, <em>name</em>.</returns>
        private string GetValueByName(string name)
        {
            JSObjectItem item = GetJSObjectItemByName(name);

            if (item == null)
                return null;

            return item.StringValue;
        }

        /// <summary>
        /// Gets the JSObjectItem with the property or method name, <em>name</em>.
        /// </summary>
        /// <param name="name">The name of the JSObjectItem you are looking for.</param>
        /// <returns>The JSObjectItem in this collection with the property or method name, <em>name</em>.</returns>
        private JSObjectItem GetJSObjectItemByName(string name)
        {
            return _items.SingleOrDefault(item => item.Name == name); //This is linq
        }

        #endregion

        #region IEnumerator Methods

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)_items).GetEnumerator();
        }

        #endregion

        #region ICollection Members

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">array is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero. </exception>
        /// <exception cref="T:System.ArgumentException">array is multidimensional.-or- index is equal to or greater than the length of array.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"></see> is greater than the available space from index to the end of the destination array. </exception>
        /// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.ICollection"></see> cannot be cast automatically to the type of the destination array. </exception>
        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)_items).CopyTo(array, index);
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe).
        /// </summary>
        /// <value></value>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe); otherwise, false.</returns>
        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)_items).IsSynchronized; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.</returns>
        object ICollection.SyncRoot
        {
            get { return ((ICollection)_items).SyncRoot; }
        }

        #endregion

        #region IList Members

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.IList"></see>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"></see> to add to the <see cref="T:System.Collections.IList"></see>.</param>
        /// <returns>
        /// The position into which the new element was inserted.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only.-or- The <see cref="T:System.Collections.IList"></see> has a fixed size. </exception>
        int IList.Add(object value)
        {
            //TODO: Add some type checking here.
            return ((IList)_items).Add(value);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.IList"></see>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only. </exception>
        void IList.Clear()
        {
            ((IList)_items).Clear();
        }


        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.IList"></see> contains a specific value.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"></see> to locate in the <see cref="T:System.Collections.IList"></see>.</param>
        /// <returns>
        /// true if the <see cref="T:System.Object"></see> is found in the <see cref="T:System.Collections.IList"></see>; otherwise, false.
        /// </returns>
        bool IList.Contains(object value)
        {
            return ((IList)_items).Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.IList"></see>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"></see> to locate in the <see cref="T:System.Collections.IList"></see>.</param>
        /// <returns>
        /// The index of value if found in the list; otherwise, -1.
        /// </returns>
        int IList.IndexOf(object value)
        {
            return ((IList)_items).IndexOf(value);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.IList"></see> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="value">The <see cref="T:System.Object"></see> to insert into the <see cref="T:System.Collections.IList"></see>.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.IList"></see>. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only.-or- The <see cref="T:System.Collections.IList"></see> has a fixed size. </exception>
        /// <exception cref="T:System.NullReferenceException">value is null reference in the <see cref="T:System.Collections.IList"></see>.</exception>
        void IList.Insert(int index, object value)
        {
            ((IList)_items).Insert(index, value);
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IList"></see> has a fixed size.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Collections.IList"></see> has a fixed size; otherwise, false.</returns>
        bool IList.IsFixedSize
        {
            get { return ((IList)_items).IsFixedSize; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IList"></see> is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Collections.IList"></see> is read-only; otherwise, false.</returns>
        bool IList.IsReadOnly
        {
            get { return ((IList)_items).IsReadOnly; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"></see>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"></see> to remove from the <see cref="T:System.Collections.IList"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only.-or- The <see cref="T:System.Collections.IList"></see> has a fixed size. </exception>
        void IList.Remove(object value)
        {
            ((IList)_items).Remove(value);
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.IList"></see> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.IList"></see>. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only.-or- The <see cref="T:System.Collections.IList"></see> has a fixed size. </exception>
        void IList.RemoveAt(int index)
        {
            ((IList)_items).RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> at the specified index.
        /// </summary>
        /// <value></value>
        object IList.this[int index]
        {
            get
            {
                return ((IList)_items)[index];
            }
            set
            {
                ((IList)_items)[index] = value;
            }
        }

        #endregion

    }
}
