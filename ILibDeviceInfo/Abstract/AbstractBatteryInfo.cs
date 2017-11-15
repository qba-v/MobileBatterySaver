using System;
using System.Reactive.Linq;

namespace LibDeviceInfo
{
    public abstract class AbstractBatteryInfo : IBatteryInfo
    {
        public virtual IObservable<int> WhenBatteryPercentageChanged() => Observable.Empty<int>();
        public virtual IObservable<PowerStatus> WhenPowerStatusChanged() => Observable.Return(PowerStatus.Unknown);
    }
}