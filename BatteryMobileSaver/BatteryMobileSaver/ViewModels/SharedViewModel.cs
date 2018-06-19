
using BatteryMobileSaver.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatteryMobileSaver.ViewModels
{
    public  class SharedViewModel : INotifyPropertyChanged
    {
        public long TotalMemoryUsage { get; set; }
        public long AvailableMemoryUsage { get; set; }

        public SharedViewModel()
        {
            appInfoList = new ObservableCollection<AppInfoModel>();
            processList = new ObservableCollection<ProcessInfoModel>();
        }

        private int _processesCount;
        public int ProcessesCount
        {
            get { return _processesCount; }
            set { _processesCount = value; NotifyPropertyChanged("ProcessesCount"); }
        }

        private ObservableCollection<AppInfoModel> appInfoList;
        public ObservableCollection<AppInfoModel> AppInfoList
        {
            get { return appInfoList; }
            set { appInfoList = value; NotifyPropertyChanged("AppInfoList"); }
        }

        private ObservableCollection<ProcessInfoModel> processList;
        public ObservableCollection<ProcessInfoModel> ProcessList
        {
            get { return processList; }
            set { processList = value; NotifyPropertyChanged("ProcessList"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }


    }
}
