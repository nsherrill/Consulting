using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Data;

namespace Tipshare
{
    public class SaveInternalData
    {
        public string Name, EmployeeID;
        public DateTime DateOfData;
        public double SuggestedAmount, AdjustedAmount;

        public SaveInternalData(string Name, string EmployeeID, DateTime DateOfData, double SuggestedAmount, double AdjustedAmount)
        {
            this.Name = Name;
            this.EmployeeID = EmployeeID;
            this.DateOfData = DateOfData;
            this.SuggestedAmount = SuggestedAmount;
            this.AdjustedAmount = AdjustedAmount;
        }
    }

    public class SaveUploadData
    {
        public string SSN;
        public DateTime DateOfData;
        public double AdjustedAmount;

        public SaveUploadData(string SSN, DateTime DateOfData, double AdjustedAmount)
        {
            this.SSN = SSN;
            this.DateOfData = DateOfData;
            this.AdjustedAmount = AdjustedAmount;
        }
    }

    public class DeclaredTipshare
    {
        public DateTime dateOfSales;
        public double amTipshareValues, pmTipshareValues;
    }

    public class ServerSales
    {
        public string empId;
        public double salesAM;
        public double salesPM;

        public bool shouldOverride = false;
    }

    public class Entry : IComparable
    {
        public string Name, EmployeeID;
        public double SuggestedAmount, AdjustedAmount, Hours;
        public Entry(string Name, string EmployeeID, double SuggestedAmount, double AdjustedAmount, double Hours)
        {
            this.Name = Name;
            this.EmployeeID = EmployeeID;
            this.SuggestedAmount = SuggestedAmount;
            this.AdjustedAmount = AdjustedAmount;
            this.Hours = Hours;
        }

        public int CompareTo(object obj)
        {
            if (obj is Entry)
                return this.Name.CompareTo(((Entry)obj).Name);
            else
                return 0;
        }
    }

    public class Shift
    {
        public string Name, EmployeeID, JobCode;
        public DateTime ClockIn, ClockOut;
        public Shift(string Name, string EmployeeID, string JobCode, DateTime ClockIn, DateTime ClockOut)
        {
            this.Name = Name;
            this.EmployeeID = EmployeeID;
            this.JobCode = JobCode;
            this.ClockIn = ClockIn;
            this.ClockOut = ClockOut;
        }

        public double Hours
        {
            get
            {
                return (ClockOut - ClockIn).TotalHours;
            }
        }

        //public bool IsAM
        //{
        //    get
        //    {
        //        return this.ClockOut.Hour <= 17 && this.ClockOut.Hour > 4;
        //    }
        //}
    }

    public class ThreadArgs
    {
        public DateTime DateToCheck;
        public bool CheckForSavedData;
        public ThreadArgs(DateTime DateToCheck, bool CheckForSavedData)
        {
            this.DateToCheck = DateToCheck;
            this.CheckForSavedData = CheckForSavedData;
        }
    }

    public class Ticket
    {
        public string EmployeeID;
        public DateTime ServeDate;
        public double Amount;
        public Ticket(string EmployeeID, DateTime ServeDate, double Amount)
        {
            this.EmployeeID = EmployeeID;
            this.ServeDate = ServeDate;
            this.Amount = Amount;
        }
    }

    public class Employee
    {
        public string Name, SSN, EmployeeID;

        public Employee(string Name, string EmployeeID, string SSN)
            : this(Name, EmployeeID)
        {
            this.SSN = SSN;
        }

        public Employee(string Name, string EmployeeID)
        {
            this.Name = Name;
            this.EmployeeID = EmployeeID;
        }
    }

    public class Colors
    {
        public static Color BackGood = Color.Green;
        public static Color BackBad = Color.Red;
        public static Color FontGood = Color.White;
        public static Color FontBad = Color.White;
    }

    public class DebugContainer
    {
        private DateTime _dateOfInfo;
        public List<string> ErrorStrings;
        public List<DebugInfo> DebugInfo;
        public Dictionary<int, Shift> Shifts;
        public Dictionary<int, Ticket> Tickets;
        public double AMSales = -1, PMSales = -1, AMTipshare = -1, PMTipshare = -1;
        public double AMBarHours = -1, PMBarHours = -1, AMHostHours = -1, PMHostHours = -1;
        public double AMOverrideTipsAddition = 0, PMOverrideTipsAddition = 0;

        public DateTime DateOfInfo
        {
            get
            {
                return _dateOfInfo;
            }
        }

        public DebugContainer(DateTime dateOfInfo)
        {
            this._dateOfInfo = dateOfInfo;
            this.DebugInfo = new List<DebugInfo>();
            this.ErrorStrings = new List<string>();
            this.Shifts = new Dictionary<int, Shift>();
            this.Tickets = new Dictionary<int, Ticket>();
            this.AMSales = -1;
            this.PMSales = -1;
            this.AMTipshare = -1;
            this.PMTipshare = -1;
        }

        public DebugContainer(DateTime dateOfInfo, List<DebugInfo> debugInfo, List<string> errorStrings,
            Dictionary<int, Shift> shifts, Dictionary<int, Ticket> tickets, double amSales, double pmSales, double amTipshare, double pmTipshare,
            double AMOverrideTipsAddition, double PMOverrideTipsAddition)
        {
            this._dateOfInfo = dateOfInfo;
            this.DebugInfo = debugInfo;
            this.ErrorStrings = errorStrings;
            this.Shifts = shifts;
            this.Tickets = tickets;
            this.AMSales = amSales;
            this.PMSales = pmSales;
            this.AMTipshare = amTipshare;
            this.PMTipshare = pmTipshare;
            this.AMOverrideTipsAddition = AMOverrideTipsAddition;
            this.PMOverrideTipsAddition = PMOverrideTipsAddition;
        }

        public string GetInfoData()
        {
            StringBuilder sb = new StringBuilder(DateOfInfo.ToString());
            sb.AppendLine();

            if (ErrorStrings != null && ErrorStrings.Count > 0)
            {
                foreach (string str in ErrorStrings)
                    sb.AppendLine("ERROR OCCURRED: " + str);
                sb.AppendLine();
            }

            if (DebugInfo != null && DebugInfo.Count > 0)
                foreach (DebugInfo di in DebugInfo)
                    sb.AppendLine(di.Name + ": " + di.Info);

            return sb.ToString();
        }

        public DataTable GetEmployeeDataSet(out int[] colWidths)
        {
            DataTable dt = new DataTable("Employees");
            dt.Columns.Add("Name");
            dt.Columns.Add("EmpId");
            dt.Columns.Add("Clock-In");
            dt.Columns.Add("Clock-Out");
            dt.Columns.Add("Job");
            colWidths = new int[5];

            colWidths[0] = 120;
            colWidths[1] = 40;
            colWidths[2] = 60;
            colWidths[3] = 60;
            colWidths[4] = 35;

            if (Shifts != null && Shifts.Count > 0)
                foreach (int key in Shifts.Keys)
                {
                    Shift shft = Shifts[key];
                    dt.Rows.Add(shft.Name, shft.EmployeeID, shft.ClockIn.ToShortTimeString(), shft.ClockOut.ToShortTimeString(), shft.JobCode);
                }

            return dt;
        }

        public DataTable GetTicketsDataSet(out int[] colWidths)
        {
            DataTable dt = new DataTable("Tickets");
            dt.Columns.Add("EmpID");
            dt.Columns.Add("Time");
            dt.Columns.Add("Amount");
            colWidths = new int[3];
            colWidths[0] = 50;
            colWidths[1] = 75;
            colWidths[2] = 60;

            double sales = 0;

            if (Tickets != null && Tickets.Count > 0)
            {
                foreach (int key in Tickets.Keys)
                {
                    Ticket ticket = Tickets[key];
                    dt.Rows.Add(ticket.EmployeeID, ticket.ServeDate.ToShortTimeString(), ticket.Amount);
                    sales += ticket.Amount;
                }
                dt.Rows.Add("TOTAL", Tickets.Count + " tickets", "$" + sales);
            }
            return dt;
        }

        public DataTable GetSalesDataSet(out int[] colWidths)
        {
            DataTable dt = new DataTable("Sales");
            dt.Columns.Add("Description");
            dt.Columns.Add("Sales");
            colWidths = new int[2];
            colWidths[0] = 100;
            colWidths[1] = 65;

            dt.Rows.Add("AM Sales", AMSales);
            dt.Rows.Add("PM Sales", PMSales);
            dt.Rows.Add("Total Server Sales", AMSales + PMSales);
            dt.Rows.Add("", "");
            dt.Rows.Add("AM Tipshare", AMTipshare);
            dt.Rows.Add("AM Tips added", AMOverrideTipsAddition);
            dt.Rows.Add("PM Tipshare", PMTipshare);
            dt.Rows.Add("PM Tips added", PMOverrideTipsAddition);
            dt.Rows.Add("Total Tipshare", AMTipshare + PMTipshare);


            return dt;
        }
    }

    public class DebugInfo
    {
        public string Name;
        public string Info;
    }
}
