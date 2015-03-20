using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace ScratchPaper
{
    class Program
    {
        static void Main(string[] args)
        {
            newMethod();
            //RunOldMethod();
            Console.ReadLine();
        }

        private static void newMethod()
        {
            List<string> result;
            TipshareWS.TipshareWS ts = new ScratchPaper.TipshareWS.TipshareWS();
            DateTime now = new DateTime
                (DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.Now.AddDays(-1).Day,
                0, 0, 0);
            while (now.DayOfWeek != DayOfWeek.Thursday)
                now = now.AddDays(-1);
            result = ts.GetProgramUsage(now, "").ToList<string>();

            foreach (string str in result)
            {
                if (str.Contains(","))
                {
                    string[] temp = str.Split(',');
                    DateTime dtTemp;
                    if (temp != null && temp.Length > 1 && DateTime.TryParse(temp[0], out dtTemp))
                        Console.WriteLine(temp[1] + " used the prog at: " + 
                            dtTemp.ToShortTimeString() +" on "+dtTemp.ToShortDateString());
                }
            }
        }

        private static void RunOldMethod()
        {
            List<string> lTemp = new List<string>();

            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.InitialCatalog = "elstar";
            sb.DataSource = "192.168.0.15";
            sb.UserID = "sa";
            sb.Password = "ratsle";
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
                        lTemp.Add(dr[1].ToString().ToUpper() + " " + dr[2].ToString().ToUpper() + "," +
                            dr[0].ToString() + "," +
                            dr[3].ToString());//.Insert(3, "-").Insert(7, "-")));
                    }
                }
            }
            catch (Exception)
            { }

            foreach (string str in lTemp)
                Console.WriteLine(str);
            //return lTemp;
        }
    }
}
