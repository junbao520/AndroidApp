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
using System.Threading;

namespace TwoPole.Chameleon3
{
    [Activity(Label = "Loading")]
    public class Loading : Activity
    {
        //�򿪵�ʱ�����һ�����ڶԻ��� 
        //it is time
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.loading);
            //��ʱ5����,
            Handler handler = new Handler();
            //�ӳ�4��
            handler.PostDelayed(PostDelayAction, 4000);
        }
        public void PostDelayAction()
        {
            //��ת������ҳ��
            this.Finish();
        }
        
    }

}