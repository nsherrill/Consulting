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


        private static string[] _barJobCodes = null;
        public static string[] BarJobCodes
        {
            get
            {
                if (_barJobCodes == null)
                {
                    if (ConfigHelper.BarJobCodes == null)
                    {
                        switch (PosType)
                        {
                            case POSType.AppleOne:
                                _barJobCodes = new string[] { "7", "19" };
                                break;
                            case POSType.Aloha:
                                _barJobCodes = new string[] { "31" };
                                break;
                            case POSType.Unknown:
                            default:
                                break;
                        }
                    }
                    else _barJobCodes = ConfigHelper.BarJobCodes;
                }
                return _barJobCodes;
            }
        }

        private static string[] _hostJobCodes = null;
        public static string[] HostJobCodes
        {
            get
            {
                if (_hostJobCodes == null)
                {
                    if (ConfigHelper.HostJobCodes == null)
                    {
                        switch (PosType)
                        {
                            case POSType.AppleOne:
                                _hostJobCodes = new string[] { "6" };
                                break;
                            case POSType.Aloha:
                                _hostJobCodes = new string[] { "15", "55" };
                                break;
                            case POSType.Unknown:
                            default:
                                break;
                        }
                    }
                    else _hostJobCodes = ConfigHelper.HostJobCodes;
                }
                return _hostJobCodes;
            }
        }


        private static string[] _barHostJobCodes = null;
        public static string[] BarHostJobCodes
        {
            get
            {
                if (_barHostJobCodes == null)
                {
                    List<string> barHostJobs = new List<string>();
                    barHostJobs.AddRange(BarJobCodes);
                    barHostJobs.AddRange(HostJobCodes);
                    _barHostJobCodes = barHostJobs.ToArray();
                }
                return _barHostJobCodes;
            }
        }


        private static string[] _otherJobCodes = null;
        public static string[] OtherJobCodes
        {
            get
            {
                if (_otherJobCodes == null)
                {
                    if (ConfigHelper.OtherJobCodes == null)
                    {
                        switch (PosType)
                        {
                            case POSType.AppleOne:
                                _otherJobCodes = new string[] { "1", "2", "3", "4", "5", "8", "11", "12", "13", "17", "18" };
                                break;
                            case POSType.Aloha:
                                _otherJobCodes = new string[] { "19", "20", "32", "45", "90" };
                                break;
                            case POSType.Unknown:
                            default:
                                break;
                        }
                    }
                    else _otherJobCodes = ConfigHelper.OtherJobCodes;
                }
                return _otherJobCodes;
            }
        }

        private static string[] _serverJobCodes = null;
        public static string[] ServerJobCodes
        {
            get
            {
                if (_serverJobCodes == null)
                {
                    if (ConfigHelper.ServerJobCodes == null)
                    {
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
                    }
                    else _serverJobCodes = ConfigHelper.ServerJobCodes;
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
