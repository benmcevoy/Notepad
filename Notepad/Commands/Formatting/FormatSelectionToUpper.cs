using System.Windows.Controls;
using System.Windows.Input;

namespace Notepad.Commands.Formatting
{
    public class FormatSelectionToUpper : Command
    {
        public FormatSelectionToUpper(State state) : base(state) { }

        public override void Execute(object parameter)
        {
            var raw = State.TextBox.SelectedText;

            State.TextBox.SelectedText = raw.ToUpper();
        }

        public override string Name { get; } = "Format selection as upper case";
        public override InputBinding[] ApplicationInputBindings()
        {
            return new InputBinding[]
                {
                    new KeyBinding(this, Key.U, ModifierKeys.Control | ModifierKeys.Shift),
                };
        }

        public override string ContextMenuParentName { get; } = "Format";

        public override MenuItem ContextMenuItem
        {
            get => new MenuItem { Header = "To upper", Command = this, InputGestureText = "Ctrl+Shift+U" };
        }
    }
}
