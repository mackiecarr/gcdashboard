using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace GcDashboard.Core.Entities
{

    /// <summary>
    /// Base representation of an account in a tree of accounts.
    /// </summary>
    /// <remarks>
    /// An account can have a parent and a collection of child accounts.
    /// </remarks>
    public interface IAccount
    {
        string Name { get; }
        string Description { get; }
        IAccount ParentIAccount { get; }
        IList<IAccount> ChildIAccounts { get; }
    }

}
