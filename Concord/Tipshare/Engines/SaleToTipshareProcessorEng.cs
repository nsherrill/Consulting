using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tipshare.Engines
{
    public class SaleToTipshareProcessorEng
    {
        public void ProcessSales(List<ServerSales> allSales, out double tipAm, out double tipPm)
        {
            tipAm = 0;
            tipPm = 0;
        }

        public void GetSales(DateTime currDate, Dictionary<int, Shift> serverShifts, Dictionary<int, Ticket> tickets,
            out double dAMSales, out double dOverrideAMTips, out double dPMSales, out double dOverridePMTips)
        {
            dAMSales = 0;
            dPMSales = 0;
            dOverrideAMTips = 0;
            dOverridePMTips = 0;

            double dUnallocatedSales = 0;
            bool bContinue = true;
            DateTime dtEarliestShiftClockIn = Common.GetFirstClockIn(serverShifts);

            List<ServerSales> serverSales = new List<ServerSales>();
            foreach (int key in serverShifts.Keys)
            {
                var newSales = new ServerSales()
                {
                    shouldOverride = Globals.ArrayContains(StaticProps.ServerOverrideNames, serverShifts[key].Name),
                    salesAM = 0,
                    salesPM = 0,
                    empId = serverShifts[key].EmployeeID,
                };

                if (newSales.shouldOverride)
                {
                    Globals.Log($"newSales.shouldOverride:{newSales.shouldOverride}", true);
                    string sError;
                    var salesInfo = GenericDBMethods.GetSpecificTipshareForOverrideUser(StaticProps.PosType, StaticProps.dbAddy, serverShifts[key].EmployeeID, dtEarliestShiftClockIn.Date, out sError);
                    dOverrideAMTips += salesInfo.amTipshareValues;
                    dOverridePMTips += salesInfo.pmTipshareValues;
                }

                serverSales.Add(newSales);
            }

            for (int i = 0; i < tickets.Count && bContinue; i++)
            {
                Shift shft = Common.IsTicketInShifts(tickets[i], serverShifts);

                if (shft == null)
                { }
                else if (tickets[i].ServeDate.Hour < StaticProps.AMPMBreakHour
                    && tickets[i].ServeDate.Hour > 4) // 1am is a pm ticket.
                {
                    var serverSale = serverSales.Find(m =>
                        m.empId == shft.EmployeeID);

                    if (!serverSale.shouldOverride)
                    {
                        dAMSales += tickets[i].Amount;
                        serverSale.salesAM += tickets[i].Amount;
                    }
                }
                else
                {
                    var serverSale = serverSales.Find(m =>
                        m.empId == shft.EmployeeID);


                    if (!serverSale.shouldOverride)
                    {
                        dPMSales += tickets[i].Amount;
                        serverSale.salesPM += tickets[i].Amount;
                    }
                }

            }

            double totalSales = 0;
            StringBuilder sb = new StringBuilder("Calculated sales by server.");
            sb.Append(" ... AM: ");
            serverSales.ForEach(sales =>
            {
                if (sales.salesAM > 0)
                {
                    sb.Append(sales.empId);
                    sb.Append(":");
                    sb.Append(sales.salesAM);
                    sb.Append(", ");
                    totalSales += sales.salesAM;
                }
            });

            sb.Append(".... AM Total");
            sb.Append(":");
            sb.Append(totalSales);

            totalSales = 0d;
            sb.Append(" ...  PM: ");

            serverSales.ForEach(sales =>
            {
                if (sales.salesPM > 0)
                {
                    sb.Append(sales.empId);
                    sb.Append(":");
                    sb.Append(sales.salesPM);
                    sb.Append(", ");
                    totalSales += sales.salesPM;
                }
            });

            sb.Append(".... PM Total");
            sb.Append(":");
            sb.Append(totalSales);

            Globals.Log($"{currDate.ToShortDateString()} / {sb.ToString()}", true);
            Globals.Log($"{currDate.ToShortDateString()} / dAMSales:{dAMSales} / dPMSales:{dPMSales}", true);

            if (dUnallocatedSales > 0)
            {
                //catch bugs
            }
        }
    }
}
