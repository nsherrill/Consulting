using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Common;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace ScratchConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadDbfFile();

            //InsertRequest("AB03");
            //InsertStores();            
        }

        private static void ReadDbfFile()
        {
            //Console.WriteLine("enter query");
            //string file = Console.ReadLine();

            //string file = "select employee, price, dob, hour, minute from gnditem.dbf where item <> 105 order by hour desc, minute desc";
            string file = "select {0} from gnditem.dbf g, emp.dbf e where g.employee = e.usernumber and employee=3070 and  item <> 105 {1}";
            string groupBy = "skip";
            while (!string.IsNullOrEmpty(groupBy))
            {
                string query = "";
                if (groupBy != "skip" && groupBy != "cols")
                    query = string.Format(file, groupBy + ", sum(price) as summm", 
                        //" and summm > 0 "+
                    "group by " + groupBy);
                else
                    query = string.Format(file, "g.*", " order by hour desc, minute desc");

                DataSet ds = ReadFile(query);
                DisplayFile(ds, groupBy != "skip",  groupBy == "cols");

                Console.WriteLine(Environment.NewLine + "enter query");
                groupBy = Console.ReadLine();
            }
            //exit
        }

        private static void DisplayFile(DataSet ds, bool displayAll, bool justCols)
        {
            StringBuilder output = new StringBuilder();
            if (ds != null && ds.Tables!= null && ds.Tables.Count > 0)
            {
                foreach (DataTable dt in ds.Tables)
                {

                    if (dt != null)
                    {
                        int colToUse = 0;
                        if (dt.Columns != null && dt.Columns.Count > 0)
                        {
                            for (int i= 0; i<dt.Columns.Count; i++)
                            {
                                string colName = dt.Columns[i].ColumnName.ToString();
                                Console.Write(i+":"+colName + "\t");
                                output.Append(i + ":" + colName + "\t");
                                if (colName.Equals("discpric", StringComparison.InvariantCultureIgnoreCase))
                                    colToUse = i;
                            }
                        }
                        Console.WriteLine();
                        output.AppendLine();
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            double sum = 0;
                            bool dataDisplayed = false;
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr != null && dr.ItemArray != null && dr.ItemArray.Length > 0)
                                {
                                    double tempSum;
                                    if (colToUse>0 && double.TryParse(dr.ItemArray[colToUse].ToString(), out tempSum))
                                        sum += tempSum;
                                    if ((displayAll || !dataDisplayed) && !justCols)
                                    {
                                        foreach (object ob in dr.ItemArray)
                                        {
                                            output.Append(ob.ToString() + "\t");
                                            Console.Write(ob.ToString() + "\t");
                                        }
                                        Console.WriteLine();
                                        output.AppendLine();
                                        //dataDisplayed = true;
                                    }
                                }
                            }
                            output.AppendLine("sum: " + sum);
                            Console.WriteLine("sum: " + sum);
                        }
                    }
                    output.AppendLine();
                    Console.WriteLine();
                }
            }

            string outputFile = @"c:\temp\tempoutput.txt";
            FileInfo fi = new FileInfo(outputFile);
            if (fi.Exists)
                fi.Delete();
            using (FileStream fs = new FileStream(outputFile, FileMode.CreateNew))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(output.ToString());
                }
            }
            
        }

        private static DataSet ReadFile(string sQuery)
        {
            DataSet ds = new DataSet();
            OleDbConnection con = new OleDbConnection(
                "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\bootdrv\\aloha\\20110416;Extended Properties=dBASE IV;User ID=;Password=;");
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
                Console.WriteLine(e.ToString());
            }

            return ds;
        }

        #region request
        private static void InsertRequest(string ShortStoreName)
        {
            ConcordEI.TipshareWS.TipshareWS tws = new ScratchConsole.ConcordEI.TipshareWS.TipshareWS();
            ConcordEI.TipshareWS.SPRequest req = new ScratchConsole.ConcordEI.TipshareWS.SPRequest();
            Stores st = Stores.FindByShortName(ShortStoreName);

            if (st != null)
            {
                req.Source = ScratchConsole.ConcordEI.TipshareWS.SPSource.Manual;
                req.Status = ScratchConsole.ConcordEI.TipshareWS.SPRequestStatus.Requested;
                req.StoreId = st.Id;
                req.SubmitDate = DateTime.Now;

                tws.UpdateRequest(req);
            }
        }

        private static void InsertStores()
        {
            List<string> queries = new List<string>();

            //queries.Add("Insert into concord.dbo.regions (name) values ('All Stores')");

            queries.Add(GetQuery("AB01", "Topeka", 0, ""));
            queries.Add(GetQuery("AB02", "Manhattan", 0, ""));
            queries.Add(GetQuery("AB03", "Old Cheney", 0, ""));
            queries.Add(GetQuery("AB04", "St Joe", 0, ""));
            queries.Add(GetQuery("AB05", "North Platte", 0, ""));
            queries.Add(GetQuery("AB06", "Scottsbluff", 0, ""));
            queries.Add(GetQuery("AB07", "Laramie", 0, ""));
            queries.Add(GetQuery("AB08", "Gateway", 0, ""));
            queries.Add(GetQuery("AB09", "Emporia", 0, ""));
            queries.Add(GetQuery("AB10", "Rock Springs", 0, ""));
            queries.Add(GetQuery("AB11", "Lubbock", 0, ""));
            queries.Add(GetQuery("AB12", "Amarillo", 0, ""));
            queries.Add(GetQuery("AB13", "Wichita Falls", 0, ""));
            queries.Add(GetQuery("AB14", "Lawton", 0, ""));
            queries.Add(GetQuery("AB15", "Ardmore", 0, ""));
            queries.Add(GetQuery("AB16", "North 27th", 0, ""));
            queries.Add(GetQuery("AB17", "Evansville", 0, ""));
            queries.Add(GetQuery("AB18", "Downtown", 0, ""));
            queries.Add(GetQuery("AB19", "Grand Island", 0, ""));
            queries.Add(GetQuery("AB20", "Maryville", 0, ""));
            queries.Add(GetQuery("AB21", "York", 0, ""));
            queries.Add(GetQuery("AB22", "Gillette", 0, ""));
            queries.Add(GetQuery("AB23", "Kearney", 0, ""));
            queries.Add(GetQuery("AB24", "Columbus", 0, ""));
            queries.Add(GetQuery("AB25", "Airport", 0, ""));
            queries.Add(GetQuery("AB26", "Bayou", 0, ""));
            queries.Add(GetQuery("AB28", "Destin", 0, ""));
            queries.Add(GetQuery("AB29", "Government", 0, ""));
            queries.Add(GetQuery("AB30", "Amarillo Blvd", 0, ""));
            queries.Add(GetQuery("AB31", "Nine Mile", 0, ""));
            queries.Add(GetQuery("AB32", "Foley", 0, ""));
            queries.Add(GetQuery("AB33", "Ocean Springs", 0, ""));
            queries.Add(GetQuery("AB34", "Crestview", 0, ""));
            queries.Add(GetQuery("AB35", "South McAllen", 0, ""));
            queries.Add(GetQuery("AB36", "North McAllen", 0, ""));
            queries.Add(GetQuery("AB37", "Laredo", 0, ""));
            queries.Add(GetQuery("AB38", "Harlingen", 0, ""));
            queries.Add(GetQuery("AB39", "Corpus Christi", 0, ""));
            queries.Add(GetQuery("AB40", "Lubbock 4th", 0, ""));
            queries.Add(GetQuery("AB41", "Altus", 0, ""));
            queries.Add(GetQuery("AB42", "Ada", 0, ""));
            queries.Add(GetQuery("AB42", "Edinburg", 0, ""));

            queries.Add(GetQuery("AB42", "Manhattan", 0, ""));
            queries.Add(GetQuery("AB42", "St Joe", 0, ""));

            queries.Add(GetQuery("VI01", "Norfolk", 0, ""));
            queries.Add(GetQuery("VI02", "Manhattan", 0, ""));
            queries.Add(GetQuery("VI09", "Hays", 0, ""));
            queries.Add(GetQuery("VI11", "North Platte", 0, ""));
            queries.Add(GetQuery("VI12", "Emporia", 0, ""));

            queries.Add(GetQuery("HI01", "York", 0, ""));
            
            try
            {
                string connString =
                    @"Data Source=SC\SQLEXPRESS;initial catalog=Concord;Persist Security " +
                    @"Info=false;User ID=ccweb;password=thund3r!;";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    foreach (string query in queries)
                    {
                        using (SqlCommand com = new SqlCommand(query, conn))
                        {
                            com.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private static string GetQuery(string sshort, string sname, int region, string ip)
        {
            return "Insert into concord.dbo.stores (name, shortname, regionid, ipaddress) " +
                "values ('" + sname + "', '" + sshort + "', " + region + ", '" + ip + "')";
        }
        #endregion
    }
}
