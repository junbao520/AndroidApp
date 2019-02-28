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

namespace Twopole.Chameleon3.Domain
{
    //自定义灯光
    //添加属性
    public class CustomLight
    {
        //允许灯光
        public string Allowlights;
        //禁止灯光
        public string Fobbidlights;
        //交替灯光次数
        public int AlternateCount;
    }
}