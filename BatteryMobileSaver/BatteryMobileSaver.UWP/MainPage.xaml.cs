using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Xamarin.Forms.Platform.UWP;
using Windows.Foundation.Metadata;
using Windows.System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

namespace BatteryMobileSaver.UWP
{
    public sealed partial class MainPage : WindowsPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            //   var list = GetAppInfo();

            //list.Result.ToList().ForEach(o => uwpViewModel.AppInfoList.Add(new Models.AppInfoModel(o.AppInfo)));
            
            LoadApplication(new BatteryMobileSaver.App());
        }

        //public async Task<IList<AppDiagnosticInfo>> GetAppInfo()
        //{
        //    IList<AppDiagnosticInfo> list = await AppDiagnosticInfo.RequestInfoAsync();
        //    list.ToList().ForEach(o => uwpViewModel.AppInfoList.Add(new Models.AppInfoModel(o.AppInfo)));

        //    return list;
        //}
    }
}
