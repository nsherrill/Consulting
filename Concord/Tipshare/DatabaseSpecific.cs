using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Tipshare
{
    public enum POSType
    {
        AppleOne,
        Aloha,
        Unknown
    }

    public static class POSTypeFetcher
    {
        public static POSType GetPOS()
        {
            POSType ptype = POSType.Unknown;
            string config = System.Configuration.ConfigurationManager.AppSettings["POSType"];
            if (!string.IsNullOrEmpty(config))
            {
                config = config.ToLowerInvariant();
                if (config.Contains("apple") || config.Contains("a1"))
                    ptype = POSType.AppleOne;
                else if (config.Contains("aloha") || config.Contains("daves") || Directory.Exists(@"c:\bootdrv\Aloha"))
                    ptype = POSType.Aloha;
            }

            return ptype;
        }
    }

    public static class AlohaMethods
    {
        public static double GetPercent(out string sError)
        {
            double result = 2.5;
            sError = "";

            return result;
        }

        public static Dictionary<int, Employee> GetEmployees(out string sError)
        {
            Dictionary<int, Employee> results = new Dictionary<int, Employee>();
            sError = "";
            string sError1, sError2 = "";

            /*
            firstname	emp.dbf	firstname   0
            lastname	emp.dbf	lastname    1
            emp id	    emp.dbf	usernumber  2
            SSN	        emp.dbf	ssn         3
            */
            string dirDate = ConfigHelper.DateTimeNow.ToString("yyyyMMdd");
            List<Employee> temp = new List<Employee>();
            temp = (ParseEmployees(ReadFile(dirDate,
                "select firstname, lastname, usernumber, ssn from emp.dbf order by lastname",
                out sError1)));

            foreach (Employee emp in temp)
                results.Add(results.Count, emp);

            if (results.Count == 0)
            {
                dirDate = ConfigHelper.DateTimeNow.AddDays(-1).ToString("yyyyMMdd");
                temp = (ParseEmployees(ReadFile(dirDate,
                    "select firstname, lastname, usernumber, ssn from emp.dbf order by lastname",
                    out sError2)));

                foreach (Employee emp in temp)
                    results.Add(results.Count, emp);
            }

            sError = sError1 + (sError2.Length > 0 ? Environment.NewLine + sError2 : "");

            return results;
        }

        public static Dictionary<int, Ticket> GetTickets(DateTime dtEarliestViewable, out string sError)
        {
            sError = "";
            string sError1;
            Dictionary<int, Ticket> results = new Dictionary<int, Ticket>();
            DateTime dtTemp = ConfigHelper.DateTimeNow.Date;

            while (dtTemp >= dtEarliestViewable.Date)
            {
                string dirDate = dtTemp.ToString("yyyyMMdd");
                /*
                employeeid	gnditem.dbf	employee            0
                amount	    gnditem.dbf	price (discpric)    1
                date	    gnditem.dbf	dob                 2
                hour	    gnditem.dbf	hour                3
                minute	    gnditem.dbf	minute              4
                 * 
gndsale.dbf	employee
gndsale.dbf	amount
gndsale.dbf	dob
gndsale.dbf	closehour
gndsale.dbf	closemin

gndtndr.dbf	employee
gndtndr.dbf	amount
gndtndr.dbf	date
gndtndr.dbf	hour
gndtndr.dbf	minute

                */
                List<Ticket> temp = (ParseTickets(ReadFile(dirDate,
                    "select employee, discpric, dob, hour, minute from gnditem.dbf where item <> 105 order by hour desc, minute desc",
                    //"select employee, price, dob, hour, minute from gnditem.dbf where item <> 105 order by hour desc, minute desc",
                    out sError1)));

                foreach (Ticket tick in temp)
                    results.Add(results.Count, tick);

                sError += (sError1.Length > 0 ? Environment.NewLine + sError1 : "");

                //double sum = 0;
                //foreach (Ticket t in results)
                //{
                //    sum += t.Amount;
                //}
                //double tipsharePercent = sum * .025;

                dtTemp = dtTemp.AddDays(-1);
            }

            return results;
        }

        public static Dictionary<int, Shift> GetShifts(DateTime dtEarliestViewable, out string sError)
        {
            sError = "";
            string sError1;
            Dictionary<int, Shift> results = new Dictionary<int, Shift>();
            DateTime dtTemp = ConfigHelper.DateTimeNow.Date;

            while (dtTemp >= dtEarliestViewable.Date)
            {
                string dirDate = dtTemp.ToString("yyyyMMdd");
                /*
                emp id	    	employee    0
                pay dept		jobcode     1
                date	    	date        2
                in hour	    	inhour      3
                in minute		inminute    4
                out hour		outhour     5
                out minute		outminute   6
                */

                List<Shift> temp = (ParseShifts(ReadFile(dirDate,
                    "select employee, jobcode, date, inhour, inminute, outhour, outminute, " +
                    "firstname, lastname from gndtime.dbf g, emp.dbf e where g.employee = e.usernumber",
                    out sError1)));

                foreach (Shift shft in temp)
                    results.Add(results.Count, shft);

                sError += (sError1.Length > 0 ? Environment.NewLine + sError1 : "");
                dtTemp = dtTemp.AddDays(-1);
            }

            return results;
        }

        private static List<Employee> ParseEmployees(DataSet ds)
        {
            /*
            firstname	emp.dbf	firstname   0
            lastname	emp.dbf	lastname    1
            emp id	    emp.dbf	usernumber  2
            SSN	        emp.dbf	ssn         3
            */
            List<Employee> results = new List<Employee>();

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null)
                {
                    if (dt.Rows != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr != null && dr.ItemArray != null && dr.ItemArray.Length > 3)
                            {
                                string socialFix = dr.ItemArray[3].ToString();
                                while (!string.IsNullOrEmpty(socialFix) && socialFix.Length < 9)
                                    socialFix = "0" + socialFix;

                                results.Add(new Employee(
                                    (dr.ItemArray[0].ToString() + " " + dr.ItemArray[1].ToString()).ToUpperInvariant(),
                                    dr.ItemArray[2].ToString(),
                                    socialFix));
                            }
                        }
                    }
                }
            }

            return results;
        }

        private static List<Shift> ParseShifts(DataSet ds)
        {
            /*
            emp id	    	employee    0
            pay dept		jobcode     1
            date	    	date        2
            in hour	    	inhour      3
            in minute		inminute    4
            out hour		outhour     5
            out minute		outminute   6
            firstname       firstname   7
            lastname        lastname    8
            */
            List<Shift> results = new List<Shift>();

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null)
                {
                    if (dt.Rows != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr != null && dr.ItemArray != null && dr.ItemArray.Length > 8)
                            {
                                try
                                {
                                    DateTime day = DateTime.Parse(dr.ItemArray[2].ToString());
                                    int hour = int.Parse(dr.ItemArray[3].ToString());
                                    int minute = int.Parse(dr.ItemArray[4].ToString());
                                    if (hour == 24)
                                    {
                                        day = day.AddDays(1);
                                        hour = 0;
                                    }

                                    DateTime dtIn = new DateTime(
                                        day.Year, day.Month, day.Day,
                                        hour, minute, 0);

                                    int outHour = int.Parse(dr.ItemArray[5].ToString()) % 24;
                                    int outMinute = int.Parse(dr.ItemArray[6].ToString());
                                    if (outHour < hour)
                                        day = day.AddDays(1);
                                    DateTime dtOut = new DateTime(
                                        day.Year, day.Month, day.Day,
                                        outHour, outMinute, 0);

                                    results.Add(new Shift(
                                        (dr.ItemArray[7].ToString() + " " + dr.ItemArray[8].ToString()).ToUpperInvariant(),
                                        dr.ItemArray[0].ToString(),
                                        dr.ItemArray[1].ToString(),
                                        dtIn,
                                        dtOut));
                                    ;
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show(string.Format("Error with this date/time creation:\r\n   Date: {0} (<-- only date is important)\r\n   Time(s): {1}:{2} and/or {3}:{4}{5}{6}",
                                        dr.ItemArray[2].ToString(), dr.ItemArray[3].ToString(), dr.ItemArray[4].ToString(),
                                        dr.ItemArray[5].ToString(), dr.ItemArray[6].ToString(),
                                        (Environment.NewLine + Environment.NewLine + Environment.NewLine),
                                        e.ToString()));
                                }
                            }
                        }
                    }
                }
            }

            return results;
        }

        private static List<Ticket> ParseTickets(DataSet ds)
        {
            /*
            employeeid	gnditem.dbf	employee            0
            amount	    gnditem.dbf	discpric            1
            date	    gnditem.dbf	dob                 2
            hour	    gnditem.dbf	hour                3
            minute	    gnditem.dbf	minute              4
            */
            List<Ticket> results = new List<Ticket>();

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null)
                {
                    if (dt.Rows != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr != null && dr.ItemArray != null && dr.ItemArray.Length > 3)
                            {
                                DateTime day = DateTime.Parse(dr.ItemArray[2].ToString());
                                DateTime dtTicket = new DateTime(
                                    day.Year, day.Month, day.Day,
                                    int.Parse(dr.ItemArray[3].ToString()),
                                    int.Parse(dr.ItemArray[4].ToString()),
                                    0);
                                double amount = double.Parse(dr.ItemArray[1].ToString());
                                if (amount != 0)
                                    results.Add(new Ticket(
                                        dr.ItemArray[0].ToString(),
                                        dtTicket,
                                        amount));
                            }
                        }
                    }
                }
            }

            return results;
        }

        private static DataSet ReadFile(string dateString, string sQuery, out string sError)
        {
            sError = "";
            DataSet ds = new DataSet();
            OleDbConnection con = new OleDbConnection(
                "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\bootdrv\\aloha\\" + dateString + ";Extended Properties=dBASE IV;User ID=;Password=;");
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(sQuery, con);
                da.Fill(ds);
                con.Close();
                //int i = ds.Tables[0].Rows.Count;
            }
            catch (Exception e)
            {
                sError = e.ToString();
            }

            return ds;
        }
    }

    public static class AppleOneMethods
    {
        //static const string DB_USER = "sa";
        //static const string DB_PASSWORD = "ratsle";

        const string DB_USER = "franuser";
        const string DB_PASSWORD = "tNCn31cVeP";

        public static double GetPercent(string dbAddy, out string sError)
        {
            double result = 0.0;
            sError = "";

            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.InitialCatalog = "elstar";
            sb.DataSource = dbAddy;
            sb.UserID = DB_USER;
            sb.Password = DB_PASSWORD;
            sb.MultipleActiveResultSets = true;

            try
            {
                using (SqlConnection conn = new SqlConnection(sb.ToString()))
                {
                    conn.Open();

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "select svalue from configsystem " +
                        "where iconfigurationvaluesid=67";
                    var queryResult = cmd.ExecuteScalar();
                    Globals.Log($"GetPercent:{queryResult}", true);
                    result = double.Parse(queryResult.ToString());
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("timeout"))
                    sError = "Unable to hit database.  Please contact ITS.";
            }

            return result;
        }

        public static Dictionary<int, Employee> GetEmployees(string dbAddy, out string sError)
        {
            List<Employee> lTemp = new List<Employee>();
            sError = "";

            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.InitialCatalog = "elstar";
            sb.DataSource = dbAddy;
            sb.UserID = DB_USER;
            sb.Password = DB_PASSWORD;
            sb.MultipleActiveResultSets = true;

            try
            {
                using (SqlConnection conn = new SqlConnection(sb.ToString()))
                {
                    conn.Open();

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "select employee.iemployeeid, employee.sfirstname, " +
                        "employee.slastname, employee.sssn from employee order by " +
                        "employee.iemployeeid desc";
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        lTemp.Add(new Employee(
                            dr[1].ToString().ToUpper() + " " + dr[2].ToString().ToUpper(),
                            dr[0].ToString(),
                            dr[3].ToString()));//.Insert(3, "-").Insert(7, "-")));
                    }
                }
            }
            catch (Exception)
            { }

            Dictionary<int, Employee> result = new Dictionary<int, Employee>();
            foreach (Employee emp in lTemp)
                result.Add(result.Count, emp);

            return result;
        }

        public static Dictionary<int, Ticket> GetTickets(string dbAddy, DateTime dtEarliestViewable, out string sError)
        {
            sError = "";
            Dictionary<int, Ticket> ltResult = new Dictionary<int, Ticket>();

            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.InitialCatalog = "elstar";
            sb.DataSource = dbAddy;
            sb.UserID = DB_USER;
            sb.Password = DB_PASSWORD;
            sb.MultipleActiveResultSets = true;
            sb.ConnectTimeout = 500;

            ltResult = (AppleOneMethods.HitDBForTicketsWithSqlString(sb.ToString(), dtEarliestViewable, out sError));
            //       if (lTemp.Count == 0)
            {
                sb = new SqlConnectionStringBuilder();
                sb.InitialCatalog = "elstarhistory";
                sb.DataSource = dbAddy;
                sb.UserID = DB_USER;
                sb.Password = DB_PASSWORD;
                sb.MultipleActiveResultSets = true;
                sb.ConnectTimeout = 1000;

                int resultsOffset = ltResult.Keys.Count;
                Dictionary<int, Ticket> temp = (AppleOneMethods.HitDBForTicketsWithSqlString(sb.ToString(), dtEarliestViewable, out sError));
                foreach (int key in temp.Keys)
                {
                    ltResult.Add(key + resultsOffset, temp[key]);
                }
            }

            return ltResult;
        }

        public static Dictionary<int, Shift> GetShifts(string dbAddy, DateTime dtEarliestViewable, out string sError)
        {
            sError = "";
            Dictionary<int, Shift> results = new Dictionary<int, Shift>();

            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.InitialCatalog = "elstar";
            sb.DataSource = dbAddy;
            sb.UserID = DB_USER;
            sb.Password = DB_PASSWORD;
            sb.MultipleActiveResultSets = true;

            results = (AppleOneMethods.HitDBForShiftsWithSqlString(sb.ToString(), dtEarliestViewable, out sError));

            sb = new SqlConnectionStringBuilder();
            sb.InitialCatalog = "elstarhistory";
            sb.DataSource = dbAddy;
            sb.UserID = DB_USER;
            sb.Password = DB_PASSWORD;
            sb.MultipleActiveResultSets = true;

            int resultsOffset = results.Keys.Count;
            Dictionary<int, Shift> temp = (AppleOneMethods.HitDBForShiftsWithSqlString(sb.ToString(), dtEarliestViewable, out sError));
            foreach (int key in temp.Keys)
            {
                results.Add(key + resultsOffset, temp[key]);
            }

            return results;
        }

        private static Dictionary<int, Shift> HitDBForShiftsWithSqlString(string sqlstring, DateTime earliestDay, out string sError)
        {
            Dictionary<int, Shift> lTemp = new Dictionary<int, Shift>();
            sError = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlstring))
                {
                    conn.Open();

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = $@"select e.sfirstname, e.slastname, timeclock.iemployeeid, timeclock.ipaydept, 
                            timeclock.dttimein, timeclock.dttimeout 
                        from elstar.dbo.employee as e, timeclock 
                        where e.iemployeeid=timeclock.iemployeeid 
                            and timeclock.dttimein> {earliestDay.ToShortDateString()} 
                        order by timeclock.dttimein desc";
                    SqlDataReader dr = cmd.ExecuteReader();
                    bool bContinue = true;
                    while (dr.Read() && bContinue)
                    {
                        string tempstr = dr[5].ToString();
                        DateTime temp = DateTime.Parse(dr[4].ToString());
                        if (temp >= earliestDay.Date)
                            lTemp.Add(lTemp.Count, (new Shift(
                                dr[0].ToString().ToUpper() + " " + dr[1].ToString().ToUpper(),
                                dr[2].ToString(),
                                dr[3].ToString(),
                                temp,
                                tempstr.Equals("") ? ConfigHelper.DateTimeNow : DateTime.Parse(tempstr)
                                )));
                        else
                            bContinue = false;
                    }
                }
            }
            catch (Exception)
            { }

            return lTemp;
        }

        private static Dictionary<int, Ticket> HitDBForTicketsWithSqlString(string sqlstring, DateTime earliestDay, out string sError)
        {
            Dictionary<int, Ticket> lTemp = new Dictionary<int, Ticket>();
            sError = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlstring))
                {
                    conn.Open();
                    Globals.Log("hitting db", true);
                    try
                    {
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText =

                            $@"
select top 80000 j.iemployeeid, js.iamount, j.dttimeclosed 
    from  journal j 
        join journalsales js on js.ijournalid=j.ijournalid 
        left join elstar.dbo.discount d on js.idiscountid = d.idiscountid 
    where js.ijournalid=j.ijournalid  
        and j.istatus_pullback=0  
        and js.ivoidid=0  
        and js.isource in ( select iSource from elstar.dbo.sourcedisplaycharacter where iTipable = 1)   
        and sticketitemname!='Gift Card'  
        and js.icompid=0   
        and js.iamount!=0   
        and j.dttimeclosed>= {earliestDay.ToShortDateString()}  
        and (   
        	(js.idiscountid = 0 or    
        		(d.sname not like '%vet day%' and d.sname not like '%vets day%'))   
        	OR (   
        		(d.sname like '%vet day%' or d.sname like '%vets day%' or d.sname like '%vets day%')   
        		AND js.iamount>0)  
            )			  
        order by j.dttimeclosed desc";

                        //"select top 80000 journal.iemployeeid, " +
                        //"journalsales.iamount, journal.dttimeclosed from journal, " +
                        //"journalsales where journalsales.ijournalid=journal.ijournalid " +
                        //"and journal.istatus_pullback=0 and journalsales.ivoidid=0 " +
                        //"and sticketitemname!='Gift Card' and journalsales.icompid=0 " +
                        //"and journalsales.iamount!=0 and " +
                        //"journal.dttimeclosed>=" + earliestDay.ToShortDateString() +

                        //// here is where this code needs to be added:

                        ////and (journalsales.idiscountid <> {DISCOUNTID} 
                        ////    OR (journalsales.idiscountid = {DISCOUNTID}
                        ////        AND journalsales.iamount>0))

                        //" order by journal.dttimeclosed desc";

                        cmd.CommandTimeout = 200;
                        SqlDataReader dr = cmd.ExecuteReader();
                        bool bContinue = true;
                        while (dr.Read() && bContinue)
                        {
                            DateTime temp = DateTime.Parse(dr[2].ToString());
                            if (temp >= earliestDay.Date)
                                lTemp.Add(lTemp.Count, new Ticket(
                                    dr[0].ToString(),
                                    temp,
                                    double.Parse(dr[1].ToString())));
                            else
                                bContinue = false;
                        }
                    }
                    catch (Exception e)
                    {
                        Globals.Log("error while getting tickets: \r\n" + e.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                sError = "Exception occured while trying to hit DB: " + e.Message;
            }
            return lTemp;
        }

        public static DeclaredTipshare GetSpecificTipshareForOverrideUser(string dbAddy, DateTime dtDateRequested, string employeeId, out string sError)
        {
            sError = "";
            DeclaredTipshare result = new DeclaredTipshare()
            {
                dateOfSales = DateTime.MinValue
            };

            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.InitialCatalog = "elstar";
            sb.DataSource = dbAddy;
            sb.UserID = DB_USER;
            sb.Password = DB_PASSWORD;
            sb.MultipleActiveResultSets = true;

            result = AppleOneMethods.HitDBForDeclaredTipshareValuesWithSqlString(sb.ToString(), dtDateRequested, employeeId, out sError);

            if (result.amTipshareValues == 0 && result.pmTipshareValues == 0)
            {
                sb = new SqlConnectionStringBuilder();
                sb.InitialCatalog = "elstarhistory";
                sb.DataSource = dbAddy;
                sb.UserID = DB_USER;
                sb.Password = DB_PASSWORD;
                sb.MultipleActiveResultSets = true;

                var result2 = AppleOneMethods.HitDBForDeclaredTipshareValuesWithSqlString(sb.ToString(), dtDateRequested, employeeId, out sError);
                if (result2 != null && (result2.amTipshareValues > 0 || result2.pmTipshareValues > 0))
                {
                    result.amTipshareValues += result2.amTipshareValues;
                    result.pmTipshareValues += result2.pmTipshareValues;
                }
            }

            return result;
        }

        private static DeclaredTipshare HitDBForDeclaredTipshareValuesWithSqlString(string sqlstring, DateTime dateToFind, string employeeId, out string sError)
        {
            DeclaredTipshare result = new DeclaredTipshare()
            {
                dateOfSales = dateToFind
            };

            sError = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlstring))
                {
                    conn.Open();
                    Globals.Log("hitting db", true);
                    try
                    {
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText =

                            @"
SELECT TOP 1000 
      sum([iTip]) as 'amtips', 
      [dtTime] as 'date',
      j.iEmployeeId
  FROM [dbo].[JournalPayment] jp
	join dbo.Journal j on jp.ijournalid = j.ijournalid
  WHERE 
	[dtTime] = '" + dateToFind.ToShortDateString() + @"'
	AND j.iEmployeeID = " + employeeId + @"
	and [dtTimeClosed] - [dtTime] < '" + StaticProps.AMPMBreakAsTimeString + @"'
  GROUP BY 	
    [dtTime], 
	j.iEmployeeId
  order by 'date'";

                        cmd.CommandTimeout = 200;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            result.amTipshareValues = double.Parse(dr[0].ToString());
                        }

                        dr.Close();

                        cmd = conn.CreateCommand();
                        cmd.CommandText =

                            @"
SELECT TOP 1000 
      sum([iTip]) as 'pmtips', 
      [dtTime] as 'date',
      j.iEmployeeId
  FROM [dbo].[JournalPayment] jp
	join dbo.Journal j on jp.ijournalid = j.ijournalid
  WHERE 
	[dtTime] = '" + dateToFind.ToShortDateString() + @"'
	AND j.iEmployeeID = " + employeeId + @"
	and [dtTimeClosed] - [dtTime] >= '" + StaticProps.AMPMBreakAsTimeString + @"'
  GROUP BY 	
    [dtTime], 
	j.iEmployeeId
  order by 'date'";

                        cmd.CommandTimeout = 200;
                        dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            result.pmTipshareValues = double.Parse(dr[0].ToString());
                        }
                    }
                    catch (Exception e)
                    {
                        Globals.Log("error while getting specific tipshare val: \r\n" + e.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                sError = "Exception occured while trying to hit DB: " + e.Message;
            }
            return result;
        }
    }

    public static class GenericDBMethods
    {
        public static double HitDBForPercent(POSType ptype, string dbAddy, out string sError)
        {
            double lTemp = 0.0;
            sError = "";

            if (ptype == POSType.Aloha)
            {
                lTemp = AlohaMethods.GetPercent(out sError);
            }
            else if (ptype == POSType.AppleOne)
            {
                lTemp = AppleOneMethods.GetPercent(dbAddy, out sError);
            }

            return lTemp / 100.0;
        }

        public static Dictionary<int, Ticket> HitDBForTickets(POSType ptype, string dbAddy, DateTime dtEarliestViewable, out string sError)
        {
            Dictionary<int, Ticket> lTemp = new Dictionary<int, Ticket>();
            sError = "";
            if (ptype == POSType.Aloha)
            {
                lTemp = AlohaMethods.GetTickets(dtEarliestViewable, out sError);
            }
            else if (ptype == POSType.AppleOne)
            {
                lTemp = AppleOneMethods.GetTickets(dbAddy, dtEarliestViewable, out sError);
            }

            return lTemp;
        }

        public static Dictionary<int, Shift> HitDBForShifts(POSType ptype, string dbAddy, DateTime dtEarliestViewable, out string sError)
        {
            Dictionary<int, Shift> lTemp = new Dictionary<int, Shift>();

            sError = "";
            if (ptype == POSType.Aloha)
            {
                lTemp = AlohaMethods.GetShifts(dtEarliestViewable, out sError);
            }
            else if (ptype == POSType.AppleOne)
            {
                lTemp = AppleOneMethods.GetShifts(dbAddy, dtEarliestViewable, out sError);
            }

            Dictionary<int, Shift> lResult = new Dictionary<int, Shift>();
            foreach (var item in lTemp)
                if (item.Value.Hours > (5.0 / 60.0)) // if the shift is longer than 5 min
                    lResult.Add(item.Key, item.Value);

            return lResult;
        }

        public static Dictionary<int, Employee> HitDBForEmployees(POSType ptype, string dbAddy, out string sError)
        {
            Dictionary<int, Employee> lTemp = new Dictionary<int, Employee>();

            sError = "";
            if (ptype == POSType.Aloha)
            {
                lTemp = AlohaMethods.GetEmployees(out sError);
            }
            else if (ptype == POSType.AppleOne)
            {
                lTemp = AppleOneMethods.GetEmployees(dbAddy, out sError);
            }
            return lTemp;
        }

        public static DeclaredTipshare GetSpecificTipshareForOverrideUser(POSType ptype, string dbAddy, string employeeId, DateTime dtDateRequsted, out string sError)
        {
            sError = string.Empty;
            if (ptype == POSType.AppleOne)
            {
                return AppleOneMethods.GetSpecificTipshareForOverrideUser(dbAddy, dtDateRequsted, employeeId, out sError);
            }
            else
                return new DeclaredTipshare()
                {
                    dateOfSales = dtDateRequsted,
                    amTipshareValues = 0,
                    pmTipshareValues = 0
                };
        }
    }
}
