using Blazor.Extensions.Storage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhyNotLang.EmbeddedResources.Reader;
using WhyNotLang.Interpreter;
using WhyNotLang.Tokenizer;

namespace WhyNotLang.Web.Components
{
    public class EditorBase : ComponentBase
    {
        [Inject] IExecutor Executor { get; set; }
        [Inject] IResourceReader SampleReader { get; set; }
        [Inject] IJSRuntime JsRuntime { get; set; }
        [Inject] IUriHelper UriHelper { get; set; }
        [Inject] LocalStorage LocalStorage { get; set; }
        [Parameter] public string SelectedSample { get; set; } = "";

        public string programCode { get; set; }
        protected string lastSavedKey = "LastSaved";
        protected TextIO textIO { get; set; }
        protected bool isRunning { get; set; } = false;
        protected List<string> CodeSamples { get; set; } = new List<string>();
        private readonly string _localStorageKey = "customProgram";

        protected override async Task OnInitAsync()
        {
            CodeSamples = SampleReader.GetSampleList().ToList();
            var fromStorage = await LocalStorage.GetItem<string>(_localStorageKey);
            if (string.IsNullOrWhiteSpace(SelectedSample))
            {
                if (!string.IsNullOrEmpty(fromStorage))
                {
                    SelectedSample = lastSavedKey;
                }
                else
                {
                    SelectedSample = "Pong";
                }
            }

            programCode = await ReadSample(SelectedSample);
        }

        protected async Task Save()
        {
            await LocalStorage.SetItem(_localStorageKey, programCode);
        }

        protected override async Task OnAfterRenderAsync()
        {
            await JsRuntime.InvokeAsync<string>("WhyNotLang.Text.allowTextAreaTabs", "txtProgramCode");
        }

        protected async Task OnSampleSelected(UIChangeEventArgs e)
        {
            var sampleName = e.Value.ToString();
            programCode = await ReadSample(sampleName);
            UriHelper.NavigateTo($"/sample/{sampleName.ToLower()}");
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

        private async Task<string> ReadSample(string sampleName)
        {
            var fromStorage = await LocalStorage.GetItem<string>(_localStorageKey);
            if (sampleName.Equals(lastSavedKey, StringComparison.OrdinalIgnoreCase))
            {
                SelectedSample = lastSavedKey;
                return fromStorage ?? "";
            }

            SelectedSample = SampleReader.FindProgramNameCaseInsensitive(sampleName);
            return SampleReader.ReadSample(SelectedSample);
        }
    }
}
