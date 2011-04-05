using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE.UI
{
    /// <summary>
    /// Custom collection class for managing objects of page slots called TemplateSlot.
    /// </summary>
    public class TemplateSlotCollection : IEnumerable<TemplateSlot>
    {
        private Dictionary<string, TemplateSlot> _templateSlots = new Dictionary<string, TemplateSlot>();

        /// <summary>
        /// Gets the slot names in this slot collection
        /// </summary>
        public IEnumerator<string> SlotNames
        {
            get
            {
                return _templateSlots.Keys.GetEnumerator();
            }
        }

        /// <summary>
        /// Gets a count of the number of TemplateSlots in this collection.
        /// </summary>
        public int Count
        {
            get
            {
                return _templateSlots.Keys.Count;
            }
        }

        /// <summary>
        /// Gets the TemplateSlot with the name, slotName.
        /// </summary>
        /// <param name="slotName"></param>
        /// <returns></returns>
        public TemplateSlot this[string slotName]
        {
            get
            {
                if (_templateSlots.ContainsKey(slotName))
                    return _templateSlots[slotName];
                else
                    return null;
            }
        }

        /// <summary>
        /// Checks to see if this collection contains a TemplateSlot with the name, slotName.
        /// </summary>
        /// <param name="slotName">The name of the slot to look for.</param>
        /// <returns></returns>
        public bool ContainsSlotName(string slotName)
        {
            return _templateSlots.ContainsKey(slotName);
        }


        /// <summary>
        /// Adds a TemplateSlot to this TemplateSlotCollection
        /// </summary>
        /// <param name="slot"></param>
        public void Add(TemplateSlot slot)
        {
            try
            {
                _templateSlots.Add(slot.ID, slot);
            }
            catch
            {
                throw new Exception("The slot collection already contains a slot named " + slot.ID);
            }
        }

        #region IEnumerable<TemplateSlot> Members

        public IEnumerator<TemplateSlot> GetEnumerator()
        {
            return _templateSlots.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)_templateSlots.Values).GetEnumerator();
        }

        #endregion
    }
}
