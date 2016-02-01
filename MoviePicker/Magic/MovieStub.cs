using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePicker.Magic
{
    public class MovieStub
    {
        public MovieStub(DirectoryInfo di)
        {
            this.Directory = di;
            this.movie = new Lazy<Movie>(() => new Movie(this.Directory));
        }

        public DirectoryInfo Directory { get; }

        private readonly Lazy<Movie> movie;
        public Movie Movie => this.movie.Value;
    }
}
