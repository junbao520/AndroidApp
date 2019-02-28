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
using Android_serialport_api;
using CN.Wch.Ch34xuartdriver;

namespace TwoPole.Chameleon3.Infrastructure.Devices.CarSignal
{
    //通用的CarSignalSeed接口主要是为了使用依赖注入 Ioc


    public interface ISensorCarSignalSeed
    {
        bool InitAsync(CH34xUARTDriver usbDriver = null, Connections serialDriver = null);
        void StartAsync();
    }
}