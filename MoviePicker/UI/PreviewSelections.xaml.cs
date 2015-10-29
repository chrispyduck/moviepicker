using MoviePicker.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoviePicker.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class PreviewSelections : Window
    {
        public PreviewSelections(PreviewSelectionsViewModel model)
        {
            InitializeComponent();
            this.ViewModel = model;
        }

        public PreviewSelectionsViewModel ViewModel
        {
            get { return (PreviewSelectionsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(PreviewSelectionsViewModel), typeof(PreviewSelections), new PropertyMetadata(null, ViewModelChanged));
        private static void ViewModelChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var me = (PreviewSelections)sender;

            var old = args.OldValue as PreviewSelectionsViewModel;
            if (old != null)
            {
                old.ResetView -= me.OnResetView;
                old.CloseView -= me.OnCloseView;
            }

            var @new = args.NewValue as PreviewSelectionsViewModel;
            if (@new != null)
            {
                @new.ResetView += me.OnResetView;
                @new.CloseView += me.OnCloseView;
            }
        }
        private void OnResetView(object sender, EventArgs e)
        {
            this.currentRow = 0;
            this.currentColumn = 0;
        }
        private void OnCloseView(object sender, EventArgs e)
        {
            this.Close();
        }

        private int currentRow = 0;
        private int currentColumn = 0;

        private void GridLayoutUpdated(object sender, EventArgs e)
        {
            // find grid
            var itemsPresenter = this.Generator.GetVisualChild<ItemsPresenter>();
            var grid = (Grid)VisualTreeHelper.GetChild(itemsPresenter, 0);

            while (grid.ColumnDefinitions.Count < this.ViewModel.ColumnCount)
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            while (grid.RowDefinitions.Count < this.ViewModel.RowCount)
                grid.RowDefinitions.Add(new RowDefinition());

            // assign grid cells
            foreach (var child in grid.Children.OfType<FrameworkElement>().Where(m => m.Tag == null))
            {
                // compute next cell
                if (this.currentColumn >= this.ViewModel.ColumnCount)
                {
                    this.currentColumn = 0;
                    this.currentRow++;
                }
                
                // assign cell to child
                Grid.SetRow(child, this.currentRow);
                Grid.SetColumn(child, this.currentColumn);

                // tag as assigned so we don't process it again
                child.Tag = true;

                // move forward
                this.currentColumn++;
            }
        }
    }
}
