using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class SPResults
    {
        private int _id, _storeId, _requestId;
        private DateTime _completedDate;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public int StoreId
        {
            get
            {
                return _storeId;
            }
            set
            {
                _storeId = value;
            }
        }
        public int RequestId
        {
            get
            {
                return _requestId;
            }
            set
            {
                _requestId = value;
            }
        }
        public DateTime CompletedDate
        {
            get
            {
                return _completedDate;
            }
            set
            {
                _completedDate = value;
            }
        }

        public static SPResults GetRecentCompleted(string shortStoreName)
        {
            //todo:
            return null;
        }

        public static bool InsertNew(SPResults newResults)
        {
            //todo:
            return true;
        }
    }
}
