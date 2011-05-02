using System;
using System.Collections.Generic;
using System.Text;
using Endeca.Navigation;

namespace NCI.Search.Endeca
{
    /// <summary>
    /// This is an abstraction of an Endeca.Navigation.ERecSearch.
    /// </summary>
    public class EndecaNavigationSearchParameter
    {

        #region Semi-informative comment
        //An ERecSearch defines the search query.  Since they did not document it, I will.
        //ERecSearch(string key, string searchTerms, string opts);
        //
        // key -->  This is the name of the search interface to use, or, optionally what fields
        // to search on.  So for us, we would choose ALL.  If it is just searching against some
        // fields(Properties), I.E. Keywords and Title, then it would look like Keywords|Title.  
        // The | character separates the different fields.  Note, a new search interface requires
        // a full indexing, so XMLSearch replacement we might just uses fields instead of creating
        // new interfaces.
        //
        // searchTerms --> This is pretty self explanitory
        //
        // opts --> These are the options to search with.  This is where you set the search modes.
        // We use "mode matchallpartial" for now, if there are more options, then they are separated
        // with the | character.  
        //
        // Note: This is not really documented in the search documentation, and where it is is in the
        // URL Search Parameters section of the appendicies.  (And then it is not exactly documented)
        //
        // I will show an example of what needs to be done for xmlsearch elsewhere.
        #endregion

        #region Fields

        private string _searchTerm = "";
        private string _searchInterface = "All";
        private Dictionary<string, string> _searchOptions;
        private EndecaMatchModes _matchMode = EndecaMatchModes.MatchAll;

        //TODO: Flesh this out. It would be nice if we did not have _searchOptions at all,
        //but make them strongly typed, like the rel option would be a good example.

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the search term for this search parameter. (Ntt parameter, page 520)
        /// </summary>
        public string SearchTerm
        {
            get { return _searchTerm; }
            set { _searchTerm = value; }
        }

        /// <summary>
        /// Gets and sets the search interface for this search parameter. (Ntk parameter, page 519)
        /// <remarks>Endeca calls this the search key which makes sounds very confusing.
        /// This can either be a search interface defined in the developers studio, a property, or even a 
        /// dimension.
        /// </remarks>
        /// </summary>
        public string SearchInterface
        {
            get { return _searchInterface; }
            set { _searchInterface = value; }
        }

        /// <summary>
        /// Gets a collection of search options for this search parameter.  DO NOT SET THE MATCH MODE! (Ntx parameter, page 521)
        /// </summary>
        public Dictionary<string, string> SearchOptions
        {
            get { return _searchOptions; }
        }

        /// <summary>
        /// Gets and sets the match mode for this search parameter.  (This has query parameter, it is part of the Ntx parameter)
        /// </summary>
        public EndecaMatchModes MatchMode
        {
            get { return _matchMode; }
            set { _matchMode = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of a GSSEndecaNavigationSearchParameter which defines the search term and
        /// the field or interface for doing a normal search.
        /// </summary>
        /// <param name="searchTerm">The term to search for (Ntt)</param>
        /// <param name="searchInterface">The field to search (Ntk)</param>
        public EndecaNavigationSearchParameter(string searchTerm, string searchInterface)
        {
            _searchOptions = new Dictionary<string, string>();
            _searchTerm = searchTerm;
            _searchInterface = searchInterface;
        }

        /// <summary>
        /// Creates an instance of a GSSEndecaNavigationSearchParameter which defines the search term,
        /// the field or interface, and the match mode for doing a normal search.
        /// </summary>
        /// <param name="searchTerm">The term to search for (Ntt)</param>
        /// <param name="searchInterface">The field to search (Ntk)</param>
        /// <param name="matchMode">The match mode for the search (Ntx=mode)</param>
        public EndecaNavigationSearchParameter(string searchTerm, string searchInterface, EndecaMatchModes matchMode)
        {
            _searchOptions = new Dictionary<string, string>();
            _searchTerm = searchTerm;
            _searchInterface = searchInterface;
            _matchMode = matchMode;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets an <see cref="Endeca.Navigation.ERecSearch"/> object from a GSSEndecaNavigationSearchParameter
        /// </summary>
        /// <returns></returns>
        public ERecSearch GetERecSearch()
        {
            string searchOptions = "";

            //First set the match mode
            searchOptions = "mode " + _matchMode.ToString();

            //Now add the options
            foreach (string option in _searchOptions.Keys)
            {
                searchOptions += " " + option + " " + _searchOptions[option];
            }

            return new ERecSearch(_searchInterface, _searchTerm, searchOptions);
        }

        #endregion
    }
}
