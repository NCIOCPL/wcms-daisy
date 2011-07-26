using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using NCI.Logging.Configuration;
using System.Configuration;
using NCI.Util;

namespace NCI.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class Sink
    {
        private string _name;
        private string _providerName;
        private NCIErrorLevel _errorLevels;
        private string[] _matchStrings;

        /// <summary>
        /// Nme of the Sink
        /// </summary>
        public string Name
        {
            get { return _name; }
        }
        /// <summary>
        /// ErrorLevels that apply to this Sink
        /// </summary>
        public NCIErrorLevel ErrorLevels
        {
            get { return _errorLevels; }
        }
        /// <summary>
        /// The ProviderName that applies to this Sink
        /// </summary>
        public string ProviderName
        {
            get { return _providerName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Sink()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="errorLevels"></param>
        /// <param name="providerName"></param>
        /// <param name="matchStrings"></param>
        public Sink(string name, NCIErrorLevel errorLevels, string providerName, string[] matchStrings)
        {
            _name = name;
            _providerName = providerName;
            _errorLevels = errorLevels;
            _matchStrings = matchStrings;
        }
               
        /// <summary>
        /// Determines if the facility passed in as a string matches any of the patterns in the facilities specified in the configuration file
        /// </summary>
        /// <returns>Boolean value indicating if the facility passed in as a string matches any of the patterns in the facilities specified in the configuration file </returns>
        public bool DoesFacilityMatch(string facility)
        {
            try
            {
                foreach (string matchstring in _matchStrings)
                {
                    if (Strings.StringMatchesPattern(facility, matchstring, true))
                        return true;
                }
               
                return false;
            }
            catch (Exception ex)
            {
                throw new NCILoggingException("NCI.Logging.Sink: Error in DoesFacilityMatch. ", ex);
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class SinkCollection : List<Sink>
    {
       
        /// <summary>
        /// Returns an array of strings containing the ProviderNames by Error Level and Facility.
        /// </summary>
        /// <param name="facility">The facility for which ProviderNames are needed.</param>
        /// <param name="level">The ErrorLevel for which ProviderNames are needed.</param>
        /// <returns>An Array of string containing the ProviderNames</returns>
        public string[] GetProviderNamesByErrorAndFacility(string facility, NCIErrorLevel level)
        {
            try
            {
                return this.GetSinksByErrorLevel(level).GetSinksByFacility(facility).GetProviderNames();
            }
            catch (Exception ex)
            {
                throw new NCILoggingException("NCI.Logging.SinkCollection: Error in GetProviderNamesByErrorAndFacility. ", ex);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public SinkCollection()
        {
          
        }


        /// <summary>
        /// Gets a Collection of Sinks based on the errorlevel passed in.
        /// </summary>
        /// <param name="level">The errorlevel passed in as a parameter</param>
        /// <returns>A Collection of Sinks</returns>
        private SinkCollection GetSinksByErrorLevel(NCIErrorLevel level)
        {                              
            SinkCollection sc = new SinkCollection();
               this.ForEach(
                delegate(Sink s)
                {
                    if ((s.ErrorLevels & level) > 0)
                        sc.Add(s);
                });
           
            return sc;
        }

        /// <summary>
        /// Gets a Collection of Sinks based on the facility passed in.
        /// </summary>
        /// <param name="facility">the facility passed in as a string</param>
        /// <returns>A collection of sinks</returns>
        private SinkCollection GetSinksByFacility(string facility)
        {
            SinkCollection sc = new SinkCollection();
            this.ForEach(
                delegate(Sink s)
                {
                    if (s.DoesFacilityMatch(facility))
                        sc.Add(s);
                });

            return sc;
        }

        /// <summary>
        /// Gets an Array of strings containing providernames
        /// </summary>
        /// <returns></returns>
        private string[] GetProviderNames()
        {
            List<string> providers = new List<string>();
            this.ForEach(
                delegate(Sink s)
                {
                    if (!providers.Contains(s.ProviderName))
                        providers.Add(s.ProviderName);
                });

            return providers.ToArray();
        }
    }
    

       
}
    

   


     

