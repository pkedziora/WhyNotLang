using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WhyNotLang.Interpreter;
using WhyNotLang.Samples.Reader;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Web.Components
{
    public class EditorBase : ComponentBase
    {
        [Inject] IExecutor Executor { get; set; }
        [Inject] ISampleReader SampleReader { get; set; }
        [Inject] IJSRuntime JsRuntime { get; set; }

        protected TextIO textIO { get; set; }
        protected bool isRunning { get; set; } = false;
        protected List<string> CodeSamples { get; set; } = new List<string>();
        public string programCode { get; set; }

        protected override void OnInit()
        {
            CodeSamples = SampleReader.GetSampleList().ToList();
            var sampleName = CodeSamples.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(sampleName))
            {
                programCode = SampleReader.Read(sampleName);
            }
        }

        protected override async Task OnAfterRenderAsync()
        {
            await JsRuntime.InvokeAsync<string>("WhyNotLang.Text.allowTextAreaTabs", "txtProgramCode");
        }

        protected void OnSampleSelected(UIChangeEventArgs e)
        {
            var sampleName = e.Value.ToString();
            var sampleCode = SampleReader.Read(sampleName);
            programCode = sampleCode;
        }

        protected void Stop()
        {
            Executor.Stop();
            isRunning = false;
        }

        protected async Task Execute()
        {
            textIO.Clear();
            Executor.ProgramState.Clear();
            isRunning = true;
            try
            {
                Executor.Initialise(programCode);

                await Executor.ExecuteAll();
            }
            catch (WhyNotLangException ex)
            {
                string msg;
                if (ex.LineNumber > 0)
                {
                    msg = $"[ERROR] Line {ex.LineNumber}: {ex.Message}";
                }
                else
                {
                    msg = $"[ERROR] {ex.Message}";
                }

                textIO.WriteLine(msg);
                Console.WriteLine(msg);
            }

            isRunning = false;
        }
    }
}
