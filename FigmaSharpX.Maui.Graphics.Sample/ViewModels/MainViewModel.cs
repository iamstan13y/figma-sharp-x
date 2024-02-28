﻿using FigmaSharpX.Maui.Graphics.Sample.PropertyConfigure;
using FigmaSharpX.Maui.Graphics.Sample.Services;
using FigmaSharpX.Services;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace FigmaSharpX.Maui.Graphics.Sample.ViewModels
{
    public class MainViewModel : BindableObject
    {
        string _token;
        string _fileId;

        bool _isGenerating;
        string _code;
        ObservableCollection<string> _log;

        readonly Compiler _compiler;

        public MainViewModel()
        {
#if DEBUG
            // INSERT YOUR FIGMA ACCESS TOKEN
            Token = "";
            // INSERT THE FILE ID
            FileId = "";
#endif
            Log = new ObservableCollection<string>();
            _compiler = new Compiler();
        }

        public string Token
        {
            get { return _token; }
            set
            {
                _token = value;
                OnPropertyChanged();
            }
        }

        public string FileId
        {
            get { return _fileId; }
            set
            {
                _fileId = value;
                OnPropertyChanged();
            }
        }

        public bool IsGenerating
        {
            get { return _isGenerating; }
            set
            {
                _isGenerating = value;
                OnPropertyChanged();
            }
        }

        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Log
        {
            get { return _log; }
            set
            {
                _log = value;
                OnPropertyChanged();
            }
        }

        public ICommand GenerateCommand => new Command(async () => await GenerateCodeAsync());
        public ICommand ExportCommand => new Command(async () => await Export());

        async Task GenerateCodeAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(Token))
                {
                    var message = "In order to obtain the necessary information from Figma, it is necessary to use a Personal Access Token.";
                    Log.Add(message);
                    DialogService.Instance.DisplayAlert("Information", message);
                    return;
                }

                FigmaApplication.Init(Token);
                if (string.IsNullOrEmpty(FileId))
                {
                    var message = "In order to obtain the necessary information from Figma, it is necessary to use a FileId.";
                    Log.Add(message);
                    DialogService.Instance.DisplayAlert("Information", message);
                    return;
                }
                IsGenerating = true;
                Log.Add("Request the data to the Figma API.");

                var remoteNodeProvider = new RemoteNodeProvider();
                await remoteNodeProvider.LoadAsync(FileId);

                Log.Add($"Data obtained successfully. {remoteNodeProvider.Nodes.Count} nodes found.");

                Log.Add("Initializing the code generator.");

                var converters = AppContext.Current.GetFigmaConverters();
                var codePropertyConfigure = new CodePropertyConfigure();
                var codeRenderer = new CodeRenderService(remoteNodeProvider, converters, codePropertyConfigure);

                Log.Add("Code generator initialized successfully.");

                var stringBuilder = new StringBuilder();

                var node = codeRenderer.NodeProvider.Nodes[0];

                Log.Add($"Node {node.id} found successfully.");

                Log.Add("Generating source code...");

                var codeNode = new CodeNode(node);

                codeRenderer.GetCode(stringBuilder, codeNode);

                var code = stringBuilder.ToString();

                Code = code;

                Log.Add("Source Code generated successfully.");

                await CompileCodeAsync();
            }
            catch (Exception ex)
            {
                Log.Add(ex.Message);
            }
            finally
            {
                IsGenerating = false;
            }
        }

        async Task CompileCodeAsync()
        {
            if (_compiler == null)
                return;

            Log.Add("Compiling the generated source code...");

            string sourceCode = string.Format(@"         
                using Microsoft.Maui.Graphics;

                public void Draw(ICanvas canvas, RectF dirtyRect)
                {{
                {0}
                }}", Code);

            var compilationResult = await _compiler.CompileAsync(sourceCode);

            if (compilationResult.HasErrors)
            {
                var compilationMessages = compilationResult.CompilationMessages;

                foreach (var error in compilationMessages)
                {
                    Log.Add($"{error.DisplayMessage}");
                }
            }
            else
            {
                Log.Add("Compilation completed successfully.");
            }
        }

        async Task Export()
        {
            if (string.IsNullOrEmpty(Code))
            {
                string message = "The generated code is not correct.";
                Log.Add(message);
                DialogService.Instance.DisplayAlert("Information", message);
                return;
            }

#if MACCATALYST || WINDOWS
            try
            {
                var folderPicker = new FolderPicker();
                string folder = await folderPicker.PickFolder();
                string path = Path.Combine(folder, "FigmaToMauiGraphics.txt");
                await File.WriteAllTextAsync(path, Code);

                string message = "The file has been created successfully.";
                Log.Add(message);
                DialogService.Instance.DisplayAlert("Information", message);
            }
            catch(Exception ex)
            {
                Log.Add(ex.Message);
            }
#endif
        }
    }
}