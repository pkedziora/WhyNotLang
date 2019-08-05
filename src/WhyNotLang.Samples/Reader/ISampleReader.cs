using System.Collections.Generic;

namespace WhyNotLang.Samples.Reader
{
    public interface ISampleReader
    {
        string Read(string fileName);
        IList<string> GetSampleList();
    }
}