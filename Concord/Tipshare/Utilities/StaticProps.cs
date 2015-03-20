using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tipshare
{
    public static class StaticProps
    {
        public static POSType? _posType = null;
        public static POSType PosType
        {
            get
            {
                if (_posType == null || !_posType.HasValue)
                {
                    _posType = POSTypeFetcher.GetPOS();
                }
                return _posType.Value;
            }
        }

        public static int AMPMBreakHour
        {
            get
            {
                return 16;
            }
        }

        public static string AMPMBreakAsTimeString
        {
            get
            {
                return AMPMBreakHour + ":00";
            }
        }

        public static string[] BarJobCodes
        {
            get
            {
                string[] result = new string[] { };
                switch (PosType)
                {
                    case POSType.AppleOne:
                        result = new string[] { "7", "19" };
                        break;
                    case POSType.Aloha:
                        result = new string[] { "31" };
                        break;
                    case POSType.Unknown:
                    default:
                        break;
                }
                return result;
            }
        }

        public static string[] HostJobCodes
        {
            get
            {
                string[] result = new string[] { };
                switch (PosType)
                {
                    case POSType.AppleOne:
                        result = new string[] { "6", "18" };
                        break;
                    case POSType.Aloha:
                        result = new string[] { "15", "55" };
                        break;
                    case POSType.Unknown:
                    default:
                        break;
                }
                return result;
            }
        }

        public static string[] BarHostJobCodes
        {
            get
            {
                string[] result = new string[] { };
                switch (PosType)
                {
                    case POSType.AppleOne:
                        result = new string[] { "6", "7", "18", "19" };
                        break;
                    case POSType.Aloha:
                        result = new string[] { "31", "15", "55" };
                        break;
                    case POSType.Unknown:
                    default:
                        break;
                }
                return result;
            }
        }

        public static string[] OtherJobCodes
        {
            get
            {
                string[] result = new string[] { };
                switch (PosType)
                {
                    case POSType.AppleOne:
                        result = new string[] { "1", "2", "3", "4", "5", "8", "11", "12", "13", "17", "18" };
                        break;
                    case POSType.Aloha:
                        result = new string[] { "19", "20", "32", "45", "90" };
                        break;
                    case POSType.Unknown:
                    default:
                        break;
                }
                return result;
            }
        }

        private static string[] _serverJobCodes = null;
        public static string[] ServerJobCodes
        {
            get
            {
                if (_serverJobCodes == null)
                {
                    _serverJobCodes = new string[] { };
                    switch (PosType)
                    {
                        case POSType.AppleOne:
                            _serverJobCodes = new string[] { "3", "29" };
                            break;
                        case POSType.Aloha:
                            _serverJobCodes = new string[] { "19", "20" };
                            break;
                        case POSType.Unknown:
                        default:
                            break;
                    }

                    if (ConfigHelper.AdditionalServerJobCodes != null
                        && ConfigHelper.AdditionalServerJobCodes.Length > 0)
                    {
                        var temp = _serverJobCodes.ToList();
                        temp.AddRange(ConfigHelper.AdditionalServerJobCodes);
                        temp = temp.Distinct().ToList();
                        _serverJobCodes = temp.ToArray();
                    }
                }
                return _serverJobCodes;
            }
        }

        public static string[] ServerOverrideNames
        {
            get
            {
                return new string[] 
                {
                    "TOGO CARD",
                };
            }
        }

        public static string dbAddy { get; set; }
    }
}
