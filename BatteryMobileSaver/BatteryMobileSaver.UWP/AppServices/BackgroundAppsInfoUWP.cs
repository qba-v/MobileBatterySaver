using BatteryMobileSaver.Abstract;
using BatteryMobileSaver.UWP.Models;
using BatteryMobileSaver.UWP.ViewModels;
using BatteryMobileSaver.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.System.Diagnostics;

namespace BatteryMobileSaver.UWP.AppServices
{
    public class BackgroundAppsInfoUWP : IBackgroundAppsInfo
    {
        BatteryMobileSaver.ViewModels.SharedViewModel sharedViewModel = new BatteryMobileSaver.ViewModels.SharedViewModel();
        BatteryMobileSaver.UWP.ViewModels.UWPNativeViewModel uwpNativeViewModel = new BatteryMobileSaver.UWP.ViewModels.UWPNativeViewModel();

        public BatteryMobileSaver.ViewModels.SharedViewModel GetBeackgroundProcesses()
        {
            GetAppsList();
            LoadProcesses();
            foreach (var process in uwpNativeViewModel.ProcessList)
            {
                sharedViewModel.ProcessList.Add(new BatteryMobileSaver.Models.ProcessInfoModel
                {
                    CpuUsageTime = process.CpuUsageTime,
                    DiskBytesCount = process.DiskBytesCount,
                    ExeName = process.ExeName,
                    PageFileSize = process.PageFileSize,
                    ProcessId = process.ProcessId,
                    WorkingSetSize = process.WorkingSetSize
                });
            }
            return sharedViewModel;
        }
        public async void GetAppsList()
        {
            IList<AppDiagnosticInfo> list = await AppDiagnosticInfo.RequestInfoAsync();
            //list.ToList().ForEach(o => sharedViewModel.AppInfoList.Add(new BatteryMobileSaver.Models.AppInfoModel(o.AppInfo)));
            list.ToList().ForEach(o => sharedViewModel.AppInfoList.Add(new BatteryMobileSaver.Models.AppInfoModel
            {
                DisplayName = o.AppInfo.DisplayInfo.DisplayName,
                Description = o.AppInfo.DisplayInfo.Description,
                AppUserModelId = o.AppInfo.AppUserModelId,
                PackageFamilyName = o.AppInfo.PackageFamilyName,
                
            }));
        }
        //public async void GetAppInfo()
        //{
        //    IList<AppDiagnosticInfo> list = await AppDiagnosticInfo.GetResourceGroups();
        //    //list.ToList().ForEach(o => sharedViewModel.AppInfoList.Add(new BatteryMobileSaver.Models.AppInfoModel(o.AppInfo)));
        //    list.ToList().ForEach(o => sharedViewModel.AppInfoList.Add(new BatteryMobileSaver.Models.AppInfoModel
        //    {
        //        DisplayName = o.AppInfo.DisplayInfo.DisplayName,
        //        Description = o.AppInfo.DisplayInfo.Description,
        //        AppUserModelId = o.AppInfo.AppUserModelId,
        //        PackageFamilyName = o.AppInfo.PackageFamilyName
        //        o.
        //    }));
        //}

        private void LoadProcesses()
        {
            List<ProcessDiagnosticInfo> processList = ProcessDiagnosticInfo.GetForProcesses().ToList();
            
            processList.ForEach(o => uwpNativeViewModel.ProcessList.Add(new ProcessInfoModel(o)));
        }

        public SharedViewModel KillAvailableProcesses()
        {
            throw new NotImplementedException();
        }
        
    }
}
