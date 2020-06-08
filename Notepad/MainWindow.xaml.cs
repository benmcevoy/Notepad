using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Notepad.Internal;

namespace Notepad
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var state = new State {Owner = this, TextBox = Text};
            new Internal.Plugins().Register(state);

            DataContext = new ViewModel(state);

            Text.Focus();
        }

        private void Text_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            (DataContext as ViewModel).SetDirty(true);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            (DataContext as ViewModel).Closing(e);
        }

        private void Text_OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var data = e.Data.GetData(DataFormats.FileDrop) as string[];

                if (data == null || data.Length == 0)
                {
                    e.Handled = false;
                    return;
                }

                // possibly dragged a bunch of files on , only take first
                (DataContext as ViewModel).FileOpen.Execute(data[0]);
                e.Handled = true;
            }
        }

        private void Text_OnPreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        public void Open(string argument)
        {
            (DataContext as ViewModel).FileOpen.Execute(argument);
        }

       
    }
}
