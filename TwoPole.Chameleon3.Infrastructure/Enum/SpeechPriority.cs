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

namespace TwoPole.Chameleon3.Infrastructure
{

    /// <summary>
    /// 设置语音的优先级
    /// </summary>
    public enum SpeechPriority : byte
    {
        High= 0,

        Normal = 1,

        Low=2
    }
}