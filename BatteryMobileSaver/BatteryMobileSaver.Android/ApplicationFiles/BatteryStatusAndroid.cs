using Android.Content;
using BatteryMobileSaver.Interfaces;
using BatteryMobileSaver.Models;
using System;
using Android.App;
using Android.OS;
using BatteryStatus = Android.OS.BatteryStatus;
using BatteryMobileSaver.Droid.ApplicationFiles;

[assembly: Xamarin.Forms.Dependency(typeof(BatteryStatusAndroid))]
namespace BatteryMobileSaver.Droid.ApplicationFiles
{
    public class BatteryStatusAndroid : IBattery
    {
        #region Constructor 
        public BatteryStatusAndroid(){}
        #endregion Constructor
        #region Fields
        //private BatteryBroadcastReceiver batteryReceiver;
        #endregion Fields

        public int RemainingChargePercent
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            var level = battery.GetIntExtra(BatteryManager.ExtraLevel, -1);
                            var scale = battery.GetIntExtra(BatteryManager.ExtraScale, -1);

                            return (int)Math.Floor(level * 100D / scale);
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }

            }
        }

        public BatteryMobileSaver.Models.BatteryStatus Status
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            int status = battery.GetIntExtra(BatteryManager.ExtraStatus, -1);
                            var isCharging = status == (int)BatteryStatus.Charging || status == (int)BatteryStatus.Full;

                            var chargePlug = battery.GetIntExtra(BatteryManager.ExtraPlugged, -1);
                            var usbCharge = chargePlug == (int)BatteryPlugged.Usb;
                            var acCharge = chargePlug == (int)BatteryPlugged.Ac;
                            bool wirelessCharge = false;
                            wirelessCharge = chargePlug == (int)BatteryPlugged.Wireless;

                            isCharging = (usbCharge || acCharge || wirelessCharge);
                            if (isCharging)
                                return BatteryMobileSaver.Models.BatteryStatus.Charging;

                            switch (status)
                            {
                                case (int)BatteryStatus.Charging:
                                    return BatteryMobileSaver.Models.BatteryStatus.Charging;
                                case (int)BatteryStatus.Discharging:
                                    return BatteryMobileSaver.Models.BatteryStatus.Discharging;
                                case (int)BatteryStatus.Full:
                                    return BatteryMobileSaver.Models.BatteryStatus.Full;
                                case (int)BatteryStatus.NotCharging:
                                    return BatteryMobileSaver.Models.BatteryStatus.NotCharging;
                                default:
                                    return BatteryMobileSaver.Models.BatteryStatus.Unknown;
                            }
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }
            }
        }

        public PowerSource PowerSource
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            int status = battery.GetIntExtra(BatteryManager.ExtraStatus, -1);
                            var isCharging = status == (int)BatteryStatus.Charging || status == (int)BatteryStatus.Full;

                            var chargePlug = battery.GetIntExtra(BatteryManager.ExtraPlugged, -1);
                            var usbCharge = chargePlug == (int)BatteryPlugged.Usb;
                            var acCharge = chargePlug == (int)BatteryPlugged.Ac;

                            bool wirelessCharge = false;
                            wirelessCharge = chargePlug == (int)BatteryPlugged.Wireless;

                            isCharging = (usbCharge || acCharge || wirelessCharge);

                            if (!isCharging)
                                return PowerSource.Battery;
                            else if (usbCharge)
                                return PowerSource.Usb;
                            else if (acCharge)
                                return PowerSource.Ac;
                            else if (wirelessCharge)
                                return PowerSource.Wireless;
                            else
                                return PowerSource.Other;
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }
            }
        }
    }
}