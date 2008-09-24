using System;
using System.Collections.Generic;
using GcDashboard.Core.Entities;
using GcDashboard.Core.Data;

namespace GcDashboard.App.AdminConsole.Actions.Utility
{
    [CommandAlias("Ledger Account Balance")]
    class LedgerAccountBalanceAction : ActionBase
    {

        #region [ Methods (1) ]

        public override void RunHandler()
        {
            // we need to fetch an account, and then get all of its transactions.

            string accountName = ConsoleFunctions.Prompt("Name of account");
            var hqlParams = new Dictionary<string, object>(1);
            hqlParams.Add("accountName", accountName);
            var accounts = Find.FindListByHql<LedgerAccount>(-1, "from LedgerAccount where Name = :accountName", hqlParams);

            LedgerAccount accountToReport = null;

            if (accounts.Count == 0)
            {
                Console.WriteLine("No such account exists.");
                return;
            }

            if (accounts.Count > 1)
            {
                Console.WriteLine("More than one match, choose the account:");
                for (int i = 0; i < accounts.Count; i++)
                {
                    Console.WriteLine("  [{0}]: {1}", i + 1, accounts[i].FullName);
                }

                int indexNumber = int.Parse(ConsoleFunctions.Prompt("Account"));
                accountToReport = accounts[indexNumber - 1];

                Console.WriteLine();
            }

            if (accounts.Count == 1)
            {
                accountToReport = accounts[0];
            }


            // okay, now report

            if (accountToReport == null)
            {
                Console.WriteLine("Error.");
                return;
            }

            // get all transactions for the account

            var hqlpParams = new Dictionary<string, object>(1);
            hqlpParams.Add("account", accountToReport);
            var transactions = Find.FindListByHql<LedgerTransactionSplit>(-1,
                "from LedgerTransactionSplit as s where s.Account = :account", hqlpParams);

            decimal balance = 0;

            foreach (LedgerTransactionSplit split in transactions)
            {
                balance += split.Amount;
            }

            Console.Write(accountToReport.FullName);
            Console.WriteLine("    Balance:  {0:n}", balance);

            return;
        }

        #endregion [ Methods ]

    }
}
