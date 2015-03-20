using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using StatRetrieval.Shared.Common;
using StatRetrieval.Shared.Common.Contracts;
using StatRetrieval.Shared.Common.DataContracts;

namespace StatRetrieval.Accessors.DataAccessor
{
    public class FileAccessor : IFileAccessor
    {
        public string TestMe(string param)
        {
            return param + ",FileAccessor";
        }

        #region basic file
        public DataTable HitBasicFile(QueryRequest query, out bool goodData, out DateTime exactQueryTime)
        {
            DataTable result = new DataTable();
            try
            {
                //int lastEndLine = ReadLastLineFromDisk(query.Query);

                using (StreamReader reader = new StreamReader(query.Query))
                {
                    result = ReadDataTableFromStream(reader, query);
                }

                //WriteLastLineToDisk(query.Query, lastEndLine);
                goodData = true;
                exactQueryTime = DateTime.Now;
            }
            catch (Exception e)
            {
                LogHelper.Log("Exception occurred hitting basic file: " + e.ToString(), LogLocation.FileAccessor, LogType.Error);
                goodData = false;
                exactQueryTime = DateTime.Now;
            }
            return result;
        }
        #endregion

        #region binary file
        public DataTable HitBinaryFile(QueryRequest query, out bool goodData, out DateTime exactQueryTime)
        {
            //StringBuilder sb = new StringBuilder();
            DataTable result = new DataTable();
            try
            {
                for (int i = 0; i < bfd.FieldIds.Count; i++)
                {
                    result.Columns.Add(bfd.FieldNames[i], bfd.FieldTypes[i]);

                    //if (i > 0)
                    //    sb.Append(',');
                    //sb.Append(bfd.FieldNames[i]);
                }
                //sb.AppendLine();

                var filePath = query.Query;
                using (BinaryReader b = new BinaryReader(File.Open(filePath, FileMode.Open), Encoding.ASCII))
                {
                    int pos = 0;
                    int length = (int)b.BaseStream.Length;
                    while (pos < length)
                    {
                        try
                        {
                            var row = result.NewRow();
                            bool includeRow = false;

                            for (int i = 0; i < bfd.FieldIds.Count; i++)
                            {
                                object value = null;
                                if (bfd.FieldTypes[i] == typeof(int))
                                {
                                    value = b.ReadInt32();
                                }
                                else if (bfd.FieldTypes[i] == typeof(DateTime))
                                {
                                    var year = b.ReadUInt16();
                                    var month = b.ReadUInt16();
                                    var dayOfWeek = b.ReadUInt16(); // not used
                                    var day = b.ReadUInt16();
                                    var hour = b.ReadUInt16();
                                    var minute = b.ReadUInt16();
                                    var second = b.ReadUInt16();
                                    var millisecond = b.ReadUInt16();

                                    DateTime date = new DateTime(year, month, day, hour, minute, second, millisecond);
                                    value = date;

                                    if (date >= query.DateToUse)
                                        includeRow = true;
                                }
                                else
                                    throw new ArgumentException("Field type not yet supported: " + bfd.FieldTypes[i].ToString());

                                //if (i > 0)
                                //    sb.Append(',');
                                //sb.Append(value.ToString());
                                row.SetField(i, value);
                                pos += bfd.FieldSize[i];
                            }

                            //sb.AppendLine();
                            if (includeRow)
                                result.Rows.Add(row);
                        }
                        catch (Exception e)
                        {
                            LogHelper.Log("Exception caught while reading binary file: " + e.ToString(), LogLocation.FileAccessor, LogType.Error);
                        }
                    }
                    exactQueryTime = DateTime.Now;
                }
                goodData = true;
            }
            catch (Exception e)
            {
                exactQueryTime = DateTime.Now;
                LogHelper.Log("Exception occurred hitting basic file: " + e.ToString(), LogLocation.FileAccessor, LogType.Error);
                goodData = false;
            }

            //File.WriteAllText(@"c:\temp\service.csv", sb.ToString());

            return result;
        }

        class BinaryFileDefinition
        {
            public List<Type> FieldTypes { get; set; }
            public List<string> FieldNames { get; set; }
            public List<int> FieldIds { get; set; }
            public List<int> FieldSize { get; set; }

            public BinaryFileDefinition()
            {
                FieldTypes = new List<Type>();
                FieldNames = new List<string>();
                FieldIds = new List<int>();
                FieldSize = new List<int>();
            }

            public void AddColumn(string name, Type type, int size, int id)
            {
                if (FieldNames == null)
                    FieldNames = new List<string>();
                if (FieldTypes == null)
                    FieldTypes = new List<Type>();
                if (FieldIds == null)
                    FieldIds = new List<int>();
                if (FieldSize == null)
                    FieldSize = new List<int>();

                FieldNames.Add(name);
                FieldTypes.Add(type);
                FieldIds.Add(id);
                FieldSize.Add(size);
            }
        }

        object bfdLock = new object();
        BinaryFileDefinition _bfd = null;
        BinaryFileDefinition bfd
        {
            get
            {
                lock (bfdLock)
                {
                    if (_bfd == null)
                    {
                        _bfd = new BinaryFileDefinition();

                        _bfd.AddColumn("TransactionNumber", typeof(int), 4, 1);
                        //_bfd.AddColumn("Course", typeof(int), 4, 2);
                        _bfd.AddColumn("TerminalNumber", typeof(int), 4, 3);
                        _bfd.AddColumn("TransactionDestination", typeof(int), 4, 4);
                        _bfd.AddColumn("VirtualDisplayId", typeof(int), 4, 5);
                        _bfd.AddColumn("CurrentActivityLevel", typeof(int), 4, 6);
                        _bfd.AddColumn("DisplayGroupId", typeof(int), 4, 7);
                        _bfd.AddColumn("ItemId", typeof(int), 4, 8);
                        //_bfd.AddColumn("ItemNumber", typeof(int), 4, 9);
                        _bfd.AddColumn("Modifier1Id", typeof(int), 4, 10);
                        _bfd.AddColumn("Modifier2Id", typeof(int), 4, 11);
                        _bfd.AddColumn("Modifier3Id", typeof(int), 4, 12);
                        _bfd.AddColumn("OrderStartTime", typeof(DateTime), 16, 13);
                        _bfd.AddColumn("FirstOrderTime", typeof(int), 4, 14);
                        _bfd.AddColumn("LastTotalTime", typeof(int), 4, 15);
                        _bfd.AddColumn("LastRecalTime", typeof(int), 4, 16);
                        _bfd.AddColumn("OrderPaidTime", typeof(int), 4, 17);
                        _bfd.AddColumn("OrderFirstDisplayedTime", typeof(int), 4, 18);
                        _bfd.AddColumn("OrderParkTime", typeof(int), 4, 19);
                        _bfd.AddColumn("OrderLastBumpTime", typeof(int), 4, 20);
                        //_bfd.AddColumn("SOSTag", typeof(string), 42, 21);
                        //_bfd.AddColumn("DestinationId", typeof(int), 4, 22);
                        //_bfd.AddColumn("ServerId", typeof(int), 4, 23);
                        //_bfd.AddColumn("TableNumber", typeof(int), 4, 24);
                        //_bfd.AddColumn("TableName", typeof(string), 30, 25);
                        //_bfd.AddColumn("ItemTagTime", typeof(int), 4, 26);
                        //_bfd.AddColumn("ItemDescription", typeof(string), 30, 27);
                        //_bfd.AddColumn("ItemCookTime", typeof(int), 4, 28);
                        //_bfd.AddColumn("ItemCategory", typeof(int), 4, 29);
                        //_bfd.AddColumn("ItemCookStartTime", typeof(int), 4, 30);
                        //_bfd.AddColumn("StationType", typeof(int), 4, 31);
                        //_bfd.AddColumn("ServerName", typeof(string), 30, 32);
                        //_bfd.AddColumn("ItemQuantity", typeof(int), 4, 33);
                        //_bfd.AddColumn("OrderPrepTime", typeof(int), 4, 34);
                        //_bfd.AddColumn("DestinationName", typeof(string), 16, 35);
                        //_bfd.AddColumn("VDPName", typeof(string), 30, 36);
                    }
                }
                return _bfd;
            }
        }
        #endregion

        //object saveLocalLockObj = new object();
        //public void SaveLocalResult(string seralizedResult, string fileName, bool posted)
        //{
        //    if (!posted)
        //        LogHelper.Log("Saving local result to " + fileName, LogLocation.FileAccessor, LogType.Verbose);

        //    string filePath = fileName;
        //    if (filePath != null
        //        && filePath.ToLower().StartsWith("c:"))
        //    {
        //        // all good;
        //    }
        //    else
        //    {
        //        filePath = Path.Combine(ConfigHelper.LocalResultsPath, fileName + ".txt");
        //    }

        //    lock (saveLocalLockObj)
        //    {
        //        if (posted)
        //        {
        //            if (File.Exists(filePath))
        //            {
        //                LogHelper.Log("Deleting " + fileName, LogLocation.FileAccessor, LogType.Verbose);
        //                File.Delete(filePath);
        //            }
        //        }
        //        else
        //        {
        //            if (!File.Exists(filePath))
        //            {
        //                if (!Directory.Exists(ConfigHelper.LocalResultsPath))
        //                    Directory.CreateDirectory(ConfigHelper.LocalResultsPath);
        //                File.WriteAllText(filePath, seralizedResult);
        //            }
        //        }
        //    }
        //}

        //public LocalResult[] RetrievePreviouslyUnpostedResults()
        //{
        //    List<LocalResult> result = new List<LocalResult>();
        //    if (Directory.Exists(ConfigHelper.LocalResultsPath))
        //    {
        //        var files = Directory.GetFiles(ConfigHelper.LocalResultsPath);

        //        if (files != null && files.Length > 0)
        //        {
        //            foreach (var file in files)
        //            {
        //                var resultText = File.ReadAllText(file);
        //                if (!string.IsNullOrEmpty(resultText))
        //                {
        //                    var tempResult = GenericExtensions.Deserialize<LocalResult>(resultText);
        //                    tempResult.FailedFileName = file;
        //                    if (tempResult != null)
        //                    {
        //                        result.Add(tempResult);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return result.ToArray();
        //}

        #region privates
        private DateTime GetDate(string[] data, int dateIndex, int timeIndex)
        {
            var result = DateTime.MinValue;
            if (data.Length > dateIndex
                && data.Length > timeIndex)
            {
                string dateString = data[dateIndex] + " " + data[timeIndex];
                DateTime date;
                if (DateTime.TryParse(dateString, out date))
                {
                    result = date;
                }
                else if (data[dateIndex].Length == 8)
                {
                    dateString =
                        data[dateIndex].Substring(0, 4) + '/' +
                        data[dateIndex].Substring(4, 2) + '/' +
                        data[dateIndex].Substring(6) + " " + data[timeIndex];
                    if (DateTime.TryParse(dateString, out date))
                        result = date;
                }
            }

            return result;
        }

        #region read file
        private DataTable ReadDataTableFromStream(TextReader reader, QueryRequest query)
        {
            DataTable result = new DataTable();

            int line_number = 1;
            string line = null;
            int dateIndex = -1;
            int timeIndex = -1;

            var headerStr = reader.ReadLine();
            var headers = headerStr.Split('\t');
            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i] == "date")
                    dateIndex = i;
                if (headers[i] == "time")
                    timeIndex = i;

                result.Columns.Add(headers[i]);
            }

            while ((line = reader.ReadLine()) != null)
            {
                var data = line.Split('\t');
                var date = GetDate(data, dateIndex, timeIndex);
                if (date > DateTime.MinValue
                    && date >= query.DateToUse)
                    result.LoadDataRow(data, LoadOption.OverwriteChanges);

                line_number++;
            }
            LogHelper.Log(string.Format("FileAccessor read {0} line(s) and is returning {1} row(s) for query {2}", line_number, result.Rows.Count, query.UniqueKey)
                , LogLocation.FileAccessor, LogType.Verbose);

            return result;
        }
        #endregion
        #region lastfileline
        object fileLockObj = new object();
        //List<FileLastLine> CachedFileLastLines = null;
        //private void WriteLastLineToDisk(string file, int lastEndLine)
        //{
        //    lock (fileLockObj)
        //    {
        //        if (CachedFileLastLines == null)
        //            CachedFileLastLines = new List<FileLastLine>();

        //        if (CachedFileLastLines.Where(cfll => cfll.FilePath == file.ToLower()).Count() > 0)
        //            for (int i = 0; i < CachedFileLastLines.Count; i++)
        //            {
        //                if (CachedFileLastLines[i].FilePath == file.ToLower())
        //                    CachedFileLastLines[i].LastLine = lastEndLine;
        //            }
        //        else
        //            CachedFileLastLines.Add(new FileLastLine()
        //            {
        //                FilePath = file.ToLower(),
        //                LastLine = lastEndLine
        //            });

        //        string path = ConfigHelper.LineTrackerFilePath;
        //        File.WriteAllText(path, GenericExtensions.Serialize(CachedFileLastLines));
        //    }
        //}

        //private int ReadLastLineFromDisk(string file)
        //{
        //    lock (fileLockObj)
        //    {
        //        try
        //        {
        //            if (CachedFileLastLines == null)
        //            {
        //                string path = ConfigHelper.LineTrackerFilePath;
        //                var contents = File.ReadAllText(path);
        //                try
        //                {
        //                    CachedFileLastLines = GenericExtensions.Deserialize<List<FileLastLine>>(contents);
        //                }
        //                catch
        //                {
        //                }
        //            }

        //            if (CachedFileLastLines != null
        //                && CachedFileLastLines.Where(cfll => cfll.FilePath == file.ToLower()).Count() > 0)
        //            {
        //                return CachedFileLastLines.Where(cfll => cfll.FilePath == file.ToLower()).First().LastLine;
        //            }
        //        }
        //        catch { }
        //        return 0;
        //    }
        //}
        #endregion
        #endregion
    }
}
