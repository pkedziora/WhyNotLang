using Markdig;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhyNotLang.EmbeddedResources.Reader;

namespace WhyNotLang.Web.Pages
{
    public class ReferenceBase : ComponentBase
    {
        [Inject] IResourceReader SampleReader { get; set; }

        public MarkupString Content { get; set; }

        protected override void OnInit()
        {
            var reference = SampleReader.ReadReference();
            var html = Markdig.Markdown.ToHtml(reference, new MarkdownPipelineBuilder().UseAdvancedExtensions().Build());
            Content = new MarkupString(html);
        }
    }
}
