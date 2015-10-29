using MoviePicker.Magic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MoviePicker.UI
{
    public class PreviewSelectionsViewModel : INotifyPropertyChanged
    {
        public PreviewSelectionsViewModel(int rowCount, int columnCount, IEnumerable<Movie> randomMovieSource)
        {
            this.RowCount = rowCount;
            this.ColumnCount = columnCount;
            this.RandomMovieSource = randomMovieSource;
            this.RefreshSelectionsCommand = new RelayCommand(_ => this.RefreshSelections());
            this.PlayMovieCommand = new RelayCommand(m => this.PlayMovie((Movie)m));
            this.RefreshSelections();
        }

        public int RowCount { get; }
        public int ColumnCount { get; }
        public IEnumerable<Movie> RandomMovieSource { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private List<Movie> currentSelections;
        public List<Movie> CurrentSelections
        {
            get { return this.currentSelections; }
            set { this.currentSelections = value; this.OnPropertyChanged(); }
        }

        public event EventHandler ResetView;

        public ICommand RefreshSelectionsCommand { get; }
        public void RefreshSelections()
        {
            this.ResetView?.Invoke(this, EventArgs.Empty);
            this.CurrentSelections = this.RandomMovieSource.Take(this.RowCount * this.ColumnCount).ToList();
        }

        public ICommand PlayMovieCommand { get; }
        public void PlayMovie(Movie movie)
        {
            movie.Execute();
            this.CloseView?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CloseView;
    }
}
