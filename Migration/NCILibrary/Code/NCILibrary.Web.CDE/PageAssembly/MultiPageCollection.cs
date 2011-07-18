using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace NCI.Web.CDE
{
    public class MultiPageCollection : IEnumerable<MultiPage>
    {
        public List<MultiPage> _Pages = new List<MultiPage>();


        /// <summary>
        /// Gets the number of SnippetInfos in this collection.
        /// </summary>
        public int Count
        {
            get
            {
                return _Pages.Count;
            }
        }


        /// <summary>
        /// Adds a SnippetInfo to the SnippetInfoCollection.
        /// </summary>
        /// <param name="snippetInfo">The SnippetInfo to add.</param>
        public void Add(MultiPage page)
        {
            _Pages.Add(page);
        }

        #region IEnumerable<MultiPage> Members

        public IEnumerator<MultiPage> GetEnumerator()
        {
            return _Pages.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Pages.GetEnumerator();
        }

        #endregion

    }
}
