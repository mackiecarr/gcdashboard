using System;
using GcDashboard.Core.Entities;

namespace GcDashboard.App.Report.RunControl
{
    /// <summary>
    /// Used to run reports & processes for a given date range for a
    /// given account.
    /// </summary>
    public class RunControlFromThruForAccount : RunControlFromThruDates
    {

        #region [ Constructors (1) ]

        public RunControlFromThruForAccount()
            : base()
        { }

        #endregion [ Constructors ]

        #region [ Fields (6) ]

        private IAccount _accountToProcess;
        private bool _includeSubAccounts = false;

        #endregion [ Fields ]

        #region [ Properties (6) ]

        public IAccount AccountToProcess
        {
            get { return _accountToProcess; }
            set { _accountToProcess = value; }
        }

        public bool IncludeSubAccounts
        {
            get { return _includeSubAccounts; }
            set { _includeSubAccounts = value; }
        }

        #endregion [ Properties ]

    }
}
