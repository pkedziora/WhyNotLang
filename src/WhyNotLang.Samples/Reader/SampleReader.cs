using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WhyNotLang.Samples.Reader
{
    public class SampleReader : ISampleReader
    {
        public string Read(string programName)
        {
            if (string.IsNullOrWhiteSpace(programName))
            {
                return string.Empty;
            }

            var assembly = typeof(SampleReader).GetTypeInfo().Assembly;
            string contents;
            using (var stream = assembly.GetManifestResourceStream($"WhyNotLang.Samples.{programName}.wnl"))
            {
                using (var reader = new StreamReader(stream))
                {
                    contents = reader.ReadToEnd();
                }
            }

            return contents;
        }

        public string FindProgramNameCaseInsensitive(string programName)
        {
            if (string.IsNullOrWhiteSpace(programName))
            {
                return string.Empty;
            }

            var sampleList = GetSampleList();
            programName = programName.ToLower();
            return sampleList.FirstOrDefault(sample => sample.ToLower() == programName);
        }

        public IList<string> GetSampleList()
        {
            var assembly = typeof(SampleReader).GetTypeInfo().Assembly;
            var assemblyName = assembly.GetName().Name;
                return assembly
                    .GetManifestResourceNames()
                    .Where(r => r.EndsWith(".wnl"))
                    .Select(r => r.Replace($"{assemblyName}.", "").Replace(".wnl", ""))
                    .ToList();
        }
    }
}
