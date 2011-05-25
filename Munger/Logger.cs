using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace Munger
{
    public class Logger
    {
        public Logger()
        {
            fileOutput = new StreamWriter("LOG-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-URL-Munging.log"); ;
        }

        StreamWriter fileOutput = null;


        public void OutputError(string module, string errorType, string pageurl, string mungedUrl, string errorString)
        {
            OutputToLog(module, errorType, pageurl, mungedUrl, errorString, true);
        }
        public void OutputLine(string module, string errorType, string pageurl, string mungedUrl, string outputString)
        {
            OutputToLog(module, errorType, pageurl, mungedUrl, outputString, true);
        }
        public void Output(string module, string errorType, string pageurl, string mungedUrl, string outputString)
        {
            OutputToLog(module, errorType, pageurl, mungedUrl, outputString, false);
        }
        public void OutputWarning(string module, string errorType, string pageurl, string mungedUrl, string errorString)
        {
            OutputToLog(module, errorType, pageurl, mungedUrl, errorString, true);
        }
        private void OutputToLog(string module, string errorType, string pageurl, string mungedUrl, string message, bool isLine)
        {
            string stamp = DateTime.Now.ToString("MM-dd HH:mm:ss");

            string outputText = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", stamp, module, errorType, pageurl, mungedUrl, message);

            if (isLine)
                fileOutput.WriteLine(outputText);
            else
                fileOutput.Write(outputText);
            fileOutput.Flush();
        }

        public void CloseOutputFile()
        {
            if (fileOutput == null)
                fileOutput.Close();

        }

    }
}
