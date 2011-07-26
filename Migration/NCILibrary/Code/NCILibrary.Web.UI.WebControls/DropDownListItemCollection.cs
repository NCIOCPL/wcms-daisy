using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls
{

    /// <summary>
    /// Used inside DropDownBoundField For GridView.
    /// Usage:
    ///   <Items>
    ///                 <sk:DropDownListItem text="223" value="223"  Default="true" />
    ///                 <sk:DropDownListItem text="2" value="2"   />
    ///                 <sk:DropDownListItem text="1" value="1" />
    ///             </Items>
    /// </summary>
    public class DropDownListItemCollection : StateManagedCollection
    {
        #region private variables
        private static readonly Type[] _knownTypes = new Type[] { typeof(DropDownListItem) };
        #endregion

        #region protected methods
        /// <summary>
        /// Store an index in view state for the type of a contained element. 
        /// Storing an index rather than a fully qualified type name improves performance
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override object CreateKnownType(int index)
        {
            switch (index)
            {
                case 0:
                    return new DropDownListItem();
                default:
                    throw new ArgumentOutOfRangeException("Unknown Type");
            }
        }

        /// <summary>
        /// Reeturn an index in view state for the type of a contained element. 
        /// </summary>
        /// <returns></returns>
        protected override Type[] GetKnownTypes()
        {
            return _knownTypes;
        }

        /// <summary>
        ///  Forces the entire collection to be serialized to view state, 
        /// rather than just serializing changes made to state since the last time it was loaded. 
        /// </summary>
        /// <param name="o"></param>
        protected override void SetDirtyObject(object o)
        {
            ((DropDownListItem)o).SetDirty();
        }
        #endregion

        #region public method
        /// <summary>
        /// Add item to list
        /// </summary>
        /// <param name="item"></param>
        public void Add(DropDownListItem item)
        {
            ((IList)this).Add(item);
        }

        /// <summary>
        /// Return a default ListItem
        /// </summary>
        /// <returns></returns>
        public string GetDefaultValue()
        {
            foreach (DropDownListItem item in this)
            {
                if (item.Default)
                    return item.Value;
            }

            throw new Exception("There is no default value");
        }
        #endregion
    }
}
