using System;
using System.Collections.Generic;
using System.Text;
using Endeca.Navigation;

namespace NCI.Search.Endeca
{
    public interface IEndecaSearchDefinition
    {
        void SetupQuery(ENEQuery searchQuery);

    }
}
