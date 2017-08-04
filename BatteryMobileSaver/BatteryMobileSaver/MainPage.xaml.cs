using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BatteryMobileSaver
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            
        }
        

        private void btn_Clicked(object sender, EventArgs e)
        {
            btn.Text = "chacnge";
        }
    }
}
