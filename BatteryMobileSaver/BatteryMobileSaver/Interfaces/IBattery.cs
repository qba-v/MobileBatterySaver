using Android.OS;
using BatteryMobileSaver.Models;

namespace BatteryMobileSaver.Interfaces
{
    public interface IBattery
    {
        int RemainingChargePercent { get; }
        Models.BatteryStatus Status { get; }
        PowerSource PowerSource { get; }
    }
}
