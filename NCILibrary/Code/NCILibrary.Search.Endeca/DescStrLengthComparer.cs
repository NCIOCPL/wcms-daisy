using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace NCI.Search.Endeca
{
    public class DescStrLengthComparer : IComparer
    {
        // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
        int IComparer.Compare(Object x, Object y)
        {
            String str1 = (String)x;
            String str2 = (String)y;
            return (str2.Length - str1.Length);
        }
    }
}
