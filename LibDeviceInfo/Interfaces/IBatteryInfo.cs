using System;
using System.Collections.Generic;
using System.Text;

namespace LibDeviceInfo.Interfaces
{
    public interface IBatteryInfo
    {
        IObservable<int> WhenBatteryPercentageChanged();
        IObservable<PowerStatus> WhenPowerStatusChanged();
    }
}
