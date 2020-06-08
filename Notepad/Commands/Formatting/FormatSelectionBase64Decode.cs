using System;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Notepad.Commands.Formatting
{
    public class FormatSelectionBase64Decode : Command
    {
        public FormatSelectionBase64Decode(State state) : base(state) { }

        public override void Execute(object parameter)
        {
            var raw = State.TextBox.SelectedText;

            State.TextBox.SelectedText = Encoding.UTF8.GetString(Convert.FromBase64String(raw));
        }

        public override string Name { get; } = "Format selection from base64";
        public override InputBinding[] ApplicationInputBindings() => null;
        public override MenuItem ContextMenuItem
        {
            get
            {
                var menu = new MenuItem { Header = "Format selection", Name = "Format" };

                menu.Items.Add(new MenuItem { Header = "Base64 decode", Command = this });

                return menu;

            }
        }

        public override bool CanExecute(object parameter) => !string.IsNullOrEmpty(State.TextBox.SelectedText);
    }
}
