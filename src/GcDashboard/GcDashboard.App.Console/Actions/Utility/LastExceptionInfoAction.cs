using System;

namespace GcDashboard.App.AdminConsole.Actions.Utility
{
    [CommandAlias("Show Last Error")]
    [System.ComponentModel.Description(
        "Get more information about the last exception thrown.")]
    class LastExceptionInfoAction : ActionBase
    {

        #region [ Methods (1) ]

        public override void RunHandler()
        {
            Exception ex = Program.LastThrownException;

            if (ex == null)
                return;

            Console.WriteLine(ex.ToString());
        }

        #endregion [ Methods ]

    }
}
