using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MoviePicker.Magic
{
    public class MediaFolder
    {
        public MediaFolder(DirectoryInfo dir)
        {
            this.directory = dir;

            var sw = new Stopwatch();
            sw.Start();
            this.movieDirs = dir.EnumerateDirectories()
               // ignore system and hidden folders
               .Where(d => d.Exists && (d.Attributes & (FileAttributes.System | FileAttributes.Hidden)) == 0)
               .Select(d => new MovieStub(d))
               .ToList();
            sw.Stop();
            Debug.WriteLine("Loading MediaFolder took {0}ms", sw.ElapsedMilliseconds);
        }

        private readonly DirectoryInfo directory;
        private readonly List<MovieStub> movieDirs;

        public List<Movie> Movies { get; } = new List<Movie>();

        public List<Movie> InvalidMovieDirs { get; } = new List<Movie>();
        public Dictionary<DirectoryInfo, Exception> Exceptions { get; } = new Dictionary<DirectoryInfo, Exception>();

        public IEnumerable<Movie> RandomMovies
        {
            get
            {
                while (true)
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    var dir = this.movieDirs.GetRandom();
                    Movie movie = null;
                    try
                    {
                        movie = dir.Movie;
                    }
                    catch (Exception e)
                    {
                        this.Exceptions.Add(dir.Directory, e);
                        this.movieDirs.Remove(dir);
                    }

                    if (movie == null)
                        continue;

                    if (!movie.IsValid)
                    {
                        this.InvalidMovieDirs.Add(movie);
                        this.movieDirs.Remove(dir);
                        continue;
                    }

                    sw.Stop();
                    Debug.WriteLine("MediaFolder.RandomMovies.take(1) took {0}ms", sw.ElapsedMilliseconds);
                    yield return movie;                    
                }

            }
        }
    }
}
