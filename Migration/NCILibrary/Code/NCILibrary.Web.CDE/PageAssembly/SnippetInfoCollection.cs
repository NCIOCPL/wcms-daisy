using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace NCI.Web.CDE
{

    /// <summary>
    /// This class contains a collection of the snippets to be displayed on the page.
    /// </summary>
    public class SnippetInfoCollection : IEnumerable<SnippetInfo>
    {
        /// <summary>
        /// Internal storage for the list of snippets.  Held in a dictinoary keyed on the slot name for the snippet.
        /// </summary>
        private List<SnippetInfo> _snippetInfos = new List<SnippetInfo>();


        /// <summary>
        /// Gets the number of SnippetInfos in this collection.
        /// </summary>
        public int Count
        {
            get
            {
                return _snippetInfos.Count;
            }
        }


        /// <summary>
        /// Adds a SnippetInfo to the SnippetInfoCollection.
        /// </summary>
        /// <param name="snippetInfo">The SnippetInfo to add.</param>
        public void Add(SnippetInfo snippetInfo)
        {
            _snippetInfos.Add(snippetInfo);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            SnippetInfoCollection target = obj as SnippetInfoCollection;

            if (target == null)
                return false;

            if (
                (_snippetInfos != null && target._snippetInfos == null) ||
                (_snippetInfos == null && target._snippetInfos != null)
                )
            {
                return false;
            }

            if (_snippetInfos.Count != target._snippetInfos.Count)
                return false;

            for (int i = 0; i < _snippetInfos.Count; i++)
            {
                if (!_snippetInfos[i].Equals(_snippetInfos[i]))
                    return false;
            }

            return true;
        }

        #region IEnumerable<SnippetInfo> Members

        public IEnumerator<SnippetInfo> GetEnumerator()
        {
            return _snippetInfos.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _snippetInfos.GetEnumerator();
        }

        #endregion
    }
}

