using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Diagnostics;

namespace MoviePicker.Magic
{
    /// <summary>
    /// Represents a folder that contains a movie
    /// </summary>
    public class Movie
    {
        private static readonly Regex[] MovieFilePatterns = new[]
        {
            new Regex("\\.m[k4]v$", RegexOptions.Compiled),
            new Regex("\\.avi$"),
            new Regex("\\.mp[4g]$"),
            new Regex("^VIDEO_TS\\.IFO$")
        };
        private static readonly Regex DirectoryNameParser = new Regex(@"^(?<title>.*?) \((?<year>[0-9]{4})\)( \[.*?\])*$", RegexOptions.ExplicitCapture);

        public Movie(DirectoryInfo containingFolder)
        {
            var sw = new Stopwatch();
            sw.Start();

            this.directory = containingFolder;
            this.metadata = new Lazy<Metadata>(GetMetadata);

            this.MediaFile = this
                .GetFiles("*.*")
                .FirstOrDefault(f => MovieFilePatterns.Any(p => p.IsMatch(f.Name)));

            this.PosterFile = this
                .GetFiles("*.jpg")
                .OrderByDescending(i => i.Length)
                .FirstOrDefault();

            this.ExecuteCommand = new RelayCommand(
                _ => this.Execute(),
                _ => this.IsValid);

            sw.Stop();
            Debug.WriteLine("Movie({0}).ctor took {1}ms", this, sw.ElapsedMilliseconds);
        }

        private readonly DirectoryInfo directory;

        private IEnumerable<FileInfo> GetFiles(string pattern) => this
            .directory
            .GetFileSystemInfos(pattern)
            .OfType<FileInfo>();

        /// <summary>
        /// Gets a <see cref="FileInfo"/> instance for the first file likely to be a movie in this <see cref="Movie"/>
        /// </summary>
        public FileInfo MediaFile { get; }

        public FileInfo PosterFile { get; }
        public bool HasPoster => this.PosterFile != null;
        
        private readonly Lazy<Metadata> metadata;
        private Metadata GetMetadata()
        {
            var sw = new Stopwatch();
            sw.Start();
            var meta = this
                .GetFiles("*.nfo")
                .Select(f => Metadata.Deserialize(f))
                .FirstOrDefault();
            if (meta != null)
                return meta;

            meta = new Metadata();

            var match = DirectoryNameParser.Match(this.directory.Name);
            if (match.Success)
            {
                meta.Title = match.Groups["title"].Value;
                int year;
                if (int.TryParse(match.Groups["year"].Value, out year))
                    meta.Year = year;
            }
            else
                meta.Title = this.directory.Name;

            sw.Stop();
            Debug.WriteLine("GetMetadata({0}) took {1}ms", this, sw.ElapsedMilliseconds);
            return meta;
        }

        public Metadata Metadata => this.metadata.Value;

        public string Title => string.Format("{0} ({1})", this.Metadata.Title, this.Metadata.Year);
        public bool IsValid => this.MediaFile != null;
        public string Summary => this.Metadata.Summary;

        public ICommand ExecuteCommand { get; }

        public void Execute()
        {
            var processStartInfo = new ProcessStartInfo();
            processStartInfo.UseShellExecute = true;
            processStartInfo.FileName = this.MediaFile.FullName;
            Process.Start(processStartInfo);
        }

        public override string ToString()
        {
            return this.metadata.IsValueCreated ? this.Title : this.directory.Name;
        }
    }
}
