using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace Notepad.Commands
{
    public class FileOpen : ICommand
    {
        private readonly State _state;
        public FileOpen(State state) => _state = state;

        public void Execute(object parameter)
        {
            if (_state.IsDirty)
            {
                var result = DialogHelper.SaveDontSave(_state.Owner, $"Do you want to save changes to {_state.FileName}?");

                if (result == MessageBoxResult.Cancel) return;

                if (result == MessageBoxResult.Yes)
                {
                    FileSave.Save(_state);
                }
            }

            if (parameter != null)
            {
                OpenFileFromParameter(parameter as string);
                return;
            }

            OpenFileFromDialog();
        }

        private void OpenFileFromParameter(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter)) return;
            if (!File.Exists(parameter)) return;

            Open(parameter);
        }

        private void OpenFileFromDialog()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "All Files|*.*|Text (.txt)|*.txt"
            };

            if (dialog.ShowDialog() != true) return;

            Open(dialog.FileName);
        }

        private void Open(string file)
        {
            try
            {
                _state.TextBox.Text = File.ReadAllText(file);
                _state.File = new FileInfo(file);
                _state.Status = _state.File.Name;
                _state.IsDirty = false;
            }
            catch (IOException exception)
            {
                DialogHelper.Error(_state.Owner, exception.Message);
            }
        }

        public bool CanExecute(object parameter) => true;
        public event EventHandler CanExecuteChanged;
    }
}
