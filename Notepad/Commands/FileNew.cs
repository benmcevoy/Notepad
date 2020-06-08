using System;
using System.Windows;
using System.Windows.Input;

namespace Notepad.Commands
{
    public class FileNew : ICommand
    {
        private readonly State _state;
        public FileNew(State state) => _state = state;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (_state.File != null || _state.IsDirty)
            {
                var result = DialogHelper.SaveDontSave(_state.Owner,$"Do you want to save changes to {_state.FileName}?");

                if (result == MessageBoxResult.Cancel) return;

                if (result == MessageBoxResult.Yes)
                {
                    FileSave.Save(_state);
                    return;
                }
            }

            _state.File = null;
            _state.TextBox.Text = "";
            _state.IsDirty = false;
        }

        public event EventHandler CanExecuteChanged;

       // public string Name { get; } = "Create new file";
        //public InputBinding[] ApplicationInputBindings() => new[] {new KeyBinding(this, Key.N, ModifierKeys.Control)};
       // public MenuItem ContextMenuItem { get; }
    }
}
