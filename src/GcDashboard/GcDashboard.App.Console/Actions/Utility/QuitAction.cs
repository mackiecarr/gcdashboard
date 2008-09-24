using System;

namespace GcDashboard.App.AdminConsole.Actions.Utility
{
    [CommandAlias("Quit")]
    [CommandAlias("Exit")]
    [CommandAlias("Good bye")]
    class QuitAction : IActionHandler
    {

		#region [ Methods (1) ]

        public int Run()
        {
            Console.WriteLine("End of line.");
            Console.WriteLine();

            System.Threading.Thread.Sleep(1750);

            return 1;
        }

		#endregion [ Methods ]

    }
}
