using System;
using System.Collections.Generic;
using System.Text;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Implement this interface when creating nodes to be used with the TemplatedTreeView control.
    /// The implementing class will usually have to implement IEnumerable as well so the tree view
    /// can itererate the nodes for child contol creation. Alternatively, instances of the 
    /// implementing class can be put into a list before being passed into the TemplatedTreeView.
    /// </remarks>
    public interface ITemplatedTreeNode : ITemplatedDataItem
    {
        /// <summary>
        /// Gets the path to and including the node.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets a unique id for the node in the tree.
        /// </summary>
        string TemplatedTreeNodeID { get; }

        /// <summary>
        /// Gets the depth of this node in the tree.
        /// </summary>
        int Depth { get; }

        /// <summary>
        /// Gets whether or not this node is selected.
        /// </summary>
        bool IsSelected { get;}

        /// <summary>
        /// Gets whether or not this node is expanded.
        /// </summary>
        bool IsExpanded { get; }

        /// <summary>
        /// Gets whether or not this node has child nodes.
        /// </summary>
        bool HasChildren { get; }
    }
}
