using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace GcDashboard.Core.Entities
{

    /// <summary>
    /// Represents an account on the general ledger.
    /// </summary>
    public class LedgerAccount : EntityBase, IAccount
    {

        #region [ Constructors (1) ]

        public LedgerAccount()
        {
            _childAccounts = new List<LedgerAccount>();
        }

        #endregion [ Constructors ]

        #region [ Fields (7) ]

        private bool _isPlaceholder = false;
        private Guid _gnuCashIdentifier;
        private IList<LedgerAccount> _childAccounts;
        private LedgerAccount _parentAccount;
        private LedgerAccountType _accountType;
        private string _name;
        private string _description;

        #endregion [ Fields ]

        #region [ Properties (11) ]

        public virtual LedgerAccountType AccountType
        {
            get { return _accountType; }
            set
            {
                _accountType = value;
                NotifyPropertyChanged("AccountType");
                Validate("AccountType");
            }
        }

        public virtual IList<LedgerAccount> ChildAccounts
        {
            get { return _childAccounts; }
            set { _childAccounts = value; }
        }

        public virtual BindingList<LedgerAccount> ChildAccountsList
        {
            get
            {
                BindingList<LedgerAccount> list = new BindingList<LedgerAccount>(_childAccounts);
                list.AllowEdit = true;
                list.AllowNew = true;
                list.AllowRemove = true;
                return list;
            }
        }

        public virtual IList<IAccount> ChildIAccounts
        {
            get
            {
                var a = new List<IAccount>(_childAccounts.Count);
                foreach (LedgerAccount b in _childAccounts)
                {
                    a.Add(b);
                }

                return a;
            }
        }

        public virtual string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                NotifyPropertyChanged("Description");
                Validate("Description");
            }
        }

        public virtual string FullName
        {
            get { return CalculateFullName(this); }
            set { }
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

        public virtual bool IsPlaceholder
        {
            get { return _isPlaceholder; }
            set
            {
                _isPlaceholder = value;
                NotifyPropertyChanged("IsPlaceholder");
                Validate("IsPlaceholder");
            }
        }

        public virtual string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
                Validate("Name");
            }
        }

        public virtual LedgerAccount ParentAccount
        {
            get { return _parentAccount; }
            set
            {
                if (this.IsParent(value))
                    throw new InvalidOperationException("This would cause a circular reference.");

                value.AddChildAccount(this);
                _parentAccount = value;
                NotifyPropertyChanged("ParentAccount");
                Validate("ParentAccount");
            }
        }

        public virtual IAccount ParentIAccount
        {
            get { return _parentAccount; }
        }

        #endregion [ Properties ]

        #region [ Public Methods (3) ]

        public override int GetHashCode()
        {
            string signature = FullName;
            return signature.GetHashCode();
        }

        /// <summary>
        /// Indicates whether this account is a parent, either directly or indirectly
        /// of the given account.
        /// </summary>
        /// <param name="account">The account to search for.</param>
        /// <returns>True, if the account exists in the child structure; otherwise false.</returns>
        public virtual bool IsParent(LedgerAccount account)
        {
            LedgerAccount current = account;

            while (current != null)
            {
                if (current.Equals(this))
                    return true;

                if (current.ParentAccount != null)
                {
                    current = current.ParentAccount;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion [ Public Methods ]

        #region [ Private Methods (2) ]

        private string CalculateFullName(LedgerAccount a)
        {
            if (a.ParentAccount != null &&
                a.ParentAccount.ParentAccount != null)
                return CalculateFullName(a.ParentAccount) + Configuration.General.AccountSeperator + a.Name;
            else
                return a.Name;
        }

        private void AddChildAccount(LedgerAccount acct)
        {
            _childAccounts.Add(acct);
        }

        #endregion [ Private Methods ]

    }

}
