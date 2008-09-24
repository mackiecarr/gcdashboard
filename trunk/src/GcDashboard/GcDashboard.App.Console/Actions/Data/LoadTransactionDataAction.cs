using System;
using System.Collections.Generic;
using System.Xml;
using GcDashboard.Core.Entities;

namespace GcDashboard.App.AdminConsole.Actions.Data
{

    [CommandAlias("Load Transactions")]
    class LoadTransactionDataAction : ActionBase
    {

        public override void RunHandler()
        {
            Core.Data.Connection.RunNativeSQL("delete from ledger_transaction");
            Core.Data.Connection.RunNativeSQL("delete from ledger_transaction_split");

            string sourceFilename = ConsoleFunctions.Prompt("Source Filename", "..\\..\\..\\resources\\gc\\sample.gnucash");
            string workFilename = sourceFilename + ".work";

            Helper.DataLoadHelper.Extract(sourceFilename);
            Helper.DataLoadHelper.ReadXml(workFilename);

            XmlNodeList listOfTransactions = Helper.DataLoadHelper.GetTransactionNodes();

            foreach (XmlNode node in listOfTransactions)
            {
                LedgerTransaction t = new LedgerTransaction();

                foreach (XmlNode innerNode in node.ChildNodes)
                {
                    switch (innerNode.LocalName)
                    {
                        case "description":
                            t.Description = innerNode.InnerText;
                            break;

                        case "id":
                            t.GnuCashIdentifier = new Guid(innerNode.InnerText);
                            break;

                        case "date-posted":
                            try
                            {
                                t.DatePosted = DateTime.Parse(innerNode.InnerText);
                            }
                            catch (Exception ex)
                            {
                                int v = 1;
                            }
                            break;

                        case "date-entered":
                            try
                            {
                                t.DateEntered = DateTime.Parse(innerNode.ChildNodes[0].InnerText);
                            }
                            catch (Exception ex)
                            {
                                int o = 1;
                            }
                            break;


                        case "slots":
                            SlotsHandler(t, innerNode);
                            break;


                        /*

                           <trn:description>Home Depot</trn:description>
<trn:slots>
<slot>
  <slot:key>notes</slot:key>
  <slot:value type="string">[NB] Laser Level</slot:value>
</slot>
</trn:slots> 
                              
                             
                         
                         * */




                        case "splits":
                            SplitHandler(t, innerNode);
                            break;

                        default:
                            int i = 1;
                            break;
                    }
                }

                Core.Data.Connection.Save(-1, t);
            }
        }

        private void SplitHandler(LedgerTransaction t, XmlNode splitNode)
        {

            foreach (XmlNode node in splitNode.ChildNodes)
            {

                LedgerTransactionSplit s = new LedgerTransactionSplit();

                foreach (XmlNode innerNode in node.ChildNodes)
                {

                    switch (innerNode.LocalName)
                    {

                        case "id":
                            s.GnuCashIdentifier = new Guid(innerNode.InnerText);
                            break;

                        case "memo":
                            s.Memo = innerNode.InnerText;
                            break;

                        case "reconciled-state":
                            s.ReconciledState = innerNode.InnerText;
                            break;

                        case "value":
                            string[] parts = innerNode.InnerText.Split('/');
                            decimal amount = int.Parse(parts[0]) / (decimal)int.Parse(parts[1]);
                            s.Amount = amount;
                            break;

                        case "account":
                            Guid accountGuid = new Guid(innerNode.InnerText);

                            //search
                            Dictionary<string, object> hqlParams = new Dictionary<string, object>();
                            hqlParams.Add("accountGuid", accountGuid);

                            LedgerAccount a = Core.Data.Find.FindByHql<LedgerAccount>(-1,
                                "from LedgerAccount a where a.GnuCashIdentifier = :accountGuid", hqlParams);

                            s.Account = a;
                            break;

                        default:
                            int i = 1;
                            break;



                    }

                }

                t.AddSplit(s);

            }



        }


        private void SlotsHandler(LedgerTransaction t, XmlNode slotNode)
        {
            foreach (XmlNode node in slotNode.ChildNodes)
            {
                string key = null;
                string value = null;

                foreach (XmlNode innerNode in node.ChildNodes)
                {
                    if (innerNode.LocalName == "key")
                        key = innerNode.InnerText;

                    if (innerNode.LocalName == "value")
                        value = innerNode.InnerText;
                }

                switch (key)
                {
                    case "notes":
                        t.Notes = value;
                        break;
                }

            }
        }

    }
}
