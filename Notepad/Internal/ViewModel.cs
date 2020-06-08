using System.ComponentModel;
using System.Windows;
using Notepad.Commands;
using Notepad.Commands.Find;

namespace Notepad.Internal
{
    internal class ViewModel : NotifyObject
    {
        public ViewModel(State state)
        {
            State = state;

            FileOpen = new FileOpen(State);
            FileSave = new FileSave(State);
            FileSaveAs = new FileSaveAs(State);
            FileNew = new FileNew(State);
            Find = new Find(State);
            FindNext = new FindNext(State);
            FindPrevious = new FindPrevious(State);
            DeleteLine = new DeleteLine(State);

            ShowAllCommands = new ShowAllCommands(State);
        }

        public void SetDirty(bool value)
        {
            State.IsDirty = value;
        }

        public void Closing(CancelEventArgs cancelEventArgs)
        {
            if (!State.IsDirty) return;
            
            var dialog = new Dialog();

            var result = dialog.ShowDialog(State.Owner, $"Do you want to save changes to {State.FileName}?", "Notepad", 
                DialogButton.YesNoCancel, MessageBoxImage.Warning);
            
            if (result == MessageBoxResult.Cancel)
            {
                cancelEventArgs.Cancel = true;
                return;
            }

            if (result == MessageBoxResult.Yes)
            {
                FileSave.Execute(null);
            }
        }

        public State State { get; }
        public FileOpen FileOpen { get; set; }
        public FileSave FileSave { get; set; }
        public FileSaveAs FileSaveAs { get; set; }
        public FileNew FileNew { get; set; }
        public Find Find { get; set; }
        public FindNext FindNext { get; set; }
        public FindPrevious FindPrevious { get; set; }
        public ShowAllCommands ShowAllCommands { get; set; }
        public DeleteLine DeleteLine { get; set; }
    }
}
