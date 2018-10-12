namespace Munger
{
    public interface ILogger
    {
        void CloseOutputFile();
        void Output(string module, string errorType, string pageurl, string mungedUrl, string outputString);
        void OutputError(string module, string errorType, string pageurl, string mungedUrl, string errorString);
        void OutputLine(string module, string errorType, string pageurl, string mungedUrl, string outputString);
        void OutputWarning(string module, string errorType, string pageurl, string mungedUrl, string errorString);
    }
}