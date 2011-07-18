using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE
{
    /// <summary>
    /// This file contains  immutable string constants, int constants, etc, enums 
    /// </summary>
    /// 
    public enum DisplayVersion
    {
        Image = 1,
        [Obsolete("Text-only is dead.  Long live text-only!")]
        Text = 2,
        Print = 3
    }

    public enum DisplayLanguage
    {
        English = 1,
        Spanish = 2
    }

    public struct DisplayInformation
    {
        public DisplayVersions Version;
        public DisplayLanguage Language;
    }

    /// <summary>
    /// Used with PopEmail.aspx to specify where the "Email this Page"
    /// pop up was invoked from.
    /// </summary>
    public enum EmailPopupInvokedBy
    {
        Unspecified = 0,
        ClinicalTrialSearchResults = 1,
        ClinicalTrialPrintableSearchResults = 2
    }

    ///<summary>
    ///Defines values for application error type constants<br/>
    ///</summary>
    public class ErrorType
    {
        //Data type errors
        public static int InvalidGuid = 100;
        public static int InvalidArgument = 101;

        //Database errors
        public static int DbUnavailable = 200;
        public static int DbNoData = 201;

        //Include file errors
        public static int FileNotFound = 300;

        //Endeca errors
        public static int EndecaError = 400;


        //XML errors
        public static int XmlStringParseError = 500;
    }

    //public enum PDQVersion
    //{
    //    Unknown = -1,
    //    Patient = 0,
    //    HealthProfessional = 1
    //}

}
