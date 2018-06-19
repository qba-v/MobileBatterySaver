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
            //foreach (var process in uwpNativeViewModel.ProcessList)
            //{
            //    sharedViewModel.ProcessList.Add(new BatteryMobileSaver.Models.ProcessInfoModel
            //    {
            //        CpuUsageTime = process.CpuUsageTime,
            //        DiskBytesCount = process.DiskBytesCount,
            //        ExeName = process.ExeName,
            //        PageFileSize = process.PageFileSize,
            //        ProcessId = process.ProcessId,
            //        WorkingSetSize = process.WorkingSetSize
            //    });
            //}

            

            return sharedViewModel;
        }
        public async void GetAppsList()
        {
            var list = await AppDiagnosticInfo.RequestInfoAsync();


            ////list.ToList().ForEach(o => sharedViewModel.AppInfoList.Add(new BatteryMobileSaver.Models.AppInfoModel(o.AppInfo)));
            //list.ToList().ForEach(o => sharedViewModel.AppInfoList.Add(new BatteryMobileSaver.Models.AppInfoModel
            //{
            //    DisplayName = o.AppInfo.DisplayInfo.DisplayName,
            //    Description = o.AppInfo.DisplayInfo.Description,
            //    AppUserModelId = o.AppInfo.AppUserModelId,
            //    PackageFamilyName = o.AppInfo.PackageFamilyName,


            //}));
            try
            {

                foreach (var app in list.ToList())
                {
                    BatteryMobileSaver.Models.AppInfoModel appInfo = new BatteryMobileSaver.Models.AppInfoModel();
                    appInfo.DisplayName = app.AppInfo.DisplayInfo.DisplayName;
                    appInfo.Description = app.AppInfo.DisplayInfo.Description;
                    appInfo.PackageFamilyName = app.AppInfo.PackageFamilyName;

                    AppDiagnosticInfo appDiagnostic = app;
                    var resourceGroup = app.GetResourceGroups();
                    var group = resourceGroup.FirstOrDefault();
                    if (group != null)
                        appInfo.MemoryUsage = "Memory usage: " + group.GetMemoryReport().TotalCommitUsage.ToString() + " B";

                    sharedViewModel.AppInfoList.Add(appInfo);
                }
            }
            catch (Exception)
            {

                Console.WriteLine("System not support dll");
            }
            

            
        }
        public async void GetAppInfo()
        {
            IList<AppDiagnosticInfo> list = await AppDiagnosticInfo.RequestInfoForAppAsync();
            //list.ToList().ForEach(o => sharedViewModel.AppInfoList.Add(new BatteryMobileSaver.Models.AppInfoModel(o.AppInfo)));
            list.ToList().ForEach(o => sharedViewModel.AppInfoList.Add(new BatteryMobileSaver.Models.AppInfoModel
            {
                DisplayName = o.AppInfo.DisplayInfo.DisplayName,
                Description = o.AppInfo.DisplayInfo.Description,
                AppUserModelId = o.AppInfo.AppUserModelId,
                PackageFamilyName = o.AppInfo.PackageFamilyName
                
            }));


        }

        private void LoadProcesses()
        {
            List<ProcessDiagnosticInfo> processList = ProcessDiagnosticInfo.GetForProcesses().ToList();
            
            foreach (var process in processList)
            {
                BatteryMobileSaver.Models.ProcessInfoModel processInfo = new BatteryMobileSaver.Models.ProcessInfoModel();

                processInfo.ExeName = process.ExecutableFileName;
                processInfo.ProcessId = process.ProcessId;
                var memory = process.MemoryUsage;
                processInfo.MemoryUsage = "Memory usage: " + memory.GetReport().WorkingSetSizeInBytes.ToString() + " B";
                processInfo.CpuUsage = "Cpu usage: " + process.CpuUsage.GetReport().KernelTime.Seconds.ToString() + " seconds";

                sharedViewModel.ProcessList.Add(processInfo);
            }

            var avm = MemoryManager.ExpectedAppMemoryUsageLimit / 1048576;
            var tm = MemoryManager.AppMemoryUsage / 1048576;

            sharedViewModel.AvailableMemoryUsage = (long)avm;
            sharedViewModel.TotalMemoryUsage = (long)tm;
            processList.ForEach(o => uwpNativeViewModel.ProcessList.Add(new ProcessInfoModel(o)));
        }

        public SharedViewModel KillAvailableProcesses()
        {
            throw new NotImplementedException();
        }
        
    }
}
