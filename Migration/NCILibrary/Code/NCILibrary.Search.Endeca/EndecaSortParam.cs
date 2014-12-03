using System;
using System.Collections.Generic;
using System.Text;

using Endeca.Navigation;

namespace NCI.Search.Endeca
{

    /// <summary>
    /// This is an abstraction of an Endeca.Navigation.ERecSortKey.
    /// </summary>
    public class EndecaSortParam
    {

        #region Fields

        private string _sortField = "";
        private bool _isAscending = true;
        private double _lat = 0.0d;
        private double _lon = 0.0d;

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the field to be used for sorting.
        /// </summary>
        public string SortField
        {
            get { return _sortField; }
            set { _sortField = value; }
        }

        /// <summary>
        /// Should the sorting be ascending?
        /// </summary>
        public bool IsAscending
        {
            get { return _isAscending; }
            set { _isAscending = value; }
        }

        /// <summary>
        /// Target location latitude
        /// </summary>
        public double Latitude
        {
            get { return _lat; }
            set { _lat = value; }
        }

        /// <summary>
        /// Target location longitude
        /// </summary>
        public double Longitude
        {
            get { return _lon; }
            set { _lon = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates one GSSEndecaSortParm to be used for sorting normal search results using the default sort order of ascending.
        /// </summary>
        /// <param name="sortField">The field to be used for sorting</param>
        public EndecaSortParam(string sortField)
        {
            _sortField = sortField;
        }

        /// <summary>
        /// Creates one GSSEndecaSortParm to be used for sorting normal search results.
        /// </summary>
        /// <param name="sortField">The field to be used for sorting</param>
        /// <param name="isAscending">Should the sorting be ascending?</param>
        public EndecaSortParam(string sortField, bool isAscending)
        {
            _sortField = sortField;
            _isAscending = isAscending;
        }

        /// <summary>
        /// Creates one GSSEndecaSortParm to be used for sorting location by distance.
        /// </summary>
        /// <param name="sortField">The field to be used for sorting</param>
        /// <param name="isAscending">Should the sorting be ascending?</param>
        /// <param name="lat">latitude</param>
        /// <param name="lon">longitude</param>
        public EndecaSortParam(string sortField, bool isAscending, double lat, double lon)
        {
            _sortField = sortField;
            _isAscending = isAscending;
            _lat = lat;
            _lon = lon;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets an <see cref="Endeca.Navigation.ERecSortKey"/> from a <see cref="GSS.Search.EndecaSearch.GSSEndecaSortParam"/>
        /// </summary>
        /// <returns></returns>
        public ERecSortKey GetERecSortKey()
        {
            return new ERecSortKey(_sortField, _isAscending);
        }

        /// <summary>
        /// Gets an <see cref="Endeca.Navigation.ERecSortKey"/> from a <see cref="GSS.Search.EndecaSearch.GSSEndecaSortParam"/>
        /// </summary>
        /// <returns></returns>
        public ERecSortKey GetERecGeoSortKey()
        {
            return new ERecSortKey(_sortField, _isAscending, _lat, _lon);
        }

        #endregion

    }
}
