using System;
using Endeca.Navigation;
using NCI.Util;
using System.Configuration;

namespace NCI.Search.Endeca
{
	/// <summary>
	/// This class represents a search result from an Endeca Search.
	/// </summary>
	public class EndecaResult {
		/// <summary>
		/// The title of the record
		/// </summary>
		private string title = "";
		public string Title {
			get { return title; }
		}

		/// <summary>
		/// The description of the record
		/// </summary>
		private string description = "";
		public string Description {
			get { return description; }
		}

		/// <summary>
		/// The url of the record
		/// </summary>
		private string url = "";
		public string Url {
			get { return url; }
		}

		public string DisplayUrl 
		{
			get { return Strings.Wrap((Strings.Clean(url)), 85, "\n"); }
		}

		/// <summary>
		/// The VolumeNumber property of the record
		/// Currently used for Cancer Bulletin search
		/// </summary>
		private string volumeNumber = "";
		public string VolumeNumber
		{
			get { return volumeNumber; }
		}


		/// <summary>
		/// Creates a new instance of the CancerGov.Search.EndecaSearching class by getting data from an ERec
		/// </summary>
		/// <param name="record"></param>
		public EndecaResult(ERec record)
		{
			// Ahh... ERec properties.  Well, gee, what could these be?  The world may never know.  Because...
			// IT IS NOT DOCUMENTED IN ONE SINGLE PLACE!!!
			//
			// A property could be, something from the property mapper, these are the properties we set up, so
			// like Title, MetaTitle, Description
			//
			// Then they could be things like DGraph.WhyDidItMatch which tells us why a record matched.
			// See that is all I know, but there are more, like what was the documents score?  Don't know.
			//
			// Also note, that this is not really a hashtable, is is a PropertyMap, why does that matter?
			// because some things like DGraph.WhyDidItMatch are collections of properties.
			//
			//  While the NDocs say that record.Properties["Title"] will throw an exception, the documentation
			//  says that is the way to go.  I have tried it and it does work.  But it may change.

			//Set the title
			if (record.Properties.Contains("MetaTitle")) 
			{
				title = (string)record.Properties["MetaTitle"];
			} 
			else if (record.Properties.Contains("Title")) 
			{
				title = (string)record.Properties["Title"];
			}
			if (title.Trim().Length==0)
			{
				title = "Untitled";
			}

			bool show_summary=false;
			string set=ConfigurationSettings.AppSettings["EndecaDocSummaryMode"];
			if (set!=null && set.ToLower().Equals("true"))
				show_summary=true;

			//Set the description
			if (record.Properties.Contains("Description")) {
				description = (string)record.Properties["Description"];
			} else if (show_summary && record.Properties.Contains("Doc_Summary")) {
				description = (string)record.Properties["Doc_Summary"];
			}
			if (description.Trim().Length==0)
			{
				description = "No Description";
			}

			//Set the url
			if (record.Properties.Contains("URL")) {
				url = (string)record.Properties["URL"];
			}

			//Set VolumeNumber
			if (record.Properties.Contains("NCI.VolumeNumber")) 
			{
				volumeNumber = (string)record.Properties["NCI.VolumeNumber"];
			}

		}
	}
}
