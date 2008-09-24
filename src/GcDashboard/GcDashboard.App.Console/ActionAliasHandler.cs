using System;
using System.Collections.Generic;
using System.Reflection;

namespace GcDashboard.App.AdminConsole
{

    internal static class ActionAliasHandler
    {

		#region [ Constructors (1) ]

        static ActionAliasHandler()
        {
            _aliases = new Dictionary<string,string>();

            // get all types in this assembly

            Assembly asm = Assembly.GetExecutingAssembly();
            Type[] types = asm.GetTypes();

            foreach (Type t in types)
            {
                if (typeof(IActionHandler).IsAssignableFrom(t))
                {
                    //get custom attributes
                    object[] attribs = t.GetCustomAttributes(true);

                    foreach (Attribute attrib in attribs)
                    {
                        if (attrib is CommandAliasAttribute)
                        {
                            _aliases.Add(((CommandAliasAttribute)attrib).Alias.ToLower(), t.FullName);
                        }
                    }
                }
            }
        }

		#endregion [ Constructors ]

		#region [ Fields (1) ]

        private static Dictionary<string, string> _aliases;

		#endregion [ Fields ]


        internal static List<string> KnownAliases
        {
            get
            {
                List<string> aliases = new List<string>();

                foreach (string value in _aliases.Keys)
                {
                    aliases.Add(value);
                }

                return aliases;
            }
        }
		

        #region [ Private Methods (1) ]

                internal static string Translate(string alias)
        {
            if (_aliases.ContainsKey(alias.ToLower()))
                return _aliases[alias.ToLower()];
            else
                return null;
        }

		#endregion [ Private Methods ]

    }

}
