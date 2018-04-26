using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using System.Runtime.InteropServices;
using ObjCRuntime;

namespace BatteryMobileSaver.iOS
{
    public class Application
    {
        //[DllImport(Constants.SystemLibrary)]
        //internal static extern int sysctlbyname(
        //    [MarshalAs(UnmanagedType.LPStr)] string property, // name of the property
        //    IntPtr output, // output
        //    IntPtr oldLen, // IntPtr.Zero
        //    IntPtr newp, // IntPtr.Zero
        //    uint newlen // 0
        //);

        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            //const string HardwareProperty = "hw.machine";
            //string hardware = "unknown";

            //var pLen = Marshal.AllocHGlobal(sizeof(int));
            //sysctlbyname(HardwareProperty, IntPtr.Zero, pLen, IntPtr.Zero, 0);

            //var length = Marshal.ReadInt32(pLen);

            //// check to see if we got a length
            //if (length == 0)
            //{
            //    Marshal.FreeHGlobal(pLen);
            //   // return "Unknown";
            //}

            //// get the hardware string
            //var pStr = Marshal.AllocHGlobal(length);
            //sysctlbyname(HardwareProperty, pStr, pLen, IntPtr.Zero, 0);

            //// convert the native string into a C# string
            //hardware = Marshal.PtrToStringAnsi(pStr);

            //// cleanup
            //Marshal.FreeHGlobal(pLen);
            //Marshal.FreeHGlobal(pStr);


            //var tmp = UIDevice.CurrentDevice.SystemName;

            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
