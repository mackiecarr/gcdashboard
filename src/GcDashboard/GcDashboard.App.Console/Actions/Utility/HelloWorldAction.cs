using System;

namespace GcDashboard.App.AdminConsole.Actions.Utility
{
    [CommandAlias("Hello World")]
    [System.ComponentModel.Description(
        "Simple command to test the admin console command handler.")]
    class HelloWorldAction : ActionBase
    {

        #region [ Methods (1) ]

        public override void RunHandler()
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine();
        }

        #endregion [ Methods ]

    }
}
