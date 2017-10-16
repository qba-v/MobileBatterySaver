using BatteryMobileSaver.Interfaces;
using BatteryMobileSaver.iOS.ApplicationFiles;
using BatteryMobileSaver.Models;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(BatteryStatusiOS))]
namespace BatteryMobileSaver.iOS.ApplicationFiles
{
    public class BatteryStatusiOS : IBattery
    {
        #region Constructor
        public BatteryStatusiOS()
        {
            UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;
        }
        #endregion Constructor

        #region Properties
        public int RemainingChargePercent { get { return (int)(UIDevice.CurrentDevice.BatteryLevel * 100F); } }
        public BatteryStatus Status
        {
            get
            {
                switch (UIDevice.CurrentDevice.BatteryState)
                {
                    case UIDeviceBatteryState.Charging:
                        return BatteryStatus.Charging;
                    case UIDeviceBatteryState.Full:
                        return BatteryStatus.Full;
                    case UIDeviceBatteryState.Unplugged:
                        return BatteryStatus.Discharging;
                    default:
                        return BatteryStatus.Unknown;
                }
            }
        }

        public PowerSource PowerSource
        {
            get
            {
                switch (UIDevice.CurrentDevice.BatteryState)
                {
                    case UIDeviceBatteryState.Charging:
                        return PowerSource.Ac;
                    case UIDeviceBatteryState.Full:
                        return PowerSource.Ac;
                    case UIDeviceBatteryState.Unplugged:
                        return PowerSource.Battery;
                    default:
                        return PowerSource.Other;
                }
            }
        }
        #endregion Properties
    }
}