using System;
using System.Collections.Generic;
using System.Text;

using Endeca.Navigation;

namespace NCI.Search.Endeca
{
    /// <summary>
    /// This is an abstraction of an Endeca.Navigation.RangeFilter.
    /// <remarks>We do not do anything about geocode filtering.  Maybe later.  This can be used for
    /// Nf and Df parameters (Normal searching or Navigation searching)
    /// </remarks>
    /// </summary>
    public class EndecaRangeFilter
    {

        #region Fields

        private string _rangeFilterField = "";
        private RangeFilterOperators _rangeFilterOperator = RangeFilterOperators.LT;
        private string _rangeFilterValue1 = "";
        private string _rangeFilterValue2 = "";

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the field used for the RangeFilter.  This is a property or dimension that is an int or a float.
        /// </summary>
        public string RangeFilterField
        {
            get { return _rangeFilterField; }
            set { _rangeFilterField = value; }
        }

        /// <summary>
        /// Gets and sets the filter operator.
        /// </summary>
        public RangeFilterOperators RangeFilterOperator
        {
            get { return _rangeFilterOperator; }
            set { _rangeFilterOperator = value; }
        }

        /// <summary>
        /// Gets and sets the primary value used by the range filter operator.
        /// </summary>
        public string RangeFilterValue1
        {
            get { return _rangeFilterValue1; }
            set { _rangeFilterValue1 = value; }
        }

        /// <summary>
        /// Gets and sets the secondary value used by the range filter operator.  Note, this is only
        /// used by the BTWN operator.
        /// </summary>
        public string RangeFilterValue2
        {
            get { return _rangeFilterValue2; }
            set { _rangeFilterValue2 = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a GSSEndecaRangeFilter used to select documents that fall within a certain range.  Do not
        /// use this constructor for BTWN operations.
        /// </summary>
        /// <param name="rangeFilterField">The property or dimension to filter on</param>
        /// <param name="rangeFilterOperator">The filtering operator</param>
        /// <param name="value">The value the filtering operator operates on</param>
        public EndecaRangeFilter(string rangeFilterField, RangeFilterOperators rangeFilterOperator, string value)
        {
            _rangeFilterField = rangeFilterField;
            _rangeFilterOperator = rangeFilterOperator;
            _rangeFilterValue1 = value;
        }

        /// <summary>
        /// Creates a GSSEndecaRangeFilter used to select documents that fall within a certain range.
        /// Use this constructor only for BTWN operations.
        /// </summary>
        /// <param name="rangeFilterField">The property or dimension to filter on</param>
        /// <param name="value1">The min value for the BTWN operator</param>
        /// <param name="value2">The max value for the BTWN operator</param>
        public EndecaRangeFilter(string rangeFilterField, string value1, string value2)
        {
            //Only for between
            _rangeFilterField = rangeFilterField;
            _rangeFilterOperator = RangeFilterOperators.BTWN;
            _rangeFilterValue1 = value1;
            _rangeFilterValue2 = value2;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a <see cref="Endeca.Navigation.RangeFilter"/> object from this <see cref="GSS.Search.Endeca.GSSEndecaRangeFilter"/>.
        /// </summary>
        /// <returns><see cref="Endeca.Navigation.RangeFilter"/></returns>
        public RangeFilter GetRangeFilter()
        {
            //Man they need to work on thier code a bit.  This stuff is ugly.

            string filter = _rangeFilterField + "|" + _rangeFilterOperator.ToString() + " " + _rangeFilterValue1;

            if (_rangeFilterOperator == RangeFilterOperators.BTWN)
            {
                filter += " " + _rangeFilterValue2;
            }

            return new RangeFilter(filter);
        }

        #endregion

    }

}
