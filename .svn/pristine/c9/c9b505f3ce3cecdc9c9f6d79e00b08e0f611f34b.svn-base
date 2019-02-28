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

namespace TwoPole.Chameleon3
{
    class LoadingDialog:Dialog
    {
        private TextView tv_text;

        public LoadingDialog(Context context):base(context)
        {
           // super(context);
            /**设置对话框背景透明*/
            //GetWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
            SetContentView(Resource.Layout.loading);
           // tv_text = (TextView)FindViewById(Resource.Id.tv_text);
            SetCanceledOnTouchOutside(false);
        }
        /**
         * 为加载进度个对话框设置不同的提示消息
         *
         * @param message 给用户展示的提示信息
         * @return build模式设计，可以链式调用
         */
        public LoadingDialog setMessage(string message)
        {
            tv_text.Text = message;
            return this;
        }

    }
}