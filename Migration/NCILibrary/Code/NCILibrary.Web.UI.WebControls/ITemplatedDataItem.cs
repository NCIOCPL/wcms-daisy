using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.UI.WebControls
{
    public interface ITemplatedDataItem
    {
        /// <summary>
        /// Gets the type of this TemplatedDataItem.
        /// </summary>
        string ItemType { get; }

        /// <summary>
        /// Gets the data to be used for databinding.
        /// </summary>
        object Data { get; }

    }
}
