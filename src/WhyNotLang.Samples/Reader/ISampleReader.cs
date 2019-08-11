using System.Collections.Generic;

namespace WhyNotLang.Samples.Reader
{
    public interface ISampleReader
    {
        string ReadSample(string fileName);
        string ReadReference();
        string FindProgramNameCaseInsensitive(string programName);
        IList<string> GetSampleList();
    }
}