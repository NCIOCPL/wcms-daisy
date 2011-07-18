using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.UI.WebControls
{
    public class TemplatedDataItem : ITemplatedDataItem
    {
        private string _itemType = string.Empty;
        private object _data = null;


        public TemplatedDataItem(string itemType, object data)
        {
            _itemType = itemType;
            _data = data;
        }

        #region ITemplatedDataItem Members

        public string ItemType
        {
            get { return _itemType; }
        }

        public object Data
        {
            get { return _data; }
        }

        #endregion
    }
}
