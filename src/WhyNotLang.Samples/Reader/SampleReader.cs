using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WhyNotLang.Samples.Reader
{
    public class SampleReader : ISampleReader
    {
        public string Read(string fileName)
        {
            var assembly = typeof(SampleReader).GetTypeInfo().Assembly;
            string contents;
            using (var stream = assembly.GetManifestResourceStream($"WhyNotLang.Samples.{fileName}"))
            {
                using (var reader = new StreamReader(stream))
                {
                    contents = reader.ReadToEnd();
                }
            }

            return contents;
        }

        public IList<string> GetSampleList()
        {
            var assembly = typeof(SampleReader).GetTypeInfo().Assembly;
            var assemblyName = assembly.GetName().Name;
                return assembly
                    .GetManifestResourceNames()
                    .Where(r => r.EndsWith(".wnl"))
                    .Select(r => r.Replace($"{assemblyName}.", ""))
                    .ToList();
        }
    }
}
