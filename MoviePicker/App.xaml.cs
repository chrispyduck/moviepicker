using MoviePicker.Magic;
using MoviePicker.UI;
using Ookii.CommandLine;
using System;
using System.IO;
using System.Windows;


namespace MoviePicker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Arguments Arguments { get; private set; }

        public DirectoryInfo TargetDirectory => !string.IsNullOrEmpty(this.Arguments.Directory)
            ? new DirectoryInfo(this.Arguments.Directory)
            : new DirectoryInfo(Environment.CurrentDirectory);
        
        protected override void OnStartup(StartupEventArgs e)
        {
            var parser = new CommandLineParser(typeof(Arguments));
            this.Arguments = (Arguments)parser.Parse(e.Args);
            if (this.Arguments.DisplayUsage)
            {
                using (var tw = new StringWriter())
                {
                    parser.WriteUsage(tw, 130);
                    MessageBox.Show(tw.ToString(), "MoviePicker Usage");
                }
                this.Shutdown(0);
            }

            var folder = new MediaFolder(this.TargetDirectory);
            var model = new PreviewSelectionsViewModel(this.Arguments.Rows, this.Arguments.Columns, folder.RandomMovies);
            var preview = new PreviewSelections(model);
            this.MainWindow = preview;
            preview.Show();
        }
    }
}
