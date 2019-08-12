using System.Collections.Generic;

namespace WhyNotLang.EmbeddedResources.Reader
{
    public interface IResourceReader
    {
        string ReadSample(string fileName);
        string ReadReference();
        string FindProgramNameCaseInsensitive(string programName);
        IList<string> GetSampleList();
    }
}