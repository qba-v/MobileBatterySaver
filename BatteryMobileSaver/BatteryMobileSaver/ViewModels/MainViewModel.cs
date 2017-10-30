using BatteryMobileSaver.Interfaces;
using BatteryMobileSaver.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BatteryMobileSaver.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public IBattery Battery { get; private set; }
        public ICommand ClearBattery { get; }

        public MainViewModel(IBattery battery)
        {
            this.Battery = battery;

            this.ClearBattery = new Command(this.BatteryEvents.Clear);
        }

        public ObservableCollection<BatteryEventViewModel> BatteryEvents { get; } = new ObservableCollection<BatteryEventViewModel>();


    }
}
