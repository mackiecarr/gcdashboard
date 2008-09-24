using System;
using System.Collections.Generic;
using System.Reflection;

namespace GcDashboard.App.AdminConsole.Actions.Utility
{
    [CommandAlias("Command Reference")]
    [System.ComponentModel.Description(
        "Lists available commands and their usage.")]
    class CommandReferenceAction : ActionBase
    {

        #region [ Methods (1) ]

        public override void RunHandler()
        {
            List<string> commands = new List<string>();

            // get all types in this assembly

            Assembly asm = Assembly.GetExecutingAssembly();
            Type[] types = asm.GetTypes();

            foreach (Type t in types)
            {
                if (typeof(IActionHandler).IsAssignableFrom(t))
                {
                    //get custom attributes
                    object[] attribs = t.GetCustomAttributes(true);

                    string command = string.Empty;
                    string usage = string.Empty;

                    foreach (Attribute attrib in attribs)
                    {
                        if (attrib is CommandAliasAttribute)
                        {
                            if (string.IsNullOrEmpty(command))
                                command = ((CommandAliasAttribute)attrib).Alias;
                        }

                        if (attrib is System.ComponentModel.DescriptionAttribute)
                        {
                            usage = ((System.ComponentModel.DescriptionAttribute)attrib).Description;
                        }
                    }

                    if (!string.IsNullOrEmpty(command))
                        commands.Add(string.Format("{0}:{1}", command, usage));
                }
            }

            commands.Sort();

            foreach (string s in commands)
            {
                string[] pieces = s.Split(':');
                Console.WriteLine(pieces[0].PadRight(25) + " - " + pieces[1]);
            }
        }

        #endregion [ Methods ]

    }
}
