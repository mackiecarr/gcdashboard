using System;
using System.Collections.Generic;
using System.Xml;
using GcDashboard.Core.Entities;

namespace GcDashboard.App.AdminConsole.Actions.Data
{

    [CommandAlias("Load Accounts")]
    class LoadAccountDataAction : ActionBase
    {

        #region [ Fields (1) ]

        private static Dictionary<Guid, LedgerAccount> _accountCache;

        #endregion [ Fields ]

        #region [ Public Methods (1) ]

        public override void RunHandler()
        {
            _accountCache = new Dictionary<Guid, LedgerAccount>(50);

            Core.Data.Connection.RunNativeSQL("delete from ledger_account");

            string sourceFilename = ConsoleFunctions.Prompt("Source Filename", "..\\..\\..\\resources\\gc\\sample.gnucash");
            string workFilename = sourceFilename + ".work";

            Helper.DataLoadHelper.Extract(sourceFilename);
            Helper.DataLoadHelper.ReadXml(workFilename);

            XmlNodeList listOfAccounts = Helper.DataLoadHelper.GetAccountNodes();
            List<LedgerAccount> accounts = new List<GcDashboard.Core.Entities.LedgerAccount>(50);

            foreach (XmlNode node in listOfAccounts)
            {
                LedgerAccount a = new LedgerAccount();

                foreach (XmlNode innerNode in node.ChildNodes)
                {
                    switch (innerNode.LocalName)
                    {
                        case "name":
                            a.Name = innerNode.InnerText;
                            break;

                        case "id":
                            a.GnuCashIdentifier = new Guid(innerNode.InnerText);
                            break;

                        case "type":
                            a.AccountType = (LedgerAccountType)Enum.Parse(typeof(LedgerAccountType), innerNode.InnerText, true);
                            break;

                        case "description":
                            a.Description = innerNode.InnerText;
                            break;

                        case "parent":
                            Guid parentGuid = new Guid(innerNode.InnerText);

                            try
                            {
                                a.ParentAccount = LookupAccountByGuid(parentGuid);
                            }
                            catch (Exception)
                            { }
                            break;

                        default:
                            int i = 1;
                            break;
                    }
                }

                accounts.Add(a);
                _accountCache.Add(a.GnuCashIdentifier, a);
            }

            foreach (LedgerAccount account in accounts)
            {
                Core.Data.Connection.Save(-1, account);
            }

            Console.WriteLine("Loaded {0} accounts", accounts.Count);
        }

        #endregion [ Public Methods ]

        #region [ Private Static Methods (1) ]

        internal static LedgerAccount LookupAccountByGuid(Guid identifier)
        {
            if (_accountCache.ContainsKey(identifier))
                return _accountCache[identifier];
            else
                return null;
        }

        #endregion [ Private Static Methods ]

    }
}
