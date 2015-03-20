using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace OldTipshareRetriever
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args !=null && args.Length>=2)
            {
                // [0] store
                // [1] driveLetter
                // [2] date
                
                string store = args[0];
                
                string driveLetter = null;// = args[1];
                string date = null;// = args[2];

                if (args.Length >= 3)
                {
                    driveLetter = args[1];
                    date = args[2];
                }
                else
                    date = args[1];

                if(string.IsNullOrEmpty(driveLetter))
                    Console.WriteLine("Store " + store + " (hit via UNC path) is requesting date " + date);
                else
                    Console.WriteLine("Store " + store + " mapped at " + driveLetter + ": is requesting date " + date);

                string storeData = GetStoreData(store, date, driveLetter);
                if (!string.IsNullOrEmpty(storeData))
                    WriteStoreData(store, storeData);
                else
                    Console.WriteLine("No store data was created, not writing to file");
            }
        }

        private static void WriteStoreData(string storeName, string storeData)
        {
            using (FileStream fs = new FileStream(storeName+".csv", FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {                    
                    Console.WriteLine("Writing to " + storeName + ".csv");
                    sw.Write(storeData);
                }
            }
        }

        private static string GetStoreData(string storeName, string date, string driveLetter)
        {
            string result = string.Empty;

            //debug csv is line 2            
            string file = string.Format(@"\\192.168.1{0}.1\c$\network\mirror\TS_{1}.csv", storeName.Substring(2), date.Replace('\\', '-').Replace('/', '-'));
            if (!string.IsNullOrEmpty(driveLetter))
            {
                file = string.Format(@"{0}:\network\mirror\TS_{1}.csv", driveLetter.ToUpperInvariant().Replace(":", ""), date.Replace('\\', '-').Replace('/', '-'));
            }

            Console.WriteLine("Opening file: " + file);
            FileInfo csvFile = new FileInfo(file);
            //FileInfo csvFile = new FileInfo(@"c:\CCtest\TS\TS_" + date + ".csv");

            if (csvFile.Exists)
            {
                Employee[] emps = DataRetriever.GetEmployees(csvFile.FullName, date);
                Console.WriteLine(emps.Length + " employees found for store#"+storeName.Substring(2));

                //debug ssns is line2
                Dictionary<string, string> empSSNs = DataRetriever.GetSSNs(DataRetriever.GetIdsFromEmployees(emps), storeName.Substring(2));
                //Dictionary<string, string> empSSNs = DataRetriever.MockGetSSNs(DataRetriever.GetIdsFromEmployees(emps), storeName);

                foreach (Employee emp in emps)
                {
                    result += DataRetriever.GetFormattedDataFromEmployee(emp, empSSNs[emp.id]);
                    result += "\r\n";
                }

                result.Trim();
            }
            else
                Console.WriteLine("File doesn't exist ("+csvFile.Name+")");

            return result;
        }
    }

    static class DataRetriever
    {
        public static string[] GetIdsFromEmployees(Employee[] emps)
        {
            List<string> result = new List<string>();
            foreach (Employee emp in emps)
            {
                if (emp.id.Equals("UNDIST") || emp.name.Contains("Undistributed"))
                    Console.WriteLine("** Skipping and UNDIST user **");
                else
                    result.Add(emp.id);
            }
            return result.ToArray();
        }

        public static Employee[] GetEmployees(string location, string dateString)
        {
            List<Employee> result = new List<Employee>();

            FileInfo file = new FileInfo(location);
            if (file.Exists)
            {
                DateTime date = DateTime.Parse(dateString);
                using (FileStream fs = new FileStream(location, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        sr.ReadLine();//throw away top line

                        int dayToSubtract = 7;
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            if (!string.IsNullOrEmpty(line))
                            {
                                result.AddRange(ParseLine(line, date.AddDays(-1 * dayToSubtract).ToShortDateString()));
                            }
                            dayToSubtract--;
                        }
                    }
                }
            }

            Console.WriteLine(result.Count + " employees found");

            return result.ToArray();
        }

        private static List<Employee> ParseLine(string line, string date)
        {
            List<Employee> result = new List<Employee>();
            int oldCommaIndex = line.IndexOf(',');
            int newCommaIndex = oldCommaIndex;
            int numOfEmps;
            if (int.TryParse(line.Substring(0, oldCommaIndex), out numOfEmps))
            {
                for (int i = 0; i < numOfEmps; i++)
                {
                    oldCommaIndex = newCommaIndex;
                    newCommaIndex = line.IndexOf(',', oldCommaIndex + 1);

                    string tempName = line.Substring(oldCommaIndex + 1, newCommaIndex - oldCommaIndex-1);
                    oldCommaIndex = newCommaIndex;
                    newCommaIndex = line.IndexOf(',', oldCommaIndex + 1);
                    oldCommaIndex = newCommaIndex;
                    newCommaIndex = line.IndexOf(',', oldCommaIndex + 1);

                    string amount = line.Substring(oldCommaIndex + 1, newCommaIndex - oldCommaIndex-1);

                    string empName = tempName.Substring(0, tempName.IndexOf(" - "));
                    string empId = tempName.Substring(tempName.IndexOf(" - ") + " - ".Length);
                    if(empName.Contains("Undistributed") || empId == "UNDIST")
                        Console.WriteLine("** Skipping and UNDIST user **");
                    else
                        result.Add(new Employee(
                            empName,
                            empId,
                            date,
                            double.Parse(amount)));
                }
            }

            oldCommaIndex = newCommaIndex;
            newCommaIndex = line.IndexOf(',', oldCommaIndex + 1);
            string num = line.Substring(oldCommaIndex+1, newCommaIndex - oldCommaIndex-1);
            if (int.TryParse(num, out numOfEmps))
            {
                for (int i = 0; i < numOfEmps; i++)
                {
                    oldCommaIndex = newCommaIndex;
                    newCommaIndex = line.IndexOf(',', oldCommaIndex + 1);

                    string tempName = line.Substring(oldCommaIndex + 1, newCommaIndex - oldCommaIndex-1);
                    oldCommaIndex = newCommaIndex;
                    newCommaIndex = line.IndexOf(',', oldCommaIndex + 1);
                    oldCommaIndex = newCommaIndex;
                    newCommaIndex = line.IndexOf(',', oldCommaIndex + 1);

                    string amount = line.Substring(oldCommaIndex + 1, newCommaIndex - oldCommaIndex-1);

                    string empName = tempName.Substring(0, tempName.IndexOf(" - "));
                    string empId = tempName.Substring(tempName.IndexOf(" - ") + " - ".Length);
                    if (empName.Contains("Undistributed") || empId == "UNDIST")
                        Console.WriteLine("** Skipping and UNDIST user **");
                    else
                        result.Add(new Employee(
                            empName,
                            empId,
                            date,
                            double.Parse(amount)));
                }
            }

            return result;
        }

        public static Dictionary<string, string> GetSSNs(string[] ids, string storeNumber)
        {
            string dbAddy = string.Format("192.168.1{0}.55", storeNumber);

            string sqlIds = GetCommaStringFromArray(ids);
            Dictionary<string, string> result = new Dictionary<string, string>();            

            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.InitialCatalog = "elstar";
            sb.DataSource = dbAddy;
            sb.UserID = "sa";
            sb.Password = "ratsle";
            sb.MultipleActiveResultSets = true;

            using (SqlConnection conn = new SqlConnection(sb.ToString()))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception)
                {
                    Console.WriteLine("Exception connecting to POS5, trying POS4...");
                    dbAddy = dbAddy.Replace(".55", ".54");
                    sb.DataSource = dbAddy;
                    conn.ConnectionString = sb.ToString();
                    conn.Open();
                }

                Console.WriteLine("DB access to " + dbAddy + " granted");
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select employee.iemployeeid, employee.sssn from employee where employee.iemployeeid in (" + sqlIds + ")";

                try
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        result.Add(dr[0].ToString(), dr[1].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception thrown while executing command:\r\n" + cmd.CommandText + "\r\n\r\n");
                    throw ex;
                }
            }

            return result;
        }

        private static string GetCommaStringFromArray(string[] ids)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string id in ids)
            {
                sb.Append(string.Format("'{0}', ", id));
            }

            string result = sb.ToString();
            if (sb.Length > 0)
                result = sb.ToString(0, sb.Length - ", ".Length);
            return result;
        }

        internal static string GetFormattedDataFromEmployee(Employee emp, string ssn)
        {
            string result = string.Format("{0},{1},{2}", emp.date, ssn, emp.amount);
            return result;
        }

        internal static Dictionary<string, string> MockGetSSNs(string[] ids, string storeNumber)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (string id in ids)
            {
                if(!result.Keys.Contains(id))
                    result.Add(id, "SSN_" + id);
            }

            return result;
        }
    }
}
