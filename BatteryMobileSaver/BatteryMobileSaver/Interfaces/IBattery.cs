using BatteryMobileSaver.Models;
using System;

namespace BatteryMobileSaver.Interfaces
{
    public interface IBattery
    {
        int RemainingChargePercent { get; }
        Models.BatteryStatus Status { get; }
        PowerSource PowerSource { get; }
    }
}
