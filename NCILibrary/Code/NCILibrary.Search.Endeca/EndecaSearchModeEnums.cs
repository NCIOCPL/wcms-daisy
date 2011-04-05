using System;
using System.Collections.Generic;
using System.Text;

namespace NCI.Search.Endeca
{
    /// <summary>
    /// These are the different match modes for endeca searching
    /// </summary>
    public enum EndecaMatchModes
    {
        /// <summary>
        /// Match all user search terms (i.e. perform a conjunctive search).  This is the default mode.
        /// </summary>
        MatchAll = 1,
        /// <summary>
        /// Match at least one user search term.
        /// </summary>
        MatchAny = 2,
        /// <summary>
        /// Match some user search terms.
        /// </summary>
        MatchPartial = 3,
        /// <summary>
        /// Match all user search terms if possible, otherwise match at least one.  Not recommended in cases
        /// where queries can exceed two words.
        /// </summary>
        MatchAllAny = 4,
        /// <summary>
        /// Match all user search terms if possible, otherwise match some.
        /// </summary>
        MatchAllPartial = 5,
        /// <summary>
        /// Match a maximal subset of user search terms.
        /// </summary>
        MatchPartialMax = 6,
        /// <summary>
        /// Match using a Boolean query.
        /// </summary>
        MatchBoolean = 7
    }

    /// <summary>
    /// These are the different range filtering operators that endeca supports for their range filters.
    /// </summary>
    public enum RangeFilterOperators
    {
        /// <summary>
        /// &lt;
        /// </summary>
        LT = 1,
        /// <summary>
        /// &lt;=
        /// </summary>
        LTEQ = 2,
        /// <summary>
        /// &gt;
        /// </summary>
        GT = 3,
        /// <summary>
        /// &lt;=
        /// </summary>
        GTEQ = 4,
        /// <summary>
        /// Between.
        /// </summary>
        BTWN = 5,
        /// <summary>
        /// &lt;= for geocode
        /// </summary>
        GCLT = 6,
        /// <summary>
        /// &lt;= for geocode
        /// </summary>
        GCGT = 7,
        /// <summary>
        /// &lt;= for geocode
        /// </summary>
        GCBWTN = 8
    }
}
