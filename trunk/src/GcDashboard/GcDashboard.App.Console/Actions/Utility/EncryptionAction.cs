using System;

namespace GcDashboard.App.AdminConsole.Actions.Utility
{
    [CommandAlias("Encrypt")]
    class EncryptionAction : IActionHandler
    {

        #region [ Methods (1) ]

        public int Run()
        {
            string key = ConsoleFunctions.Prompt("Encryption Key");
            string value = ConsoleFunctions.Prompt("Value to Encrypt");

            Console.WriteLine("Encrypted:       '{0}'", Core.Utility.Encryption.Encrypt(value, key));
            Console.WriteLine();

            return 0;
        }

        #endregion [ Methods ]

    }
}
