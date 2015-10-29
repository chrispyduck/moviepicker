using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MoviePicker.Magic
{
    public class MediaFolder
    {
        public MediaFolder(DirectoryInfo dir)
        {
            this.directory = dir;

            dir.EnumerateDirectories()
               // ignore system and hidden folders
               .Where(d => d.Exists && (d.Attributes & (FileAttributes.System | FileAttributes.Hidden)) == 0)
               // ignore directories that we don't have access to
               .Select(d =>
               {
                   try
                   {
                       return new Movie(d);
                   }
                   catch (Exception se)
                   {
                       this.Exceptions.Add(d, se);
                       return null;
                   }
               })
               // null check required to weed out exceptions from the previous check
               .Where(d => d != null)
               .Split(d => d.IsValid, this.Movies, this.InvalidMovieDirs);
        }

        private readonly DirectoryInfo directory;

        public List<Movie> Movies { get; } = new List<Movie>();

        public List<Movie> InvalidMovieDirs { get; } = new List<Movie>();
        public Dictionary<DirectoryInfo, Exception> Exceptions { get; } = new Dictionary<DirectoryInfo, Exception>();

        public IEnumerable<Movie> RandomMovies
        {
            get
            {
                while (true)
                    yield return this.Movies.GetRandom();
            }
        }
    }
}
