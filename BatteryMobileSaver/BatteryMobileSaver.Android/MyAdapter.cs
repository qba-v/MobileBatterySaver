using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace BatteryMobileSaver.Droid
{
    public class MyAdapter : BaseAdapter
    {
        List<ActivityManager.RunningAppProcessInfo> processes;
        Context context;
        public override int Count => throw new NotImplementedException();

        public MyAdapter(List<ActivityManager.RunningAppProcessInfo>
       processes, Context context)
        {
            this.context = context;
            this.processes = processes;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return processes.FirstOrDefault(x => x.Pid == position);
        }

        public override long GetItemId(int position)
        {
            return processes.FirstOrDefault(x => x.Pid == position).Pid;
        }

        public View GetView(int position, View convertView, ViewGroup parent)
        {
            Process pro;

            if (convertView == null)
            {
                convertView = new TextView(context);
                pro = new Process();
                pro.name = (TextView)convertView;

                //convertView.SetTag(position, pro);
            }
            else
            {
                //pro = (Process)convertView.GetTag();
            }

            //pro.name.SetText(processes.Get(position).processName);

            return convertView;
        }
        
        class Process
        {
            public TextView name;
        }
    }
}