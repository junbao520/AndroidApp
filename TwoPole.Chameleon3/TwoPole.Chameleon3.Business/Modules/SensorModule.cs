using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using TwoPole.Chameleon3.Infrastructure;
using Android.Util;

namespace TwoPole.Chameleon3.Business.Modules
{
    public class SensorModule : ModuleBase
    {
        public SensorModule() 
   
        {
        }

        public override Task StartAsync(ExamContext context)
        {
            return Task.Run(() =>Log.Debug("ModuleBase", "开始检测模块"));
        }

        public override Task StopAsync(bool close=false)
        {
            return Task.Run(() => Log.Debug("ModuleBase", "停止检测模块"));
        }
    }
}
