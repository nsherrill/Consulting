using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tipshare.Engines
{
    public class SaveDataEng
    {
        private BasicCalculationEng basicCalcEng = new BasicCalculationEng();

        public void SaveDaysData(int iCurrDay, Dictionary<int, Shift> lsShifts, DateTime dtCurrentDay,
            List<TextBox> ltbAMAdj, List<TextBox> ltbAMSugg, List<TextBox> ltbAMHours,
            List<TextBox> ltbAMIDs, List<TextBox> ltbAMNames, List<ComboBox> lcbAMNames,
            List<TextBox> ltbPMAdj, List<TextBox> ltbPMSugg, List<TextBox> ltbPMHours,
            List<TextBox> ltbPMIDs, List<TextBox> ltbPMNames, List<ComboBox> lcbPMNames,
            out List<Entry> amEntries, out List<Entry> pmEntries,
            double amSugg, double pmSugg)
        {
            //         int iDayIndex = GetDateIndex(dtCurrentDay);
            //lddDaysData[iCurrDay].AMEntries = new List<Entry>();
            //lddDaysData[iCurrDay].PMEntries = new List<Entry>();
            amEntries = new List<Entry>();
            pmEntries = new List<Entry>();


            for (int i = 0; i < ltbAMAdj.Count - 5; i++)
            {
                try
                {
                    double temp, tHours;
                    if (!double.TryParse(ltbAMAdj[i].Text, out temp))
                        temp = 0.0;
                    tHours = double.Parse(ltbAMHours[i].Text);
                    if (tHours == -1)
                        //tHours = GetHours(lsShifts, dtCurrentDay, ltbAMIDs[i].Text, StaticProps.BarHostJobCodes, double.Parse(ltbAMSugg[i].Text), lddDaysData[iCurrDay].TotalAMSuggested, true);
                        tHours = basicCalcEng.GetHours(lsShifts, dtCurrentDay, ltbAMIDs[i].Text, StaticProps.BarHostJobCodes, double.Parse(ltbAMSugg[i].Text), amSugg, true);
                    //lddDaysData[iCurrDay].AMEntries.Add(new Entry(
                    //    ltbAMNames[i].Text, ltbAMIDs[i].Text, double.Parse(ltbAMSugg[i].Text), temp, tHours));
                    amEntries.Add(new Entry(ltbAMNames[i].Text, ltbAMIDs[i].Text, double.Parse(ltbAMSugg[i].Text), temp, tHours));
                }
                catch (Exception e)
                {
                    Logger.Log("SaveDaysData A exception thrown: i = " + i +
                        "\r\n  ltbAMAdj.Count=" + ltbAMAdj.Count +
                        "\r\n  ltbAMHours.Count=" + ltbAMHours.Count +
                        "\r\n  ltbAMIDs.Count=" + ltbAMIDs.Count +
                        "\r\n  ltbAMSugg.Count=" + ltbAMSugg.Count +
                        "\r\n  ltbAMNames.Count=" + ltbAMNames.Count + 
                        Environment.NewLine + e.ToString());
                }
            }
            int iIndex;
            for (int i = 0; i < lcbAMNames.Count; i++)
            {
                try
                {
                    iIndex = ltbAMAdj.Count - lcbAMNames.Count + i;
                    if (iIndex < 14)
                    {
                        string temp = ltbAMAdj[iIndex].Text;
                        double tempd;
                        if (temp == ""
                            || (double.TryParse(temp, out tempd)
                                && tempd == 0)
                            || lcbAMNames[i].SelectedItem == null
                            || lcbAMNames[i].SelectedItem.ToString().Equals(""))
                        { /*dont include it*/ }
                        else
                        {
                            double tHours = double.Parse(ltbAMHours[iIndex].Text);
                            if (tHours == -1)
                                tHours = 0;
                            //lddDaysData[iCurrDay].AMEntries.Add(new Entry(
                            //    GetName(lcbAMNames[i].SelectedItem.ToString()),
                            //    GetID(lcbAMNames[i].SelectedItem.ToString()),
                            //    0, tempd,
                            //    tHours));
                            amEntries.Add(new Entry(
                                Common.GetName(lcbAMNames[i].SelectedItem.ToString()),
                                Common.GetID(lcbAMNames[i].SelectedItem.ToString()),
                                0, tempd,
                                tHours));
                        }
                    }
                    else
                        break;
                }
                catch (Exception e)
                {
                    Logger.Log("SaveDaysData B exception thrown: i = " + i +
                        "\r\n  lcbAMNames.Count=" + lcbAMNames.Count + 
                        Environment.NewLine + e.ToString());
                }
            }

            for (int i = 0; i < ltbPMAdj.Count - 5; i++)
            {
                try
                {
                    double temp, tHours;
                    if (!double.TryParse(ltbPMAdj[i].Text, out temp))
                        temp = 0.0;
                    tHours = double.Parse(ltbPMHours[i].Text);
                    if (tHours == -1)
                        tHours = basicCalcEng.GetHours(lsShifts, dtCurrentDay, ltbPMIDs[i].Text, StaticProps.BarHostJobCodes, double.Parse(ltbPMSugg[i].Text), pmSugg, false);
                    //tHours = GetHours(lsShifts, dtCurrentDay, ltbPMIDs[i].Text, StaticProps.BarHostJobCodes, double.Parse(ltbPMSugg[i].Text), lddDaysData[iCurrDay].TotalPMSuggested, false);
                    //lddDaysData[iCurrDay].PMEntries.Add(new Entry(
                    //    ltbPMNames[i].Text, ltbPMIDs[i].Text, double.Parse(ltbPMSugg[i].Text), temp, double.Parse(ltbPMHours[i].Text)));
                    pmEntries.Add(new Entry(ltbPMNames[i].Text, ltbPMIDs[i].Text, double.Parse(ltbPMSugg[i].Text), temp, double.Parse(ltbPMHours[i].Text)));
                }
                catch (Exception e)
                {
                    Logger.Log("SaveDaysData C exception thrown: i = " + i +
                        "\r\n  ltbPMAdj.Count=" + ltbPMAdj.Count +
                        "\r\n  ltbPMHours.Count=" + ltbPMHours.Count +
                        "\r\n  ltbPMIDs.Count=" + ltbPMIDs.Count +
                        "\r\n  ltbPMSugg.Count=" + ltbPMSugg.Count +
                        "\r\n  ltbPMNames.Count=" + ltbPMNames.Count +
                        "\r\n  ltbPMIDs.Count=" + ltbPMIDs.Count +
                        "\r\n  ltbPMSugg.Count=" + ltbPMSugg.Count +
                        "\r\n  ltbPMHours.Count=" + ltbPMHours.Count + 
                        Environment.NewLine+ e.ToString());
                }
            }

            for (int i = 0; i < lcbPMNames.Count; i++)
            {
                try
                {
                    iIndex = ltbPMAdj.Count - lcbPMNames.Count + i;
                    if (iIndex < 14)
                    {
                        string temp = ltbPMAdj[iIndex].Text;
                        double tempd;
                        if (temp == ""
                            || (double.TryParse(temp, out tempd)
                                && tempd == 0)
                            || lcbPMNames[i].SelectedItem == null
                            || lcbPMNames[i].SelectedItem.ToString().Equals(""))
                        { /*dont include it*/ }
                        else
                        {
                            //lddDaysData[iCurrDay].PMEntries.Add(new Entry(
                            //    GetName(lcbPMNames[i].SelectedItem.ToString()),
                            //    GetID(lcbPMNames[i].SelectedItem.ToString()),
                            //    0, tempd,
                            //    double.Parse(ltbPMHours[iIndex].Text)));                            
                            pmEntries.Add(new Entry(
                                Common.GetName(lcbPMNames[i].SelectedItem.ToString()),
                                Common.GetID(lcbPMNames[i].SelectedItem.ToString()),
                                0, tempd,
                                double.Parse(ltbPMHours[iIndex].Text)));
                        }
                    }
                    else
                        break;
                }
                catch (Exception e)
                {
                    Logger.Log("SaveDaysData D exception thrown: i = " + i +
                        "\r\n  lcbPMNames.Count=" + lcbPMNames.Count + 
                        Environment.NewLine + e.ToString());
                }
            }
        }
    }
}
