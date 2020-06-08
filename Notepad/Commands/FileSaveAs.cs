using System;
using System.IO;
using System.Windows.Input;
using Microsoft.Win32;

namespace Notepad.Commands
{
    public class FileSaveAs : ICommand
    {
        private readonly State _state;
        public FileSaveAs(State state) => _state = state;

        public void Execute(object parameter)
        {
            var dialog = new SaveFileDialog
            {
                DefaultExt = "txt",
                Filter = "All Files|*.*|Text (.txt)|*.txt",
                FileName = _state.FileName,
            };

            if (dialog.ShowDialog() != true) return;

            _state.File = new FileInfo(dialog.FileName);

            FileSave.Save(_state);

            _state.Status = "File saved.";
        }

        public bool CanExecute(object parameter) => true;
        public event EventHandler CanExecuteChanged;
    }
}
