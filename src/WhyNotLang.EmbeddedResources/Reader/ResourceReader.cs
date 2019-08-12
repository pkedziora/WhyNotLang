using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WhyNotLang.EmbeddedResources.Reader
{
    public class ResourceReader : IResourceReader
    {
        public string ReadSample(string sampleName)
        {
            return ReadResource($"WhyNotLang.EmbeddedResources.{sampleName}.wnl");
        }

        public string ReadReference()
        {
            return ReadResource($"WhyNotLang.EmbeddedResources.REFERENCE.md");
        }

        private string ReadResource(string resourceName)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
            {
                return string.Empty;
            }

            var assembly = typeof(ResourceReader).GetTypeInfo().Assembly;
            string contents;
            using (var stream = assembly.GetManifestResourceStream(resourceName))
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
            var assembly = typeof(ResourceReader).GetTypeInfo().Assembly;
            var assemblyName = assembly.GetName().Name;
                return assembly
                    .GetManifestResourceNames()
                    .Where(r => r.EndsWith(".wnl"))
                    .Select(r => r.Replace($"{assemblyName}.", "").Replace(".wnl", ""))
                    .ToList();
        }
    }
}
