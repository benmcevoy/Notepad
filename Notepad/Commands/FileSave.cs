using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Notepad.Commands
{
    public class FileSave : ICommand
    {
        private readonly State _state;
        public FileSave(State state) => _state = state;

        public void Execute(object parameter)
        {
            if (_state.File == null)
            {
                new FileSaveAs(_state).Execute(null);
                return;
            }

            Save(_state);
            _state.Status = "File saved.";
        }

        public static void Save(State state)
        {
            try
            {
                File.WriteAllText(state.File.FullName, state.TextBox.Text, Encoding.UTF8);
                state.IsDirty = false;
            }
            catch (IOException exception)
            {
                DialogHelper.Error(state.Owner, exception.Message);
            }
        }

        public bool CanExecute(object parameter) => true;
        public event EventHandler CanExecuteChanged;
    }
}
