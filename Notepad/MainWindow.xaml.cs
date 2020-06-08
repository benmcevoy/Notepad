using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Notepad.Internal;

namespace Notepad
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var state = new State(this, Text);
            new Plugins().Register(state);

            DataContext = new ViewModel(state);
            Text.Focus();
        }

        internal void Open(string argument) => ViewModel.FileOpen.Execute(argument);

        private ViewModel ViewModel => DataContext as ViewModel;
        private void Text_OnTextChanged(object sender, TextChangedEventArgs e) => ViewModel.SetDirty(true);
        private void MainWindow_OnClosing(object sender, CancelEventArgs e) => ViewModel.Closing(e);

        private void Text_OnDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            var data = e.Data.GetData(DataFormats.FileDrop) as string[];

            if (data == null || data.Length == 0)
            {
                e.Handled = false;
                return;
            }

            // possibly dragged a bunch of files on, only take first
            ViewModel.FileOpen.Execute(data[0]);

            e.Handled = true;
        }

        private void Text_OnPreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }
    }
}
