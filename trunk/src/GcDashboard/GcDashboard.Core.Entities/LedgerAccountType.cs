using System;

namespace GcDashboard.Core.Entities
{
    /// <summary>
    /// See GnuCash `GNCAccountType` for more information.
    /// </summary>
    public enum LedgerAccountType
    {
        Root,

        /// <summary>
        /// Denotes a savings or checking account held at a bank. Such an account is often
        /// interest bearing.
        /// </summary>
        Bank,

        /// <summary>
        /// Denotes a shoe-box or pillowcase stuffed with cash. In other words, actual currency
        /// held by the user.
        /// </summary>
        Cash,

        /// <summary>
        /// Denotes credit card accounts.
        /// </summary>
        Credit,

        /// <summary>
        /// Denotes a generic asset account.
        /// </summary>
        Asset,

        /// <summary>
        /// Denotes a generic liability account.
        /// </summary>
        Liability,

        /// <summary>
        /// A stock account containing stock shares.
        /// </summary>
        Stock,

        /// <summary>
        /// A mutual fund account containing fund shares.
        /// </summary>
        Mutual,

        /// <summary>
        /// Denotes a currency trading account. In many ways, a currency trading account is like
        /// a stock trading account, where both quantities and prices are set. However, generally
        /// both currency and security are national currencies.
        /// </summary>
        Currency,

        /// <summary>
        /// Denotes an income account. The GnuCash financial model does not use 'categories'.
        /// Actual accounts are used instead.
        /// </summary>
        Income,

        /// <summary>
        /// Denotes an expense account.
        /// </summary>
        Expense,

        /// <summary>
        /// Denotes an equity account.
        /// </summary>
        Equity,
    }

}
