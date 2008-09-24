using System;
using System.Collections.Generic;

namespace GcDashboard.App.AdminConsole.Actions.Utility
{
    [CommandAlias("Help")]
    [CommandAlias("?")]
    class HelpAction : IActionHandler
    {

        #region [ Methods (1) ]

        public int Run()
        {
            List<string> aliases = ActionAliasHandler.KnownAliases;
            aliases.Sort();

            for (int i = 0; i < aliases.Count; i += 3)
            {
                string val1 = string.Empty;
                string val2 = string.Empty;
                string val3 = string.Empty;

                val1 = aliases[i].PadRight(25);

                if (!(i + 1 >= aliases.Count))
                    val2 = aliases[i+1].PadRight(25);

                if (!(i + 2 >= aliases.Count))
                    val3 = aliases[i + 2];

                string output = string.Format("{0} {1} {2}", val1, val2, val3);
                Console.WriteLine(output);
            }

            Console.WriteLine();

            return 0;
        }

        #endregion [ Methods ]

    }
}
