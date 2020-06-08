using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Notepad.Internal;

namespace Notepad.Commands
{
    public class ShowAllCommands : Command
    {
        public ShowAllCommands(State state) : base(state) { }

        public override void Execute(object parameter)
        {
            var dialog = new Dialog
            {
                Height = 480,
                Width = 640,
            };

            dialog.ShowDialog(State.Owner, Message(), "Notepad", DialogButton.OK, MessageBoxImage.Information);
        }

        public override string Name { get; } = "Show all commands";
        public override InputBinding[] ApplicationInputBindings()
        {
            return new[] { new KeyBinding(this, Key.P, ModifierKeys.Control | ModifierKeys.Shift) };
        }

        public override MenuItem ContextMenuItem { get; }

        private static string Message()
        {
            var sb = new StringBuilder();
            var keyGestureConverter = new KeyGestureConverter();

            foreach (var command in Plugins.Commands)
            {
                var gestures = command.ApplicationInputBindings() ?? new InputBinding[] { };
                var keyBindings = "";
                var delimiter = "";

                foreach (var gesture in gestures)
                {
                    if (!(gesture is KeyBinding g)) continue;

                    keyBindings += $"{delimiter}{keyBindings}({keyGestureConverter.ConvertTo(g.Gesture, typeof(string))})";
                    delimiter = ", ";
                }

                sb.AppendLine($"{command.Name} {keyBindings}");
            }

            return sb.ToString();
        }
    }
}
