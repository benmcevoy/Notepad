using System;
using System.Text;
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

        public override MenuItem ContextMenuItem
        {
            get
            {
                var menu = new MenuItem { Header = "Format selection", Name = "Format" };

                menu.Items.Add(new MenuItem { Header = "To upper", Command = this, InputGestureText = "Ctrl+Shift+U" });

                return menu;

            }
        }

        public override bool CanExecute(object parameter) => !string.IsNullOrEmpty(State.TextBox.SelectedText);
    }
}
