using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Notepad.Commands
{
    public class InsertDateTime : Command
    {
        private readonly State _state;

        public InsertDateTime(State state) : base(state) => _state = state;

        public override void Execute(object parameter)
        {
            InsertText(_state.TextBox, $"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}");
        }

        private static void InsertText(TextBox textBox, string text)
        {
            textBox.SelectedText = text;
            textBox.CaretIndex += text.Length;
            textBox.SelectionLength = 0;
        }

        public override string Name { get; } = "Insert date";
        public override InputBinding[] ApplicationInputBindings()  => new InputBinding[]
            {
                new KeyBinding(this, Key.F5, ModifierKeys.None )
            };

        public override string ContextMenuParentName { get; } = "Insert";

        public override MenuItem ContextMenuItem
        {
            get => new MenuItem { Header = "Date and time", Command = this, InputGestureText = "F5" };
        }
    }
}
