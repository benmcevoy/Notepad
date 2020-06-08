using System;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Notepad.Commands.Formatting
{
    public class FormatSelectionBase64Encode : Command
    {
        public FormatSelectionBase64Encode(State state) : base(state) { }

        public override void Execute(object parameter)
        {
            var raw = State.TextBox.SelectedText;

            State.TextBox.SelectedText = Convert.ToBase64String(Encoding.UTF8.GetBytes(raw));
        }

        public override string Name { get; } = "Format selection as base64";
        public override InputBinding[] ApplicationInputBindings() => null;

        public override MenuItem ContextMenuItem
        {
            get
            {
                var menu = new MenuItem { Header = "Format selection", Name = "Format" };

                menu.Items.Add(new MenuItem { Header = "Base64 encode", Command = this });

                return menu;

            }
        }

        public override bool CanExecute(object parameter) => !string.IsNullOrEmpty(State.TextBox.SelectedText);
    }
}
