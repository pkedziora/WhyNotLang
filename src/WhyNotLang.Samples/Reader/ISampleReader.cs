using System.Collections.Generic;

namespace WhyNotLang.Samples.Reader
{
    public interface ISampleReader
    {
        string Read(string fileName);
        string FindProgramNameCaseInsensitive(string programName);
        IList<string> GetSampleList();
    }
}