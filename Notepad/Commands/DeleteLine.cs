using System;
using System.Windows.Input;

namespace Notepad.Commands
{
    public class DeleteLine : ICommand
    {
        private readonly State _state;
        public DeleteLine(State state) => _state = state;

        public void Execute(object parameter)
        {
            var currentLine = _state.TextBox.GetLineIndexFromCharacterIndex(_state.TextBox.CaretIndex);

            _state.TextBox.SelectionStart = 0;
            _state.TextBox.SelectionLength = _state.TextBox.GetLineLength(currentLine);
            
            _state.TextBox.SelectedText = "";
            _state.TextBox.SelectionLength = 0;
        }

        public bool CanExecute(object parameter) => !string.IsNullOrEmpty(_state.TextBox.Text);
        public event EventHandler CanExecuteChanged;
    }
}
