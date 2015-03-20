using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace GuestCountUploader.Engines
{
    public class DataConversionEngine
    {
        private const string LINE_FORMATTER = "{0},{1}";

        public string FormatDefinitions(CustomerDefinition[] sourceDefs, DateTime desiredDate)
        {
            StringBuilder sb = new StringBuilder();
            var allDefs = sourceDefs.Where(cd => cd.LastFullDate == desiredDate.Date);
            foreach (var def in allDefs)
            {
                sb.AppendLine(string.Format(LINE_FORMATTER, def.Id, def.LastGuestCount));
            }

            return sb.ToString();
        }
    }
}
