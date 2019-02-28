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
using GalaSoft.MvvmLight.Messaging;

namespace TwoPole.Chameleon3.Infrastructure.Messages
{
    public class ModifyGearOverMessage : MessageBase
    {
        /// <summary>
        /// 完成的项目编码
        /// </summary>
        public string PassedItemCode { get; set; }
    }
}