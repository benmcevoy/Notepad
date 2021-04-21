using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Snippets
{
    public class InsertSnippet : Notepad.Command
    {
        private readonly Dictionary<string, Snippet> _snippets = new Dictionary<string, Snippet>();

        public InsertSnippet(Notepad.State state) : base(state)
        {
            var lines = new List<string>(File.ReadAllLines(Path.Combine(State.PluginFolder, "snippets.ini")));
            var index = 0;

            while (index < lines.Count)
            {
                Snippet snippet = null;

                if (lines[index].Trim().Equals("[Snippet]"))
                {
                    snippet = new Snippet();

                    index++;
                }

                if (index < lines.Count && lines[index].StartsWith("name="))
                {
                    if (snippet != null)
                    {
                        snippet.Name = lines[index].Replace("name=", "");
                    }

                    index++;
                }

                if (index < lines.Count && lines[index].StartsWith("gesture="))
                {
                    if (snippet != null)
                    {
                        snippet.Gesture = lines[index].Replace("gesture=", "");
                    }

                    index++;
                }

                if (index < lines.Count && lines[index].StartsWith("value="))
                {
                    if (snippet != null)
                    {
                        var value = lines[index].Replace("value=", "");

                        index++;

                        while (index < lines.Count && !lines[index].Trim().Equals("[Snippet]"))
                        {
                            value += Environment.NewLine + lines[index];

                            index++;
                        }

                        snippet.Value = value;

                        _snippets[snippet.Name.ToLowerInvariant()] = snippet;
                        --index;
                    }
                }

                index++;
            }
        }

        public override void Execute(object parameter)
        {
            if (!(parameter is string snippet)) return;

            var key = snippet.ToLowerInvariant();

            if (_snippets.ContainsKey(key))
            {
                InsertText(State.TextBox, _snippets[key].Value);
            }
        }

        public override string Name { get; } = "Insert snippets";

        public override InputBinding[] ApplicationInputBindings()
        {
            var result = new InputBinding[_snippets.Count];
            var i = 0;

            foreach (var snippet in _snippets)
            {
                result[i] = new KeyBinding(this, (KeyGesture)new KeyGestureConverter().ConvertFromString(snippet.Value.Gesture))
                {
                    CommandParameter = snippet.Key
                };
                i++;
            }

            return result;
        }
        
        public override MenuItem ContextMenuItem { get; }

        private static void InsertText(TextBox textBox, string text)
        {
            textBox.SelectedText = text.Replace(@"\r\n", Environment.NewLine);
            textBox.CaretIndex += text.Length;
            textBox.SelectionLength = 0;
        }
    }
}
