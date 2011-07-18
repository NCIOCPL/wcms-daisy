using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE
{

    /// <summary>
    /// Defines a delegate which modifies the value which field filter passes
    /// </summary>
    public delegate void FieldFilterDelegate(string name, FieldFilterData fieldFilterData);
   
}
