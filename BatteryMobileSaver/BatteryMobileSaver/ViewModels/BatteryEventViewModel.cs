using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatteryMobileSaver.Models
{
    public class BatteryEventViewModel
    {
        public string Text { get; } = DateTime.Now.ToString("T");
        public string Detail { get; set; }
    }
}
