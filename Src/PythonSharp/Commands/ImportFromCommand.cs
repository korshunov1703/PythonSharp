﻿namespace PythonSharp.Commands
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using PythonSharp.Compiler;
    using PythonSharp.Exceptions;
    using PythonSharp.Expressions;
    using PythonSharp.Language;
    using PythonSharp.Utilities;

    public class ImportFromCommand : ICommand
    {
        private string modname;
        private IList<string> names;

        public ImportFromCommand(string name)
            : this(name, null)
        {
        }

        public ImportFromCommand(string name, IList<string> names)
        {
            this.modname = name;
            this.names = names;
        }

        public string ModuleName { get { return this.modname; } }

        public ICollection<string> Names { get { return this.names; } }

        public void Execute(IContext context)
        {
            Module module = null;

            if (TypeUtilities.IsNamespace(this.modname))
            {
                var types = TypeUtilities.GetTypesByNamespace(this.modname);

                module = new Module(context.GlobalContext);

                foreach (var type in types)
                    module.SetValue(type.Name, type);
            }
            else
            {
                string filename = ModuleUtilities.ModuleFileName(this.modname);

                if (filename == null)
                    throw new ImportError(string.Format("No module named {0}", this.modname));

                Parser parser = new Parser(new StreamReader(filename));
                ICommand command = parser.CompileCommandList();

                module = new Module(context.GlobalContext);

                command.Execute(module);
            }

            if (this.names != null)
                foreach (string name in this.names)
                    context.SetValue(name, module.GetValue(name));
            else
                foreach (string name in module.GetNames())
                    context.SetValue(name, module.GetValue(name));
        }
    }
}
