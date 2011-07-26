using System;
using System.Collections;
using Endeca.Navigation;
using NCI.Util;

namespace NCI.Search.Endeca
{
	/// <summary>
	/// Summary description for EndecaBestBetResult.
	/// </summary>
	public class EndecaBestBetResult
	{
		private string categoryName = "";
		private Guid listID;
		private string categoryID;
 
		public string CategoryName {
			get { return categoryName; }
		}

		public Guid ListID {
			get { return listID; }
		}

		public string CategoryID {
			get { return categoryID; }
		}

		public EndecaBestBetResult(ERec record)
		{

			//NOTE: These properties should all be in the record, it just throws exceptions when they are not

			//CategoryName is title for now.
			if (record.Properties.Contains("Title")) {
				categoryName = record.Properties["Title"].ToString();
			}

			if (record.Properties.Contains("ListID")) {
				listID = Strings.ToGuid(record.Properties["ListID"].ToString());
			}

			if (record.Properties.Contains("CatID")) {
				categoryID = record.Properties["CatID"].ToString();
			}

		}
	}
}
