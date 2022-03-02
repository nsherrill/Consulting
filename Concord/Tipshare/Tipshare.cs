using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Drawing.Printing;
using System.Configuration;
using Tipshare.Engines;
using Common;

namespace Tipshare
{
    public partial class Tipshare : Form
    {
        #region GlobalVars
        SaleToTipshareProcessorEng processorEng = new SaleToTipshareProcessorEng();
        EntryCalculationEng entryCalcEng = new EntryCalculationEng();
        BasicCalculationEng basicCalcEng = new BasicCalculationEng();
        SaveDataEng saveDataEng = new SaveDataEng();
        ShiftDeterminationEng shiftDeterEng = new ShiftDeterminationEng();

        string sStoreName;
        string sUploadFilename;
        string sInternalFilename;
        string sBackwardsFilename;
        int iBarPercent, iHostPercent;
        double dCollectionPercent;

        double dLowerDifferenceAllowed = 1.0, dUpperDifferenceAllowed = 10.0;

        DateTime dtNow, dtCurrentDay, dtEarliestViewable, dtLastFullRefresh, dtDueSOWD;
        int iCurrDay;
        Version vVersion;
        Dictionary<int, Employee> leEmployees;
        Dictionary<int, Shift> lsShifts;
        Dictionary<int, Ticket> ltTickets;

        List<TextBox> ltbAMNames, ltbAMIDs, ltbAMSugg, ltbAMAdj, ltbAMHours,
                      ltbPMNames, ltbPMIDs, ltbPMSugg, ltbPMAdj, ltbPMHours;
        List<ComboBox> lcbAMNames, lcbPMNames;
        List<bool> lbDaysViewed, lbTotalsOkay;
        List<DaysData> lddDaysData;
        List<Thread> ltThreads;

        string[] saEmployeeNamesToAddToUnallocated;

        //System.Configuration.ConfigurationSettings.AppSettings["setting"].Contains("test");
        //bool bAllowUndistributed = false;

        SettingsBox fSettingsBox;
        DebugDisplay fDebugDisplay;
        List<DebugContainer> dcDebugContainer;
        //bool bDebugDataSet = false;

        #endregion

        #region Properties
        #endregion

        #region Methods
        #region Constructors
        public Tipshare()
        {
            try
            {
                string errorString;
                InitializeComponent();
                sStoreName = Common.GetStoreName();
                Logger.Log("App started.. store: " + sStoreName, true);
                StaticProps.dbAddy = ConfigurationManager.AppSettings["dbAddy"];
                GetDateData();

                iBarPercent = int.Parse(ConfigurationManager.AppSettings["bar_percent"]);
                iHostPercent = int.Parse(ConfigurationManager.AppSettings["host_percent"]);

                dLowerDifferenceAllowed = ConfigHelper.LowerAmountAllowed;
                dUpperDifferenceAllowed = ConfigHelper.UpperAmountAllowed;

                dCollectionPercent = GenericDBMethods.HitDBForPercent(StaticProps.PosType, StaticProps.dbAddy, out errorString);
                if (errorString.Length > 0)
                    Logger.Log("Error getting percent: " + errorString);
                errorString = "";
                leEmployees = GenericDBMethods.HitDBForEmployees(StaticProps.PosType, StaticProps.dbAddy, out errorString);
                if (errorString.Length > 0)
                    Logger.Log("Error getting employees: " + errorString);
                vVersion = new Version(Application.ProductVersion);
                //new Version(
                //System.Configuration.ConfigurationSettings.AppSettings["version"]);
                dtLastFullRefresh = DateTime.Now;
                tTimer.Interval = 1000;
                tTimer.Start();

                SetUpDateButtons();
                this.lDate.Font = new Font(lDate.Font, FontStyle.Bold);
                SetDate();
                this.Text = this.Text
                    .Replace("##STORENAME##", sStoreName)
                    .Replace("##VERSION##", "V" + vVersion.ToString(3));
                fSettingsBox = new SettingsBox(this.Location.X / 2, this.Location.Y / 2);
                fSettingsBox.btnSave.Click += new EventHandler(Settings_Saved);
                fSettingsBox.btnShowDebug.Click += new EventHandler(DisplayDebugData);
                fDebugDisplay = new DebugDisplay();
            }
            catch (Exception e)
            {
                Globals.Log("didn't exit first part...: \r\n" + e.ToString());
            }

            string[] temp = null;
            try
            {
                WebServiceCaller wsc = new WebServiceCaller();

                if (sStoreName.Equals("Vinny", StringComparison.InvariantCultureIgnoreCase))
                    ConfigHelper.AllowUndistributed = true;
                else
                {
                    temp = wsc.GetUndistributedStores().ToLowerInvariant().Split(',');
                    ConfigHelper.AllowUndistributed = temp != null && Globals.ArrayContains(temp, sStoreName.ToLowerInvariant());
                }

                saEmployeeNamesToAddToUnallocated = wsc.GetListOfCardsToSetAsUnallocated();
                if (saEmployeeNamesToAddToUnallocated != null)
                    for (int i = 0; i < saEmployeeNamesToAddToUnallocated.Length; i++)
                        saEmployeeNamesToAddToUnallocated[i] = saEmployeeNamesToAddToUnallocated[i].ToUpperInvariant();

            }
            catch (Exception e)
            {
                dcDebugContainer[iCurrDay].ErrorStrings.Add("Exception in constructor: " + e.ToString());
                Logger.Log("Exception thrown getting undistributed stores... " + e.ToString());
                ConfigHelper.AllowUndistributed = false;
                saEmployeeNamesToAddToUnallocated = new string[] { };
            }

            Logger.Log("Exit constructor");
        }

        private void Tipshare_Load(object sender, EventArgs e)
        {
            Logger.Log(sStoreName);
            FullRefresh();
            SetUpDateButtons();
            GetDateData();
            for (int i = 0; i < ltThreads.Count; i++)
                ltThreads[i].Start(new ThreadArgs(lddDaysData[i].Date, true));
            while (ltThreads[iCurrDay].IsAlive)
                Thread.Sleep(1000);
            DisplayDay(dtCurrentDay);
        }
        #endregion

        #region ManipulatingDisplay
        private void DisplayDay(DateTime currDate)
        {
            Logger.Log("Displaying day: " + currDate.ToShortDateString());
            DisplayDebugData(iCurrDay);
            //          int i = GetDateIndex(currDate);
            DisplayDay(lddDaysData[iCurrDay].AMEntries, lddDaysData[iCurrDay].PMEntries, lddDaysData[iCurrDay].OtherEntries,
                lddDaysData[iCurrDay].AMUnallocatedSugg, lddDaysData[iCurrDay].PMUnallocatedSugg,
                lddDaysData[iCurrDay].AMUnallocatedAdj, lddDaysData[iCurrDay].PMUnallocatedAdj);
            lbDaysViewed[iCurrDay] = true;
        }

        private void SetUpDateButtons()
        {
            this.lDay.Location = new Point(this.gbControls.Size.Width / 2 - this.lDay.Size.Width / 2,
                this.lDay.Location.Y);
            this.bDayForward.Location = new Point(this.gbControls.Size.Width / 2 - this.bDayForward.Size.Width / 2
                + this.lDay.Size.Width + 10, this.bDayForward.Location.Y);
            this.bDayBack.Location = new Point(this.gbControls.Size.Width / 2 - this.bDayBack.Size.Width / 2
                - this.lDay.Size.Width - 10, this.bDayBack.Location.Y);
        }

        private void SetLabels(int DateIndex)
        {
            double dTotalAdj = lddDaysData[DateIndex].TotalAdjusted;
            double dTotalSugg = lddDaysData[DateIndex].TotalSuggested;
            double dTotalAMAdj = lddDaysData[DateIndex].TotalAMAdjusted;
            double dTotalAMSugg = lddDaysData[DateIndex].TotalAMSuggested;
            double dTotalPMAdj = lddDaysData[DateIndex].TotalPMAdjusted;
            double dTotalPMSugg = lddDaysData[DateIndex].TotalPMSuggested;
            double dTotalUnallocated = lddDaysData[DateIndex].AMUnallocatedAdj + lddDaysData[DateIndex].PMUnallocatedAdj;

            tbAMUnallocatedAdj.Text = lddDaysData[DateIndex].AMUnallocatedAdj.ToString("N2");
            tbPMUnallocatedAdj.Text = lddDaysData[DateIndex].PMUnallocatedAdj.ToString("N2");

            btnDistributeUnallocated.Enabled = dTotalUnallocated > 0;

            int iTotalShifts = lddDaysData[DateIndex].AMEntries.Count + lddDaysData[DateIndex].PMEntries.Count;

            lTotalAdj.Text = dTotalAdj.ToString("N2");
            lTotalSugg.Text = dTotalSugg.ToString("N2");
            lbTotalsOkay[DateIndex] =
                (dTotalAdj > (dTotalSugg - dLowerDifferenceAllowed) && dTotalAdj < (dTotalSugg + dUpperDifferenceAllowed)
                    && (dTotalSugg == 0 ? dTotalAdj == 0 : true))
                // check for amount > or < should be
                && (iTotalShifts == 0 || ((iTotalShifts) > 0) && dTotalAdj > 0)
                // check for shifts > 0 then amount should be > 0
                && (dTotalUnallocated < dLowerDifferenceAllowed && dTotalUnallocated > -1 * dUpperDifferenceAllowed);
            // check for unallocated total ~= 0;
            lTotalSugg.BackColor = lbTotalsOkay[DateIndex] ? Colors.BackGood : Colors.BackBad;
            lTotalSugg.ForeColor = lbTotalsOkay[DateIndex] ? Colors.FontGood : Colors.FontBad;

            bool bTemp = dTotalAMAdj > dTotalAMSugg - dLowerDifferenceAllowed && dTotalAMAdj < dTotalAMSugg + dUpperDifferenceAllowed;
            lTotalAMAdj.Text = dTotalAMAdj.ToString("N2");
            lTotalAMSugg.Text = dTotalAMSugg.ToString("N2");
            lTotalAMSugg.BackColor = bTemp ? Colors.BackGood : Colors.BackBad;
            lTotalAMSugg.ForeColor = bTemp ? Colors.FontGood : Colors.FontBad;

            bTemp = dTotalPMAdj > dTotalPMSugg - dLowerDifferenceAllowed && dTotalPMAdj < dTotalPMSugg + dUpperDifferenceAllowed;
            lTotalPMAdj.Text = dTotalPMAdj.ToString("N2");
            lTotalPMSugg.Text = dTotalPMSugg.ToString("N2");
            lTotalPMSugg.BackColor = bTemp ? Colors.BackGood : Colors.BackBad;
            lTotalPMSugg.ForeColor = bTemp ? Colors.FontGood : Colors.FontBad;
        }

        private void DisplayDay(List<Entry> eAM, List<Entry> ePM, List<Entry> eOther,
            double dAMUnallocatedSugg, double dPMUnallocatedSugg, double dAMUnallocatedAdj, double dPMUnallocatedAdj)
        {
            RemoveDisplay();

            string[] others = new string[eOther.Count];
            for (int i = 0; i < others.Length; i++)
                others[i] = eOther[i].Name + " - " + eOther[i].EmployeeID;

            ltbAMNames = new List<TextBox>();
            ltbAMIDs = new List<TextBox>();
            ltbAMSugg = new List<TextBox>();
            ltbAMAdj = new List<TextBox>();
            ltbAMHours = new List<TextBox>();
            ltbPMNames = new List<TextBox>();
            ltbPMIDs = new List<TextBox>();
            ltbPMSugg = new List<TextBox>();
            ltbPMAdj = new List<TextBox>();
            ltbPMHours = new List<TextBox>();

            lcbAMNames = new List<ComboBox>();
            lcbPMNames = new List<ComboBox>();

            Size szNameBox = new Size(146, 20);
            Size szIDBox = new Size(44, 20);
            Size szAmtBox = new Size(49, 20);
            Size szDropBox = new Size(196, 20);
            int iNameX = 6;
            int iIDX = 158;
            int iSuggX = 208;
            int iAdjX = 263;
            int iStartY = 32;
            int iIncrement = 26;

            int iIndexAM;
            for (iIndexAM = 0; iIndexAM < eAM.Count; iIndexAM++)
            {
                TextBox temp1 = new TextBox();
                temp1.Text = eAM[iIndexAM].Name;
                temp1.Location = new Point(iNameX, iStartY + iIncrement * iIndexAM);
                temp1.Size = szNameBox;
                temp1.Enabled = false;
                TextBox temp2 = new TextBox();
                temp2.Text = eAM[iIndexAM].EmployeeID;
                temp2.Location = new Point(iIDX, iStartY + iIncrement * iIndexAM);
                temp2.Size = szIDBox;
                temp2.Enabled = false;
                TextBox temp3 = new TextBox();
                temp3.Text = eAM[iIndexAM].SuggestedAmount.ToString("N2");
                temp3.Location = new Point(iSuggX, iStartY + iIncrement * iIndexAM);
                temp3.Size = szAmtBox;
                temp3.Enabled = false;
                TextBox temp4 = new TextBox();
                temp4.Text = eAM[iIndexAM].AdjustedAmount.ToString("N2");
                temp4.Location = new Point(iAdjX, iStartY + iIncrement * iIndexAM);
                temp4.Size = szAmtBox;
                temp4.TextChanged += tbAdjAmountChanged;
                TextBox temp5 = new TextBox();
                temp5.Text = eAM[iIndexAM].Hours.ToString("N2");
                temp5.Visible = false;

                ltbAMNames.Add(temp1);
                ltbAMIDs.Add(temp2);
                ltbAMSugg.Add(temp3);
                ltbAMAdj.Add(temp4);
                ltbAMHours.Add(temp5);

                gbAM.Controls.Add(ltbAMNames[ltbAMNames.Count - 1]);
                gbAM.Controls.Add(ltbAMIDs[ltbAMIDs.Count - 1]);
                gbAM.Controls.Add(ltbAMSugg[ltbAMSugg.Count - 1]);
                gbAM.Controls.Add(ltbAMAdj[ltbAMAdj.Count - 1]);
                gbAM.Controls.Add(ltbAMHours[ltbAMHours.Count - 1]);
            }

            int iIndexPM;
            for (iIndexPM = 0; iIndexPM < ePM.Count; iIndexPM++)
            {
                TextBox temp1 = new TextBox();
                temp1.Text = ePM[iIndexPM].Name;
                temp1.Location = new Point(iNameX, iStartY + iIncrement * iIndexPM);
                temp1.Size = szNameBox;
                temp1.Enabled = false;
                TextBox temp2 = new TextBox();
                temp2.Text = ePM[iIndexPM].EmployeeID;
                temp2.Location = new Point(iIDX, iStartY + iIncrement * iIndexPM);
                temp2.Size = szIDBox;
                temp2.Enabled = false;
                TextBox temp3 = new TextBox();
                temp3.Text = ePM[iIndexPM].SuggestedAmount.ToString("N2");
                temp3.Location = new Point(iSuggX, iStartY + iIncrement * iIndexPM);
                temp3.Size = szAmtBox;
                temp3.Enabled = false;
                TextBox temp4 = new TextBox();
                temp4.Text = ePM[iIndexPM].AdjustedAmount.ToString("N2");
                temp4.Location = new Point(iAdjX, iStartY + iIncrement * iIndexPM);
                temp4.Size = szAmtBox;
                temp4.TextChanged += tbAdjAmountChanged;
                TextBox temp5 = new TextBox();
                temp5.Text = ePM[iIndexPM].Hours.ToString("N2");
                temp5.Visible = false;

                ltbPMNames.Add(temp1);
                ltbPMIDs.Add(temp2);
                ltbPMSugg.Add(temp3);
                ltbPMAdj.Add(temp4);
                ltbPMHours.Add(temp5);

                gbPM.Controls.Add(ltbPMNames[ltbPMNames.Count - 1]);
                gbPM.Controls.Add(ltbPMIDs[ltbPMIDs.Count - 1]);
                gbPM.Controls.Add(ltbPMSugg[ltbPMSugg.Count - 1]);
                gbPM.Controls.Add(ltbPMAdj[ltbPMAdj.Count - 1]);
                gbPM.Controls.Add(ltbPMHours[ltbPMHours.Count - 1]);
            }

            bool bQuitAM = false;
            bool bQuitPM = false;

            for (int i = 0; i < 5 && (!bQuitAM || !bQuitPM); i++)
            {
                if (!bQuitAM && ltbAMNames.Count + i <= 13)
                {
                    ComboBox cb1 = new ComboBox();
                    cb1.Items.AddRange(others);
                    cb1.DropDownStyle = ComboBoxStyle.DropDownList;
                    cb1.Location = new Point(iNameX, iStartY + iIncrement * iIndexAM);
                    cb1.Size = szDropBox;

                    TextBox temp1 = new TextBox();
                    temp1.Text = "0.00";
                    temp1.Location = new Point(iSuggX, iStartY + iIncrement * iIndexAM);
                    temp1.Size = szAmtBox;
                    temp1.Enabled = false;
                    TextBox temp2 = new TextBox();
                    temp2.Text = "0.00";
                    temp2.Location = new Point(iAdjX, iStartY + iIncrement * iIndexAM);
                    temp2.Size = szAmtBox;
                    temp2.TextChanged += tbAdjAmountChanged;
                    TextBox temp5 = new TextBox();
                    temp5.Text = "0.00";
                    temp5.Visible = false;

                    lcbAMNames.Add(cb1);
                    ltbAMSugg.Add(temp1);
                    ltbAMAdj.Add(temp2);
                    ltbAMHours.Add(temp5);

                    gbAM.Controls.Add(lcbAMNames[lcbAMNames.Count - 1]);
                    gbAM.Controls.Add(ltbAMSugg[ltbAMSugg.Count - 1]);
                    gbAM.Controls.Add(ltbAMAdj[ltbAMAdj.Count - 1]);
                    gbAM.Controls.Add(ltbAMHours[ltbAMHours.Count - 1]);
                    iIndexAM++;
                }
                else
                    bQuitAM = true;


                if (!bQuitPM && ltbPMNames.Count + i <= 13)
                {
                    ComboBox cb2 = new ComboBox();
                    cb2.Items.AddRange(others);
                    cb2.DropDownStyle = ComboBoxStyle.DropDownList;
                    cb2.Location = new Point(iNameX, iStartY + iIncrement * iIndexPM);
                    cb2.Size = szDropBox;

                    TextBox temp3 = new TextBox();
                    temp3.Text = "0.00";
                    temp3.Location = new Point(iSuggX, iStartY + iIncrement * iIndexPM);
                    temp3.Size = szAmtBox;
                    temp3.Enabled = false;
                    TextBox temp4 = new TextBox();
                    temp4.Text = "0.00";
                    temp4.Location = new Point(iAdjX, iStartY + iIncrement * iIndexPM);
                    temp4.Size = szAmtBox;
                    temp4.TextChanged += tbAdjAmountChanged;
                    TextBox temp6 = new TextBox();
                    temp6.Text = "0.00";
                    temp6.Visible = false;

                    lcbPMNames.Add(cb2);
                    ltbPMSugg.Add(temp3);
                    ltbPMAdj.Add(temp4);
                    ltbPMHours.Add(temp6);

                    gbPM.Controls.Add(lcbPMNames[lcbPMNames.Count - 1]);
                    gbPM.Controls.Add(ltbPMSugg[ltbPMSugg.Count - 1]);
                    gbPM.Controls.Add(ltbPMAdj[ltbPMAdj.Count - 1]);
                    gbPM.Controls.Add(ltbPMHours[ltbPMHours.Count - 1]);

                    iIndexPM++;
                }
                else
                    bQuitPM = true;
            }

            tbAMUnallocatedSugg.Text = dAMUnallocatedSugg.ToString("N2");
            tbPMUnallocatedSugg.Text = dPMUnallocatedSugg.ToString("N2");

            tbAMUnallocatedAdj.Text = dAMUnallocatedAdj.ToString("N2");
            tbPMUnallocatedAdj.Text = dPMUnallocatedAdj.ToString("N2");

            SetLabels(iCurrDay);
        }

        private void RemoveDisplay()
        {
            Size tmpsz = gbAM.Size;
            Point tmplocAM = gbAM.Location;
            Point tmplocPM = gbPM.Location;
            string tmptxtAM = gbAM.Text;
            string tmptxtPM = gbPM.Text;

            this.Controls.Remove(gbAM);
            this.Controls.Remove(gbPM);

            gbAM = new GroupBox();
            gbAM.Text = tmptxtAM;
            gbAM.Size = tmpsz;
            gbAM.Location = tmplocAM;
            gbAM.Controls.Add(this.lAMName);
            gbAM.Controls.Add(this.lAMID);
            gbAM.Controls.Add(this.lAMAdj);
            gbAM.Controls.Add(this.lAMSugg);
            gbAM.Controls.Add(this.lblAMCurrentUnallocated);
            gbAM.Controls.Add(this.lblAMUnallocated);
            gbAM.Controls.Add(this.tbAMUnallocatedAdj);
            gbAM.Controls.Add(this.tbAMUnallocatedSugg);

            gbPM = new GroupBox();
            gbPM.Text = tmptxtPM;
            gbPM.Size = tmpsz;
            gbPM.Location = tmplocPM;
            gbPM.Controls.Add(this.lPMName);
            gbPM.Controls.Add(this.lPMID);
            gbPM.Controls.Add(this.lPMSugg);
            gbPM.Controls.Add(this.lPMAdj);
            gbPM.Controls.Add(this.lblPMCurrentUnallocated);
            gbPM.Controls.Add(this.lblPMUnallocated);
            gbPM.Controls.Add(this.tbPMUnallocatedAdj);
            gbPM.Controls.Add(this.tbPMUnallocatedSugg);

            this.Controls.Add(gbAM);
            this.Controls.Add(gbPM);

            /*            
            for (int i = 4; i < gbAM.Controls.Count; i++)
                gbPM.Controls.RemoveAt(i);
            for (int i = 4; i < gbPM.Controls.Count; i++)
                gbPM.Controls.RemoveAt(i);
             */
        }
        #endregion

        #region GettingData
        private void GetDataForDayByThread(object o)
        {
            ThreadArgs ta = (ThreadArgs)o;
            int i = Common.GetDateIndex(ta.DateToCheck, dtNow);
            Thread.Sleep((ltThreads.Count - i) * 2000);
            GetDataForDay(ta.CheckForSavedData, ta.DateToCheck, i, false);
        }

        private void GetDateData()
        {
            dtNow = DateTime.Now.Hour > StaticProps.AMPMBreakHour ? DateTime.Now : DateTime.Now.AddDays(-1);
            //todo: get rid of date move
            //dtNow = dtNow.AddDays(-32);
            dtCurrentDay = dtNow;
            dcDebugContainer = new List<DebugContainer>();
            DateTime dtTemp = dtCurrentDay;
            int i = 0;
            lbDaysViewed = new List<bool>();
            lbTotalsOkay = new List<bool>();
            lddDaysData = new List<DaysData>();
            ltThreads = new List<Thread>();
            while (dtTemp.DayOfWeek != ConfigHelper.StartOfWeekDay)
            {
                i++;
                lbDaysViewed.Add(false);
                lbTotalsOkay.Add(true);
                lddDaysData.Add(new DaysData(dtTemp, null, null, null));
                ltThreads.Add(new Thread(new ParameterizedThreadStart(GetDataForDayByThread)));
                dcDebugContainer.Add(new DebugContainer(dtTemp));
                dtTemp = dtTemp.AddDays(-1);
            }
            lbDaysViewed.Add(false);
            lbTotalsOkay.Add(true);
            lddDaysData.Add(new DaysData(dtTemp, null, null, null));
            ltThreads.Add(new Thread(new ParameterizedThreadStart(GetDataForDayByThread)));
            dcDebugContainer.Add(new DebugContainer(dtTemp));

            dtEarliestViewable = dtTemp.Date;

            dtDueSOWD = dtCurrentDay.AddDays(1);
            while (dtDueSOWD.DayOfWeek != ConfigHelper.StartOfWeekDay)
                dtDueSOWD = dtDueSOWD.AddDays(1);

            iCurrDay = Common.GetDateIndex(dtCurrentDay, dtNow);

            string testStart = "";
            if (ConfigHelper.bTest)
                testStart = "test-";
            sUploadFilename = testStart + "CurrentWeekUpload.csv";
            sBackwardsFilename = testStart + "CurrentWeekTS.csv";

            sInternalFilename = Common.GetInternalFilename(dtDueSOWD);

            bDayBack.Enabled = (dtCurrentDay.Date > dtEarliestViewable.Date);
            bDayForward.Enabled = (dtCurrentDay.Date < dtNow.Date);
        }

        private void GetMoreData()
        {
            int iDaysTilSDOW = FindHowManyDaysToAdd();
            Logger.Log("getting more data:  " + iDaysTilSDOW + " more days");
            for (int i = 0; i < iDaysTilSDOW; i++)
            {
                int index = lddDaysData.Count;
                lbDaysViewed.Add(false);
                lbTotalsOkay.Add(true);
                lddDaysData.Add(new DaysData(lddDaysData[index - 1].Date.AddDays(-1), null, null, null));
                ltThreads.Add(new Thread(new ParameterizedThreadStart(GetDataForDayByThread)));
                ltThreads[ltThreads.Count - 1].Start(new ThreadArgs(lddDaysData[index].Date, true));
                dcDebugContainer.Add(new DebugContainer(lddDaysData[index].Date));
            }
            DateTime dtold = dtEarliestViewable;
            dtEarliestViewable = lddDaysData[lddDaysData.Count - 1].Date;
            Logger.Log("new currently earliest viewable: " + dtEarliestViewable.ToShortDateString() + ", and total day count: " + lddDaysData.Count);
            if (dtold.Date > dtEarliestViewable.Date)
                FullRefresh();
        }

        private int FindHowManyDaysToAdd()
        {
            DateTime temp = dtEarliestViewable.AddDays(-1);
            int i = 1;
            while (temp.DayOfWeek != ConfigHelper.StartOfWeekDay)
            {
                i++;
                temp = temp.AddDays(-1);
            }
            return i;
        }

        private void GetDataForDay(bool shouldCheckForSavedData, DateTime currDate, bool Reset)
        {
            GetDataForDay(shouldCheckForSavedData, currDate, iCurrDay, Reset);
        }

        private void GetDataForDay(bool shouldCheckForSavedData, DateTime currDate)
        {
            GetDataForDay(shouldCheckForSavedData, currDate, iCurrDay, false);
        }

        private void GetDataForDay(bool shouldCheckForSavedData, DateTime currDate, int dateIndex, bool Reset)
        {
            try
            {
                Logger.Log("Getting data for: " + currDate.ToShortDateString() + "(" + dateIndex + ")");
                //           DaysData dd = new DaysData(currDate, null, null, null);
                while (lddDaysData.Count <= dateIndex)
                    lddDaysData.Add(new DaysData(currDate, null, null, null));

                if (!lddDaysData[dateIndex].DataSet || Reset)
                {
                    bool bRefresh = true;
                    if (!Reset && shouldCheckForSavedData
                        && !lbDaysViewed[dateIndex] &&
                        (File.Exists(sInternalFilename)
                        || File.Exists(sBackwardsFilename)))
                    {
                        int lineNumber = (int)Math.Ceiling((currDate - dtEarliestViewable).TotalDays);
                        string[] savedLines;
                        if (File.Exists(sBackwardsFilename))
                        {
                            savedLines = File.ReadAllLines(sBackwardsFilename);
                            File.Delete(sBackwardsFilename);
                        }
                        else
                            savedLines = File.ReadAllLines(sInternalFilename);

                        if (savedLines != null && savedLines.Length > lineNumber && lineNumber >= 0)
                        {
                            bRefresh = parseLine(savedLines[lineNumber],
                                out lddDaysData[dateIndex].AMEntries, out lddDaysData[dateIndex].PMEntries,
                                out lddDaysData[dateIndex].AMUnallocatedSugg, out lddDaysData[dateIndex].PMUnallocatedSugg);
                            if (!bRefresh)
                            {
                                lddDaysData[dateIndex].OtherEntries = entryCalcEng.GetOtherEntries(
                                    currDate, lsShifts, StaticProps.OtherJobCodes,
                                    lddDaysData[dateIndex].AMEntries, lddDaysData[dateIndex].PMEntries,
                                    saEmployeeNamesToAddToUnallocated);
                                lddDaysData[dateIndex].DataSet = true;

                                GetTotals(lddDaysData[dateIndex].AMEntries, lddDaysData[dateIndex].PMEntries,
                                    lddDaysData[dateIndex].AMUnallocatedSugg, lddDaysData[dateIndex].PMUnallocatedSugg,
                                    out lddDaysData[dateIndex].TotalAMSuggested, out lddDaysData[dateIndex].TotalPMSuggested);
                                lddDaysData[dateIndex].TotalSuggested = lddDaysData[dateIndex].TotalAMSuggested + lddDaysData[dateIndex].TotalPMSuggested;

                                lddDaysData[dateIndex].SetAdjustedEqualToSuggested();

                                lddDaysData[dateIndex].DataSet = true;
                            }
                        }
                        else
                            bRefresh = true;
                    }

                    if (Reset || bRefresh)
                    {
                        if (dtLastFullRefresh < DateTime.Now.AddMinutes(-30))
                            FullRefresh();

                        Dictionary<int, Shift> lsCurrServerShifts = Common.GetServerShifts(currDate, lsShifts);
                        foreach (int key in lsCurrServerShifts.Keys)
                        {
                            string currId = lsCurrServerShifts[key].EmployeeID;
                            foreach (int key2 in lsCurrServerShifts.Keys)
                                if (lsCurrServerShifts[key2].EmployeeID == currId && key != key2)
                                {
                                }
                        }
                        dcDebugContainer[dateIndex].Shifts = lsCurrServerShifts;
                        double dTotalPMSales, dTotalAMSales;
                        double dOverrideAMTips, dOverridePMTips;

                        processorEng.GetSales(lsCurrServerShifts, ltTickets, out dTotalAMSales, out dOverrideAMTips, out dTotalPMSales, out dOverridePMTips);

                        dcDebugContainer[dateIndex].Tickets = GetAllTicketsForDate(ltTickets, dtCurrentDay);
                        dcDebugContainer[dateIndex].AMSales = dTotalAMSales;
                        dcDebugContainer[dateIndex].PMSales = dTotalPMSales;
                        dcDebugContainer[dateIndex].AMOverrideTipsAddition = dOverrideAMTips;
                        dcDebugContainer[dateIndex].PMOverrideTipsAddition = dOverridePMTips;

                        double dTotalAMBarHours, dTotalPMBarHours;
                        double dTotalAMHostHours, dTotalPMHostHours;

                        Dictionary<int, Shift> lsCurrAMHostShifts, lsCurrAMBarShifts;
                        Dictionary<int, Shift> lsCurrPMHostShifts, lsCurrPMBarShifts;
                        List<Entry> leOthers;

                        shiftDeterEng.GetShifts(currDate, lsShifts,
                            StaticProps.BarJobCodes, out lsCurrAMBarShifts, out lsCurrPMBarShifts);
                        shiftDeterEng.GetShifts(currDate, lsShifts,
                            StaticProps.HostJobCodes, out lsCurrAMHostShifts, out lsCurrPMHostShifts);
                        leOthers = entryCalcEng.GetOtherEntries(currDate, lsShifts, StaticProps.OtherJobCodes,
                            lsCurrAMBarShifts, lsCurrAMHostShifts,
                            lsCurrPMBarShifts, lsCurrPMHostShifts,
                            saEmployeeNamesToAddToUnallocated);

                        Dictionary<int, Shift> shiftsToAddTo = dcDebugContainer[dateIndex].Shifts;

                        foreach (int key in lsCurrAMHostShifts.Keys)
                            shiftsToAddTo.Add(shiftsToAddTo.Count, lsCurrAMHostShifts[key]);
                        foreach (int key in lsCurrAMBarShifts.Keys)
                            shiftsToAddTo.Add(shiftsToAddTo.Count, lsCurrAMBarShifts[key]);
                        foreach (int key in lsCurrPMHostShifts.Keys)
                            shiftsToAddTo.Add(shiftsToAddTo.Count, lsCurrPMHostShifts[key]);
                        foreach (int key in lsCurrPMBarShifts.Keys)
                            shiftsToAddTo.Add(shiftsToAddTo.Count, lsCurrPMBarShifts[key]);

                        dcDebugContainer[dateIndex].Shifts = shiftsToAddTo;

                        dTotalAMBarHours = basicCalcEng.AddUpHours(lsCurrAMBarShifts);
                        dTotalPMBarHours = basicCalcEng.AddUpHours(lsCurrPMBarShifts);
                        dTotalAMHostHours = basicCalcEng.AddUpHours(lsCurrAMHostShifts);
                        dTotalPMHostHours = basicCalcEng.AddUpHours(lsCurrPMHostShifts);

                        dcDebugContainer[dateIndex].AMBarHours = dTotalAMBarHours;
                        dcDebugContainer[dateIndex].PMBarHours = dTotalPMBarHours;
                        dcDebugContainer[dateIndex].AMHostHours = dTotalAMHostHours;
                        dcDebugContainer[dateIndex].PMHostHours = dTotalPMHostHours;

                        double dTotalAMShare = Math.Round(dTotalAMSales * dCollectionPercent, 2);
                        double dTotalPMShare = Math.Round(dTotalPMSales * dCollectionPercent, 2);

                        dcDebugContainer[dateIndex].AMTipshare = dTotalAMShare;
                        dcDebugContainer[dateIndex].PMTipshare = dTotalPMShare;

                        if (ConfigHelper.EnableBarHostSplit)
                        {
                            lddDaysData[dateIndex].AMEntries = entryCalcEng.GetEntries(
                                dTotalAMShare * ((double)iBarPercent) / 100.0, lsCurrAMBarShifts, dTotalAMBarHours,
                                dTotalAMShare * ((double)iHostPercent) / 100.0, lsCurrAMHostShifts, dTotalAMHostHours,
                                saEmployeeNamesToAddToUnallocated, out lddDaysData[dateIndex].AMUnallocatedSugg);

                            lddDaysData[dateIndex].PMEntries = entryCalcEng.GetEntries(
                                dTotalPMShare * ((double)iBarPercent) / 100.0, lsCurrPMBarShifts, dTotalPMBarHours,
                                dTotalPMShare * ((double)iHostPercent) / 100.0, lsCurrPMHostShifts, dTotalPMHostHours,
                                saEmployeeNamesToAddToUnallocated, out lddDaysData[dateIndex].PMUnallocatedSugg);
                        }
                        else
                        {
                            Dictionary<int, Shift> allAMShifts = new Dictionary<int, Shift>();
                            foreach (var key in lsCurrAMBarShifts.Keys)
                                allAMShifts.Add(key, lsCurrAMBarShifts[key]);
                            foreach (var key in lsCurrAMHostShifts.Keys)
                                allAMShifts.Add(allAMShifts.Keys.Count + 1, lsCurrAMHostShifts[key]);

                            Dictionary<int, Shift> allPMShifts = new Dictionary<int, Shift>();
                            foreach (var key in lsCurrPMBarShifts.Keys)
                                allPMShifts.Add(key, lsCurrPMBarShifts[key]);
                            foreach (var key in lsCurrPMHostShifts.Keys)
                                allPMShifts.Add(allPMShifts.Keys.Count + 1, lsCurrPMHostShifts[key]);

                            lddDaysData[dateIndex].AMEntries = entryCalcEng.GetEntries(
                                dTotalAMShare, allAMShifts, dTotalAMBarHours + dTotalAMHostHours,
                                0, null, 0,
                                saEmployeeNamesToAddToUnallocated, out lddDaysData[dateIndex].AMUnallocatedSugg);

                            lddDaysData[dateIndex].PMEntries = entryCalcEng.GetEntries(
                                dTotalPMShare, allPMShifts, dTotalPMBarHours + dTotalPMHostHours,
                                0, null, 0,
                                saEmployeeNamesToAddToUnallocated, out lddDaysData[dateIndex].PMUnallocatedSugg);
                        }

                        lddDaysData[dateIndex].AMUnallocatedSugg += dOverrideAMTips;
                        lddDaysData[dateIndex].PMUnallocatedSugg += dOverridePMTips;

                        lddDaysData[dateIndex].OtherEntries = leOthers;

                        GetTotals(lddDaysData[dateIndex].AMEntries, lddDaysData[dateIndex].PMEntries,
                            lddDaysData[dateIndex].AMUnallocatedSugg, lddDaysData[dateIndex].PMUnallocatedSugg,
                            out lddDaysData[dateIndex].TotalAMSuggested, out lddDaysData[dateIndex].TotalPMSuggested);
                        lddDaysData[dateIndex].TotalSuggested = lddDaysData[dateIndex].TotalAMSuggested + lddDaysData[dateIndex].TotalPMSuggested;

                        lddDaysData[dateIndex].SetAdjustedEqualToSuggested();

                        lddDaysData[dateIndex].DataSet = true;

                        //DisplayDebugData(dateIndex);
                    }
                }

            }
            catch (Exception except)
            {
                dcDebugContainer[dateIndex].ErrorStrings.Add("Exception getting data for day: " + except.ToString());
                Logger.Log("Exception caught getting data for " + currDate.ToShortDateString() + ", " + except.ToString());
            }
        }

        private Dictionary<int, Ticket> GetAllTicketsForDate(Dictionary<int, Ticket> tickets, DateTime currentDay)
        {
            Dictionary<int, Ticket> results = new Dictionary<int, Ticket>();
            if (tickets != null && tickets.Count > 0)
                foreach (int key in tickets.Keys)
                {
                    Ticket tick = tickets[key];
                    if (tick.ServeDate.Date == currentDay.Date ||
                        (tick.ServeDate.Date == currentDay.AddDays(1).Date && tick.ServeDate.Hour < 4))
                        results.Add(results.Count, tick);
                    else { }
                }
            return results;
        }

        private void GetTotals(List<Entry> eAM, List<Entry> ePM,
            double AMUnallocatedSugg, double PMUnallocatedSugg,
            out double totalAM, out double totalPM)
        {
            totalAM = AMUnallocatedSugg;
            totalPM = PMUnallocatedSugg;
            foreach (Entry e in eAM)
                totalAM += e.SuggestedAmount;
            foreach (Entry e in ePM)
                totalPM += e.SuggestedAmount;
        }

        private void DistributeUnallocatedFunds()
        {
            double dAMUnallocated, dPMUnallocated;
            if (double.TryParse(tbAMUnallocatedAdj.Text, out dAMUnallocated)
                && double.TryParse(tbPMUnallocatedAdj.Text, out dPMUnallocated))
            {
                int dateIndex = Common.GetDateIndex(dtCurrentDay, dtNow);
                double dAMTotalHours = 0, dPMTotalHours = 0;
                if (dAMUnallocated != 0)
                {
                    foreach (Entry ent in lddDaysData[dateIndex].AMEntries)
                        dAMTotalHours += ent.Hours;
                    if (dAMTotalHours > 0)
                    {
                        double dTempHours;
                        foreach (Entry ent in lddDaysData[dateIndex].AMEntries)
                        {
                            dTempHours = ent.Hours;
                            if (dTempHours > 0)
                                ent.AdjustedAmount += Math.Round((dTempHours / dAMTotalHours * dAMUnallocated), 2);
                        }
                    }
                }
                if (dPMUnallocated != 0)
                {
                    foreach (Entry ent in lddDaysData[dateIndex].PMEntries)
                        dPMTotalHours += ent.Hours;
                    if (dPMTotalHours > 0)
                    {
                        double dTempHours;
                        foreach (Entry ent in lddDaysData[dateIndex].PMEntries)
                        {
                            dTempHours = ent.Hours;
                            if (dTempHours > 0)
                                ent.AdjustedAmount += Math.Round((dTempHours / dPMTotalHours * dPMUnallocated), 2);
                        }
                    }
                }
            }
            else
                MessageBox.Show("Unallocated text boxes should contain numberical data");
        }
        #endregion

        #region BasicMethods
        private void SetDate()
        {
            //dcDebugContainer[GetDateIndex(dtCurrentDay)] = new DebugContainer(dtCurrentDay);
            this.lDate.Text = dtCurrentDay.ToLongDateString();
            int x = this.gbControls.Size.Width / 2 - this.lDate.Size.Width / 2 - 1;
            this.lDate.Location = new Point(x, this.lDate.Location.Y);
        }

        private string GetSSN(string Name, string EmployeeID)
        {
            Name = Name.ToLower();
            foreach (int key in leEmployees.Keys)
            {
                Employee emp = leEmployees[key];
                if (emp.Name.ToLower() == Name && emp.EmployeeID == EmployeeID)
                    return emp.SSN;
            }
            foreach (int key in leEmployees.Keys)
            {
                Employee emp = leEmployees[key];
                if (emp.EmployeeID == EmployeeID)
                    return emp.SSN;
            }
            return "**" + EmployeeID;
        }

        private void FullRefresh()
        {
            Logger.Log("Performing full refresh");
            string errorString;
            lsShifts = GenericDBMethods.HitDBForShifts(StaticProps.PosType, StaticProps.dbAddy, dtEarliestViewable, out errorString);
            if (errorString.Length > 0)
                Logger.Log("Error getting shifts: " + errorString);
            errorString = "";
            ltTickets = GenericDBMethods.HitDBForTickets(StaticProps.PosType, StaticProps.dbAddy, dtEarliestViewable, out errorString);
            if (errorString.Length > 0)
                Logger.Log("Error getting tickets: " + errorString);
        }

        //private int isTicketInShifts(Ticket ticket, Dictionary<int, Shift> shifts)
        //{
        //    foreach (int i in shifts.Keys)
        //    {
        //        if (shifts[i].EmployeeID == ticket.EmployeeID)
        //        {
        //            int temp = ShiftAndTicketIsAM(shifts[i], ticket);
        //            if (temp != 0)
        //            {
        //                return temp;
        //            }
        //        }
        //    }
        //    return 0;
        //}
        #endregion

        #region ImportFromFiles
        private bool parseLine(string Line, out List<Entry> eAM, out List<Entry> ePM, out double amUnallocated, out double pmUnallocated)
        {
            bool Errored = false;
            eAM = new List<Entry>();
            ePM = new List<Entry>();
            amUnallocated = 0;
            pmUnallocated = 0;
            string[] cells = Line.Split(',');
            if (cells != null && cells.Length > 1)
            {
                try
                {
                    int NumFields = 3;
                    int NameEmpField = 1;
                    int SuggField = 2;
                    int AdjField = 3;
                    int iNumAM = int.Parse(cells[0]);
                    for (int i = 0; i < iNumAM; i++)
                        eAM.Add(new Entry(
                            Common.GetName(cells[NumFields * i + NameEmpField]),
                            Common.GetID(cells[NumFields * i + NameEmpField]),
                            double.Parse(cells[NumFields * i + SuggField]),
                            double.Parse(cells[NumFields * i + AdjField]),
                            -1
                            ));
                    int PMOffset = iNumAM * NumFields + 1;
                    int iNumPM = int.Parse(cells[iNumAM * AdjField + 1]);
                    for (int i = 0; i < iNumPM; i++)
                        ePM.Add(new Entry(
                            Common.GetName(cells[PMOffset + NumFields * i + NameEmpField]),
                            Common.GetID(cells[PMOffset + NumFields * i + NameEmpField]),
                            double.Parse(cells[PMOffset + NumFields * i + SuggField]),
                            double.Parse(cells[PMOffset + NumFields * i + AdjField]),
                            -1
                            ));

                    int AMUnallocatedOffset = PMOffset + NumFields * iNumPM + 1;
                    int PMUnallocatedOffset = PMOffset + NumFields * iNumPM + 2;
                    if (cells.Length > PMUnallocatedOffset)
                    {
                        amUnallocated = double.Parse(cells[AMUnallocatedOffset]);
                        pmUnallocated = double.Parse(cells[PMUnallocatedOffset]);
                    }
                    Errored = false;
                }
                catch (Exception e)
                {
                    Errored = true;
                    dcDebugContainer[iCurrDay].ErrorStrings.Add("Exception parsing entries: " + e.ToString());
                }
            }
            else
                Errored = true;
            return Errored;
        }
        #endregion

        #region Printing

        //StringReader myReader;
        //PrintDocument ThePrintDocument = new PrintDocument();
        //PrintDialog pd;

        private string GetPrintText()
        {
            string text = dtCurrentDay.ToLongDateString();
            //       int currDay = GetDateIndex(dtCurrentDay);
            text += Environment.NewLine + Environment.NewLine + "AM";
            foreach (Entry ent in lddDaysData[iCurrDay].AMEntries)
            {
                text += Environment.NewLine + "_________\t  $" +
                    (ent.AdjustedAmount < 10.0 ? " " + ent.AdjustedAmount : "" + ent.AdjustedAmount) +
                    "\t" + ent.EmployeeID + " - " + ent.Name;
            }
            text += Environment.NewLine + Environment.NewLine + "PM";
            foreach (Entry ent in lddDaysData[iCurrDay].PMEntries)
            {
                text += Environment.NewLine + "_________\t  $" +
                    (ent.AdjustedAmount < 10.0 ? " " + ent.AdjustedAmount : "" + ent.AdjustedAmount) +
                    "\t" + ent.EmployeeID + " - " + ent.Name;
            }
            return text;
        }
        #endregion

        #region Save
        private void SaveData()
        {
            try
            {
                bool bContinue = true;
                string message = "";
                bool bEndLoop = false;

                int iStartDateIndex = Common.GetDateIndex(DetermineFirstDayToSave(), dtNow);
                DateTime dtEndDate = DetermineLastDayToSave();
                int iEndDateIndex = Common.GetDateIndex(dtEndDate, dtNow);

                dtDueSOWD = dtEndDate;
                while (dtDueSOWD.DayOfWeek != ConfigHelper.StartOfWeekDay)
                    dtDueSOWD = dtDueSOWD.AddDays(1);

                for (int i = iStartDateIndex; i >= iEndDateIndex && !bEndLoop; i--)
                {
                    Logger.Log("date okay? " + lddDaysData[i].Date.ToShortDateString() + ", " + lbTotalsOkay[i].ToString());
                    if (lbTotalsOkay.Count > i)// && i >0)
                    {
                        bContinue = lbTotalsOkay[i] && bContinue;
                        message += lbTotalsOkay[i] ? "" : Environment.NewLine + " not done for " + Common.GetDayName(i) + ".";
                    }
                    else
                    {
                        Logger.Log("trying to save " + i + " (between " + iStartDateIndex + " & " + iEndDateIndex + "), total okay count: " + lbTotalsOkay.Count);
                        bContinue = false;
                        bEndLoop = true;
                    }
                }

                if (bContinue)
                {
                    List<List<SaveInternalData>> leInternalToSaveAM = new List<List<SaveInternalData>>();
                    List<List<SaveInternalData>> leInternalToSavePM = new List<List<SaveInternalData>>();
                    List<double> ldAMUnallocatedSugg = new List<double>();
                    List<double> ldPMUnallocatedSugg = new List<double>();
                    List<SaveUploadData> leUploadToSave = new List<SaveUploadData>();
                    //int i = 0;
                    //foreach (DaysData data in lddDaysData)
                    for (int i = iStartDateIndex; i >= iEndDateIndex; i--)
                    {
                        leInternalToSaveAM.Add(new List<SaveInternalData>());
                        leInternalToSavePM.Add(new List<SaveInternalData>());

                        foreach (Entry entry in lddDaysData[i].AMEntries)
                        {
                            leInternalToSaveAM[iStartDateIndex - i].Add(new SaveInternalData(entry.Name, entry.EmployeeID, lddDaysData[i].Date.Date, entry.SuggestedAmount, entry.AdjustedAmount));
                            if (entry.EmployeeID != "UNDIST")
                            {
                                leUploadToSave.Add(new SaveUploadData(entry.EmployeeID, lddDaysData[i].Date.Date, entry.AdjustedAmount));
                            }
                        }
                        foreach (Entry entry in lddDaysData[i].PMEntries)
                        {
                            leInternalToSavePM[iStartDateIndex - i].Add(new SaveInternalData(entry.Name, entry.EmployeeID, lddDaysData[i].Date.Date, entry.SuggestedAmount, entry.AdjustedAmount));
                            if (entry.EmployeeID != "UNDIST")
                            {
                                leUploadToSave.Add(new SaveUploadData(entry.EmployeeID, lddDaysData[i].Date.Date, entry.AdjustedAmount));
                            }
                            else
                            {
                            }
                        }
                        ldAMUnallocatedSugg.Add(lddDaysData[i].AMUnallocatedSugg);
                        ldPMUnallocatedSugg.Add(lddDaysData[i].PMUnallocatedSugg);
                        //i++;
                    }
                    string sUploadToWrite = GenerateUploadString(leUploadToSave);
                    string sInternalToWrite = GenerateInternalString(
                        leInternalToSaveAM, leInternalToSavePM, ldAMUnallocatedSugg, ldPMUnallocatedSugg);

                    using (StreamWriter sw = new StreamWriter(sUploadFilename))
                    {
                        sw.Write(sUploadToWrite);
                        sw.Close();
                    }
                    using (StreamWriter sw = new StreamWriter(sBackwardsFilename))
                    {
                        sw.Write(sInternalToWrite);
                        sw.Close();
                    }
                    using (StreamWriter sw = new StreamWriter(Common.GetInternalFilename(dtDueSOWD)))
                    {
                        sw.Write(sInternalToWrite);
                        sw.Close();
                    }

                    string result = "fail";
                    try
                    {
                        WebServiceCaller wsc = new WebServiceCaller();

                        result = wsc.RecordProgramUsage(sStoreName);
                    }
                    catch (Exception e)
                    {
                        Logger.Log("Exception thrown while hitting ws during save: " + e.ToString());
                        dcDebugContainer[iCurrDay].ErrorStrings.Add("Exception hitting webservice: " + e.ToString());
                        result = "fail";
                    }
                    if (!result.Contains("Success"))
                        MessageBox.Show("Save successful, but did not write to WS." + Environment.NewLine +
                            "Contact ITS to get credit for this save!", "Confirmed Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Save successful", "Confirmed Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Save unsuccessful... If you just opened the program, give it a moment to refresh.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception e)
            {
                dcDebugContainer[iCurrDay].ErrorStrings.Add("Exception saving: " + e.ToString());
                MessageBox.Show("Save unsuccessful... If you just opened the program, give it a moment to refresh.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public DateTime DetermineFirstDayToSave()
        {
            DateTime temp = dtCurrentDay;
            while (temp.DayOfWeek != ConfigHelper.StartOfWeekDay)
                temp = temp.AddDays(-1);
            return temp;
        }

        public DateTime DetermineLastDayToSave()
        {
            DateTime temp = dtCurrentDay;
            while (temp.DayOfWeek != ConfigHelper.StartOfWeekDay_MinusOne && temp.Date < dtNow.Date)
                temp = temp.AddDays(1);
            return temp;
        }

        #region Save Strings
        private string GenerateUploadString(List<SaveUploadData> uploadToSave)
        {
            if (uploadToSave.Count > 0)
            {
                StringBuilder result = new StringBuilder();
                foreach (SaveUploadData sud in uploadToSave)
                    result.Append(Environment.NewLine + sud.DateOfData.ToShortDateString() + "," +
                        sud.SSN + "," + sud.AdjustedAmount);
                return result.Remove(0, Environment.NewLine.Length).ToString();
            }
            return "";
        }

        private string GenerateInternalString
            (List<List<SaveInternalData>> internalToSaveAM, List<List<SaveInternalData>> internalToSavePM,
            List<double> amUnallocatedSugg, List<double> pmUnallocatedSugg)
        {
            if (internalToSaveAM.Count > 0
                && internalToSavePM.Count > 0)
            {
                StringBuilder result = new StringBuilder(sStoreName + "," + dtDueSOWD);
                for (int i = 0; i < internalToSaveAM.Count; i++)
                {
                    result.Append(Environment.NewLine + internalToSaveAM[i].Count);
                    foreach (SaveInternalData sid in internalToSaveAM[i])
                        result.Append("," + sid.Name + " - " + sid.EmployeeID + "," + sid.SuggestedAmount + "," + sid.AdjustedAmount);
                    result.Append("," + internalToSavePM[i].Count + "");
                    foreach (SaveInternalData sid in internalToSavePM[i])
                        result.Append("," + sid.Name + " - " + sid.EmployeeID + "," + sid.SuggestedAmount + "," + sid.AdjustedAmount);
                    result.Append("," + amUnallocatedSugg[i] + "," + pmUnallocatedSugg[i]);
                    //  result += Environment.NewLine;
                }
                return result.ToString();
            }
            return "";
        }
        #endregion
        #endregion

        #region Events
        private void tbAdjAmountChanged(object sender, EventArgs e)
        {
            lddDaysData[iCurrDay].TotalAdjusted = 0;
            lddDaysData[iCurrDay].TotalAMAdjusted = 0;
            lddDaysData[iCurrDay].TotalPMAdjusted = 0;
            double dTemp;
            for (int i = 0; i < ltbAMAdj.Count; i++)
            {
                if ((i < ltbAMAdj.Count - lcbAMNames.Count ||
                    (i >= ltbAMAdj.Count - lcbAMNames.Count
                    && lcbAMNames[i - ltbAMAdj.Count + lcbAMNames.Count].SelectedItem != null
                    && lcbAMNames[i - ltbAMAdj.Count + lcbAMNames.Count].SelectedItem.ToString() != ""))
                    && double.TryParse(ltbAMAdj[i].Text, out dTemp))
                {
                    lddDaysData[iCurrDay].TotalAMAdjusted += Math.Round(dTemp, 2);
                }
            }
            for (int i = 0; i < ltbPMAdj.Count; i++)
            {
                if ((i < ltbPMAdj.Count - lcbPMNames.Count ||
                    (i >= ltbPMAdj.Count - lcbPMNames.Count
                    && lcbPMNames[i - ltbPMAdj.Count + lcbPMNames.Count].SelectedItem != null
                    && lcbPMNames[i - ltbPMAdj.Count + lcbPMNames.Count].SelectedItem.ToString() != ""))
                    && double.TryParse(ltbPMAdj[i].Text, out dTemp))
                {
                    lddDaysData[iCurrDay].TotalPMAdjusted += Math.Round(dTemp, 2);
                }
            }

            lddDaysData[iCurrDay].TotalAdjusted = lddDaysData[iCurrDay].TotalAMAdjusted + lddDaysData[iCurrDay].TotalPMAdjusted;

            SetLabels(iCurrDay);
        }
        #endregion

        #region EventsRegistered
        private void Settings_Saved(object sender, EventArgs e)
        {
            if (fSettingsBox.PastAvailable())
            {
                GetMoreData();
            }
            bDayBack.Enabled = (dtCurrentDay.Date > dtEarliestViewable.Date)
                || fSettingsBox.PastAvailable();
            bDayForward.Enabled = (dtCurrentDay.Date < dtNow.Date);
        }

        private void DisplayDebugData(object sender, EventArgs e)
        {
            DisplayDebugData(iCurrDay);
            fDebugDisplay.Visible = true;
        }

        private void DisplayDebugData(int dayToDisplay)
        {
            if (fDebugDisplay == null)
                fDebugDisplay = new DebugDisplay() { Visible = false };

            fDebugDisplay.DisplayData(dcDebugContainer[dayToDisplay]);
            if (fDebugDisplay.Visible)
                fDebugDisplay.TopMost = true;
        }
        #endregion

        #region Button Clicks
        private void bDayBack_Click(object sender, EventArgs e)
        {
            List<Entry> amEntries, pmEntries;
            saveDataEng.SaveDaysData(iCurrDay, lsShifts, dtCurrentDay,
                ltbAMAdj, ltbAMSugg, ltbAMHours,
                ltbAMIDs, ltbAMNames, lcbAMNames,
                ltbPMAdj, ltbPMSugg, ltbPMHours,
                ltbPMIDs, ltbPMNames, lcbPMNames,
                out amEntries, out pmEntries,
                this.lddDaysData[iCurrDay].TotalAMSuggested,
                this.lddDaysData[iCurrDay].TotalPMSuggested);
            this.lddDaysData[iCurrDay].AMEntries = amEntries;
            this.lddDaysData[iCurrDay].PMEntries = pmEntries;

            dtCurrentDay = dtCurrentDay.AddDays(-1);
            iCurrDay = Common.GetDateIndex(dtCurrentDay, dtNow);
            SetDate();

            while (!lddDaysData[iCurrDay].DataSet)
                Thread.Sleep(1000);

            DisplayDay(dtCurrentDay);

            bDayBack.Enabled = (dtCurrentDay.Date > dtEarliestViewable.Date)
                || fSettingsBox.PastAvailable();
            bDayForward.Enabled = (dtCurrentDay.Date < dtNow.Date);

            if (fSettingsBox.PastAvailable() && lddDaysData.Count <= iCurrDay + 1)
                GetMoreData();
        }

        private void bDayForward_Click(object sender, EventArgs e)
        {
            List<Entry> amEntries, pmEntries;
            saveDataEng.SaveDaysData(iCurrDay, lsShifts, dtCurrentDay,
                ltbAMAdj, ltbAMSugg, ltbAMHours,
                ltbAMIDs, ltbAMNames, lcbAMNames,
                ltbPMAdj, ltbPMSugg, ltbPMHours,
                ltbPMIDs, ltbPMNames, lcbPMNames,
                out amEntries, out pmEntries,
                this.lddDaysData[iCurrDay].TotalAMSuggested,
                this.lddDaysData[iCurrDay].TotalPMSuggested);
            this.lddDaysData[iCurrDay].AMEntries = amEntries;
            this.lddDaysData[iCurrDay].PMEntries = pmEntries;

            dtCurrentDay = dtCurrentDay.AddDays(1);
            iCurrDay = Common.GetDateIndex(dtCurrentDay, dtNow);
            SetDate();
            //     GetDataForDay(true, dtCurrentDay);
            while (!lddDaysData[iCurrDay].DataSet)
                Thread.Sleep(1000);
            //     Thread.Sleep(2000);
            DisplayDay(dtCurrentDay);

            bDayBack.Enabled = (dtCurrentDay.Date > dtEarliestViewable.Date)
                || fSettingsBox.PastAvailable();
            bDayForward.Enabled = (dtCurrentDay.Date < dtNow.Date);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            GetDataForDay(false, dtCurrentDay, true);
            DisplayDay(dtCurrentDay);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            fSettingsBox.Open(iBarPercent, iHostPercent);
            fSettingsBox.TopMost = true;
        }

        private void tTimer_Tick(object sender, EventArgs e)
        {
            if (fSettingsBox.bRefresh)
            {
                fSettingsBox.bRefresh = false;
                fSettingsBox.dtDateChanged = DateTime.Now;
                GetDataForDay(false, dtCurrentDay);
                DisplayDay(dtCurrentDay);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<Entry> amEntries, pmEntries;
            saveDataEng.SaveDaysData(iCurrDay, lsShifts, dtCurrentDay,
                ltbAMAdj, ltbAMSugg, ltbAMHours,
                ltbAMIDs, ltbAMNames, lcbAMNames,
                ltbPMAdj, ltbPMSugg, ltbPMHours,
                ltbPMIDs, ltbPMNames, lcbPMNames,
                out amEntries, out pmEntries,
                this.lddDaysData[iCurrDay].TotalAMSuggested,
                this.lddDaysData[iCurrDay].TotalPMSuggested);
            this.lddDaysData[iCurrDay].AMEntries = amEntries;
            this.lddDaysData[iCurrDay].PMEntries = pmEntries;

            SaveData();
        }

        private void miHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Each day needs to be viewed and the program needs to be saved.");
        }

        private void miAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "This Tipshare program was written by Nick Sherrill and " +
                "distributed to Concord Enterprises for the exclusive use " +
                "of their stores.  Any questions need to be directed to " +
                "solutioncenter@concordei.com.");
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("exiting");
        }

        private void miPrint_Click(object sender, EventArgs e)
        {
            try
            {
                //PrintDialog pd = new PrintDialog();
                //TextPrintDocument tDoc = new TextPrintDocument();
                //tDoc.Text = GetPrintText();//.Replace("\t", "  ");
                //tDoc.Font = new Font("Serif", 12);
                //pd.Document = tDoc;

                //tDoc.Print();
            }
            catch (Exception exc)
            {
                //string tmep = exc.ToString();
                dcDebugContainer[iCurrDay].ErrorStrings.Add("Exception attempting to print: " + exc.ToString());
                MessageBox.Show("Exception thrown attempting to print.  Please email IT with this error:" +
                    Environment.NewLine + exc.Message.ToString(), "Exception thrown",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnDistributeUnallocated_Click(object sender, EventArgs e)
        {
            List<Entry> amEntries, pmEntries;
            saveDataEng.SaveDaysData(iCurrDay, lsShifts, dtCurrentDay,
                ltbAMAdj, ltbAMSugg, ltbAMHours,
                ltbAMIDs, ltbAMNames, lcbAMNames,
                ltbPMAdj, ltbPMSugg, ltbPMHours,
                ltbPMIDs, ltbPMNames, lcbPMNames,
                out amEntries, out pmEntries,
                this.lddDaysData[iCurrDay].TotalAMSuggested,
                this.lddDaysData[iCurrDay].TotalPMSuggested);
            this.lddDaysData[iCurrDay].AMEntries = amEntries;
            this.lddDaysData[iCurrDay].PMEntries = pmEntries;

            DistributeUnallocatedFunds();
            DisplayDay(dtCurrentDay);
            tbAdjAmountChanged(null, null);
        }
        #endregion
        #endregion
    }
}
