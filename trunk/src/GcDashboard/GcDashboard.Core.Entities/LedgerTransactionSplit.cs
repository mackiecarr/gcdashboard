using System;
using System.Collections.Generic;

namespace GcDashboard.Core.Entities
{
    public class LedgerTransactionSplit : EntityBase
    {

        #region [ Fields (6) ]

        private decimal _amount;
        private Guid _gnuCashIdentifier;
        private LedgerAccount _account;
        private LedgerTransaction _parentTransaction;
        private string _memo;
        private string _reconciledState;

        #endregion [ Fields ]

        #region [ Properties (6) ]

        public virtual LedgerAccount Account
        {
            get { return _account; }
            set
            {
                _account = value;
                NotifyPropertyChanged("Account");
                Validate("Account");
            }
        }

        public virtual decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                NotifyPropertyChanged("Amount");
                Validate("Amount");
            }
        }

        public virtual Guid GnuCashIdentifier
        {
            get { return _gnuCashIdentifier; }
            set
            {
                _gnuCashIdentifier = value;
                NotifyPropertyChanged("GnuCashIdentifier");
                Validate("GnuCashIdentifier");
            }
        }

        public virtual string Memo
        {
            get { return _memo; }
            set
            {
                _memo = value;
                NotifyPropertyChanged("Memo");
                Validate("Memo");
            }
        }

        public virtual LedgerTransaction ParentTransaction
        {
            get { return _parentTransaction; }
            set
            {
                _parentTransaction = value;
                NotifyPropertyChanged("ParentTransaction");
                Validate("ParentTransaction");
            }
        }

        public virtual string ReconciledState
        {
            get { return _reconciledState; }
            set
            {
                _reconciledState = value;
                NotifyPropertyChanged("ReconciledState");
                Validate("ReconciledState");
            }
        }

        public virtual bool IsBudgeted
        {
            get { return !_memo.Contains("[NB]"); }
        }

        #endregion [ Properties ]

    }
}
