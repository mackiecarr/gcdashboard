using System;
using System.Collections.Generic;
using GcDashboard.Core.Entities;
using GcDashboard.Core.Reporting.Excel;


namespace GcDashboard.App.Report.Excel
{
    /// <summary>
    /// Generates a report listing all of the transactions for a given ledger account.
    /// </summary>
    public class LedgerAccountTransactions
    {

        public LedgerAccountTransactions(RunControl.RunControlFromThruForAccount rc)
        {
            _rcFromThruForAccount = rc;
        }


        private int _currentRow;
        private RunControl.RunControlFromThruForAccount _rcFromThruForAccount;


        public void Run()
        {

            //get root budget account
            Dictionary<string, object> hqlParams = new Dictionary<string, object>(1);
            hqlParams.Add("account", _rcFromThruForAccount.AccountToProcess);

            LedgerAccount accountToReport = Core.Data.Find.FindByHql<LedgerAccount>(-1, "from LedgerAccount b where b = :account", hqlParams);

            using (ExcelWorkbook book = new ExcelWorkbook())
            {
                System.IO.FileInfo templateFile = new System.IO.FileInfo(@"..\..\..\resources\xls\ledger account transactions.xlsx");
                book.Open(templateFile.FullName);

                _currentRow = 6;
                ProcessAccount(accountToReport, book);

                System.IO.FileInfo outputFile = new System.IO.FileInfo(@"..\..\..\data\output.xlsx");
                book.SaveAs(outputFile.FullName);
            }
        }

        private void ProcessAccount(LedgerAccount account, ExcelWorkbook book)
        {
            //get all transactions for this account in the given date range
            var hqlParams = new Dictionary<string, object>(3);
            hqlParams.Add("account", account);
            hqlParams.Add("fromDate", _rcFromThruForAccount.FromDate);
            hqlParams.Add("thruDate", _rcFromThruForAccount.ThruDate);

            IList<LedgerTransactionSplit> transactions = Core.Data.Find.FindListByHql<LedgerTransactionSplit>(-1,
                "from LedgerTransactionSplit a where a.Account = :account and a.ParentTransaction.DatePosted between :fromDate and :thruDate order by a.ParentTransaction.DatePosted", hqlParams);


            foreach (LedgerTransactionSplit split in transactions)
            {
                if (_currentRow >= 7)
                {
                    book.Range("A" + _currentRow, "J" + (_currentRow + 1)).FillDown();
                }

                book.Cell("A" + _currentRow).Value2 = split.ParentTransaction.DatePosted;
                //book.Cell("B" + _row).Value2 = split.ParentTransaction.
                book.Cell("C" + _currentRow).Value2 = split.ParentTransaction.Description;
                book.Cell("D" + _currentRow).Value2 = split.Account.FullName;
                book.Cell("E" + _currentRow).Value2 = split.Amount;
                _currentRow++;

            }


            int i = 1;
        }



    }

}
