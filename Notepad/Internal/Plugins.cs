using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;

namespace Notepad.Internal
{
    internal class Plugins
    {
        internal static List<Command> Commands = new List<Command>(64);

        internal void Register(State state)
        {
            var assembly = GetType().Assembly;
            var pluginFolder = Path.GetFullPath(Path.Combine(AssemblyPath(assembly), "plugins"));

            state.PluginFolder = pluginFolder;

            // https://docs.microsoft.com/en-us/dotnet/core/tutorials/creating-app-with-plugin-support
            var plugins = new List<Assembly>();

            foreach (var file in Directory.GetFiles(pluginFolder, "*.dll"))
            {
                var loadContext = new PluginLoadContext(file);

                plugins.Add(loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(file))));
            }

            LoadPlugins(state, new[] { assembly }.Concat(plugins).ToArray());
        }

        private string AssemblyPath(Assembly assembly)
        {
            var codeBase = assembly.CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return System.IO.Path.GetDirectoryName(path);
        }

        private static void LoadPlugins(State state, Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                // scan for all types that derive from Command
                var exportedCommands = assembly
                    .GetExportedTypes()
                    .Where(type => type.IsSubclassOf(typeof(Command)));


                foreach (var command in exportedCommands)
                {
                    var instance = Activator.CreateInstance(command, state) as Command;

                    if (instance == null) continue;

                    Commands.Add(instance);

                    state.Owner.InputBindings.AddRange(instance.ApplicationInputBindings() ?? new InputBinding[] { });

                    if (instance.ContextMenuItem == null) continue;

                    var isSubMenu = false;

                    // group
                    foreach (var menuItem in state.TextBox.ContextMenu.Items)
                    {
                        if (!(menuItem is MenuItem mi)) continue;
                        if (mi.Name != instance.ContextMenuItem.Name) continue;

                        foreach (MenuItem subMenuItem in instance.ContextMenuItem.Items)
                        {
                            mi.Items.Add(Clone(subMenuItem));
                        }

                        isSubMenu = true;
                    }

                    if (!isSubMenu) state.TextBox.ContextMenu.Items.Add(instance.ContextMenuItem);
                }
            }
        }

        // this will probably cause issues
        private static MenuItem Clone(MenuItem source)
        {
            var result =  new MenuItem();

            foreach (var propertyInfo in source.GetType().GetProperties())
            {
                if(propertyInfo.CanWrite)
                    propertyInfo.SetValue(result, propertyInfo.GetValue(source));
            }

            return result;
        }
    }
}
