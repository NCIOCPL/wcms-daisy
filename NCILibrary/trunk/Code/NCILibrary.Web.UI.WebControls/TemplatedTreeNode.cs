using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// A standard implementation of the ITemplatedTreeNode interface. Recursive in nature, it has references
    /// to it child, parent, and siblings. The IEnumerable implementation recursively iterates in a depth-first
    /// manner to properly build up a tree. 
    /// </summary>
    public class TemplatedTreeNode : ITemplatedTreeNode, IEnumerable
    {
        #region Members

        private TemplatedTreeNode _parentNode;
        private TemplatedTreeNode _prevNode;
        private TemplatedTreeNode _nextNode;
        private TemplatedTreeNode _firstChildNode;

        private string _templatedTreeNodeID = string.Empty;
        private string _nodeType = string.Empty;
        private object _data;
        private bool _isSelected;
        private bool _isExpanded;

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the parent TemplatedTreeNode of this node.
        /// </summary>
        public TemplatedTreeNode Parent
        {
            get { return _parentNode; }
            set { _parentNode = value; }
        }

        /// <summary>
        /// Gets and sets the previous TemplatedTreeNode of this node.
        /// </summary>
        public TemplatedTreeNode Prev
        {
            get { return _prevNode; }
            set { _prevNode = value; }
        }

        /// <summary>
        /// Gets and sets the next TemplatedTreeNode of this node.
        /// </summary>
        public TemplatedTreeNode Next
        {
            get { return _nextNode; }
            set { _nextNode = value; }
        }

        /// <summary>
        /// Gets and sets the first child TemplatedTreeNode of this node.
        /// </summary>
        public TemplatedTreeNode FirstChild
        {
            get { return _firstChildNode; }
            set { _firstChildNode = value; }
        }

        /// <summary>
        /// Gets the count of all of the nodes this node contains.  When called from
        /// a root TemplatedTreeNode it will return a count of all the nodes in the tree.
        /// </summary>
        public int Count
        {
            get
            {
                int count = 1;

                TemplatedTreeNode curr = FirstChild;

                while (curr != null)
                {
                    count += curr.Count;
                    curr = curr.Next;
                }

                return count;
            }
        }

        #endregion

        public TemplatedTreeNode() { } 

        public TemplatedTreeNode(string nodeType) 
        {
            _nodeType = nodeType;
        }

        public TemplatedTreeNode(string nodeType, string templatedTreeNodeID)
        {
            _nodeType = nodeType;
            _templatedTreeNodeID = templatedTreeNodeID;
        }

        public TemplatedTreeNode(string nodeType, string templatedTreeNodeID, object data)
        {
            _nodeType = nodeType;
            _templatedTreeNodeID = templatedTreeNodeID;
            _data = data;
        }

        #region ITemplatedTreeNode Members

        /// <summary>
        /// Gets a unique id for the node in the tree.
        /// </summary>
        /// <value></value>
        public string TemplatedTreeNodeID
        {
            get { return _templatedTreeNodeID; }
            set { _templatedTreeNodeID = value; }
        }

        /// <summary>
        /// Gets the data to be used for databinding.
        /// </summary>
        /// <value></value>
        public object Data
        {
            //In some implementations this could be 'return this;'.
            get { return _data; }
            set { _data = value; }
        }

        /// <summary>
        /// Gets the path to and including the node.
        /// </summary>
        /// <value></value>
        public string Path
        {
            get
            {
                if (_parentNode != null)
                    return Parent.Path + "|" + TemplatedTreeNodeID;
                else
                    return TemplatedTreeNodeID;
            }
        }

        /// <summary>
        /// Gets the depth of this node in the tree.
        /// </summary>
        /// <value></value>
        public int Depth
        {
            get
            {
                string[] starr = Path.Split('|');
                if (starr != null)
                    return starr.Length;
                else
                    return -1;
            }
        }

        /// <summary>
        /// Gets the type of the node.
        /// </summary>
        /// <value></value>
        public string ItemType
        {
            get { return _nodeType; }
            set { _nodeType = value; }
        }

        /// <summary>
        /// Gets whether or not this node has child nodes.
        /// </summary>
        /// <value></value>
        public bool HasChildren
        {
            get { return (_firstChildNode != null); }
        }

        /// <summary>
        /// Gets whether or not this node is selected.
        /// </summary>
        /// <value></value>
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        /// <summary>
        /// Gets whether or not this node is expanded.
        /// </summary>
        /// <value></value>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { _isExpanded = value; }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            yield return this;

            TemplatedTreeNode curr = this.FirstChild;

            while (curr != null)
            {                
                foreach (TemplatedTreeNode child in curr)
                {
                    yield return child;
                }

                curr = curr.Next;
            }
        }

        #endregion

    }
}
