using Android.App;
using BatteryMobileSaver.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace BatteryMobileSaver
{
    public class App : Xamarin.Forms.Application
    {
        //public App(UWPViewModel uwpViewModel)
        //{
        //    this.MainPage
        //}

        public App(UWPViewModel uwpViewModel = null)
        {

            this.MainPage = new MainPage(uwpViewModel);
            
        }
    }
}
