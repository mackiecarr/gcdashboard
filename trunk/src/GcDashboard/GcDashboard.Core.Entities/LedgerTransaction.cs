using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace GcDashboard.Core.Entities
{

    /// <summary>
    /// A transaction in the ledger.
    /// </summary>
    public class LedgerTransaction : EntityBase
    {

        #region [ Constructors (1) ]

        /// <summary>
        /// Initializes a new instance of the <see cref="LedgerTransaction"/> class.
        /// </summary>
        public LedgerTransaction()
        {
            _splits = new List<LedgerTransactionSplit>();
        }

        #endregion [ Constructors ]

        #region [ Fields (6) ]

        private DateTime _dateEntered;
        private DateTime _datePosted;
        private Guid _gnuCashIdentifier;
        private IList<LedgerTransactionSplit> _splits;
        private string _description;
        private string _notes;

        #endregion [ Fields ]

        #region [ Properties (6) ]

        public virtual DateTime DateEntered
        {
            get { return _dateEntered; }
            set { _dateEntered = value; }
        }

        public virtual DateTime DatePosted
        {
            get { return _datePosted; }
            set { _datePosted = value; }
        }

        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
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

        public virtual string Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }

        public virtual BindingList<LedgerTransactionSplit> Splits
        {
            get
            {
                BindingList<LedgerTransactionSplit> list = new BindingList<LedgerTransactionSplit>(_splits);
                list.AllowEdit = false;
                list.AllowNew = false;
                list.AllowRemove = false;

                return list;
            }
        }

        #endregion [ Properties ]

        #region [ Public Methods (1) ]

        public virtual void AddSplit(LedgerTransactionSplit s)
        {
            s.ParentTransaction = this;
            _splits.Add(s);
        }

        #endregion [ Public Methods ]

        private IList<LedgerTransactionSplit> SplitsList
        {
            get { return _splits; }
            set { _splits = value; }
        }

    }

}
