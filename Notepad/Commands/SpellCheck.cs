using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Notepad.Commands
{
    public sealed class SpellCheck : Command
    {
        private readonly MenuItem _speller;

        public SpellCheck(State state) : base(state)
        {
            _speller = new MenuItem { Name = "Notepad_Commands_SpellCheck", Header = Name, Command = this };

            State.TextBox.ContextMenu.Opened += Suggest;
        }

        private void Suggest(object sender, RoutedEventArgs args)
        {
            _speller.Items.Clear();

            var spellingError = State.TextBox.GetSpellingError(State.TextBox.CaretIndex);

            if (spellingError == null) return;

            var cmdIndex = 0;

            foreach (string suggestion in spellingError.Suggestions)
            {
                var menuItem = new MenuItem
                {
                    Header = suggestion,
                    FontWeight = FontWeights.Bold,
                    Command = EditingCommands.CorrectSpellingError,
                    CommandParameter = suggestion,
                    CommandTarget = State.TextBox
                };

                _speller.Items.Insert(cmdIndex, menuItem);

                cmdIndex++;
            }
        }

        public override void Execute(object parameter) { }
        public override string Name { get; } = "Spelling";
        public override InputBinding[] ApplicationInputBindings() => null;
        public override MenuItem ContextMenuItem { get => _speller; }
    }
}
