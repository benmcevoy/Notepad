using System.IO;
using System.Windows;
using System.Windows.Controls;
using Notepad.Internal;

namespace Notepad
{
    public class State : NotifyObject
    {
        public Window Owner { get; set; }

        public TextBox TextBox { get; set; }
        
        public FileInfo File
        {
            get => _file;
            set
            {
                _file = value;
                OnPropertyChanged(nameof(File));
                OnPropertyChanged(nameof(FileName));
            }
        }

        public string FileName => File?.Name ?? "Untitled.txt";

        public bool IsDirty { get; set; }

        public string PluginFolder { get; internal set; }

        private string _status;
        private FileInfo _file;

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
    }
}
