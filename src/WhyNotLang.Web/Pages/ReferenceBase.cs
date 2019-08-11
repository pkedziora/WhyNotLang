using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhyNotLang.Samples.Reader;

namespace WhyNotLang.Web.Pages
{
    public class ReferenceBase : ComponentBase
    {
        [Inject] ISampleReader SampleReader { get; set; }

        protected override void OnInit()
        {
            Console.WriteLine(SampleReader.ReadReference());
            base.OnInit();
        }
    }
}
