using System;

namespace LibDeviceInfo
{
    public static class CrossShared
    {
#if NETSTANDARD1_0
        const string ERROR = "[LibDeviceInfo] Platform implementation not found.  Have you added a nuget reference to your platform project?";
#endif
        static IBatteryInfo batt;
        public static IBatteryInfo Battery
        {
            get
            {
#if NETSTANDARD1_0
                throw new Exception(ERROR);
#else
                batt = batt ?? new BatteryInfo();
                return batt;
#endif
            }
            set { batt = value; }
        }
    }
}
