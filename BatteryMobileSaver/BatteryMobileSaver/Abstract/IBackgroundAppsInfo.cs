using BatteryMobileSaver.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatteryMobileSaver.Abstract
{
    public interface IBackgroundAppsInfo
    {
        SharedViewModel GetBeackgroundProcesses();

        SharedViewModel KillAvailableProcesses();
        
    }
}
