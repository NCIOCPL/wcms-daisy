using System;
using System.Collections.Generic;
using System.Text;
using Endeca.Navigation;

namespace NCI.Search.Endeca
{
    public class EndecaRecordSearchDefinition : IEndecaSearchDefinition
    {
        private string _recordID;

        public string RecordID
        {
            get { return _recordID; }
            set { _recordID = value; }
        }

        public EndecaRecordSearchDefinition()
        {
            
        }

        #region IEndecaSearchDefinition Members

        public void SetupQuery(ENEQuery searchQuery)
        {
            if (!String.IsNullOrEmpty(_recordID))
            {
                searchQuery.ERecs = new ERecIdList();
                searchQuery.ERecs.Add(_recordID);
            }
        }

        #endregion

    }
}
