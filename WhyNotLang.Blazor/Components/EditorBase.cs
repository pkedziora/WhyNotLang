using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
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
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] ILocalStorageService LocalStorage { get; set; }
        [Parameter] public string SelectedSample { get; set; } = "";

        public string ProgramCode { get; set; }
        protected string LastSavedKey = "LastSaved";
        protected TextIO TextIO { get; set; }
        protected bool IsRunning { get; set; } = false;
        protected List<string> CodeSamples { get; set; } = new List<string>();
        private readonly string _localStorageKey = "customProgram";

        protected override async Task OnInitializedAsync()
        {
            Stop();
            CodeSamples = SampleReader.GetSampleList().ToList();
            var fromStorage = await LocalStorage.GetItemAsync<string>(_localStorageKey);
            if (string.IsNullOrWhiteSpace(SelectedSample))
            {
                SelectedSample = string.IsNullOrEmpty(fromStorage) ? "Pong" : LastSavedKey;
            }

            ProgramCode = await ReadSample(SelectedSample);
        }

        protected async Task Save()
        {
            await LocalStorage.SetItemAsync(_localStorageKey, ProgramCode);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JsRuntime.InvokeAsync<string>("WhyNotLang.Text.allowTextAreaTabs", "txtProgramCode");
        }

        protected async Task OnSampleSelected(ChangeEventArgs e)
        {
            var sampleName = e.Value.ToString();
            ProgramCode = await ReadSample(sampleName);
            NavigationManager.NavigateTo($"/sample/{sampleName.ToLower()}");
        }

        protected void Stop()
        {
            Executor.Stop();
            IsRunning = false;
        }

        protected async Task Execute()
        {
            TextIO.Clear();
            Executor.ProgramState.Clear();
            IsRunning = true;
            try
            {
                Executor.Initialise(ProgramCode);

                await Executor.ExecuteAll();
            }
            catch (WhyNotLangException ex)
            {
                var msg = ex.LineNumber > 0 ? $"[ERROR] Line {ex.LineNumber}: {ex.Message}" : $"[ERROR] {ex.Message}";
                if (!Executor.Stopped)
                {
                    TextIO.WriteLine(msg);
                }

                Console.WriteLine(msg);
            }

            IsRunning = false;
        }

        private async Task<string> ReadSample(string sampleName)
        {
            var fromStorage = await LocalStorage.GetItemAsync<string>(_localStorageKey);
            if (sampleName.Equals(LastSavedKey, StringComparison.OrdinalIgnoreCase))
            {
                SelectedSample = LastSavedKey;
                return fromStorage ?? "";
            }

            SelectedSample = SampleReader.FindProgramNameCaseInsensitive(sampleName);
            return SampleReader.ReadSample(SelectedSample);
        }
    }
}
